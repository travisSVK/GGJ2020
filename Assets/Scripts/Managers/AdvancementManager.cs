using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancementManager : MonoBehaviour
{
    public class CombatResult
    {
        public GameObject obj;
        public float time;
    }

    [SerializeField] private GameObject m_breakthroughPrefab;
    [SerializeField] private GameObject m_defeatPrefab;

    private ObjectPool m_breakthroughs;
    private ObjectPool m_defeats;
    private List<CombatResult> m_activeBreakthroughs;
    private List<CombatResult> m_activeDefeats;

    public void NewBreakthough(Vector3 position)
    {
        GameObject obj = m_breakthroughs.GetPooledObject();
        if (obj != null)
        {
            obj.transform.position = position;
            CombatResult newBreakthough = new CombatResult();
            newBreakthough.obj = obj;
            newBreakthough.time = 3.0f;
            m_activeBreakthroughs.Add(newBreakthough);
        }
    }

    public void NewDefeat(Vector3 position)
    {
        GameObject obj = m_defeats.GetPooledObject();
        if (obj != null)
        {
            obj.transform.position = position;
            CombatResult newDefeat = new CombatResult();
            newDefeat.obj = obj;
            newDefeat.time = 3.0f;
            m_activeDefeats.Add(newDefeat);
        }
    }

    private void Awake()
    {
        GameObject breakthroughObj = new GameObject("Breakthroughs");
        m_breakthroughs = breakthroughObj.AddComponent<ObjectPool>();
        m_breakthroughs.Initialize(1000, "Breakthrough", m_breakthroughPrefab);

        GameObject defeatObj = new GameObject("Defeats");
        m_defeats = defeatObj.AddComponent<ObjectPool>();
        m_defeats.Initialize(1000, "Defeat", m_defeatPrefab);

        m_activeBreakthroughs = new List<CombatResult>();
        m_activeDefeats = new List<CombatResult>();
    }

    private void Update()
    {
        for (int i = m_activeBreakthroughs.Count - 1; i >= 0; --i)
        {
            m_activeBreakthroughs[i].time -= Time.deltaTime;
            if (m_activeBreakthroughs[i].time < 0.0f)
            {
                m_breakthroughs.Kill(m_activeBreakthroughs[i].obj);
                m_activeBreakthroughs.RemoveAt(i);
            }
        }

        for (int i = m_activeDefeats.Count - 1; i >= 0; --i)
        {
            m_activeDefeats[i].time -= Time.deltaTime;
            if (m_activeDefeats[i].time < 0.0f)
            {
                m_defeats.Kill(m_activeDefeats[i].obj);
                m_activeDefeats.RemoveAt(i);
            }
        }
    }
}
