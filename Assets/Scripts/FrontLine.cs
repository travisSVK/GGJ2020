using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frontline : MonoBehaviour, ISelectable
{
    [SerializeField] private SpriteRenderer m_inactive = null;
    [SerializeField] private SpriteRenderer m_active = null;
    [SerializeField] private float speed = 0.2f;
    private Transform m_baseTile;
    private Transform m_enemyTile;
    private List<Pawn> m_pawns = new List<Pawn>();
    private int m_armyId;

    public int armyId
    {
        set { m_armyId = value; }
    }

    public void Attack()
    {
        Vector3 direction = (m_enemyTile.position - m_baseTile.position).normalized;
        float unitDistance = Grid.GetUnitDistance();
        List<Pawn> enemyPawns = Grid.GetEnemyPawns(m_enemyTile.position, m_armyId);
        foreach (Pawn pawn in m_pawns)
        {
            // if not in combat, check if any enemy pawn is standing infront
            if (!pawn.isInCombat)
            {
                GameObject gameObject = null;
                foreach (Pawn enemyPawn in enemyPawns)
                {
                    float distanceToEnemy;
                    if (direction.x != 0)
                    {
                        distanceToEnemy = Mathf.Abs(enemyPawn.transform.position.y - pawn.transform.position.y);
                    }
                    else
                    {
                        distanceToEnemy = Mathf.Abs(enemyPawn.transform.position.x - pawn.transform.position.x);
                        
                    }
                    if (unitDistance >= distanceToEnemy)
                    {
                        pawn.hasWonCombat = false;
                        // create combat instance
                        gameObject = new GameObject();
                        CombatInstance combatInstance = gameObject.AddComponent(typeof(CombatInstance)) as CombatInstance;
                        combatInstance.SetCombat(pawn, enemyPawn);
                        break;
                    }
                }
                // if not any enemy, advance forward, but only if the combat was won recently
                if (!gameObject && pawn.hasWonCombat)
                {
                    pawn.transform.position += direction * unitDistance * speed;
                }
            }
        }
    }

    public void OnBeginHover()
    {
        m_inactive.enabled = false;
        m_active.enabled = true;
    }

    public void OnEndHover()
    {
        m_inactive.enabled = true;
        m_active.enabled = false;
    }

    public void OnSelect()
    {

    }

    public void OnDeselect()
    {   
    }

    public void AddPawn(Pawn pawn)
    {
        m_pawns.Add(pawn);
    }

    public void SetTiles(Transform baseTile, Transform enemyTile)
    {
        m_baseTile = baseTile;
        m_enemyTile = enemyTile;
    }

    private void Awake()
    {
        m_inactive.enabled = true;
        m_active.enabled = false;
    }
}
