using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using URandom = UnityEngine.Random;

public enum TeamColor { BLUE, YELLOW }
public enum GameMode { PVP, PVC, CVC }
public enum GameDifficulty { EASY, MEDIUM, HARD }
public enum GameState { START, PLAYERTURN, CPUTURN, CPU2TURN, WON, LOST, END }
public enum GameMap { NONE, CASTLE, CHEESE, DONKEY, JUNGLE, LABORATORY, MARS, TANK, TERRAIN, WINDMILL }

public class Gameplay : MonoBehaviour
{
    [SerializeField]
    public static GameMode activeGameMode;
    [SerializeField]
    public static GameDifficulty activeGameDifficulty;
    [SerializeField]
    public GameMap activeMap;
    [SerializeField]
    [Range(0, 2)]
    public int numberOfComputers;
    public static TeamColor activeTeamColor;
    public static GameState activeGameState;

    [SerializeField]
    public Map castlePrefab;
    [SerializeField]
    public Map cheesePrefab;
    [SerializeField]
    public Map donkeyPrefab;
    [SerializeField]
    public Map junglePrefab;
    [SerializeField]
    public Map laboratoryPrefab;
    [SerializeField]
    public Map marsPrefab;
    [SerializeField]
    public Map tankPrefab;
    [SerializeField]
    public Map terrainPrefab;
    [SerializeField]
    public Map windmillPrefab;

    public static List<TeamColor> validTeamColors = new List<TeamColor> { TeamColor.BLUE, TeamColor.YELLOW };
    private int currentTeamIndex;

    private float turnDuration = 60f;
    private static float turnTimer;
    public static float TurnTimer
    {
        get { return turnTimer; }
        set { turnTimer = value; }
    }
	public static int turnTimerSeconds;

    private int randomSign;

    void Start()
    {
        SetGameStateAtStart();
        SetGameMap();

        StartTurn();
    }

    void Update()
    {
        CheckGameEnd();
        turnTimer -= Time.deltaTime;
		turnTimerSeconds = Mathf.RoundToInt(turnTimer);

        if (turnTimer <= 0)
        {
            EndTurn();
        }

        // Here you can implement team actions based on the active team
    }

    void SetGameStateAtStart() {
        activeGameState = GameState.START;
        currentTeamIndex = -1;
        //currentTeamIndex = URandom.Range(-1, validTeamColors.Count-1);
        switch (activeGameMode) {
            case GameMode.PVP:
                numberOfComputers = 0;
                break;
            case GameMode.PVC:
                if (numberOfComputers > 1) {
                    numberOfComputers = 1;
                }
                break;
            case GameMode.CVC:
                if (numberOfComputers < 2) {
                    numberOfComputers = 2;
                }
                break;
            default:
                activeGameMode = GameMode.PVC;
                numberOfComputers = 1;
                break;
        }
    }

    void SetGameMap() {
        GameObject mapObject = GameObject.Find("Map");

        if (mapObject != null)
        {
            switch (activeMap) {
            case GameMap.CASTLE:
                Instantiate(castlePrefab, mapObject.transform.position, Quaternion.identity);
                break;
            case GameMap.CHEESE:
                Instantiate(cheesePrefab, mapObject.transform.position, Quaternion.identity);
                break;
            case GameMap.DONKEY:
                Instantiate(donkeyPrefab, mapObject.transform.position, Quaternion.identity);
                break;
            case GameMap.JUNGLE:
                Instantiate(junglePrefab, mapObject.transform.position, Quaternion.identity);
                break;
            case GameMap.LABORATORY:
                Instantiate(laboratoryPrefab, mapObject.transform.position, Quaternion.identity);
                break;
            case GameMap.MARS:
                Instantiate(marsPrefab, mapObject.transform.position, Quaternion.identity);
                break;
            case GameMap.TANK:
                Instantiate(tankPrefab, mapObject.transform.position, Quaternion.identity);
                break;
            case GameMap.TERRAIN:
                Instantiate(terrainPrefab, mapObject.transform.position, Quaternion.identity);
                break;
            case GameMap.WINDMILL:
                Instantiate(windmillPrefab, mapObject.transform.position, Quaternion.identity);
                break;
            case GameMap.NONE:
            default:
                Map[] myMaps = { castlePrefab, cheesePrefab, donkeyPrefab, junglePrefab, laboratoryPrefab, marsPrefab, tankPrefab, terrainPrefab, windmillPrefab };
                int randomMap = URandom.Range(0, myMaps.Length);
                Instantiate(myMaps[randomMap], mapObject.transform.position, Quaternion.identity);
                break;
            }
        }
        else
        {
            Debug.LogError("Object not found");
        }
    }

    void StartTurn()
    {
        turnTimer = turnDuration;
		turnTimerSeconds = Mathf.RoundToInt(turnTimer);
        currentTeamIndex = (currentTeamIndex + 1) % validTeamColors.Count;
        activeTeamColor = validTeamColors[currentTeamIndex];

        switch (activeTeamColor) {
            case TeamColor.BLUE:
                if (activeGameMode == GameMode.PVP || activeGameMode == GameMode.PVC) {
                    activeGameState = GameState.PLAYERTURN;
                } else {
                    activeGameState = GameState.CPUTURN;
                }
                break;
            case TeamColor.YELLOW:
                if (activeGameMode == GameMode.PVP || activeGameMode == GameMode.PVC) {
                    activeGameState = GameState.CPUTURN;
                } else {
                    activeGameState = GameState.CPU2TURN;
                }
                break;
            default:
                activeTeamColor = TeamColor.BLUE;
                activeGameState = GameState.PLAYERTURN;
                break;
        }
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

    void CheckGameEnd() {
        //TODO
    }
}
