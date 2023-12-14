using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exemple : MonoBehaviour
{

    public int pointsDeVie;


    // Start is called before the first frame update
    void Start()
    {
        pointsDeVie = 100;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        if(pointsDeVie - damage <0)
        {
            pointsDeVie = 0;

        }
        else
        {
            pointsDeVie = pointsDeVie - damage;

        }
    }
}
