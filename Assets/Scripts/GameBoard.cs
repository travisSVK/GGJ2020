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

    [SerializeField] private GameObject m_pawnPrefab = null;

    private ObjectPool m_playerPool = null;
    private ObjectPool m_enemyPool = null;

    private Tile[,] m_tiles = null;
    private List<FrontlineObject> m_frontlines;

    public List<Pawn> GetEnemyPawnsOfTile(Vector2Int position, int id)
    {
        return m_tiles[position.x, position.y].GetEnemyPawns(id);
    }

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

    private void Awake()
    {
        GameObject pawnPoolObj = new GameObject("PlayerPool");
        m_playerPool = pawnPoolObj.AddComponent<ObjectPool>();
        m_playerPool.Initialize(10000, "PlayerUnit", m_pawnPrefab);

        GameObject enemyPoolObj = new GameObject("EnemyPool");
        m_enemyPool = enemyPoolObj.AddComponent<ObjectPool>();
        m_enemyPool.Initialize(10000, "EnemyUnit", m_pawnPrefab);

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
                            m_tiles[x, y].m_pawns[xi, yi] = pawn.GetComponent<Pawn>();
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
