using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using URandom = UnityEngine.Random;

public enum TeamColor { NEUTRAL, BLUE, YELLOW }
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
    public GameObject activeWorm;

    private float turnDuration = 60f;
    private static float turnTimer;
    public static float TurnTimer {
        get { return turnTimer; }
        set { turnTimer = value; }
    }
	public static int turnTimerSeconds;

    private int randomSign;
    private Vector3[] mapStartingPoints;


    void Start() {
        SetGameStateAtStart();
        SetGameMap();
        StartTurn();
    }

    void Update() {
        CheckGameEnd();
        turnTimer -= Time.deltaTime;
		turnTimerSeconds = Mathf.RoundToInt(turnTimer);

        if (turnTimer <= 0) {
            EndTurn();
        }
    }

    void SetGameStateAtStart() {
        activeGameState = GameState.START;
        currentTeamIndex = URandom.Range(0, validTeamColors.Count);
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

        if (mapObject != null) {

            for (int i = mapObject.transform.childCount - 1; i >= 0; i--) {
                Destroy(mapObject.transform.GetChild(i).gameObject);
            }

            switch (activeMap) {
                case GameMap.CASTLE:
                    mapStartingPoints = new Vector3[] {
                        new Vector3(16.3f, -5.9f, 0),
                        new Vector3(13.5f, -4.75f, 0),
                        new Vector3(13f, -0.15f, 0),
                        new Vector3(9.45f, 0.733f, 0),
                        new Vector3(5.78f, 1.05f, 0),
                        new Vector3(1.67f, 1.52f, 0),
                        new Vector3(-2.48f, 2.38f, 0),
                        new Vector3(-7.58f, 2.22f, 0),
                        new Vector3(-11.65f, -2.53f, 0),
                        new Vector3(-14.1f, -3.8f, 0),
                        new Vector3(-17.5f, -6.05f, 0)
                    };
                    Instantiate(castlePrefab, mapObject.transform.position, Quaternion.identity, mapObject.transform);
                    break;
                case GameMap.CHEESE:
                    mapStartingPoints = new Vector3[] {
                        new Vector3(-18, 6.2f, 0),
                        new Vector3(-14, 3.1f, 0),
                        new Vector3(-9.5f, 4.5f, 0),
                        new Vector3(-15.3f, -5.3f, 0),
                        new Vector3(-11, -2.4f, 0),
                        new Vector3(-5.31f, 0.68f, 0),
                        new Vector3(-1.35f, 0.52f, 0),
                        new Vector3(0.16f, -4.83f, 0),
                        new Vector3(5.2f, 0.95f, 0),
                        new Vector3(16.4f, -4.4f, 0),
                        new Vector3(16.6f, 1.4f, 0),
                        new Vector3(8.25f, -3.14f, 0),
                        new Vector3(11.8f, -3.2f, 0),
                        new Vector3(-6.8f, 6.22f, 0),
                        new Vector3(-3.22f, 6.22f, 0),
                        new Vector3(-0.26f, 6.22f, 0),
                        new Vector3(2.46f, 4.58f, 0),
                        new Vector3(6.7f, 6.2f, 0),
                        new Vector3(11.3f, 3.7f, 0),
                        new Vector3(16.44f, 6.23f, 0)
                    };
                    Instantiate(cheesePrefab, mapObject.transform.position, Quaternion.identity, mapObject.transform);
                    break;
                case GameMap.DONKEY:
                    mapStartingPoints = new Vector3[] {
                        new Vector3(-15.75f, -3.5f, 0),
                        new Vector3(-12.61f, -1.14f, 0),
                        new Vector3(-8.3f, -1.11f, 0),
                        new Vector3(-3.02f, -2.41f, 0),
                        new Vector3(0.28f, -3.12f, 0),
                        new Vector3(-2.84f, 5.19f, 0),
                        new Vector3(-1, 2.75f, 0),
                        new Vector3(2.85f, 3.31f, 0),
                        new Vector3(5.04f, -2.71f, 0),
                        new Vector3(10.17f, -1.43f, 0),
                        new Vector3(14.05f, -4.36f, 0),
                        new Vector3(17.35f, -5.13f, 0)
                    };
                    Instantiate(donkeyPrefab, mapObject.transform.position, Quaternion.identity, mapObject.transform);
                    break;
                case GameMap.JUNGLE:
                    mapStartingPoints = new Vector3[] {
                        new Vector3(-18.08f, 2.82f, 0),
                        new Vector3(-16.83f, -0.53f, 0),
                        new Vector3(-16.04f, 5.18f, 0),
                        new Vector3(-14.25f, 2.42f, 0),
                        new Vector3(-13.61f, -0.14f, 0),
                        new Vector3(-11.47f, -3.8f, 0),
                        new Vector3(-10.89f, 4.99f, 0),
                        new Vector3(-6.28f, 2.69f, 0),
                        new Vector3(-5.14f, -2.58f, 0),
                        new Vector3(-5.21f, 6.01f, 0),
                        new Vector3(-2.36f, 4.78f, 0),
                        new Vector3(-1.3f, 0.41f, 0),
                        new Vector3(0, 7.25f, 0),
                        new Vector3(2.21f, 0.72f, 0),
                        new Vector3(3.36f, 3.76f, 0),
                        new Vector3(5.72f, 5.55f, 0),
                        new Vector3(7.78f, 0.1f, 0),
                        new Vector3(10.22f, -4.86f, 0),
                        new Vector3(10.65f, 2.34f, 0),
                        new Vector3(15.42f, -3.74f, 0),
                        new Vector3(15.94f, 5.18f, 0),
                        new Vector3(16.84f, 0.97f, 0)
                    };
                    Instantiate(junglePrefab, mapObject.transform.position, Quaternion.identity, mapObject.transform);
                    break;
                case GameMap.LABORATORY:
                    mapStartingPoints = new Vector3[] {
                        new Vector3(-16.02f, -3.05f, 0),
                        new Vector3(-14.31f, 2.03f, 0),
                        new Vector3(-12.11f, -1.86f, 0),
                        new Vector3(-9.46f, 0.51f, 0),
                        new Vector3(-7.45f, -0.52f, 0),
                        new Vector3(-5.14f, 0.7f, 0),
                        new Vector3(-5.94f, 5.84f, 0),
                        new Vector3(-0.21f, 1.9f, 0),
                        new Vector3(4.93f, 4.61f, 0),
                        new Vector3(8.6f, 6.04f, 0),
                        new Vector3(7.42f, 0.71f, 0),
                        new Vector3(3.37f, -0.3f, 0),
                        new Vector3(8.63f, -0.97f, 0),
                        new Vector3(10.67f, -0.53f, 0),
                        new Vector3(9.68f, 4.09f, 0),
                        new Vector3(15.68f, 0.02f, 0),
                        new Vector3(16.5f, -3.19f, 0)
                    };
                    Instantiate(laboratoryPrefab, mapObject.transform.position, Quaternion.identity, mapObject.transform);
                    break;
                case GameMap.MARS:
                    mapStartingPoints = new Vector3[] {
                        new Vector3(-18.1f, 2.85f, 0),
                        new Vector3(-14.72f, 6.03f, 0),
                        new Vector3(-12.45f, 0.81f, 0),
                        new Vector3(-10.23f, -0.74f, 0),
                        new Vector3(-3.01f, -2.93f, 0),
                        new Vector3(-4.25f, 1.11f, 0),
                        new Vector3(-6.33f, 4.51f, 0),
                        new Vector3(-0.06f, 3.08f, 0),
                        new Vector3(2.55f, 5.19f, 0),
                        new Vector3(7.04f, -3.11f, 0),
                        new Vector3(8.79f, 1.48f, 0),
                        new Vector3(10.61f, 5.85f, 0),
                        new Vector3(13.92f, 1.44f, 0),
                        new Vector3(16.75f, -1.23f, 0),
                        new Vector3(15.05f, -3.74f, 0),
                        new Vector3(13f, -0.37f, 0),
                        new Vector3(13.36f, -2.97f, 0)
                    };
                    Instantiate(marsPrefab, mapObject.transform.position, Quaternion.identity, mapObject.transform);
                    break;
                case GameMap.TANK:
                    mapStartingPoints = new Vector3[] {
                        new Vector3(-14.49f, -2.12f, 0),
                        new Vector3(-16.76f, 0.4f, 0),
                        new Vector3(-12.09f, 7.09f, 0),
                        new Vector3(-8.66f, 4.76f, 0),
                        new Vector3(-6.87f, -2.24f, 0),
                        new Vector3(-3.1f, 2.85f, 0),
                        new Vector3(0.27f, 2.85f, 0),
                        new Vector3(0.67f, -1.57f, 0),
                        new Vector3(4.04f, -1.48f, 0),
                        new Vector3(6.49f, 0.18f, 0),
                        new Vector3(8.46f, 6.17f, 0),
                        new Vector3(12.57f, 3.97f, 0),
                        new Vector3(16.43f, 3.72f, 0),
                        new Vector3(15.17f, -0.18f, 0),
                        new Vector3(18f, -1.44f, 0)
                    };
                    Instantiate(tankPrefab, mapObject.transform.position, Quaternion.identity, mapObject.transform);
                    break;
                case GameMap.TERRAIN:
                    mapStartingPoints = new Vector3[] {
                        new Vector3(-16.45f, -3.42f, 0),
                        new Vector3(-10.63f, -4.47f, 0),
                        new Vector3(-7.28f, -0.33f, 0),
                        new Vector3(-2.62f, 5.99f, 0),
                        new Vector3(-1.62f, -1.53f, 0),
                        new Vector3(8.07f, 2.15f, 0),
                        new Vector3(10.12f, 4.79f, 0),
                        new Vector3(14.04f, -0.2f, 0),
                        new Vector3(12.89f, -3.88f, 0),
                        new Vector3(16.33f, 0.91f, 0),
                        new Vector3(17.42f, -4.08f, 0)
                    };
                    Instantiate(terrainPrefab, mapObject.transform.position, Quaternion.identity, mapObject.transform);
                    break;
                case GameMap.WINDMILL:
                    mapStartingPoints = new Vector3[] {
                        new Vector3(14.82f, -4.28f, 0),
                        new Vector3(11.84f, -1.22f, 0),
                        new Vector3(11.13f, 1.97f, 0),
                        new Vector3(10.12f, 3.456f, 0),
                        new Vector3(8.709f, 4.84f, 0),
                        new Vector3(7.328f, 7.169f, 0),
                        new Vector3(5.39f, 1.98f, 0),
                        new Vector3(3.26f, -3.37f, 0),
                        new Vector3(0.01f, -2.22f, 0),
                        new Vector3(-7f, -2.21f, 0),
                        new Vector3(-3.09f, -2.13f, 0),
                        new Vector3(-4.47f, 4.71f, 0),
                        new Vector3(-6.47f, 7.01f, 0),
                        new Vector3(-9.98f, 6.43f, 0),
                        new Vector3(-10.68f, -1.82f, 0),
                        new Vector3(-14.11f, -1.8f, 0),
                        new Vector3(-16.32f, -5.88f, 0)
                    };
                    Instantiate(windmillPrefab, mapObject.transform.position, Quaternion.identity, mapObject.transform);
                    break;
                case GameMap.NONE:
                default:
                    int randomMap = URandom.Range(0, 9);
                    if (randomMap == 0) { goto case GameMap.CASTLE; }
                    else if (randomMap == 1) { goto case GameMap.CHEESE; }
                    else if (randomMap == 2) { goto case GameMap.DONKEY; }
                    else if (randomMap == 3) { goto case GameMap.JUNGLE; }
                    else if (randomMap == 4) { goto case GameMap.LABORATORY; }
                    else if (randomMap == 5) { goto case GameMap.MARS; }
                    else if (randomMap == 6) { goto case GameMap.TANK; }
                    else if (randomMap == 7) { goto case GameMap.TERRAIN; }
                    else if (randomMap == 8) { goto case GameMap.WINDMILL; }
                    else { Debug.Log("Wrongly generated random map"); }
                    break;
            }
        } else {
            Debug.LogError("Object not found");
        }
    }

    void StartTurn() {
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

    void EndTurn() {
        StartTurn();
    }

    public void EpisodeBegin(Transform subject, Transform enemy) {
        GameObject[] subjects = GetPlayerWorms(subject);
        GameObject[] enemies = GetPlayerWorms(enemy);
        List<Vector3> startingPoints = new List<Vector3>(mapStartingPoints);

        for (int i = 0; i < 4; i++) {
            int startingPoint = URandom.Range(0, startingPoints.Count);
            subjects[i].transform.localPosition = startingPoints[startingPoint];
            startingPoints.RemoveAt(startingPoint);
            
            startingPoint = URandom.Range(0, startingPoints.Count);
            enemies[i].transform.localPosition = startingPoints[startingPoint];
            startingPoints.RemoveAt(startingPoint);
        }

        StartTurn();
    }

    public GameObject[] GetPlayerWorms(Transform player) {
        int wormsCount = player.transform.childCount;
        GameObject[] worms = new GameObject[wormsCount];

        for (int i = 0; i < wormsCount; i++) {
            worms[i] = player.transform.GetChild(i).gameObject;
        }

        return worms;
    }

    void CheckGameEnd() {
        //TODO
    }
}
