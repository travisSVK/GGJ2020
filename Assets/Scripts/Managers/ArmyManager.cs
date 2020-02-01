using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyManager : MonoBehaviour
{
    private float m_movementSpeed = 0.5f;
    private BuildingManager m_buildingManager = null;

    public class RepairOrder
    {
        public Vector3 startPosition;
        public ForgeBuilding forge;
        public Pawn pawn;
        public float distance;
        public float movementProgress;
        public float repairProgress;
    }

    public class ShowerOrder
    {
        public Vector3 startPosition;
        public ForgeBuilding forge;
        public Pawn pawn;
        public float distance;
        public float movementProgress;
        public float repairProgress;
    }

    public class FrontlineOrder
    {
        public Vector3 startPosition;
        public Tile frontline;
        public Pawn pawn;
        public float distance;
        public float movementProgress;
    }

    private List<RepairOrder> m_repairOrders;

    public bool AskForRepair(Pawn pawn)
    {
        ForgeBuilding forgeBuilding = m_buildingManager.FindClosestForgeBuilding(pawn.transform.position);
        if (forgeBuilding == null)
        {
            return false;
        }

        RepairOrder repairOrder = new RepairOrder();
        repairOrder.startPosition = pawn.transform.position;
        repairOrder.forge = forgeBuilding;
        repairOrder.pawn = pawn;
        repairOrder.distance = Vector3.Distance(pawn.transform.position, forgeBuilding.transform.position);
        repairOrder.movementProgress = 0.0f;
        repairOrder.repairProgress = 0.0f;

        m_repairOrders.Add(repairOrder);

        return true;
    }

    public bool AskForShowerOrder(Pawn pawn)
    {
        ForgeBuilding forgeBuilding = m_buildingManager.FindClosestForgeBuilding(pawn.transform.position);
        if (forgeBuilding == null)
        {
            return false;
        }

        RepairOrder repairOrder = new RepairOrder();
        repairOrder.startPosition = pawn.transform.position;
        repairOrder.forge = forgeBuilding;
        repairOrder.pawn = pawn;
        repairOrder.distance = Vector3.Distance(pawn.transform.position, forgeBuilding.transform.position);
        repairOrder.movementProgress = 0.0f;
        repairOrder.repairProgress = 0.0f;

        m_repairOrders.Add(repairOrder);

        return true;
    }

    private void Awake()
    {
        m_buildingManager = FindObjectOfType<BuildingManager>();

        m_repairOrders = new List<RepairOrder>();
    }

    private void Update()
    {
        UpdateRepairOrders();
    }

    private void UpdateRepairOrders()
    {
        Debug.Log(m_repairOrders.Count);

        for (int i = 0; i < m_repairOrders.Count; ++i)
        {
            Pawn pawn = m_repairOrders[i].pawn;

            if (m_repairOrders[i].movementProgress < 1.0f)
            {
                // Move.
                m_repairOrders[i].movementProgress += (m_repairOrders[i].distance / Time.deltaTime) * 0.05f;
                pawn.transform.position = Vector3.Lerp(m_repairOrders[i].startPosition,
                    m_repairOrders[i].forge.transform.position,
                    Mathf.Min(1.0f, m_repairOrders[i].movementProgress));
            }
            else if (m_repairOrders[i].repairProgress < 1.0f)
            {
                // Repair.
                m_repairOrders[i].repairProgress += 0.1f * Time.deltaTime;

            }
            else
            {
                // Find new frontline.
                Debug.Log("Repaired, Looking for new frontline.");
            }
        }
    }
}
