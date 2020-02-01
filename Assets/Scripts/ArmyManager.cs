using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyManager : MonoBehaviour
{
    [SerializeField] private int m_armySize;
    [SerializeField] private int m_armyId;
    [SerializeField] private FrontlineEntry[] m_frontLinePositions;
    [SerializeField] private ObjectPool m_pawnObjecPool;
    [SerializeField] private ObjectPool m_frontLineObjectPool;
    
    private List<FrontlineObject> m_frontlines = new List<FrontlineObject>();
    
    private void Start()
    {
        PopulateArmy();
    }
    
    private void Update()
    {
        foreach (FrontlineObject frontline in m_frontlines)
        {
            frontline.Attack();
        }
    }

    private void PopulateArmy()
    {
        /*
        List<GameObject> pawns = m_pawnObjecPool.GetPooledObjects(m_armySize);
        int frontLineIndex = 0;
        int frontLineSize = m_armySize / m_frontLinePositions.Length;

        foreach (FrontlineEntry frontlineEntry in m_frontLinePositions)
        {
            GameObject obj = m_frontLineObjectPool.GetPooledObject();
            if (obj)
            {
                FrontlineObject frontline = obj.GetComponent<FrontlineObject>();
                for (int i = frontLineIndex; i < frontLineIndex + frontLineSize; i++)
                {
                    if ((i <= pawns.Count) && (pawns[i]))
                    {
                        Pawn pawn = pawns[i].GetComponent<Pawn>();
                        frontline.AddPawn(pawn);
                        frontline.SetTiles(frontlineEntry.m_baseTile, frontlineEntry.m_enemyTile);
                        frontline.armyId = m_armyId;
                    }
                }

                frontLineIndex += frontLineSize;
                m_frontlines.Add(frontline);
            }
        }
        */

    }
}
