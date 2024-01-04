using System.Collections;
using System.Collections.Generic;
using System.Resources;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceSurface : MonoBehaviour
{
    public ResourceType resourceType;
    ResourcesManager resourcesManager;
    public float baseAmount;
    float currentAmount;
    Vector3 initialScale;
    // Start is called before the first frame update
    void Awake()
    {
        currentAmount = baseAmount;
        initialScale = transform.localScale;
    }

    private void Start()
    {
        resourcesManager = GameManager.Instance.GetComponentInChildren<ResourcesManager>();
        AssignMaterialByType();


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GatherResource(float value)
    {
        float gatheredAmount;        
        currentAmount -= value;
        ReduceSizeOfSurface();

        if (currentAmount < 0)
        {
            gatheredAmount = value + currentAmount;
            currentAmount = 0;
            return gatheredAmount;
        }
        else
        {
            return value;
        }
    }

    void AssignMaterialByType()
    {
        switch (resourceType)
        {
            case ResourceType.Gold:
                {
                    GetComponent<MeshRenderer>().material = resourcesManager.goldMaterial;
                    break;
                }
            case ResourceType.Stone:
                {
                    GetComponent<MeshRenderer>().material = resourcesManager.stoneMaterial;
                    break;
                }   
            case ResourceType.Wood:
                {
                    GetComponent<MeshRenderer>().material = resourcesManager.woodMaterial;
                    break;
                }
        }
    }
    private void ReduceSizeOfSurface()
    {
        float newScale = currentAmount / baseAmount;
        transform.localScale = new Vector3(initialScale.x * newScale, transform.localScale.y, initialScale.z * newScale);
        if(newScale <=0.05f)
        {
            SelfDetroy();
        }
    }

    void SelfDetroy()
    {
        Destroy(gameObject, 1);
    }


}
