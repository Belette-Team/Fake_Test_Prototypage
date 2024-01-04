using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    public Transform groundTransform;
    public Transform player;
    public GameObject enemyPrefab;

    [Header("Game Design")]

    public float spawningTime;
    private float currentSpawningTime;
    public float minDistanceFromEnemy;
    public float maxDistanceFromEnemy;
    public float wallMargin;
    private Renderer groundRenderer;
    private ObjectPooling objectPooling;

    private void Awake()
    {
        //Get the address of the ground Renderer
        groundRenderer = groundTransform.GetComponent<Renderer>();
        objectPooling = transform.parent.GetComponent<ObjectPooling>();

        //Get an error if the min distance is higher than the max distance
        if (minDistanceFromEnemy >= maxDistanceFromEnemy)
        {
            maxDistanceFromEnemy = minDistanceFromEnemy + 1;
            Debug.Log("ERROR : Minimum Distance is higher than the maximum distance ");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        EnemySpawnAtRate();
    }

    void SpawnEnemies()
    {
        //calculate the position within the arena
        float randomPosX = Random.Range(groundRenderer.bounds.min.x + wallMargin, groundRenderer.bounds.max.x - wallMargin);
        float randomPosZ = Random.Range(groundRenderer.bounds.min.z + wallMargin, groundRenderer.bounds.max.z - wallMargin);
        Vector3 pos = new Vector3(randomPosX, player.position.y, randomPosZ);

        //Check if the enemy position is further than the minimum distance, and closer than the maximum distance. 
        while (Vector3.Distance(pos,player.transform.position) < minDistanceFromEnemy || (Vector3.Distance(pos,player.transform.position) > maxDistanceFromEnemy))
        {
            randomPosX = Random.Range(groundRenderer.bounds.min.x + wallMargin, groundRenderer.bounds.max.x - wallMargin);
            randomPosZ = Random.Range(groundRenderer.bounds.min.z + wallMargin, groundRenderer.bounds.max.z - wallMargin);
            pos = new Vector3(randomPosX, player.position.y, randomPosZ);
        }

        //Spawn the enemy and assign the EnemyManager to be his parent
        objectPooling.GetPooledObject(objectPooling.enemyPooledObjects, objectPooling.enemyPrefab, pos, transform).SetActive(true);
    }
    void EnemySpawnAtRate()
    {
        if (currentSpawningTime > spawningTime)
        {
            SpawnEnemies();
            currentSpawningTime = 0;
        }
        else
        {
            currentSpawningTime += Time.deltaTime;
        }
    }

    public void IncreaseEnemySpawnRate(float rate)
    {
        spawningTime /= rate;
    }
}
