using TMPro;
using UnityEngine;

public class gameEndManager : MonoBehaviour
{
    public TextMeshPro totalTimePlayedText;
    public TextMeshPro totalDeathsText;
    private void Start()
    {
        totalTimePlayedText.text = "Game completed in " + (finalResultManager.totalTimePlayed / 60).ToString("F2") + " minutes";
        totalDeathsText.text = "Total Deaths: " + finalResultManager.deaths.ToString();
        Destroy(finalResultManager.instance.gameObject);
    }
}
