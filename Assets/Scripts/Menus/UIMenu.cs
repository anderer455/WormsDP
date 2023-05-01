using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMenu : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI timerText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timerText.text = Gameplay.turnTimerSeconds.ToString();
        switch (Gameplay.activeTeam) {
            case TeamColor.BLUE:
                image.color = Color.blue;
                break;
            case TeamColor.RED:
                image.color = Color.red;
                break;
            case TeamColor.GREEN:
                image.color = Color.green;
                break;
            case TeamColor.YELLOW:
                image.color = Color.yellow;
                break;
            default:
                image.color = Color.white;
                break;
        }
    }
}
