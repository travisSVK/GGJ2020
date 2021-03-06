﻿using System.Collections;
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
    [SerializeField] private GameObject m_enemyPawnPrefab = null;
    [SerializeField] private GameObject m_combatPrefab = null;

    private ObjectPool m_playerPool = null;
    private ObjectPool m_enemyPool = null;
    private ObjectPool m_combatPool = null;

    private Tile[,] m_tiles = null;
    private List<Tile> m_sortedTiles;
    private List<FrontlineObject> m_frontlines;

    public Tile mostWeightedTile
    {
        get
        {
            return m_sortedTiles[0];
        }
    }

    public int tileResolution
    {
        get { return m_tileResolution; }
    }

    public Tile[,] tiles
    {
        get { return m_tiles; }
    }

    public int width
    {
        get { return m_width; }
    }

    public int height
    {
        get { return m_height; }
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

    public float GetUnitDistance()
    {
        return 1.0f / m_tileResolution;
    }

    public CombatInstance GetCombat()
    {
        GameObject combat = m_combatPool.GetPooledObject();
        CombatInstance ci = combat.GetComponent(typeof(CombatInstance)) as CombatInstance;
        ci.SetKillCallback(m_combatPool.Kill);
        return ci;
    }

    public void RemovePawnFromTile(Pawn pawn)
    {
        m_tiles[pawn.m_tilePositionX, pawn.m_tilePositionY].m_pawns[pawn.m_subTilePositionX, pawn.m_subTilePositionY] = null;
    }

    public void AssignPawnToTile(Pawn pawn, int tilePositionX, int tilePositionY, int subTilePositionX, int subTilePositionY)
    {
        RemovePawnFromTile(pawn);
        m_tiles[tilePositionX, tilePositionY].m_pawns[subTilePositionX, subTilePositionY] = pawn;
        pawn.m_tilePositionX = tilePositionX;
        pawn.m_tilePositionY = tilePositionY;
        pawn.m_subTilePositionX = subTilePositionX;
        pawn.m_subTilePositionY = subTilePositionY;
    }

    public void KillEnemyPawn(GameObject go, bool eraseTilePosition = true)
    {
        Pawn pawn = go.GetComponent<Pawn>();
        if (eraseTilePosition)
        {
            m_tiles[pawn.m_tilePositionX, pawn.m_tilePositionY].m_pawns[pawn.m_subTilePositionX, pawn.m_subTilePositionY] = null;
        }
        m_playerPool.Kill(go);
    }

    public void KillPawn(GameObject go, bool eraseTilePosition = true)
    {
        Pawn pawn = go.GetComponent<Pawn>();
        if (eraseTilePosition)
        {
            m_tiles[pawn.m_tilePositionX, pawn.m_tilePositionY].m_pawns[pawn.m_subTilePositionX, pawn.m_subTilePositionY] = null;
        }
        m_playerPool.Kill(go);
    }

    private void Awake()
    {
        m_sortedTiles = new List<Tile>();

        GameObject pawnPoolObj = new GameObject("PlayerPool");
        m_playerPool = pawnPoolObj.AddComponent<ObjectPool>();
        m_playerPool.Initialize(10000, "PlayerUnit", m_pawnPrefab);

        GameObject enemyPoolObj = new GameObject("EnemyPool");
        m_enemyPool = enemyPoolObj.AddComponent<ObjectPool>();
        m_enemyPool.Initialize(10000, "EnemyUnit", m_enemyPawnPrefab);

        GameObject combatPoolObj = new GameObject("CombatPool");
        m_combatPool = combatPoolObj.AddComponent<ObjectPool>();
        m_combatPool.Initialize(10000, "CombatInstance", m_combatPrefab);

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
                m_sortedTiles.Add(tile);
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
                        if (c > 500)
                        {
                            return;
                        }

                        GameObject pawn = m_playerPool.GetPooledObject();
                        if (pawn != null)
                        {
                            Vector3 offset = m_tiles[x, y].transform.position;
                            pawn.transform.position = new Vector3(offset.x - 0.4f + (xi * 0.2f), offset.y - 0.5f + (yi * 0.2f), 0.0f);
                            Pawn pawnComponent = pawn.GetComponent<Pawn>();
                            pawnComponent.SetKillCallback(KillPawn);
                            pawnComponent.SetRemoveFromTileCallback(RemovePawnFromTile);
                            pawnComponent.SetAssignToTileCallback(AssignPawnToTile);
                            pawnComponent.SpawnPawn(PLAYER_ARMY_ID, x, y, xi, yi);
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
                        if (c > 500)
                        {
                            return;
                        }

                        GameObject pawn = m_enemyPool.GetPooledObject();
                        if (pawn != null)
                        {
                            Vector3 offset = m_tiles[x, y].transform.position;
                            pawn.transform.position = new Vector3(offset.x - 0.4f + (xi * 0.2f), offset.y - 0.5f + (yi * 0.2f), 0.0f);
                            Pawn pawnComponent = pawn.GetComponent<Pawn>();
                            pawnComponent.SetKillCallback(KillEnemyPawn);
                            pawnComponent.SetRemoveFromTileCallback(RemovePawnFromTile);
                            pawnComponent.SetAssignToTileCallback(AssignPawnToTile);
                            pawnComponent.SpawnPawn(ENEMY_ARMY_ID, x, y, xi, yi);
                            m_tiles[x, y].m_pawns[xi, yi] = pawnComponent;
                        }
                    }
                }
            }
        }
    }

    private void Update()
    {
        m_sortedTiles.Sort((x, y) => x.weight.CompareTo(y.weight));
        m_sortedTiles.Reverse();
    }
}
