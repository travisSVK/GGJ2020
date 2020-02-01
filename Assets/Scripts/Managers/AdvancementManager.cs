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
    [SerializeField] private GameObject m_brokenSwordPrefab;
    [SerializeField] private GameObject m_brokenShieldPrefab;


    private ObjectPool m_breakthroughs;
    private ObjectPool m_defeats;
    private ObjectPool m_brokenShields;
    private ObjectPool m_brokenSwords;

    private List<CombatResult> m_activeBreakthroughs;
    private List<CombatResult> m_activeDefeats;
    private List<CombatResult> m_activeBrokenSwords;
    private List<CombatResult> m_activeBrokenShields;

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

    public void NewBrokenSword(Vector3 position)
    {
        GameObject obj = m_brokenSwords.GetPooledObject();
        if (obj != null)
        {
            obj.transform.position = position;
            CombatResult newBrokenSword = new CombatResult();
            newBrokenSword.obj = obj;
            newBrokenSword.time = 3.0f;
            m_activeBrokenSwords.Add(newBrokenSword);
        }
    }

    public void NewBrokenShield(Vector3 position)
    {
        GameObject obj = m_brokenShields.GetPooledObject();
        if (obj != null)
        {
            obj.transform.position = position;
            CombatResult newBrokenShield = new CombatResult();
            newBrokenShield.obj = obj;
            newBrokenShield.time = 3.0f;
            m_activeBrokenShields.Add(newBrokenShield);
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

        GameObject brokenSwordsObj = new GameObject("BrokenSwords");
        m_brokenSwords = brokenSwordsObj.AddComponent<ObjectPool>();
        m_brokenSwords.Initialize(1000, "BrokenSword", m_brokenSwordPrefab);

        GameObject brokenShieldsObj = new GameObject("BrokenShields");
        m_brokenShields = brokenShieldsObj.AddComponent<ObjectPool>();
        m_brokenShields.Initialize(1000, "BrokenShield", m_brokenShieldPrefab);

        m_activeBreakthroughs = new List<CombatResult>();
        m_activeDefeats = new List<CombatResult>();
        m_activeBrokenSwords = new List<CombatResult>();
        m_activeBrokenShields = new List<CombatResult>();
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

        for (int i = m_activeBrokenSwords.Count - 1; i >= 0; --i)
        {
            m_activeBrokenSwords[i].time -= Time.deltaTime;
            if (m_activeBrokenSwords[i].time < 0.0f)
            {
                m_brokenSwords.Kill(m_activeBrokenSwords[i].obj);
                m_activeBrokenSwords.RemoveAt(i);
            }
        }

        for (int i = m_activeBrokenShields.Count - 1; i >= 0; --i)
        {
            m_activeBrokenShields[i].time -= Time.deltaTime;
            if (m_activeBrokenShields[i].time < 0.0f)
            {
                m_brokenShields.Kill(m_activeBrokenShields[i].obj);
                m_activeBrokenShields.RemoveAt(i);
            }
        }
    }
}
