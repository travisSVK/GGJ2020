using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private GameObject m_combatPrefab = null;
    [SerializeField] private float m_speed = 0.2f;
    private ObjectPool m_combatPool = null;

    public int armyOwnerId = 0;
    public Tile up = null;
    public Tile down = null;
    public Tile left = null;
    public Tile right = null;
    public Pawn[, ] m_pawns;
    
    public List<Pawn> GetEnemyPawns(int id)
    {
        List<Pawn> enemyPawns = new List<Pawn>();
        for (int i = 0, k = 0; i < m_pawns.GetLength(0); ++i)
        {
            for (int j = 0; j < m_pawns.GetLength(1); ++j, k++)
            {
                Pawn pawn = m_pawns[i, j];
                if (pawn != null && pawn.armyId != id)
                {
                    enemyPawns.Add(pawn);
                }
            }
        }

        return enemyPawns;
    }

    float GetDistanceToEnemy(Pawn pawn, Vector3 pawnDirection, Pawn enemyPawn)
    {
        float distanceToEnemy;
        if (pawnDirection.y != 0)
        {
            distanceToEnemy = Mathf.Abs(enemyPawn.transform.position.y - pawn.transform.position.y);
        }
        else
        {
            distanceToEnemy = Mathf.Abs(enemyPawn.transform.position.x - pawn.transform.position.x);
        }
        return distanceToEnemy;
    }

    // TODO: if time left, right now we just check up or down based on army ID
    // we can make it more sofisticated if time 
    private Vector3 GetPawnDirection(Pawn pawn)
    {
        Vector3 direction;
        if (pawn.armyId == 0)
        {
            direction = new Vector3(0.0f, 1.0f, 0.0f);
        }
        else
        {
            direction = new Vector3(0.0f, -1.0f, 0.0f);
        }
        return direction;
    }

    private Pawn GetPawnInFrontOf(Pawn pawn, Vector3 direction, Pawn[,] pawnsToCheck)
    {
        Pawn inFrontOfPawn = null;
        float unitDistance = GameBoard.GetUnitDistance();

        foreach (Pawn anotherPawn in pawnsToCheck)
        {
            float distanceToPawn = 0.0f;
            if (direction.x != 0)
            {
                distanceToPawn = Mathf.Abs(anotherPawn.transform.position.y - pawn.transform.position.y);
            }
            else
            {
                distanceToPawn = Mathf.Abs(anotherPawn.transform.position.x - pawn.transform.position.x);

            }
            if (unitDistance >= distanceToPawn)
            {
                inFrontOfPawn = anotherPawn;
                break;
            }
        }

        return inFrontOfPawn;
    }

    private Tile GetTileInDirection(Vector3 direction)
    {
        Tile tile = null;
        if (direction.x != 0.0f)
        {
            if (direction.x < 0.0f)
            {
                return left;
            }
            return right;
        }
        if (direction.y != 0.0f)
        {
            if (direction.y < 0.0f)
            {
                return down;
            }
            return up;
        }
        return tile;
    }

    private void Update()
    {
        float unitDistance = GameBoard.GetUnitDistance();

        foreach (Pawn pawn in m_pawns)
        {
            // check pawn in front in current tile or the tile in the direction of attack/move
            Vector3 direction = GetPawnDirection(pawn);

            // if not in combat, check if any enemy pawn is standing infront
            if (!pawn.isInCombat)
            {
                Pawn inFrontOfPawn = GetPawnInFrontOf(pawn, direction, m_pawns);
                if (!inFrontOfPawn)
                {
                    // try to get from the next tile in that direction
                    Tile tile = GetTileInDirection(direction);
                    inFrontOfPawn = GetPawnInFrontOf(pawn, direction, tile.m_pawns);
                }
                // check if its enemy or not
                if (inFrontOfPawn)
                {
                    if (inFrontOfPawn.armyId == pawn.armyId)
                    {
                        // nothing to do
                        continue;
                    }
                    float distanceToEnemy = GetDistanceToEnemy(pawn, direction, inFrontOfPawn);
                    if (unitDistance >= distanceToEnemy)
                    {
                        // create combat instance
                        GameObject gameObject = m_combatPool.GetPooledObject();
                        CombatInstance combatInstance = gameObject.GetComponent(typeof(CombatInstance)) as CombatInstance;
                        combatInstance.SetCombat(pawn, inFrontOfPawn);
                        continue;
                    }
                }
                // if not any enemy, advance forward, but only if the combat was won recently
                if (!gameObject && pawn.hasWonCombat)
                {
                    pawn.transform.position += direction * unitDistance * m_speed;
                }
            }
            else
            {
                // still in combat, lets check if already won
                if (pawn.hasWonCombat)
                {
                    Pawn inFrontOfPawn = GetPawnInFrontOf(pawn, direction, m_pawns);
                    if (!inFrontOfPawn)
                    {
                        // try to get from the next tile in that direction
                        Tile tile = GetTileInDirection(direction);
                        inFrontOfPawn = GetPawnInFrontOf(pawn, direction, tile.m_pawns);
                    }
                    if (inFrontOfPawn)
                    {
                        pawn.hasWonCombat = false;
                        pawn.isInCombat = false;
                    }
                    else
                    {
                        pawn.transform.position += direction * unitDistance * m_speed;
                    }
                }
            }
        }
    }

    private void Awake()
    {
        m_pawns = new Pawn[5, 5];
        GameObject pawnPoolObj = new GameObject("CombatPool");
        m_combatPool = pawnPoolObj.AddComponent<ObjectPool>();
        m_combatPool.Initialize(10000, "CombatInstance", m_combatPrefab);
    }
}
