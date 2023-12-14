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
        player = GameObject.Find("Player");
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

    void LookAtPlayer ()
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
}
