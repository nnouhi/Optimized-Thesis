using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject objectToPool;
    [SerializeField] private int amountToPool;
    public List<GameObject> pooledObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(transform.position);
        
        // Init the pool
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject gameObject = Instantiate(objectToPool);
            pooledObjects.Add(gameObject);
            pooledObjects[i].SetActive(false);
            pooledObjects[i].transform.parent = transform;
        }
    }

    public GameObject SpawnObject(Vector3 position, Quaternion rotation)
    {
        GameObject gameObjectToReturn;
        
        // If we still have objects in the pool don't create new ones
        if (pooledObjects.Count > 0)
        {
            gameObjectToReturn = pooledObjects[0];
            pooledObjects.RemoveAt(0);
        }
        else
        {
            GameObject gameObject = Instantiate(objectToPool);
            pooledObjects.Add(gameObject);
            pooledObjects[0].SetActive(false);
            pooledObjects[0].transform.parent = transform;
            gameObjectToReturn = pooledObjects[0];
            pooledObjects.RemoveAt(0);
            // gameObjectToReturn = Instantiate(objectToPool);
            // gameObjectToReturn.transform.parent = transform;
        }

        gameObjectToReturn.transform.position = position;
        gameObjectToReturn.transform.rotation = rotation;
        gameObjectToReturn.SetActive(true);

        return gameObjectToReturn;
    }

    public void ReturnObject(GameObject gameObjectToReturn)
    {
        // Add object back tot he pool
        pooledObjects.Add(gameObjectToReturn);
        gameObjectToReturn.SetActive(false);
    }
}
