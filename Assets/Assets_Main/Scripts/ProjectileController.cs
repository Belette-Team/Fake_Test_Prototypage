using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public int projectilePiercing;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ReducePiercing()
    {
        projectilePiercing--;
        if (projectilePiercing <= 0)
        {
            Destroy(gameObject);
        }
    }
}
