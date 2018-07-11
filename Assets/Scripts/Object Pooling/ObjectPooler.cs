using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{

    [System.Serializable]
    public class Pool
    {
        public string poolTag;
        public GameObject prefab;
        public int size;
    }


    #region Singleton
    public static ObjectPooler Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;


    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.poolTag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string poolTag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(poolTag))
        {
            Debug.LogWarning("Pool with tag " + poolTag + "doesn't exist.");
            return null;
        }
        GameObject objectToSpawn = poolDictionary[poolTag].Dequeue();
        objectToSpawn.SetActive(true);

        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        // TODO: Optimize this by getting rid of this call
        IPooledObjects pooledObject = objectToSpawn.GetComponent<IPooledObjects>();

        if (pooledObject != null)
        {
            pooledObject.OnObjectSpawn();
        }

        poolDictionary[poolTag].Enqueue(objectToSpawn); // add it back to the end of the poolDictionary

        return objectToSpawn;

    }

}
