using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPooling : MonoBehaviour
{
    //collectibles
    public List<GameObject> collectiblePooledObjects = new List<GameObject>();
    public GameObject collectiblePrefab;
    [SerializeField] public int collectiblePoolSize;

    //enemies
    public List<GameObject> enemyPooledObjects = new List<GameObject>();
    public GameObject enemyPrefab;
    [SerializeField] public int enemyPoolSize;

    //projectiles
    public List<GameObject> projectilePooledObjects = new List<GameObject>();
    public GameObject projectilePrefab;
    [SerializeField] public int projectilePoolSize;


    // Start is called before the first frame update
    void Start()
    {
        InitializePools();
    }

    void InitializePools()
    {
        for (int i = 0; i < collectiblePoolSize; i++)
        {
            GameObject col = Instantiate(collectiblePrefab, transform.position, Quaternion.identity);
            col.SetActive(false);
            collectiblePooledObjects.Add(col);
        }
        for (int i = 0;i < enemyPoolSize; i++)
        {
            GameObject ene = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            ene.SetActive(false);
            enemyPooledObjects.Add(ene);
        }
        for (int i = 0; i< projectilePoolSize; i++)
        {
            GameObject pro = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            pro.SetActive(false);
            projectilePooledObjects.Add(pro);
        }
    }
    public GameObject GetPooledObject(List<GameObject> gameObjects, GameObject prefab, Vector3 position, Transform parent)
    { 
        for (int i = 0; i < gameObjects.Count; i++)
        {
            if (gameObjects[i] != null)
            {
                if (!gameObjects[i].activeInHierarchy)
                {
                    gameObjects[i].transform.position = position;
                    gameObjects[i].transform.parent = parent;
                    return gameObjects[i];
                }
            }
        }
        GameObject newObj = Instantiate(prefab, position, Quaternion.identity, parent);
        newObj.SetActive(false);
        gameObjects.Add(newObj);
        return newObj;
    }

    public GameObject GetPooledObject(List<GameObject> gameObjects, GameObject prefab, Vector3 position)
    {
        for (int i = 0; i < gameObjects.Count; i++)
        {
            if (gameObjects[i] != null)
            {
                if (!gameObjects[i].activeInHierarchy)
                {
                    gameObjects[i].transform.position = position;
                    return gameObjects[i];
                }
            }
        }
        GameObject newObj = Instantiate(prefab, position, Quaternion.identity);
        Debug.Log("Created new " + prefab.name);
        newObj.SetActive(false);
        gameObjects.Add(newObj);
        return newObj;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
