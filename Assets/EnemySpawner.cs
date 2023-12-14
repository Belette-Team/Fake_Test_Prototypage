using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    public Transform groundTransform;
    public Transform player;
    public GameObject enemyPrefab;

    [Header("Game Design")]

    public float spawningTime;
    private float currentSpawningTime;
    public float minimumDistance;
    public float wallMargin;
    private Renderer groundRenderer;
    private Bounds bounds;

    private void Awake()
    {
        groundRenderer = groundTransform.GetComponent<Renderer>();
        bounds = groundRenderer.bounds;

        //Debug.Log("Extents = " + bounds.extents);
        //Debug.Log("Center = " + bounds.center);
        //Debug.Log("Min = " + bounds.min);
        //Debug.Log("Max = " + bounds.max);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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

    void SpawnEnemies()
    {
        int[] randomNumbers = {-1,1};

        //Calculate the position of the enemy: 1. make him far from our player

        float spawningPosX = player.position.x + randomNumbers[Random.Range(0, 2)] * Random.Range(minimumDistance, bounds.size.x/2);
        float spawningPosZ = player.position.z + randomNumbers[Random.Range(0, 2)] * Random.Range(minimumDistance, bounds.size.x/2);

        //Clamp the values within the arena bounds
        spawningPosX = Mathf.Clamp(spawningPosX, bounds.min.x + wallMargin, bounds.max.x - wallMargin);
        spawningPosZ = Mathf.Clamp(spawningPosZ, bounds.min.z + wallMargin, bounds.max.z - wallMargin);

        //Calculate the position of the enemy: 
        Vector3 pos = new Vector3(spawningPosX, player.position.y, spawningPosZ);

        //Spawn the enemy
        Instantiate(enemyPrefab, pos, Quaternion.identity, transform);
    }
}
