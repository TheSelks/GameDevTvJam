using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Processors;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int maxHealth;
    public int currAmulets;

    public Image[] amulets;
    public Sprite goodAmulet;
    public Sprite gubbedAmulet;
    [SerializeField] private SceneManager sceneManager;

    void Update()
    {
        if (currAmulets <= 0)
        {
            Dead();
        }

        Mathf.Clamp(currAmulets, 0, maxHealth);

        for (int i = 0; i < amulets.Length; i++)
        {
            if (i < currAmulets)
            {
                amulets[i].sprite = goodAmulet;
            }
            else
            {
                amulets[i].sprite = gubbedAmulet;
            }
            
        }
    }

    public void ChangeHealth(bool modifier)
    {
        if (modifier)
            currAmulets++;
        else
        {
            currAmulets--;
        }
    }

    private void Dead()
    {
        sceneManager.GameOver();
    }
}
