using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoadLevel(int level)
    {
        switch (level)
        {
            case 1:
                SceneManager.LoadScene("PvP_1_VS_1 Castle");
                break;
            case 2:
                SceneManager.LoadScene("PvP_1_VS_1 Flat");
                break;
            case 3:
                SceneManager.LoadScene("PvC_1_VS_1 Castle (Easy)");
                break;
            case 4:
                SceneManager.LoadScene("PvC_1_VS_1 Castle (Medium)");
                break;
            case 5:
                SceneManager.LoadScene("PvC_1_VS_1 Castle (Hard)");
                break;
            case 6:
                SceneManager.LoadScene("PvC_1_VS_1 Castle (Medium) Clon");
                break;
            case 7:
                SceneManager.LoadScene("PvC_1_VS_1 Flat (Easy)");
                break;
            case 8:
                SceneManager.LoadScene("PvC_1_VS_1 Flat (Medium)");
                break;
            case 9:
                SceneManager.LoadScene("PvC_1_VS_1 Flat (Hard)");
                break;
            case 10:
                SceneManager.LoadScene("PvC_1_VS_1 Flat (Medium) Clon");
                break;
            case 11:
                SceneManager.LoadScene("CvC_1_VS_1 Castle (Easy)");
                break;
            case 12:
                SceneManager.LoadScene("CvC_1_VS_1 Castle (Easy) 1");
                break;
            case 13:
                SceneManager.LoadScene("CvC_1_VS_1 Castle (Easy) 2");
                break;
            case 14:
                SceneManager.LoadScene("CvC_1_VS_1 Castle (Medium)");
                break;
            case 15:
                SceneManager.LoadScene("CvC_1_VS_1 Castle (Medium) 1");
                break;
            case 16:
                SceneManager.LoadScene("CvC_1_VS_1 Castle (Hard)");
                break;
            case 17:
                SceneManager.LoadScene("CvC_1_VS_1 Castle (Easy) 3");
                break;
            case 18:
                SceneManager.LoadScene("CvC_1_VS_1 Castle (Medium) 2");
                break;
            case 19:
                SceneManager.LoadScene("CvC_1_VS_1 Castle (Hard) 1");
                break;
            case 20:
                SceneManager.LoadScene("CvC_1_VS_1 Castle (Medium) Clon");
                break;
            case 21:
                SceneManager.LoadScene("CvC_1_VS_1 Flat (Easy)");
                break;
            case 22:
                SceneManager.LoadScene("CvC_1_VS_1 Flat (Easy) 1");
                break;
            case 23:
                SceneManager.LoadScene("CvC_1_VS_1 Flat (Easy) 2");
                break;
            case 24:
                SceneManager.LoadScene("CvC_1_VS_1 Flat (Medium)");
                break;
            case 25:
                SceneManager.LoadScene("CvC_1_VS_1 Flat (Medium) 1");
                break;
            case 26:
                SceneManager.LoadScene("CvC_1_VS_1 Flat (Hard)");
                break;
            case 27:
                SceneManager.LoadScene("CvC_1_VS_1 Flat (Easy) 3");
                break;
            case 28:
                SceneManager.LoadScene("CvC_1_VS_1 Flat (Medium) 2");
                break;
            case 29:
                SceneManager.LoadScene("CvC_1_VS_1 Flat (Hard) 1");
                break;
            case 30:
                SceneManager.LoadScene("CvC_1_VS_1 Flat (Medium) Clon");
                break;
            default:
                Debug.LogWarning("Invalid level: " + level);
                break;
        }
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            UnityEngine.Application.Quit();
        #endif
    }
}
