using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LivesUI : MonoBehaviour
{
    public TMP_Text LivesText;

    // Start is called before the first frame update
    void Update()
    {
        LivesText.text = "Lives: " + PlayerStats.Lives.ToString();
    }
}
