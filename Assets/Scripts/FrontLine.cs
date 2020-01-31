using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontLine : MonoBehaviour
{
    private List<Pawn> m_pawns = new List<Pawn>();

    private void Start()
    {
        
    }
    
    private void Update()
    {
        
    }

    public void AddPawn(Pawn pawn)
    {
        m_pawns.Add(pawn);
    }
}
