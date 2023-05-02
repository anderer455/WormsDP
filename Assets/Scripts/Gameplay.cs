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

    public static List<TeamColor> validTeamColors = new List<TeamColor> { TeamColor.BLUE, TeamColor.RED, TeamColor.GREEN, TeamColor.YELLOW };
    private int currentTeamIndex;

    private float turnDuration = 60f;
    private float turnTimer;
	public static int turnTimerSeconds;

    private int randomSign;

    void Start()
    {
        currentTeamIndex = -1;
        //currentTeamIndex = URandom.Range(-1, validTeamColors.Count-1);
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
		turnTimerSeconds = Mathf.RoundToInt(turnTimer);
        currentTeamIndex = (currentTeamIndex + 1) % validTeamColors.Count;
        activeTeam = validTeamColors[currentTeamIndex];
    }

    void EndTurn()
    {
        StartTurn();
    }

    public void EpisodeBegin(Transform subject, Transform targetTransform)
    {
        randomSign = URandom.Range(0, 2) * 2 - 1;

        if (randomSign == -1) {
            subject.localPosition = new Vector3(URandom.Range(-17f, 0f), 8.2f, 0f);
            targetTransform.localPosition = new Vector3(URandom.Range(0f, 17.5f), 8.2f, 0f);
        } else {
            subject.localPosition = new Vector3(URandom.Range(0f, 17.5f), 8.2f, 0f);
            targetTransform.localPosition = new Vector3(URandom.Range(-17f, 0f), 8.2f, 0f);
        }
    }
}
