using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;

public class BatAI : MonoBehaviour
{
    private static readonly float mRadiusSquaredDistance = 5.0f;
    private static readonly float mMaxVelocity = 1.0f;
    private static readonly float mMaxCubeExtent = 3.0f/2;
    private static readonly float mMaxCubeExtentX = 9.5f/2;

    private Transform cube;
    
    private int mID = 0;
    private Vector3 mVelocity = new Vector3();
    

    void Start()
    {
        cube = GameObject.FindGameObjectWithTag("BatCube").GetComponent<Transform>();
        mVelocity = transform.forward;
        mVelocity = Vector3.ClampMagnitude( mVelocity, mMaxVelocity );
    }

    void FixedUpdate()
    {
        mVelocity += FlockingBehaviour();

        mVelocity = Vector3.ClampMagnitude( mVelocity, mMaxVelocity );

        transform.position += mVelocity * Time.deltaTime;

        transform.forward = mVelocity.normalized;

        Reposition();
    }

    private void Reposition()
    {
        Vector3 position = transform.position;

        if( position.x >= cube.position.x + mMaxCubeExtentX )
        {
            position.x = cube.position.x + mMaxCubeExtentX - 0.2f;
            mVelocity.x *= -1;
        }
        else if( position.x <= -mMaxCubeExtentX + cube.position.x )
        {
            position.x = cube.position.x + -mMaxCubeExtentX + 0.2f;
            mVelocity.x *= -1;
        }

        if( position.y >= cube.position.y + mMaxCubeExtent )
        {
            position.y = cube.position.y + mMaxCubeExtent - 0.2f;
            mVelocity.y *= -1;
        }
        else if( position.y <= -mMaxCubeExtent + cube.position.y )
        {
            position.y = cube.position.y -mMaxCubeExtent + 0.2f;
            mVelocity.y *= -1;
        }

        if( position.z >= cube.position.z + mMaxCubeExtent )
        {
            position.z = cube.position.z + mMaxCubeExtent - 0.2f;
            mVelocity.z *= -1;
        }
        else if( position.z <= -mMaxCubeExtent + cube.position.z )
        {
            position.z = cube.position.z + -mMaxCubeExtent + 0.2f;
            mVelocity.z *= -1;
        }

        transform.forward = mVelocity.normalized;
        transform.position = position;
    }

    public void SetID( int ID )
    {
        mID = ID;
    }

    public int ID
    {
        get { return mID; }
    }
    
    private Vector3 FlockingBehaviour()
    {
        List<BatAI> theFlock = Bat.instance.theFlock;

        Vector3 cohesionVector = new Vector3();
        Vector3 separateVector = new Vector3();
        Vector3 alignmentVector = new Vector3();

        int count = 0;

        for( int index = 0; index < theFlock.Count; index++ )
        {
            if( mID != theFlock[ index ].ID )
            {
                float distance = ( transform.position - theFlock[ index ].transform.position ).sqrMagnitude;

                if( distance > 0 && distance < mRadiusSquaredDistance )
                {
                    cohesionVector += theFlock[ index ].transform.position;
                    separateVector += theFlock[ index ].transform.position - transform.position;
                    alignmentVector += theFlock[ index ].transform.forward;

                    count++;
                }
            }
        }

        if( count == 0 )
        {
            return Vector3.zero;
        }

        // revert vector
        // separation step
        separateVector /= count;
        separateVector *= -1;

        // forward step
        alignmentVector /= count;

        // cohesione step
        cohesionVector /= count;
        cohesionVector = ( cohesionVector - transform.position );

        // Add All vectors together to get flocking
        Vector3 flockingVector = ( ( separateVector.normalized * Bat.instance.separationWeight ) +
                                    ( cohesionVector.normalized * Bat.instance.cohesionWeight ) +
                                    ( alignmentVector.normalized * Bat.instance.alignmentWeight ) );

        return flockingVector;
    }
}