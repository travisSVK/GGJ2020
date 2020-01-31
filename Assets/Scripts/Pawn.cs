using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    public int armyId = 0;
    private bool m_isInCombat = false;
    private bool m_hasWonCombat = false;

    public bool isInCombat
    {
        get { return m_isInCombat; }
    }

    public bool hasWonCombat
    {
        get { return m_hasWonCombat; }
        set { m_hasWonCombat = value; }
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
