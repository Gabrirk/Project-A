using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Tile : MonoBehaviour
{

    [SerializeField] private Color baseColor;
    [SerializeField] private bool isWalkable;

    public string tileName;
    private PlayerHandler playerHandler;
    public BaseUnit OccupiedUnit;


    public bool walkable => isWalkable && OccupiedUnit == null;

    private void Awake()
    {
        // Ensure TileHandler is attached to the same GameObject as this Tile
        playerHandler = GetComponent<PlayerHandler>();
        if (playerHandler != null)
        {
            playerHandler.Initialize(this);
        }
        else
        {
            Debug.LogError("TileHandler component is missing from the GameObject!");
        }
    }

    public virtual void Init(int x, int y)
    {

    }


    private void OnMouseEnter()
    {
        MenuManager.instance.ShowTileInfo(this);
    }


    private void OnMouseExit()
    {
        MenuManager.instance.ShowTileInfo(null);
    }


    private void OnMouseDown()
    {
        if (playerHandler != null)
        {
            playerHandler.HandleTileClick();
        }
        else
        {
            Debug.LogError("TileHandler is not initialized!");
        }
    }

    // This method sets the unit on the tile. It handles the movement of units from one tile to another.
    public void SetUnit(BaseUnit unit)
    {
        // If the unit is already on another tile, clear the unit from that tile.
        if (unit.OccupiedTile != null) unit.OccupiedTile.OccupiedUnit = null;

        unit.transform.position = transform.position; // Move the unit to this tile's position.
        OccupiedUnit = unit; // Set this tile's OccupiedUnit to the given unit.
        unit.OccupiedTile = this; // Update the unit's OccupiedTile reference to this tile.
    }

}