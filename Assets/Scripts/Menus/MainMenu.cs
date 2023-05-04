using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SetPvP() {
        Gameplay.activeGameMode = GameMode.PVP;
    }

    public void SetPvC() {
        Gameplay.activeGameMode = GameMode.PVC;
    }

    public void SetCvC() {
        Gameplay.activeGameMode = GameMode.CVC;
    }

    public void SetEasy() {
        Gameplay.activeGameDifficulty = GameDifficulty.EASY;
        //TODO load easy onnx onto agent
    }

    public void SetMedium() {
        Gameplay.activeGameDifficulty = GameDifficulty.MEDIUM;
        //TODO load medium onnx onto agent
    }

    public void SetHard() {
        Gameplay.activeGameDifficulty = GameDifficulty.HARD;
        //TODO load hard onnx onto agent
    }

    public void QuitGame() {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
