using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KillsUI : MonoBehaviour
{
    public TMP_Text KillsText;

    void Update()
    {
        KillsText.text = PlayerStats.Kills.ToString();
    }
}
