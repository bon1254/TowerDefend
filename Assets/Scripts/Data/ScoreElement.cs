using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreElement : MonoBehaviour
{
    public Text UsernameText;
    public Text ExpText;
    public Text KillsText;
    public Text LossText;
    public Text HighestLevel;

    public void NewScoreElement(string _Username, int _Kills, int _Loss, int _Exp, int _HighestLevel)
    {
        UsernameText.text = _Username;
        ExpText.text = _Exp.ToString();
        KillsText.text = _Kills.ToString();
        LossText.text = _Loss.ToString();
        HighestLevel.text = _HighestLevel.ToString();
    }

}