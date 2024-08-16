using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    [SerializeField] private GameObject selectedHeroObject, tileObject, tileUnitObject;

    private void Awake()
    {
        instance = this;
    }

    public void ShowSelectedHero(BaseHero hero)
    {
        if (hero == null)
        {
            selectedHeroObject.SetActive(false);
            return;
        }
        selectedHeroObject.GetComponentInChildren<Text>().text = hero.UnitName;
        selectedHeroObject.SetActive(true);
    }

    public void ShowTileInfo(Tile tile)
    {
        if (tile == null) 
        { 
            tileObject.SetActive(false);
            tileUnitObject.SetActive(false);
            return;
        }
        
        
        tileObject.GetComponentInChildren<Text>().text = tile.tileName;
        tileObject.SetActive(true);

        if (tile.OccupiedUnit)
        {
            tileUnitObject.GetComponentInChildren<Text>().text = tile.OccupiedUnit.UnitName;
            tileUnitObject.SetActive(true);
        }
    }

}
