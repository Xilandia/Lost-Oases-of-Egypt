using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NewGameOverScreen : MonoBehaviour
{
    // hH - hero's Health
    // uS - units Survived
    // r - resources
    // eK - enemies Killed


    // Text - Values
    public Text hHVT;
    public Text uSVT;
    public Text rVT;
    public Text eKVT;

    // Text - Multipliers
    public Text hHMT;
    public Text uSMT;
    public Text rMT;
    public Text eKMT;

    // Text - Total Value
    public Text hHTT;
    public Text uSTT;
    public Text rTT;
    public Text eKTT;

    // Variables - Values
    public double hHV = 250;
    public double uSV = 23;
    public double rV = 598;
    public double eKV = 321;

    // Variables - Multipliers Values
    public double hHMV = 1.25;
    public double uSMV = 2.0;
    public double rMV = 0.5;
    public double eKMV = 0.25;

    // Variables - Total Values
    public double hHTotal;
    public double uSTotal;
    public double rTotal;
    public double eKTotal;


    public Text totalPointsText;
    public double totalPoints;


    public void Setup(int score)
    {
        gameObject.SetActive(true);

        //Set Text for the values
        hHVT.text = hHTotal.ToString();
        uSVT.text = uSTotal.ToString();
        rVT.text = rTotal.ToString();
        eKVT.text = eKTotal.ToString();

        //Set Text for the multipliers
        hHMT.text = hHMV.ToString();
        uSMT.text = uSMT.ToString();
        rMT.text = rMV.ToString();
        eKMT.text = eKMV.ToString();

        //Calculate Total
        hHTotal = hHV * hHMV;
        uSTotal = uSV * uSMV;
        rTotal = rV * rMV;
        eKTotal = eKV * eKMV;

        //Set Text for the totals
        hHTT.text = hHTotal.ToString();
        uSTT.text = uSTotal.ToString();
        rTT.text = rTotal.ToString();
        eKTT.text = eKTotal.ToString();

        //Calculate Total Points
        totalPoints = hHTotal + uSTotal + rTotal + eKTotal;

        totalPointsText.text = totalPoints.ToString() + "POINTS";

    }

public void RestartButton()
    {
        SceneManager.LoadScene("Game");
    }

    public void ExitButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}