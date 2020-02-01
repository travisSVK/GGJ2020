using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForgeBuilding : MonoBehaviour
{
    private BuildingManager m_buildingManager;

    private void Awake()
    {
        m_buildingManager = FindObjectOfType<BuildingManager>();
    }

    private void OnEnable()
    {
        m_buildingManager.RegisterForgeBuilding(this);
    }

    private void OnDisable()
    {
        m_buildingManager.DeregisterForgeBuilding(this);
    }
}
