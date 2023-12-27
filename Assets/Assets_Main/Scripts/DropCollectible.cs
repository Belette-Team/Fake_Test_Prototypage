using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public enum CollectibleType
{
    XpSmallCollectible,
    XpBigCollectible,
    HealthSmallCollectible,
    HealthBigCollectible,
}

public class DropCollectible : MonoBehaviour
{
    public GameObject collectiblePrefab;
    public CollectibleType collectibleType;
    private ObjectPooling objectPooling;

    private void Start()
    {
        objectPooling = GameManager.Instance.GetComponent<ObjectPooling>();
    }
    public void DropCollectibleOnPosition(Vector3 position, CollectibleType p_collectibleType)
    {
        GameObject collectible = objectPooling.GetPooledObject(objectPooling.collectiblePooledObjects, objectPooling.collectiblePrefab, position, GameManager.Instance.gameObject.transform);
        collectible.SetActive(true);

        if (collectible.GetComponent<Collectible>() !=null)
        {
            collectible.GetComponent<Collectible>().collectibleType = p_collectibleType;
        }

        Invoke("SelfDestroy",20);
    }

    void SelfDestroy()
    {
        gameObject.SetActive(false);
    }
}