using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [Header("Starting Game Design")]
    public int numberOfProjectiles; // how many bullets you shoot
    public float fireRate; // shoots a bullet every "fireRate" seconds
    public int movementSpeed; // your move speed
    public float projectileSpeed; // your bullets speed
    public int projectilePiercing; // your bullets speed

    [Header("References")]
    public float fireRateScaling;

    [Header ("References")]
    public GameObject ballPrefab;



    private float fireRateTimer;
    private float horizontalMovement;
    private float verticalMovement;
    private Rigidbody rb3D;

    private void Awake()
    {
        rb3D = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Initialize values
        fireRateTimer = fireRate;
    }

    // Update is called once per frame
    void Update()
    {
        AutomaticShooting();

        if (Input.GetButton("SpecialLeft"))
        {
            // Do the things of special_left

        }

        if (Input.GetButtonDown("SpecialRight"))
        {
            // Do the things of special_right
        }

        // Get the Movement
        horizontalMovement = Input.GetAxis("Horizontal");
        verticalMovement = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        rb3D.velocity = new Vector3(projectileSpeed * horizontalMovement, rb3D.velocity.y, projectileSpeed * verticalMovement);
    }

    private void ShootInMultipleDirections()
    {
        for (int i = 0; i< numberOfProjectiles; i++)
        {
            // Instantiating a ball in the right direction
            Vector3 direction = Quaternion.Euler(0, i * (360/ numberOfProjectiles), 0) * Vector3.forward;
            //Debug.Log(" i = " + i + "   " + direction);

            // Add the player position
            Vector3 ballPosition = transform.position + direction.normalized;

            // Instantiate the ball
            GameObject newBall = Instantiate(ballPrefab, ballPosition, Quaternion.LookRotation(direction, Vector3.up));

            //Give it its stats
            newBall.GetComponent<ProjectileController>().projectilePiercing = projectilePiercing;

            // Giving her the right direction and speed
            Rigidbody rb = newBall.GetComponent<Rigidbody>();
            rb.velocity = direction * projectileSpeed;
        }
    }

    private void AutomaticShooting()
    {
        if (fireRateTimer >= 0)
        {
            fireRateTimer -= Time.deltaTime;
        }
        else
        {
            ShootInMultipleDirections();
            fireRateTimer = fireRate;
        }
    }

    public void IncreaseFireRate()
    {
        fireRate /= fireRateScaling;
    }

    public void IncreaseProjectilePiercing()
    {
        projectilePiercing++;
    }

    public void IncreaseProjectileNumber()
    {
        numberOfProjectiles++;
    }

}
