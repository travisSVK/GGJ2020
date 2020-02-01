using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    public const int PLAYER_ARMY_ID = 0;
    public const int ENEMY_ARMY_ID = 1;

    [SerializeField] private FrontlineObject m_frontlinePrefab = null;

    [SerializeField] private int m_width = 48;
    [SerializeField] private int m_height = 32;
    [SerializeField] private int m_tileResolution = 5;

    [SerializeField] private GameObject m_pawnPrefab = null;
    [SerializeField] private GameObject m_combatPrefab = null;

    private ObjectPool m_playerPool = null;
    private ObjectPool m_enemyPool = null;
    private ObjectPool m_combatPool = null;

    private Tile[,] m_tiles = null;
    private List<FrontlineObject> m_frontlines;
    

    public void GenereateFrontline()
    {
        foreach (FrontlineObject f in m_frontlines)
        {
            Destroy(f);
        }

        m_frontlines.Clear();

        for (int y = 0; y < m_height; ++y)
        {
            for (int x = 0; x < m_width; ++x)
            {
                int armyOwnerId = m_tiles[x, y].armyOwnerId;
                if (armyOwnerId == 0)
                {
                    if (y + 1 < m_height && m_tiles[x, y + 1].armyOwnerId == 1)
                    {

                        FrontlineObject frontline = Instantiate(m_frontlinePrefab);
                        frontline.transform.parent = transform;
                        frontline.transform.position = m_tiles[x, y].transform.position;
                    }
                }
            }
        }
    }

    public float GetUnitDistance()
    {
        return 1.0f / m_tileResolution;
    }

    public GameObject GetCombat()
    {
        GameObject combat = m_combatPool.GetPooledObject();
        CombatInstance ci = combat.GetComponent(typeof(CombatInstance)) as CombatInstance;
        ci.SetKillCallback(m_combatPool.Kill);
        return m_combatPool.GetPooledObject();
    }

    public void KillSpawn(GameObject go)
    {
        Pawn pawn = go.GetComponent<Pawn>();
        m_tiles[pawn.m_tilePositionX, pawn.m_tilePositionY].m_pawns[pawn.m_subTilePositionX, pawn.m_subTilePositionX] = null;
        m_playerPool.Kill(go);
    }

    public void KillCombat(GameObject go)
    {
        m_combatPool.Kill(go);
    }

    private void Awake()
    {
        GameObject pawnPoolObj = new GameObject("PlayerPool");
        m_playerPool = pawnPoolObj.AddComponent<ObjectPool>();
        m_playerPool.Initialize(10000, "PlayerUnit", m_pawnPrefab);

        GameObject enemyPoolObj = new GameObject("EnemyPool");
        m_enemyPool = enemyPoolObj.AddComponent<ObjectPool>();
        m_enemyPool.Initialize(10000, "EnemyUnit", m_pawnPrefab);

        GameObject combatPoolObj = new GameObject("CombatPool");
        m_combatPool = combatPoolObj.AddComponent<ObjectPool>();
        m_combatPool.Initialize(10, "CombatInstance", m_combatPrefab);

        m_frontlines = new List<FrontlineObject>();
        m_tiles = new Tile[m_width, m_height];

        float halfWidth = (m_width / 2.0f) - 0.5f;
        float halfHeight = (m_height / 2.0f) - 0.5f;

        for (int y = 0; y < m_height; ++y)
        {
            for (int x = 0; x < m_width; ++x)
            {
                GameObject obj = new GameObject();
                obj.transform.parent = transform;
                obj.transform.position = new Vector3(x - halfWidth, y - halfHeight, 0.0f);

                Tile tile = obj.AddComponent<Tile>();

                if (y > (int)halfHeight)
                {
                    tile.armyOwnerId = 1;
                }
                else
                {
                    tile.armyOwnerId = 0;
                }

                m_tiles[x, y] = tile;
            }
        }

        for (int y = 0; y < m_height; ++y)
        {
            for (int x = 0; x < m_width; ++x)
            {
                if (x + 1 < m_width)
                {
                    m_tiles[x, y].down = m_tiles[x + 1, y];
                }

                if (x - 1 >= 0)
                {
                    m_tiles[x, y].down = m_tiles[x - 1, y];
                }

                if (y + 1 < m_height)
                {
                    m_tiles[x, y].up = m_tiles[x, y + 1];
                }

                if (y - 1 >= 0)
                {
                    m_tiles[x, y].down = m_tiles[x, y - 1];
                }
            }
        }

        GenereateFrontline();

        PopulatePlayerArmy();
        PopulateEnemyArmy();
    }

    private void PopulatePlayerArmy()
    {
        // Populate player.
        for (int y = (m_height / 2) - 1, c = 0; y >= 0; --y)
        {
            for (int x = 0; x < m_width; ++x)
            {
                for (int yi = 4; yi >= 0; --yi)
                {
                    for (int xi = 0; xi < 5; ++xi, ++c)
                    {
                        if (c > 1000)
                        {
                            return;
                        }

                        GameObject pawn = m_playerPool.GetPooledObject();
                        if (pawn != null)
                        {
                            Vector3 offset = m_tiles[x, y].transform.position;
                            pawn.transform.position = new Vector3(offset.x - 0.4f + (xi * 0.2f), offset.y - 0.5f + (yi * 0.2f), 0.0f);
                            Pawn pawnComponent = pawn.GetComponent<Pawn>();
                            pawnComponent.SetKillCallback(KillSpawn);
                            pawnComponent.SpawnPawn(x, y, xi, yi);
                            m_tiles[x, y].m_pawns[xi, yi] = pawnComponent;
                        }
                    }
                }
            }
        }
    }

    private void PopulateEnemyArmy()
    {
        // Populate player.
        for (int y = (m_height / 2), c = 0; y < m_height; ++y)
        {
            for (int x = 0; x < m_width; ++x)
            {
                for (int yi = 0; yi < 5; ++yi)
                {
                    for (int xi = 0; xi < 5; ++xi, ++c)
                    {
                        if (c > 1000)
                        {
                            return;
                        }

                        GameObject pawn = m_enemyPool.GetPooledObject();
                        if (pawn != null)
                        {
                            Vector3 offset = m_tiles[x, y].transform.position;
                            pawn.transform.position = new Vector3(offset.x - 0.4f + (xi * 0.2f), offset.y - 0.5f + (yi * 0.2f), 0.0f);
                            m_tiles[x, y].m_pawns[xi, yi] = pawn.GetComponent<Pawn>();
                        }
                    }
                }
            }
        }
    }
}
