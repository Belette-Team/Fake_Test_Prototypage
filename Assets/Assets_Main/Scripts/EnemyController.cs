using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    GameObject player;
    Rigidbody rb;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        player = transform.parent.GetComponent<EnemySpawner>().player.gameObject;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Vector3 FindPlayerDirection()
    {
        Vector3 direction = player.transform.position - transform.position;
        direction = direction.normalized;
        return direction;
    }

    void LookAtPlayer()
    {
        Quaternion rotation = Quaternion.LookRotation(FindPlayerDirection(), Vector3.up);
        transform.rotation = rotation;
    }

    private void FixedUpdate()
    {
        // Move the enemy in the direction of the player at a regular speed

        rb.velocity = FindPlayerDirection() * speed * Time.deltaTime;

        LookAtPlayer();
    }

    void SelfDestroy()
    {
        if(GetComponent<DropCollectible>()!=null)
        {
            float rand = Random.Range(0, GameManager.Instance.XpSmallOdds + GameManager.Instance.XpBigOdds);
            if(rand < GameManager.Instance.XpSmallOdds)
            {
                GetComponent<DropCollectible>().DropCollectibleOnPosition(transform.position, CollectibleType.XpSmallCollectible);
            }
            else
            {
                GetComponent<DropCollectible>().DropCollectibleOnPosition(transform.position, CollectibleType.XpBigCollectible);
            }
        }
        // Do animations
        gameObject.SetActive(false);
    }

    // Detects bullets and die from them
    private void OnTriggerEnter(Collider other)
    {
        // if hits a trigger with the layer of the player, destroy
        if (other.gameObject.layer == player.layer)
        {
            other.GetComponent<ProjectileController>().ReducePiercing();
            GameManager.Instance.AddScore(1); 

            SelfDestroy();
        }
    }

    // Detects Player and kills him.
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == player.layer)
        { 
            //It's the player
            if(collision.gameObject.GetComponent<PlayerController>() != null) 
            {
                collision.gameObject.GetComponent<PlayerController>().ReducePlayerHealth(1);
                SelfDestroy();
            }

            //It's a mimic
            if (collision.gameObject.GetComponent<PlayerControllerMimic>() != null)
            {
                collision.gameObject.GetComponent<PlayerControllerMimic>().ReducePlayerHealth(1);
                SelfDestroy();
            }
        }
    }
}
