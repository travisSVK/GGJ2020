using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private int m_amountToPool;
    [SerializeField] private GameObject m_objectToPool;
    
    private List<GameObject> m_pooledObjects = new List<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < m_amountToPool; i++)
        {
            GameObject obj = (GameObject)Instantiate(m_objectToPool);
            obj.SetActive(false);
            m_pooledObjects.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < m_pooledObjects.Count; i++)
        {
            if (!m_pooledObjects[i].activeInHierarchy)
            {
                m_pooledObjects[i].SetActive(true);
                return m_pooledObjects[i];
            }
        }
        return null;
    }


    public List<GameObject> GetPooledObjects(int count)
    {
        List<GameObject> objects = new List<GameObject>(); 
        for (int i = 0; i < m_pooledObjects.Count; i++)
        {
            if (!m_pooledObjects[i].activeInHierarchy)
            {
                m_pooledObjects[i].SetActive(true);
                objects.Add(m_pooledObjects[i]);
                if (count == objects.Count)
                {
                    break;
                }
            }
        }
        return objects;
    }
}
