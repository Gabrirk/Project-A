using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState gameState;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ChangeState(GameState.GenerateGrid);
    }

    public void ChangeState(GameState newState)
    {
        gameState = newState;
        switch (newState)
        {
            case GameState.GenerateGrid:
                GridManager.Instance.GenerateGrid();
                break;
            case GameState.SpawnHeroes:
                UnitManager.Instance.SpawnHeroes();
                break;
            case GameState.SpawnEnemies:
                UnitManager.Instance.SpawnEnemies();
                break;
            case GameState.HeroesTurn:
                StartHeroTurn();
                break;
            case GameState.EnemiesTurn:
                StartEnemyTurn();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
                    
        }
    }
    private void StartHeroTurn()
    {
        foreach (BaseHero hero in UnitManager.Instance.GetHeroes())
        {
            hero.StartTurn();
        }
    }

    private void StartEnemyTurn()
    {
        foreach (BaseEnemy enemy in UnitManager.Instance.GetEnemies())
        {
            enemy.StartTurn();
        }

        // Here you could add logic for enemies to take their turn
    }

    public enum GameState
    {
        GenerateGrid = 0,
        SpawnHeroes = 1,
        SpawnEnemies = 2,
        HeroesTurn = 3,
        EnemiesTurn = 4
    }




}
