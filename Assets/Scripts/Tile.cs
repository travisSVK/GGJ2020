using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Tile up = null;
    public Tile down = null;
    public Tile left = null;
    public Tile right = null;

    public int armyOwnerId = 0;

    private Pawn[, ] m_pawns;


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

    private void Awake()
    {
        m_pawns = new Pawn[5, 5];
    }
}
