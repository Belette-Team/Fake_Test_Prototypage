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
        float gatheredAmount = value;
        currentAmount -= value;
        float percentageLeft = currentAmount / baseAmount;
        if (percentageLeft < 0.04f)
        {
            gatheredAmount += (percentageLeft) * baseAmount;
            Destroy(gameObject);
        }
        else
        {
            ReduceSizeOfSurface(percentageLeft);
        }
        return gatheredAmount;
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
    private void ReduceSizeOfSurface(float newScale)
    {
        transform.localScale = new Vector3(initialScale.x * newScale, transform.localScale.y, initialScale.z * newScale);
    }

    void SelfDetroy()
    {
        Destroy(gameObject, 1);
    }


}
