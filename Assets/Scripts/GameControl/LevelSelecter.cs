using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelSelecter : MonoBehaviour
{
    SoundsManager soundsManager;
    public SceneFader sceneFader;

    public GameObject levelBtnPrefab;
    public GameObject levelBtnGrid;

    public int highestLevel = 3;

    private void Awake()
    {
        soundsManager =  GameObject.Find("SoundsManager").GetComponent<SoundsManager>();
    }

    void Start()
    {
        int HighestLevel = PlayerPrefs.GetInt("HigheseLevel");

        for (int i = 0; i <= HighestLevel + 1; i++)
        {
            GameObject pre = Instantiate(levelBtnPrefab, levelBtnGrid.transform);
            pre.GetComponent<Button>().GetComponentInChildren<Text>().text = (i + 1).ToString();
            pre.GetComponent<Button>().onClick.AddListener(delegate { soundsManager.ButtonClick(); });
            pre.GetComponent<Button>().onClick.AddListener(delegate { sceneFader.FadeTo("Level" + pre.GetComponent<Button>().GetComponentInChildren<Text>().text); });
        }

        Invoke("Init", 0.1f);
    }

    private void Init()
    {
        Transform lastChild = levelBtnGrid.transform.GetChild(levelBtnGrid.transform.childCount - 1);

        lastChild.GetComponent<Button>().interactable = false;
        lastChild.GetComponent<Button>().image.raycastTarget = false;
        lastChild.GetComponent<Button>().GetComponentInChildren<Text>().raycastTarget = false;
    }
}
