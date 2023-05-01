using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using URandom = UnityEngine.Random;

public enum TeamColor { NEUTRAL, BLUE, RED, GREEN, YELLOW }
public enum GameState { START, PLAYERTURN, CPUTURN, CPU2TURN, CPU3TURN, CPU4TURN, WON, LOST, END }

public class Gameplay : MonoBehaviour
{
    public static TeamColor activeTeam;

    private List<TeamColor> validTeamColors = new List<TeamColor> { TeamColor.BLUE, TeamColor.RED, TeamColor.GREEN, TeamColor.YELLOW };
    private int currentTeamIndex;

    private float turnDuration = 60f;
    private float turnTimer;
	public static int turnTimerSeconds;

    void Start()
    {
        turnTimer = turnDuration;
		turnTimerSeconds = Mathf.RoundToInt(turnTimer);
        currentTeamIndex = URandom.Range(0, validTeamColors.Count);
        activeTeam = validTeamColors[currentTeamIndex];
        StartTurn();
    }

    void Update()
    {
        turnTimer -= Time.deltaTime;
		turnTimerSeconds = Mathf.RoundToInt(turnTimer);

        if (turnTimer <= 0)
        {
            EndTurn();
        }

        // Here you can implement team actions based on the active team
    }

    void StartTurn()
    {
        turnTimer = turnDuration;
    }

    void EndTurn()
    {
        currentTeamIndex = (currentTeamIndex + 1) % validTeamColors.Count;
        activeTeam = validTeamColors[currentTeamIndex];

        StartTurn();
    }
}
