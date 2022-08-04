using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ExpUI : MonoBehaviour
{
    public TMP_Text ExpText;

    void Update()
    {
        ExpText.text = PlayerStats.Exp.ToString();
    }
}
