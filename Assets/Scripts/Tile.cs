using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    
    [SerializeField] private float m_speed = 0.2f;
    private GameBoard m_gameBoard;

    public int armyOwnerId = 0;
    public Tile up = null;
    public Tile down = null;
    public Tile left = null;
    public Tile right = null;
    public Pawn[, ] m_pawns;

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
        float unitDistance = m_gameBoard.GetUnitDistance();

        foreach (Pawn anotherPawn in pawnsToCheck)
        {
            if (!anotherPawn || (pawn == anotherPawn))
            {
                continue;
            }
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
        if (!m_gameBoard)
        {
            m_gameBoard = GetComponentInParent(typeof(GameBoard)) as GameBoard;
        }
        float unitDistance = m_gameBoard.GetUnitDistance();

        foreach (Pawn pawn in m_pawns)
        {
            if (pawn)
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
                        if (tile)
                        {
                            inFrontOfPawn = GetPawnInFrontOf(pawn, direction, tile.m_pawns);
                        }
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
                            GameObject gameObject = m_gameBoard.GetCombat();
                            CombatInstance combatInstance = gameObject.GetComponent(typeof(CombatInstance)) as CombatInstance;
                            combatInstance.SetCombat(pawn, inFrontOfPawn);
                            combatInstance.SetKillCallback(m_gameBoard.KillCombat);
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
    }

    private void Awake()
    {
        m_pawns = new Pawn[5, 5];
    }
}
