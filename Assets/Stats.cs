using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{

    private int health;
    private bool isAlive = true;

    public int Health { get => health; }
    public bool IsAlive { get => isAlive; }

    public void Awake()
    {
        health = 100;
    }


    public void InflictDamage(int healthAmount)
    {
        if (health - healthAmount <= 0)
        {
            health = 0;
            isAlive = false;
        }
        else
        {
            health -= healthAmount;
        }
    }

}
