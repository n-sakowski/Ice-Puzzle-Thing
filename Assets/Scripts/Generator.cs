using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class IcePuzzleGenerator : MonoBehaviour
{
    [Header("Tilemaps")]
    public Tilemap iceTilemap;
    public Tilemap rockTilemap;

    [Header("Tiles")]
    public Tile iceTile;
    public Tile rockTile;

    [Header("Grid Settings")]
    public int width = 16;
    public int height = 9;

    [Header("Generation Settings")]
    [Range(0f, 1f)]
    public float randomRockChance = 0.2f; // chance to place extra rocks

    private enum TileType { Ice, Rock }
    private TileType[,] grid;

    private void Start()
    {
        GeneratePuzzle();
    }

    public void GeneratePuzzle()
    {
        grid = new TileType[width, height];

        // Step 1: Fill everything with ice
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grid[x, y] = TileType.Ice;
            }
        }

        // Step 2: Add perimeter rocks
        for (int x = 0; x < width; x++)
        {
            grid[x, 0] = TileType.Rock;
            grid[x, height - 1] = TileType.Rock;
        }
        for (int y = 0; y < height; y++)
        {
            grid[0, y] = TileType.Rock;
            grid[width - 1, y] = TileType.Rock;
        }

        // Step 3: Generate guaranteed path
        Vector2Int start = new Vector2Int(1, 1); // top-left inside perimeter
        Vector2Int end = new Vector2Int(width - 2, height - 2); // bottom-right inside perimeter
        List<Vector2Int> path = GenerateRandomPath(start, end);

        // Step 4: Place extra rocks randomly (but never overwrite guaranteed path)
        for (int x = 1; x < width - 1; x++)
        {
            for (int y = 1; y < height - 1; y++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                if (!path.Contains(pos) && Random.value < randomRockChance)
                    grid[x, y] = TileType.Rock;
            }
        }

        // Step 5: Paint tiles on tilemaps
        iceTilemap.ClearAllTiles();
        rockTilemap.ClearAllTiles();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int tilePos = new Vector3Int(x, y, 0);
                if (grid[x, y] == TileType.Rock)
                    rockTilemap.SetTile(tilePos, rockTile);
                else
                    iceTilemap.SetTile(tilePos, iceTile);
            }
        }

        // Optional: snap player to start position
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Vector3Int startTile = new Vector3Int(start.x, start.y, 0);
            player.transform.position = iceTilemap.CellToWorld(startTile) + new Vector3(0.5f, 0.5f, 0);
        }
    }

    private List<Vector2Int> GenerateRandomPath(Vector2Int start, Vector2Int end)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Vector2Int current = start;
        path.Add(current);

        while (current != end)
        {
            List<Vector2Int> options = new List<Vector2Int>();

            // Up
            if (current.y < height - 2) options.Add(current + Vector2Int.up);
            // Down
            if (current.y > 1) options.Add(current + Vector2Int.down);
            // Right
            if (current.x < width - 2) options.Add(current + Vector2Int.right);
            // Left
            if (current.x > 1) options.Add(current + Vector2Int.left);

            // Optionally remove already visited tiles to avoid loops
            options.RemoveAll(tile => path.Contains(tile));

            if (options.Count == 0) break; // stuck, should rarely happen

            Vector2Int next = options[Random.Range(0, options.Count)];
            path.Add(next);
            current = next;
        }

        return path;
    }
}
