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

    [SerializeField] private GameObject m_child;

    private bool m_isInCombat = false;
    private bool m_hasWonCombat = false;

    private float m_hp;
    private float m_dmg;
    private float m_shieldHp;
    private float m_swordHp;
    private bool m_isMovingToFreePlace = false;
    private ArmyManager m_armyManager = null;

    private float m_initiative;

    private float m_idleProgress;
    private float m_randomIdleSpeed;

    private KillCallback m_killCallback = null;
    public delegate void KillCallback(GameObject obj);

    private RemoveFromTileCallback m_tileRemoveCallback = null;
    public delegate void RemoveFromTileCallback(Pawn pawn);

    private AssignToTileCallback m_assignToTileCallback = null;
    public delegate void AssignToTileCallback(Pawn pawn, int tilePositionX, int tilePositionY, int subTilePositionX, int subTilePositionY);

    public enum AttackOutcome { None, Death, Repair };


    public void SetAssignToTileCallback(AssignToTileCallback callback)
    {
        m_assignToTileCallback = callback;
    }

    public void SetKillCallback(KillCallback callback)
    {
        m_killCallback = callback;
    }

    public void SetRemoveFromTileCallback(RemoveFromTileCallback callback)
    {
        m_tileRemoveCallback = callback;
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

    public bool isMovingToFreePlace
    {
        get { return m_isMovingToFreePlace; }
        set { m_isMovingToFreePlace = value; }
    }

    public bool Attack(Pawn pawn)
    {
        bool pawnDied = false;
        if (m_swordHp > 0.0f)
        {
            AttackOutcome outcome = pawn.TakeDmg(m_dmg);
            m_swordHp -= m_dmg;
            if (m_swordHp <= 0.0f)
            {
                if (armyId == GameBoard.PLAYER_ARMY_ID)
                {
                    FindObjectOfType<AdvancementManager>().NewBrokenSword(transform.position);
                }

                m_swordHp = 0.0f;
                bool isForgeAvailable = m_armyManager.AskForRepair(this);
                if (isForgeAvailable)
                {
                m_isInCombat = false;
                if (outcome == AttackOutcome.None)
                {
                    m_assignToTileCallback(pawn, m_tilePositionX, m_tilePositionY, m_subTilePositionX, m_subTilePositionY);
                    pawn.m_hasWonCombat = true;
                }
                else
                {
                    m_tileRemoveCallback(this);
                    m_killCallback?.Invoke(pawn.gameObject);
                }
                m_killCallback?.Invoke(gameObject);
                return true;
                }
            }

            if (outcome == AttackOutcome.Death || outcome == AttackOutcome.Repair)
            {
                m_hasWonCombat = true;
                m_assignToTileCallback(this, pawn.m_tilePositionX, pawn.m_tilePositionY, pawn.m_subTilePositionX, pawn.m_subTilePositionY);
                if (outcome == AttackOutcome.Death)
                {
                    m_killCallback?.Invoke(pawn.gameObject);
                }
                pawnDied = true;
            }
        }
        return pawnDied;
    }

    public AttackOutcome TakeDmg(float dmg)
    {
        m_shieldHp -= dmg;
        if (m_shieldHp < 0.0f)
        {
            m_hp += m_shieldHp;
            m_shieldHp = 0.0f;
            if (m_hp <= 0.0f)
            {
                //m_isInCombat = false;
                //m_killCallback?.Invoke(gameObject);
                if (armyId == GameBoard.PLAYER_ARMY_ID)
                {
                    FindObjectOfType<AdvancementManager>().NewDefeat(transform.position);
                }
                else
                {
                    FindObjectOfType<AdvancementManager>().NewBreakthough(transform.position);
                }
                //return AttackOutcome.Death;
            }

            if (armyId == GameBoard.PLAYER_ARMY_ID)
            {
                FindObjectOfType<AdvancementManager>().NewBrokenShield(transform.position);
            }

            bool isForgeAvailable = m_armyManager.AskForRepair(this);
            if (isForgeAvailable)
            {
                m_isInCombat = false;
                m_tileRemoveCallback(this);
                return AttackOutcome.Repair;
            }
            m_tileRemoveCallback(this);
            m_isInCombat = false;
            return AttackOutcome.Death;
        }
        return AttackOutcome.None;
    }

    public void SpawnPawn(int armyIdSet, int tilePositionX, int tilePositionY, int subTilePositionX, int subTilePositionY)
    {
        armyId = armyIdSet;
        m_hp = Random.Range(3.0f, 5.0f);
        m_dmg = Random.Range(3.0f, 5.0f);
        m_shieldHp = Random.Range(3.0f, 5.0f);
        m_swordHp = Random.Range(5.0f, 7.0f);
        m_initiative = Random.Range(1.0f, 5.0f);

        m_tilePositionX = tilePositionX;
        m_tilePositionY = tilePositionY;
        m_subTilePositionX = subTilePositionX;
        m_subTilePositionY = subTilePositionY;

        m_idleProgress = Random.Range(0.0f, 1.0f);
        m_randomIdleSpeed = Random.Range(10.0f, 100.0f);
    }

    private void Start()
    {
        m_armyManager = FindObjectOfType<ArmyManager>();
    }

    private void Update()
    {
        m_idleProgress += Time.deltaTime * m_randomIdleSpeed;
        m_child.transform.localPosition = Vector3.up * 0.02f * Mathf.Sin(m_idleProgress);
    }

    private void FindBuilding()
    {

    }
}

public struct FindBuilding
{

}
