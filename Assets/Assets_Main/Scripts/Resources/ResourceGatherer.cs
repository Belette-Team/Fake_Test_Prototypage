using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class ResourceGatherer : MonoBehaviour
{
    // Start is called before the first frame update
    Stats playerStats;
    public LayerMask resourceLayerMask;
    ResourcesManager resourcesManager;
    public float gatherSpeed;

    void Awake()
    {
    }

    private void Start()
    {
        playerStats = GetComponent<Stats>();
        resourcesManager = GameManager.Instance.gameObject.GetComponentInChildren<ResourcesManager>();
        StartCoroutine(GatherResourcesFromSurface());

    }
    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator GatherResourcesFromSurface()
    {
        while(true)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, playerStats.collectionRadius, resourceLayerMask, QueryTriggerInteraction.Collide);
            if (hitColliders.Length != 0)
            {
                foreach (Collider collider in hitColliders)
                {
                    if (collider.GetComponent<ResourceSurface>() != null)
                    {
                        ResourceType _r = collider.GetComponent<ResourceSurface>().resourceType;
                        float value = collider.GetComponent<ResourceSurface>().GatherResource(gatherSpeed);

                        resourcesManager.AddResource(_r, value);
                    }
                }
            }
            yield return new WaitForSeconds(.1f);
        }
    }

}
