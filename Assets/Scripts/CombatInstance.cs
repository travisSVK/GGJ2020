using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatInstance : MonoBehaviour
{
    [SerializeField] private float m_attackCooldown = 1.0f;
    private float m_currentCooldown = 0.0f;
    private Pawn m_pawn1;
    private Pawn m_pawn2;

    public void SetCombat(Pawn pawn1, Pawn pawn2)
    {
        m_pawn1 = pawn1;
        m_pawn2 = pawn2;
        m_pawn1.isInCombat = true;
        m_pawn2.isInCombat = true;
        m_pawn1.hasWonCombat = false;
        m_pawn2.hasWonCombat = false;
    }

    private void Update()
    {
        if (m_currentCooldown <= m_attackCooldown)
        {
            m_currentCooldown += Time.deltaTime;
        }
        else
        {
            if (m_pawn1.initiative > m_pawn2.initiative)
            {
                m_pawn1.Attack(m_pawn2);
            }
            else if (m_pawn1.initiative > m_pawn2.initiative)
            {
                m_pawn1.Attack(m_pawn2);
            }
            else
            {
                // dice roll
                int roll = Random.Range(0, 1);
                if (roll == 0)
                {
                    m_pawn1.Attack(m_pawn2);
                }
                else
                {
                    m_pawn2.Attack(m_pawn1);
                }
            }
        }
    }
}
