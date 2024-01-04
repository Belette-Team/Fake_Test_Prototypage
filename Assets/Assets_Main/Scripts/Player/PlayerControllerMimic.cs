using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControllerMimic : MonoBehaviour
{
    Stats playerStats;
    GameObject projectilePrefab;
    GameObject collectiblePrefab;

    float fireRateTimer;
    
    //UI
    TMP_Text healthText;
    int maxHealth;
    

    // Start is called before the first frame update
    void Awake()
    {
        playerStats = GetComponent<Stats>();

        maxHealth = playerStats.playerHealthMax;
        projectilePrefab = GameManager.Instance.projectilePrefab;
        collectiblePrefab = GameManager.Instance.collectiblePrefab;
        healthText = GetComponentInChildren<TMP_Text>();
        //UpdateUIHealth();
    }

    // Update is called once per frame
    void Update()
    {
        //Automatically shoots
        AutomaticShooting();

        //Automatically gathers
        GatherCollectibles(playerStats.collectionRadius, LayerMask.GetMask(LayerMask.LayerToName(collectiblePrefab.layer)));

    }

    private void ShootInMultipleDirections()
    {
        for (int i = 0; i < playerStats.numberOfProjectiles; i++)
        {
            // Instantiating a ball in the right direction
            Vector3 direction = Quaternion.Euler(0, i * (360 / playerStats.numberOfProjectiles), 0) * Vector3.forward;
            //Debug.Log(" i = " + i + "   " + direction);

            // Add the player position
            Vector3 ballPosition = transform.position + direction.normalized;

            // Instantiate the ball
            GameObject newBall = Instantiate(projectilePrefab, ballPosition, Quaternion.LookRotation(direction, Vector3.up));

            //Give it its stats
            newBall.GetComponent<ProjectileController>().projectilePiercing = playerStats.projectilePiercing;
            newBall.GetComponent<ProjectileController>().projectileRange = playerStats.projectileRange;

            // Giving her the right direction and speed
            Rigidbody rb = newBall.GetComponent<Rigidbody>();
            rb.velocity = direction * playerStats.projectileSpeed;
        }
    }

    void AutomaticShooting()
    {
        if (fireRateTimer >= 0)
        {
            fireRateTimer -= Time.deltaTime;
        }
        else
        {
            ShootInMultipleDirections();
            fireRateTimer = playerStats.fireRate;
        }
    }

    public void GatherCollectibles(float radius, LayerMask layerMask)
    {
        //Find all the collectibles in a radius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, layerMask, QueryTriggerInteraction.Collide);

        //Check what collectible it is
        foreach (Collider collider in hitColliders)
        {
            if (collider.GetComponent<Collectible>() != null)
            {
                if (collider.GetComponent<Collectible>().collectibleType == CollectibleType.XpSmallCollectible || collider.GetComponent<Collectible>().collectibleType == CollectibleType.XpBigCollectible)
                {
                    collider.GetComponent<Collectible>().GetAttracted(transform);
                }
            }
        }
    }

    public void ReducePlayerHealth(int health)
    {
        playerStats.playerHealth -= health;
        //UpdateUIHealth();
        if (playerStats.playerHealth <= 0)
        {
            playerStats.playerHealth = 0;
            SelfDestroy();
        }
    }

    //void UpdateUIHealth()
    //{
    //    healthText.text = playerStats.playerHealth.ToString();
    //    Transform progress = null;
    //    foreach(Transform child in transform)
    //    {
    //        if(child.name == "Progress")
    //        {
    //            progress = child;
    //        }
    //        if(progress != null)
    //        {
    //            progress.GetComponent<Image>().fillAmount = playerStats.playerHealth / maxHealth;
    //        }
    //    }

    //}

    public void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
