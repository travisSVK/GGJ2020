using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyManager : MonoBehaviour
{
    [SerializeField] private int m_armySize;
    [SerializeField] private FrontlineEntry[] m_frontLinePositions;
    [SerializeField] private ObjectPool m_pawnObjecPool;
    [SerializeField] private ObjectPool m_frontLineObjectPool;

    private List<Frontline> m_frontLines = new List<Frontline>();
    

    private void Start()
    {
        PopulateArmy();
    }
    
    private void Update()
    {
        
    }

    private void PopulateArmy()
    {
        List<GameObject> pawns = m_pawnObjecPool.GetPooledObjects(m_armySize);
        int frontLineIndex = 0;
        int frontLineSize = m_armySize / m_frontLinePositions.Length;

        foreach (FrontlineEntry frontlineEntry in m_frontLinePositions)
        {
            GameObject obj = m_frontLineObjectPool.GetPooledObject();
            if (obj)
            {
                Frontline frontLine = obj.GetComponent<Frontline>();
                for (int i = frontLineIndex; i < frontLineIndex + frontLineSize; i++)
                {
                    if ((i <= pawns.Count) && (pawns[i]))
                    {
                        Pawn pawn = pawns[i].GetComponent<Pawn>();
                        frontLine.AddPawn(pawn);
                        frontLine.SetTiles(frontlineEntry.m_baseTile, frontlineEntry.m_enemyTile);
                    }
                }
                frontLineIndex += frontLineSize;
                m_frontLines.Add(frontLine);
            }
        }

    }
}
