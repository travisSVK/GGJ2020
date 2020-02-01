using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    public int armyId = 0;
    public int m_tilePositionX;
    public int m_tilePositionY;
    public int m_subTilePositionX;
    public int m_subTilePositionY;

    private bool m_isInCombat = false;
    private bool m_hasWonCombat = false;

    private float m_hp;
    private float m_dmg;
    private float m_shieldHp;
    private float m_swordHp;
    
    private float m_initiative;

    private KillCallback m_killCallback = null;
    public delegate void KillCallback(GameObject obj);

    public void SetKillCallback(KillCallback callback)
    {
        m_killCallback = callback;
    }

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

    public bool Attack(Pawn pawn)
    {
        bool pawnDied = pawn.TakeDmg(m_dmg);
        m_swordHp -= m_dmg;
        if (m_swordHp <= 0.0f)
        {
            m_swordHp = 0.0f;
            // TODO go repair the sword
        }
        if (pawnDied)
        {
            m_hasWonCombat = true;
        }
        return pawnDied;
    }

    public bool TakeDmg(float dmg)
    {
        m_shieldHp -= dmg;
        if (m_shieldHp < 0.0f)
        {
            m_hp += m_shieldHp;
            m_shieldHp = 0.0f;
            if (m_hp <= 0.0f)
            {
                m_isInCombat = false;
                m_killCallback?.Invoke(gameObject);
                return true;
            }
            // TODO go repair the shield
            //return true;
        }
        return false;
    }

    public void SpawnPawn(int tilePositionX, int tilePositionY, int subTilePositionX, int subTilePositionY)
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
