using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TeamColor { NEUTRAL, BLUE, RED, GREEN, YELLOW }
public enum GameState { START, PLAYERTURN, CPUTURN, CPU2TURN, CPU3TURN, CPU4TURN, WON, LOST, END }

public class Gameplay : MonoBehaviour
{
    private TeamColor activePlayer;

    public GameObject playerPrefab;
	public GameObject enemyPrefab;

	Worm playerUnit;
	Worm enemyUnit;

	public GameState state;

    // Start is called before the first frame update
    void Start()
    {
		state = GameState.START;
		System.Random random = new System.Random();
		Type type = typeof(TeamColor);
		Array values = type.GetEnumValues();
		int index = random.Next(values.Length);
		activePlayer = (TeamColor)values.GetValue(index);
		
		if (activePlayer == TeamColor.NEUTRAL)
		{
			disableEnemyWorm();
		}
		else
		{
			disablePlayerWorm();
		}
		//StartCoroutine();
    }

	void Update()
	{
		if (activePlayer == TeamColor.NEUTRAL)
		{
			activePlayer = TeamColor.BLUE;
		}
		else
		{
			activePlayer = TeamColor.NEUTRAL;
		}
	}

	void disableEnemyWorm() {}
	void disablePlayerWorm() {}
}
