using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TurretBluePrint
{
    public GameObject prefab;
    public int cost;

    public GameObject UpgradePrefab;
    public int UpgradeCost;

    public int GetSellAmount()
    {
        return cost / 2;
    }
}
