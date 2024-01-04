using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTypes : MonoBehaviour
{
    public int playerHealth;
    public int playerHealthMax;
    public GameObject healthCollectible;
    
    // Start is called before the first frame update

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == healthCollectible.layer)
        {
            if (playerHealth < playerHealthMax)
            {
                Destroy(other);
                if (playerHealth + 10 < playerHealthMax)
                {
                    playerHealth = playerHealth + 10;
                }
                else
                {
                    playerHealth = playerHealthMax;
                }
            }
        }
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
