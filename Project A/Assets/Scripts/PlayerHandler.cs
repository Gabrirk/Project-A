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
        if (GameManager.Instance.gameState != GameManager.GameState.HeroesTurn) return;

        if (tile.OccupiedUnit != null)
        {
            if (tile.OccupiedUnit.Faction == Faction.Hero)
                UnitManager.Instance.SetSelectedHero((BaseHero)tile.OccupiedUnit);

            else
            {
                if (UnitManager.Instance.SelectedHero != null)
                {
                    if (tile.OccupiedUnit is BaseEnemy enemy)
                    {
                        BaseHero hero = (BaseHero)UnitManager.Instance.SelectedHero;
                        hero.Attack(enemy);
                        UnitManager.Instance.SetSelectedHero(null);
                    }
                }
            }
        }
        else
        {
            if (UnitManager.Instance.SelectedHero != null)
            {
                UnitManager.Instance.SelectedHero.MoveTo(tile);
                UnitManager.Instance.SetSelectedHero(null);
            }
        }
    }
}
