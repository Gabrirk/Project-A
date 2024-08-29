using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    // Singleton instance of the GridManager
    public static GridManager Instance;

    // Grid dimensions
    [SerializeField] private int widht, height;

    // Different types of tiles
    [SerializeField] private Tile GrassTile, MountainTile, ForestTile;

    // Reference to the camera to adjust its position after grid generation
    [SerializeField] private Transform cam;

    // Dictionary to store the grid tiles with their positions as keys
    private Dictionary<Vector2, Tile> tiles;

    private void Awake()
    {
        // Assign the singleton instance
        Instance = this;
    }

    // Method to generate the grid of tiles
    public void GenerateGrid()
    {
        // Initialize the tiles dictionary
        tiles = new Dictionary<Vector2, Tile>();

        // Loop through the grid dimensions to instantiate tiles
        for (int x = 0; x < widht; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Randomly select either a MountainTile or GrassTile
                var randonTile = Random.Range(0, 6) == 3 ? MountainTile : GrassTile;

                // Instantiate the selected tile at the current grid position
                var spawnedTile = Instantiate(randonTile, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";

                // Initialize the tile with its grid coordinates
                spawnedTile.Init(x, y);

                // Add the tile to the dictionary with its position as the key
                tiles[new Vector2(x, y)] = spawnedTile;
            }
        }

        // Position the camera in the center of the grid
        cam.transform.position = new Vector3((float)widht / 2 - 0.5f, (float)height / 2 - 0.5f, -10);

        // Notify the GameManager to change the game state to SpawnHeroes
        GameManager.Instance.ChangeState(GameManager.GameState.SpawnHeroes);
    }

    // Method to get a tile at a specific position
    public Tile GetTile(Vector2 pos)
    {
        if (tiles.TryGetValue(pos, out var tile)) return tile;
        return null;
    }

    // Method to get a spawn tile for a hero
    // Only considers tiles in the left half of the grid that are walkable
    public Tile GetRandomHeroSpawnTile()
    {
        return tiles.Where(t => t.Key.x < widht / 2 && t.Value.walkable)
                    .OrderBy(t => Random.value)
                    .First().Value;
    }

    // Method to get a spawn tile for an enemy
    // Only considers tiles in the right half of the grid that are walkable
    public Tile GetRandomEnemySpawnTile()
    {
        return tiles.Where(t => t.Key.x > widht / 2 && t.Value.walkable)
                    .OrderBy(t => Random.value)
                    .First().Value;
    }
}
