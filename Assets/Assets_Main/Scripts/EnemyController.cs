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
        // Do animations
        Destroy(gameObject);
    }

    void KillPlayer()
    {
        GameManager.Instance.PlayerLost();
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
            // The player loses
            KillPlayer();
        }
    }
}
