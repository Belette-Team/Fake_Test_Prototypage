using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{

    public int speed;
    public GameObject ballPrefab;
    public GameObject mouth;
    public float fireRate; // shoots a bullet every "fireRate" seconds
    private float currentFireRate;

    private Camera mainCamera;
    private float cameraPlayerDistance;

    public float projectileSpeed;

    private int direction;
    private float horizontalMovement;
    private float verticalMovement;
    private Rigidbody rb3D;

    private void Awake()
    {
        mainCamera = Camera.main;
        rb3D = GetComponent<Rigidbody>();

    }

    // Start is called before the first frame update
    void Start()
    {
        // Initialize values
        currentFireRate = 0;
        cameraPlayerDistance = transform.position.x - mainCamera.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space)) 

            // Get the Movement
        horizontalMovement = Input.GetAxis("Horizontal");
        verticalMovement = Input.GetAxis("Vertical");

        if(Input.GetButton("SpecialLeft"))
        {
            if(currentFireRate < fireRate)
            {
                currentFireRate += Time.deltaTime;
            }
            else
            {
                Shoot();
                currentFireRate = 0;
            }
        }

        if (Input.GetButtonDown("SpecialRight"))
        {
            // Do the things of special1
        }

        UpdateCameraPosition();
        LookAtMouse();
    }

    private void FixedUpdate()
    {
        rb3D.velocity = new Vector3(speed * verticalMovement * Time.deltaTime, rb3D.velocity.y, speed * -horizontalMovement * Time.deltaTime);
    }

    private void Shoot()
    {

        GameObject newBall = Instantiate(ballPrefab, mouth.transform.position, Quaternion.identity);
        Vector3 shootingDirection = transform.forward;
        Rigidbody rb = newBall.GetComponent<Rigidbody>();
        Debug.Log(mouth.transform.forward);
        rb.AddForce(shootingDirection * projectileSpeed, ForceMode.Impulse);
        LookAtMouse();
    }

    private void UpdateCameraPosition()
    {
        mainCamera.transform.position = new Vector3(transform.position.x - cameraPlayerDistance, mainCamera.transform.position.y, mainCamera.transform.position.z);
    }

    private void LookAtMouse()
    {
        Vector3 direction = new Vector3(0,0,1);

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            if (raycastHit.transform.tag == "Ground")
            {
                direction = raycastHit.point;
                Debug.Log(raycastHit.collider.gameObject.name);
            }
        }
        Debug.Log(Vector3.Angle(new Vector3(0,0,1), direction));
        direction = new Vector3 (direction.x, transform.position.y, direction.z);
        //Instantiate(ballPrefab,direction,Quaternion.identity);
        transform.LookAt(direction);
    }

    Vector3 MousePositionOnCursor()
    {
        //transform.position
        return transform.position;
    }
}
