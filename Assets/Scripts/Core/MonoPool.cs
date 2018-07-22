using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoPool : MonoBehaviour
{

    [SerializeField]
    GameObject m_prefab = null;
    [SerializeField]
    int m_capacity = 0;

    private GameObject[] m_pool = null;
    private int m_usedCount = 0;

    private void Awake()
    {
        m_usedCount = 0;
        Debug.Assert(m_capacity > 0);
        Debug.Assert(m_prefab != null);
        m_pool = new GameObject[m_capacity];
        for (int i = 0; i < m_capacity; ++i)
        {
            m_pool[i] = Instantiate(m_prefab, this.transform);
            m_pool[i].SetActive(false);
        }
    }

    public GameObject Request(Transform parent = null, bool activate = true)
    {
        if (m_usedCount >= m_capacity)
        {
            Debug.Log("Total capacity of the pool reached");
            return null;
        }

        GameObject item = m_pool[m_usedCount];
        item.transform.SetParent(parent, false);
        item.transform.localPosition = Vector3.zero;
        item.transform.localScale = Vector3.one;
        item.transform.localRotation = Quaternion.identity;
        item.SetActive(activate);
        ++m_usedCount;
        return item;
    }

    public void Return(GameObject item)
    {
        if (item == null)
            return;

        for (int i = 0; i < m_usedCount; ++i)
        {
            if (m_pool[i] == item)
            {
                item.SetActive(false);
                item.transform.SetParent(this.transform, false);
                m_pool[i] = m_pool[m_usedCount - 1];
                m_pool[m_usedCount - 1] = item;
                --m_usedCount;
                return;
            }
        }
        Debug.Assert(false, "Something went wrog in returnin. Perhaps Multiple Returns");
    }
}