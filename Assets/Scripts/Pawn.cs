using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    public int armyId = 0;
    private bool m_isInCombat = false;
    private bool m_hasWonCombat = false;

    private float m_hp;
    private float m_dmg;
    private float m_shieldHp;
    private float m_swordHp;
    private float m_initiative;

    public float initiative
    {
        get { return m_initiative; }
    }

    public bool isInCombat
    {
        get { return m_isInCombat; }
        set { m_isInCombat = value; }
    }

    public bool hasWonCombat
    {
        get { return m_hasWonCombat; }
        set { m_hasWonCombat = value; }
    }

    public void Attack(Pawn pawn)
    {
        pawn.TakeDmg(m_dmg);
        m_swordHp -= m_dmg;
        if (m_swordHp <= 0.0f)
        {
            m_swordHp = 0.0f;
            // TODO go repair the sword
        }
    }

    public void TakeDmg(float dmg)
    {
        m_shieldHp -= dmg;
        if (m_shieldHp < 0.0f)
        {
            m_hp += m_shieldHp;
            m_shieldHp = 0.0f;
            if (m_hp <= 0.0f)
            {
                gameObject.SetActive(false);
                return;
            }
            // TODO go repair the shield
        }
    }

    public void SpawnPawn()
    {
        m_hp = Random.Range(3.0f, 5.0f);
        m_dmg = Random.Range(3.0f, 5.0f);
        m_shieldHp = Random.Range(3.0f, 5.0f);
        m_swordHp = Random.Range(5.0f, 7.0f);
        m_initiative = Random.Range(1.0f, 5.0f);
    }

    private void Start()
    {

    }

    private void Update()
    {

    }

    private void FindBuilding()
    {

    }
}

public struct FindBuilding
{

}
