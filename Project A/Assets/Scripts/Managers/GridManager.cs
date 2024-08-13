using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    [SerializeField] private int widht, height;
    [SerializeField] private Tile GrassTile, MountainTile, ForestTile;
    [SerializeField] private Transform cam;

    private void Awake()
    {
        Instance = this;
    }

    public void GenerateGrid()
    {
        for (int x = 0; x < widht; x++) {
            for (int y = 0; y < height; y++) {
                var randonTile = Random.Range(0,6) == 3 ? MountainTile : GrassTile;
                var spawnedTile = Instantiate(randonTile, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";

                cam.transform.position = new Vector3((float)widht / 2 - 0.5f, (float)height / 2 - 0.5f, -10);

                GameManager.Instance.ChangeState(GameManager.GameState.SpawnHeroes);



            }
        }
    }

    public Tile GetHeroSpawnTile()
    {
        
    }



}
