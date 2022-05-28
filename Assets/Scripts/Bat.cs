using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bat : MonoBehaviour
{
    private static readonly int mNumberOfEntities = 20;
    
    public BatAI templatePrefab = null;

    public float separationWeight = 0.8f;
    public float alignmentWeight = 0.5f;
    public float cohesionWeight = 0.7f;

    public Transform batParent;
    public Vector3 batPositions;
    public static Bat instance = null;
    
    private List<BatAI> mTheFlock = new List<BatAI>();
    
    void Start ()
    {
        instance = this;
        InstantiateFlock();
    }
    
    private void InstantiateFlock()
    {
        for ( int i = 0; i < mNumberOfEntities; i++ )
        {
            BatAI flockBatAI = Instantiate( templatePrefab, batPositions, Quaternion.identity, batParent );

            flockBatAI.transform.rotation = Random.rotation;

            flockBatAI.SetID( i );

            mTheFlock.Add( flockBatAI );
        }
    }
    
    public List<BatAI> theFlock
    {
        get { return mTheFlock; }
    }
}