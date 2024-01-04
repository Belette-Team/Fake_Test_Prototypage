using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public CollectibleType collectibleType;
    private bool isAttracted = false;
    private Transform playerTransform;
    private float speed;

    private void OnTriggerEnter(Collider other)
    {
        GetCollected(other.gameObject);
    }

    private void Start()
    {
        speed = GameManager.Instance.startingSpeed;
        if(collectibleType == CollectibleType.XpBigCollectible)
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
        if (collectibleType == CollectibleType.XpSmallCollectible)
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
    }

    private void Update()
    {
        if(isAttracted)
        {
            speed = GoToPlayer(speed);
        }
    }

    public void GetCollected(GameObject playerGameObject)
    {
        if(playerGameObject.GetComponent<PlayerController>() != null || playerGameObject.GetComponent<PlayerControllerMimic>() != null)
        {
            GameManager.Instance.playerController.AddExperience(collectibleType);
            Destroy(gameObject);
        }
    }

    public void GetAttracted(Transform player)
    {
        isAttracted = true;
        playerTransform = player;
    }

    private float GoToPlayer(float speed)
    {
        if (playerTransform != null)
        {
            transform.Translate((playerTransform.position - transform.position).normalized * speed * Time.deltaTime);
            return speed *= GameManager.Instance.scalingSpeed;
        }
        else
        {
            isAttracted = false;
            return 0;
        }
    }
}

