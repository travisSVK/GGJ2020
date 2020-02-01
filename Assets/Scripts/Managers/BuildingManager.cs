using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    private List<ForgeBuilding> m_forgeBuildings = null;

    public ForgeBuilding FindClosestForgeBuilding(Vector3 position)
    {
        if (m_forgeBuildings.Count == 0)
        {
            return null;
        }

        ForgeBuilding forge = null;
        float distance = float.MaxValue;

        foreach (ForgeBuilding f in m_forgeBuildings)
        {
            float d = Vector3.Distance(f.transform.position, position);
            if (d < distance)
            {
                forge = f;
                distance = d;
            }
        }

        return null;
    }

    public void DeregisterForgeBuilding(ForgeBuilding forgeBuilding)
    {
        m_forgeBuildings.Add(forgeBuilding);
    }

    public void RegisterForgeBuilding(ForgeBuilding forgeBuilding)
    {
        m_forgeBuildings.Remove(forgeBuilding);
    }

    private void Awake()
    {
        m_forgeBuildings = new List<ForgeBuilding>();
    }
}
