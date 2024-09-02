using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color baseColor;
    [SerializeField] private bool isWalkable;
    [SerializeField] private GameObject tileHighlight;

    public string tileName;
    public BaseUnit OccupiedUnit;
    public Vector2Int Position { get; private set; }

    private static List<Tile> highlightedTiles = new List<Tile>();

    public bool walkable => isWalkable && OccupiedUnit == null;

    private void Awake()
    {
        tileHighlight.SetActive(false);
    }

    public virtual void Init(int x, int y)
    {
        Position = new Vector2Int(x, y);
    }

    private void OnMouseEnter()
    {
        MenuManager.instance.ShowTileInfo(this);
        //tileHighlight.SetActive(true);
    }

    private void OnMouseExit()
    {
        MenuManager.instance.ShowTileInfo(null);
        //tileHighlight.SetActive(false);
    }

    private void OnMouseDown()
    {
        HandleTileClick();
    }

    private void HandleTileClick()
    {
        if (GameManager.Instance.gameState != GameManager.GameState.HeroesTurn) return;

        if (OccupiedUnit != null)
        {
            // Highlight the tiles that the unit can move to
            HighlightTiles(OccupiedUnit, OccupiedUnit.MovementRange);

            if (OccupiedUnit.Faction == Faction.Hero)
            {
                UnitManager.Instance.SetSelectedHero((BaseHero)OccupiedUnit);
            }
            else
            {
                if (UnitManager.Instance.SelectedHero != null)
                {
                    if (OccupiedUnit is BaseEnemy enemy)
                    {
                        BaseHero hero = (BaseHero)UnitManager.Instance.SelectedHero;
                        hero.Attack(enemy);
                        UnitManager.Instance.SetSelectedHero(null);
                        ClearHighlights();
                    }
                }
            }
        }
        else
        {
            if (UnitManager.Instance.SelectedHero != null)
            {
                UnitManager.Instance.SelectedHero.MoveTo(this);
                UnitManager.Instance.SetSelectedHero(null);
                ClearHighlights();
            }
        }
    }

    public void SetUnit(BaseUnit unit)
    {
        if (unit.OccupiedTile != null) unit.OccupiedTile.OccupiedUnit = null;

        unit.transform.position = transform.position;
        OccupiedUnit = unit;
        unit.OccupiedTile = this;
    }

    public static void HighlightTiles(BaseUnit unit, int range)
    {
        ClearHighlights();

        if (unit == null) return;

        Vector2Int start = unit.OccupiedTile.Position;

        for (int x = -range; x <= range; x++)
        {
            for (int y = -range; y <= range; y++)
            {
                Vector2Int offset = new Vector2Int(x, y);
                Vector2Int position = start + offset;

                // Calculate Manhattan distance
                int manhattanDistance = Mathf.Abs(offset.x) + Mathf.Abs(offset.y);

                if (manhattanDistance <= range) // Check if within Manhattan distance
                {
                    Tile tile = GridManager.Instance.GetTile(new Vector2(position.x, position.y)); // Use GridManager for tile retrieval

                    if (tile != null && tile.walkable && !highlightedTiles.Contains(tile))
                    {
                        tile.ShowHighlight();
                        highlightedTiles.Add(tile);
                    }
                }
            }
        }
    }

    private void ShowHighlight()
    {
        tileHighlight.transform.position = transform.position;
        tileHighlight.SetActive(true);
    }

    public static void ClearHighlights()
    {
        foreach (Tile tile in highlightedTiles)
        {
            tile.HideHighlight();
        }
        highlightedTiles.Clear();
    }

    private void HideHighlight()
    {
        tileHighlight.SetActive(false);
    }
    public static bool IsHighlighted(Tile tile)
    {
        return highlightedTiles.Contains(tile);
    }
}
