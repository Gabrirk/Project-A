using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    private Tile tile;

    public void Initialize(Tile tile)
    {
        this.tile = tile;
    }

    public void HandleTileClick()
    {
        // If it's not the player's turn, do nothing.
        if (GameManager.Instance.gameState != GameManager.GameState.HeroesTurn) return;

        // If the tile is occupied by a unit:
        if (tile.OccupiedUnit != null)
        {
            // If the occupying unit belongs to the player's faction (Hero):
            if (tile.OccupiedUnit.Faction == Faction.Hero)
                UnitManager.Instance.SetSelectedHero((BaseHero)tile.OccupiedUnit); // Select the hero.

            // If the occupying unit belongs to the enemy faction:
            else
            {
                // If a hero is already selected:
                if (UnitManager.Instance.SelectedHero != null)
                {
                    var enemy = (BaseEnemy)tile.OccupiedUnit;
                    Destroy(enemy.gameObject); // Destroy the enemy unit.

                    UnitManager.Instance.SetSelectedHero(null); // Deselect the hero.
                    // Additional logic for dealing damage or attacking can be added here.
                }
            }
        }
        // If the tile is not occupied by any unit:
        else
        {
            // If a hero is selected:
            if (UnitManager.Instance.SelectedHero != null)
            {
                tile.SetUnit(UnitManager.Instance.SelectedHero); // Move the selected hero to this tile.
                UnitManager.Instance.SetSelectedHero(null); // Deselect the hero after moving.
            }
        }
    }
}
