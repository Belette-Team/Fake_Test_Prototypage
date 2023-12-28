using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public int projectilePiercing;
    public float projectileRange;
    private float timeBeforeDestroy;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        float speed = rb.velocity.magnitude;
        timeBeforeDestroy = projectileRange / speed;
    }

    private void OnEnable()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        float speed = rb.velocity.magnitude;
        timeBeforeDestroy = projectileRange / speed;
    }

    // Update is called once per frame
    void Update()
    {
        if(timeBeforeDestroy <=0)
        {
            SelfDestroy();
        }
        else
        {
            timeBeforeDestroy -= Time.deltaTime;
        }
    }

    public void ReducePiercing()
    {
        projectilePiercing--;
        if (projectilePiercing <= 0)
        {
            SelfDestroy();
        }
    }

    private void SelfDestroy()
    {
        gameObject.SetActive(false);
    }
}
