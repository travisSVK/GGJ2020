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
        public Tile tile;
        public Pawn pawn;
        public float distance = 0.0f;
        public float movementProgress = 0.0f;
    }

    private List<RepairOrder> m_repairOrders;
    private List<FrontlineOrder> m_frontlineOrders;

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

    public bool AskForFrontlineOrder(Pawn pawn)
    {
        GameBoard gameBoard = FindObjectOfType<GameBoard>();
        Tile tile = gameBoard.mostWeightedTile;
        FrontlineOrder frontLineOrder = new FrontlineOrder();
        frontLineOrder.pawn = pawn;
        frontLineOrder.startPosition = pawn.transform.position;
        frontLineOrder.tile = tile;
        frontLineOrder.distance = Vector3.Distance(pawn.transform.position, tile.transform.position);
        frontLineOrder.movementProgress = 0.0f;

        m_frontlineOrders.Add(frontLineOrder);

        return true;
    }

    private void Awake()
    {
        m_buildingManager = FindObjectOfType<BuildingManager>();

        m_repairOrders = new List<RepairOrder>();
        m_frontlineOrders = new List<FrontlineOrder>();
    }

    private void Update()
    {
        UpdateRepairOrders();
        UpdateFrontlineOrders();
    }

    private void UpdateRepairOrders()
    {
        for (int i = m_repairOrders.Count - 1; i >= 0; --i)
        {
            Pawn pawn = m_repairOrders[i].pawn;

            if (m_repairOrders[i].movementProgress < 1.0f)
            {
                // Move.
                m_repairOrders[i].movementProgress += (Time.deltaTime / m_repairOrders[i].distance) * 1.2f;
                pawn.transform.position = Vector3.Lerp(m_repairOrders[i].startPosition,
                    m_repairOrders[i].forge.transform.position,
                    Mathf.Min(1.0f, m_repairOrders[i].movementProgress));
            }
            else if (m_repairOrders[i].repairProgress < 1.0f)
            {
                if (i < 10)
                {
                    // Repair.
                    m_repairOrders[i].repairProgress += 0.5f * Time.deltaTime;
                }
            }
            else
            {
                if (AskForFrontlineOrder(pawn))
                {
                    m_repairOrders.RemoveAt(i);
                }
            }
        }
    }

    private void UpdateFrontlineOrders()
    {
        for (int i = m_frontlineOrders.Count - 1; i >= 0; --i)
        {
            Pawn pawn = m_frontlineOrders[i].pawn;

            if (m_frontlineOrders[i].movementProgress < 1.0f)
            {
                // Move.
                m_frontlineOrders[i].movementProgress += (Time.deltaTime / m_frontlineOrders[i].distance) * 1.2f;

                pawn.transform.position = Vector3.Lerp(m_frontlineOrders[i].startPosition, m_frontlineOrders[i].tile.transform.position,
                    Mathf.Min(1.0f, m_frontlineOrders[i].movementProgress));

                /*
                if (m_frontlineOrders[i].tile.numberOfPawns >= 5 * 5)
                {
                    if (AskForFrontlineOrder(m_frontlineOrders[i].pawn))
                    {
                        m_frontlineOrders.RemoveAt(i);
                    }
                }
                */
            }
            else
            {
                Vector3 position = Vector3.zero;
                if (m_frontlineOrders[i].tile.ReserveFirstAvailableTile(m_frontlineOrders[i].pawn, out position))
                {
                    m_frontlineOrders[i].pawn.transform.position = position;
                    m_frontlineOrders.RemoveAt(i);
                }
                else if (AskForFrontlineOrder(m_frontlineOrders[i].pawn))
                {
                    m_frontlineOrders.RemoveAt(i);
                }
            }
        }
    }
}
