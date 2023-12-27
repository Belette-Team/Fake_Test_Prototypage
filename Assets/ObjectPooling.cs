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
            GameObject obj = Instantiate(collectiblePrefab, transform.position, Quaternion.identity);
            obj.SetActive(false);
            collectiblePooledObjects.Add(obj);
        }
        for (int i = 0;i < enemyPoolSize; i++)
        {
            GameObject obj = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            obj.SetActive(false);
            enemyPooledObjects.Add(obj);
        }
        for (int i = 0; i<= projectilePoolSize; i++)
        {
            GameObject obj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            obj.SetActive(false);
            projectilePooledObjects.Add(obj);
        }
    }
    public GameObject GetPooledObject(List<GameObject> gameObjects, GameObject prefab, Vector3 position, Transform parent)
    { 
        for (int i = 0; i < gameObjects.Count; i++)
        {
            if (!gameObjects[i].activeInHierarchy)
            {
                gameObjects[i].transform.position = position;
                gameObjects[i].transform.parent = parent;
                Debug.Log("Pools" + prefab.name);
                return gameObjects[i];
            }
        }
        GameObject newObj = Instantiate(prefab, position, Quaternion.identity, parent);
        newObj.SetActive(false);
        gameObjects.Add(newObj);
        return newObj;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
