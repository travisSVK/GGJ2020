using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private int m_amountToPool;
    [SerializeField] private GameObject m_objectToPool;

    private List<GameObject> m_inactiveObjects = new List<GameObject>();

    public void Initialize(int amountToPool, string name, GameObject prefab)
    {
        m_amountToPool = amountToPool;
        m_objectToPool = prefab;

        for (int i = 0; i < m_amountToPool; i++)
        {
            GameObject obj = Instantiate(m_objectToPool);
            obj.name = name + "_" + i;
            obj.transform.parent = transform;
            obj.SetActive(false);
            m_inactiveObjects.Add(obj);
        }
    }

    public void Kill(GameObject obj)
    {
        obj.SetActive(false);
        m_inactiveObjects.Add(obj);
    }

    public GameObject GetPooledObject()
    {
        if (m_inactiveObjects.Count == 0)
        {
            return null;
        }

        GameObject obj = m_inactiveObjects[m_inactiveObjects.Count - 1];
        m_inactiveObjects.RemoveAt(m_inactiveObjects.Count - 1);
        obj.SetActive(true);
        return obj;
    }
}
