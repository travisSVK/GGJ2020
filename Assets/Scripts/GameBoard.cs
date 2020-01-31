using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    [SerializeField] private Frontline m_frontlinePrefab;

    [SerializeField] private int m_width = 48;
    [SerializeField] private int m_height = 32;

    private Tile[,] m_tiles = null;
    private List<Frontline> m_frontlines;

    public List<Pawn> GetEnemyPawnsOfTile(Vector2Int position, int id)
    {
        return m_tiles[position.x, position.y].GetEnemyPawns(id);
    }

    public void GenereateFrontline()
    {
        foreach (Frontline f in m_frontlines)
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

                        Frontline frontline = Instantiate(m_frontlinePrefab);
                        frontline.transform.parent = transform;
                        frontline.transform.position = m_tiles[x, y].transform.position;
                    }
                }
            }
        }
    }

    private void Awake()
    {
        m_frontlines = new List<Frontline>();
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

        GenereateFrontline();
    }

    private void OnDrawGizmos()
    {
        /*
        for (int y = 0; y < m_height; ++y)
        {
            for (int x = 0; x < m_width; ++x)
            {
                for (int yi = 0; yi < 5; ++yi)
                {
                    for (int xi = 0; xi < 5; ++xi)
                    {
                        Gizmos.DrawSphere(m_tiles[x, y].transform.position - new Vector3(-0.45f + (0.2f * xi), -0.45f + (0.2f * yi), 0.0f), 0.1f);
                    }
                }
            }
        }
        */
    }
}
