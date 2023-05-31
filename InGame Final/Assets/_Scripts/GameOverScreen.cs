using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    public Text timeSurvived;

    public void Setup(int time)
    {
        gameObject.SetActive(true);
        timeSurvived.text = time.ToString() + "Time Survived";
    }
}
