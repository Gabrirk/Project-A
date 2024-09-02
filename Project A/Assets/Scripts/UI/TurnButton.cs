using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnButton : MonoBehaviour
{
    public Button endTurnButton;

    private void Start()
    {
        // Attach the EndTurn method to the button's onClick event
        endTurnButton.onClick.AddListener(EndTurn);
    }

    private void EndTurn()
    {
        // Check the current game state and switch to the next state
        if (GameManager.Instance.gameState == GameManager.GameState.HeroesTurn)
        {
            GameManager.Instance.ChangeState(GameManager.GameState.EnemiesTurn);
        }
        else if (GameManager.Instance.gameState == GameManager.GameState.EnemiesTurn)
        {
            GameManager.Instance.ChangeState(GameManager.GameState.HeroesTurn);
        }
    }
}