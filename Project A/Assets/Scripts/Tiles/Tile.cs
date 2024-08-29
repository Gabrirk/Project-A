using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color baseColor;
    [SerializeField] private bool isWalkable;
    public string tileName;

    public BaseUnit OccupiedUnit;
    public bool walkable => isWalkable && OccupiedUnit == null;

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
        if (GameManager.Instance.gameState != GameManager.GameState.HeroesTurn) return;

        if (OccupiedUnit != null)
        {
            if(OccupiedUnit.Faction == Faction.Hero) UnitManager.Instance.SetSelectedHero((BaseHero)OccupiedUnit);
            else
            {
                if(UnitManager.Instance.SelectedHero != null)
                {
                    var enemy = (BaseEnemy)OccupiedUnit;
                    Destroy(enemy.gameObject);
                    UnitManager.Instance.SetSelectedHero(null);
                    //Enemy.takeDamage
                    //UnitManager.Instance.SelectedHero.attack
                }
            } 
        }
        else
        {
            if(UnitManager.Instance.SelectedHero != null)
            {
                SetUnit(UnitManager.Instance.SelectedHero);
                UnitManager.Instance.SetSelectedHero(null);
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



}

