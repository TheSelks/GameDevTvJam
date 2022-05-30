using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class JarCollection : MonoBehaviour
{
    [SerializeField] private Sprite monkey;
    [SerializeField] private Sprite human;
    [SerializeField] private Sprite bird;
    [SerializeField] private Sprite jackal;


    public void Monkey()
    {
        monkey.GameObject().SetActive(true);
    }

    public void Human()
    {
        human.GameObject().SetActive(true);
    }

    public void Bird()
    {
        bird.GameObject().SetActive(true);
    }

    public void Jackal()
    {
        jackal.GameObject().SetActive(true);
    }
}
