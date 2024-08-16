using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    [SerializeField] private int widht, height;
    [SerializeField] private Tile GrassTile, MountainTile, ForestTile;
    [SerializeField] private Transform cam;

    private Dictionary<Vector2, Tile> tiles;

    private void Awake()
    {
        Instance = this;
    }

    public void GenerateGrid()
    {
        tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < widht; x++) {
            for (int y = 0; y < height; y++) {
                var randonTile = Random.Range(0,6) == 3 ? MountainTile : GrassTile;
                var spawnedTile = Instantiate(randonTile, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";

                spawnedTile.Init(x,y);
                tiles[new Vector2 (x, y)] = spawnedTile;
                



            }
        }
        cam.transform.position = new Vector3((float)widht / 2 - 0.5f, (float)height / 2 - 0.5f, -10);
        GameManager.Instance.ChangeState(GameManager.GameState.SpawnHeroes);
    }

    public Tile GetTileAtPosition(Vector2 pos)
    {
        if (tiles.TryGetValue(pos, out var tile)) return tile;
        return null;
    }

    public Tile GetHeroSpawnTile()
    {
        return tiles.Where(t => t.Key.x < widht / 2 && t.Value.walkable).OrderBy(t => Random.value).First().Value;
    }
    public Tile GetEnemySpawnTile()
    {
        return tiles.Where(t => t.Key.x > widht / 2 && t.Value.walkable).OrderBy(t => Random.value).First().Value;
    }



}
