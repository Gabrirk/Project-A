using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;

    private List<ScriptableUnit> units;
    public BaseHero SelectedHero;

    private List<BaseHero> spawnedHeroes = new List<BaseHero>();
    private List<BaseEnemy> spawnedEnemies = new List<BaseEnemy>();

    private void Awake()
    {
        Instance = this;

        units = Resources.LoadAll<ScriptableUnit>("Units").ToList();
    }

    public void SpawnHeroes()
    {
        var heroCount = 10;

        for (int i = 0; i < heroCount; i++)
        {
            var randomPrefab = GetRandomUnit<BaseHero>(Faction.Hero);
            var spawnedHero = Instantiate(randomPrefab);
            var randomSpawnTile = GridManager.Instance.GetRandomHeroSpawnTile();

            randomSpawnTile.SetUnit(spawnedHero);
            spawnedHeroes.Add(spawnedHero);

        }

        GameManager.Instance.ChangeState(GameManager.GameState.SpawnEnemies);
    }

    public void SpawnEnemies()
    {
        var enemyCount = 3;

        for (int i = 0; i < enemyCount; i++)
        {
            var randomPrefab = GetRandomUnit<BaseEnemy>(Faction.Enemy);
            var spawnedEnemy = Instantiate(randomPrefab);
            var randomSpawnTile = GridManager.Instance.GetRandomEnemySpawnTile();

            randomSpawnTile.SetUnit(spawnedEnemy);
            spawnedEnemies.Add(spawnedEnemy);

        }

        GameManager.Instance.ChangeState(GameManager.GameState.HeroesTurn);

    }

    private T GetRandomUnit<T>(Faction faction) where T : BaseUnit
    {
        return (T)units.Where(u => u.Faction == faction).OrderBy(o => Random.value).First().UnitPrefab;
    }

    public void SetSelectedHero(BaseHero hero)
    {
        SelectedHero = hero;
        MenuManager.instance.ShowSelectedHero(hero);
    }
    public List<BaseHero> GetHeroes()
    {
        return spawnedHeroes;
    }

    public List<BaseEnemy> GetEnemies()
    {
        return spawnedEnemies;
    }




}
