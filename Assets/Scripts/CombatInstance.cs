using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatInstance : MonoBehaviour
{
    private Pawn m_pawn1;
    private Pawn m_pawn2;

    public void SetCombat(Pawn pawn1, Pawn pawn2)
    {
        m_pawn1 = pawn1;
        m_pawn2 = pawn2;
    }

    private void Update()
    {
        
    }
}
