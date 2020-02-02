using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private float m_speed = 0.05f;
    private GameBoard m_gameBoard;

    public int armyOwnerId = 0;
    public Tile up = null;
    public Tile down = null;
    public Tile left = null;
    public Tile right = null;
    public Pawn[,] m_pawns;

    public int numberOfPawns = 0;
    public float weight = 0.0f;


    public void RecalculateNumberOfPawn()
    {
        int i = 0;
        for (int x = 0; x < 5; ++x)
        {
            for (int y = 0; y < 5; ++y)
            {
                if (m_pawns[x, y] != null)
                {
                    ++i;
                }
            }
        }

        numberOfPawns = i;
    }

    public bool ReserveFirstAvailableTile(Pawn pawn, out Vector3 position)
    {
        if (numberOfPawns == 5 * 5)
        {
            position = Vector3.zero;
            return false;
        }

        for (int x = 0; x < 5; ++x)
        {
            for (int y = 0; y < 5; ++y)
            {
                if (m_pawns[x, y] == null)
                {
                    m_pawns[x, y] = pawn;
                    position = new Vector3(transform.position.x - 0.5f + (x * 0.2f), transform.position.y - 0.5f + (y * 0.2f), 0.0f);
                    return true;
                }
            }
        }

        position = Vector3.zero;
        return false;
    }

    public void RecalculateWeight()
    {
        if ((up != null && up.armyOwnerId == armyOwnerId)
            || (down != null && down.armyOwnerId == armyOwnerId))
        {
            weight = 0.0f;
        }

        weight = (float)numberOfPawns / (5.0f * 5.0f);
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

    // check and !!ALSO RESERVES!! new tile
    private bool IsNextTileReserved(Vector3 direction, Pawn pawn, ref Pawn other)
    {
        if (direction.y != 0.0f)
        {
            int positionY = pawn.m_subTilePositionY;
            // check upper cell
            if (direction.y > 0.0f)
            {
                positionY += 1;
            }
            else
            {
                positionY -= 1;
            }

            int tilePositionY = pawn.m_tilePositionY;
            if (positionY >= m_gameBoard.tileResolution)
            {
                tilePositionY += 1;
                if (tilePositionY >= m_gameBoard.height)
                {
                    return true;
                }
                if (!m_gameBoard.tiles[pawn.m_tilePositionX, tilePositionY].m_pawns[pawn.m_subTilePositionX, positionY - m_gameBoard.tileResolution])
                {
                    m_pawns[pawn.m_subTilePositionX, pawn.m_subTilePositionY] = null;
                    m_gameBoard.tiles[pawn.m_tilePositionX, tilePositionY].m_pawns[pawn.m_subTilePositionX, positionY - m_gameBoard.tileResolution] = pawn;
                    pawn.m_tilePositionY = tilePositionY;
                    pawn.m_subTilePositionY = positionY - m_gameBoard.tileResolution;
                    return false;
                }
                other = m_gameBoard.tiles[pawn.m_tilePositionX, tilePositionY].m_pawns[pawn.m_subTilePositionX, positionY - m_gameBoard.tileResolution];
                return true;
            }

            if (positionY < 0)
            {
                tilePositionY -= 1;
                if (tilePositionY < 0)
                {
                    return true;
                }
                if (!m_gameBoard.tiles[pawn.m_tilePositionX, tilePositionY].m_pawns[pawn.m_subTilePositionX, m_gameBoard.tileResolution + positionY])
                {
                    m_pawns[pawn.m_subTilePositionX, pawn.m_subTilePositionY] = null;
                    m_gameBoard.tiles[pawn.m_tilePositionX, tilePositionY].m_pawns[pawn.m_subTilePositionX, m_gameBoard.tileResolution + positionY] = pawn;
                    pawn.m_tilePositionY = tilePositionY;
                    pawn.m_subTilePositionY = m_gameBoard.tileResolution + positionY;
                    return false;
                }
                other = m_gameBoard.tiles[pawn.m_tilePositionX, tilePositionY].m_pawns[pawn.m_subTilePositionX, m_gameBoard.tileResolution + positionY];
                return true;
            }

            if (!m_pawns[pawn.m_subTilePositionX, positionY])
            {
                m_pawns[pawn.m_subTilePositionX, pawn.m_subTilePositionY] = null;
                m_pawns[pawn.m_subTilePositionX, positionY] = pawn;
                pawn.m_subTilePositionY = positionY;
                return false;
            }
            other = m_pawns[pawn.m_subTilePositionX, positionY];
            return true;
        }
        else
        {
            int positionX = pawn.m_subTilePositionX;
            // check upper cell
            if (direction.x > 0.0f)
            {
                positionX += 1;
            }
            else
            {
                positionX -= 1;
            }

            int tilePositionX = pawn.m_tilePositionX;
            if (positionX >= m_gameBoard.tileResolution)
            {
                tilePositionX += 1;
                if (tilePositionX >= m_gameBoard.width)
                {
                    return true;
                }
                if (!m_gameBoard.tiles[tilePositionX, pawn.m_tilePositionY].m_pawns[positionX - m_gameBoard.tileResolution, pawn.m_subTilePositionY])
                {
                    m_pawns[pawn.m_subTilePositionX, pawn.m_subTilePositionY] = null;
                    m_gameBoard.tiles[tilePositionX, pawn.m_tilePositionY].m_pawns[positionX - m_gameBoard.tileResolution, pawn.m_subTilePositionY] = pawn;
                    pawn.m_tilePositionX = tilePositionX;
                    pawn.m_subTilePositionX = positionX - m_gameBoard.tileResolution;
                    return false;
                }
                other = m_gameBoard.tiles[tilePositionX, pawn.m_tilePositionY].m_pawns[positionX - m_gameBoard.tileResolution, pawn.m_subTilePositionY];
                return true;
            }

            if (positionX < 0)
            {
                tilePositionX -= 1;
                if (tilePositionX < 0)
                {
                    return true;
                }
                if (!m_gameBoard.tiles[tilePositionX, pawn.m_tilePositionY].m_pawns[m_gameBoard.tileResolution + positionX, pawn.m_subTilePositionY])
                {
                    m_pawns[pawn.m_subTilePositionX, pawn.m_subTilePositionY] = null;
                    m_gameBoard.tiles[tilePositionX, pawn.m_tilePositionY].m_pawns[m_gameBoard.tileResolution + positionX, pawn.m_subTilePositionY] = pawn;
                    pawn.m_tilePositionX = tilePositionX;
                    pawn.m_subTilePositionX = m_gameBoard.tileResolution + positionX;
                    return false;
                }
                other = m_gameBoard.tiles[tilePositionX, pawn.m_tilePositionY].m_pawns[m_gameBoard.tileResolution + positionX, pawn.m_subTilePositionY];
                return true;
            }

            if (!m_pawns[positionX, pawn.m_subTilePositionY])
            {
                m_pawns[pawn.m_subTilePositionX, pawn.m_subTilePositionY] = null;
                m_pawns[positionX, pawn.m_subTilePositionY] = pawn;
                pawn.m_subTilePositionX = positionX;
                return false;
            }
            other = m_pawns[positionX, pawn.m_subTilePositionY];
            return true;
        }
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
                    Pawn other = null;
                    if (pawn.isMovingToFreePlace)
                    {
                        // we reached the place we go towards, then change to another
                        float posX = m_gameBoard.tiles[pawn.m_tilePositionX, pawn.m_tilePositionY].transform.position.x - 0.5f;
                        float posY = m_gameBoard.tiles[pawn.m_tilePositionX, pawn.m_tilePositionY].transform.position.y - 0.5f;
                        posX += unitDistance * pawn.m_subTilePositionX + 0.1f;
                        posY += unitDistance * pawn.m_subTilePositionY;
                        Vector3 desiredPosition = new Vector3(posX, posY, 0.0f);
                        if ((pawn.transform.position.x >= (desiredPosition.x - 0.05f)) && (pawn.transform.position.x <= (desiredPosition.x + 0.05f)) 
                            && (pawn.transform.position.y >= (desiredPosition.y - 0.05f)) && (pawn.transform.position.y <= (desiredPosition.y + 0.05f)))
                        {
                            pawn.isMovingToFreePlace = false;
                            pawn.transform.position = desiredPosition;
                        }
                        else
                        {
                            Vector3 newPosition = new Vector3(direction.x * unitDistance * m_speed, direction.y * unitDistance * m_speed, direction.z * unitDistance * m_speed);
                            pawn.transform.position += newPosition;
                        }
                    }
                    else if (!IsNextTileReserved(direction, pawn, ref other))
                    {
                        pawn.isMovingToFreePlace = true;
                    }
                    else
                    {
                        if (other)
                        {
                            if (other.armyId == pawn.armyId)
                            {
                                // nothing to do
                                continue;
                            }
                            // create combat instance
                            CombatInstance combatInstance = m_gameBoard.GetCombat();
                            combatInstance.SetCombat(pawn, other);
                        }
                    }
                }
            }
        }

        RecalculateNumberOfPawn();
        RecalculateWeight();
    }

    private void Awake()
    {
        m_pawns = new Pawn[5, 5];
    }
}
