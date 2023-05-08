using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Winner : MonoBehaviour
{
    [SerializeField]
    public GameObject blueWinnerPrefab;
    [SerializeField]
    public GameObject yellowWinnerPrefab;

    public GameObject winnerMenu;
    public static bool isWinner = false;
    public static bool blueOrYellow = false;

    // Start is called before the first frame update
    void Start()
    {
        winnerMenu.SetActive(false);
        isWinner = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isWinner) {
            if (blueOrYellow) {
                Instantiate(blueWinnerPrefab, winnerMenu.transform.position, Quaternion.identity, winnerMenu.transform);
            } else {
                Instantiate(yellowWinnerPrefab, winnerMenu.transform.position, Quaternion.identity, winnerMenu.transform);
            }
            winnerMenu.SetActive(true);
        }
    }
}
