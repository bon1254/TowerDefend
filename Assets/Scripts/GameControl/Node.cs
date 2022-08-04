using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class Node : MonoBehaviour
{
    public Material StartColor;
    public Material hoverColor;
    public Material NotEnoughMoneyColor; 

    public Vector3 PositionOffset;

    [HideInInspector]
    public GameObject Turret;
    [HideInInspector]
    public TurretBluePrint turretBluePrint;
    [HideInInspector]
    public bool IsUpgraded = false;

    private Renderer rend;

    BuildManager buildManager;
    SoundsManager soundsManager;
    WaveSpawner waveSpawner;

    void Start()
    {
        soundsManager = FindObjectOfType<SoundsManager>().GetComponent<SoundsManager>();
        waveSpawner = FindObjectOfType<WaveSpawner>().GetComponent<WaveSpawner>();
        rend = GetComponent<Renderer>();
        buildManager = BuildManager.instance;     
    }   

    public Vector3 GetBuildPostion()
    {
        return transform.position + PositionOffset;
    }

    void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if(Turret != null) //Select turret which on current node
        {
            buildManager.SelectNode(this);
            return;    
        }

        if (!buildManager.CanBuild)
        {
            return;
        }

        BuildTurret(buildManager.GetTurretToBuild());
    }

    public void BuildTurret(TurretBluePrint blueprint)
    {
        if (PlayerStats.Money < blueprint.cost)
        {
            Debug.Log("Not Enough Money");
            return;
        }

        if (waveSpawner.countdown <= 0)
        {
            Debug.Log("Enemy is Coming, you can't build it!");
            return;
        }

        PlayerStats.Money -= blueprint.cost;

        GameObject turret = Instantiate(blueprint.prefab, GetBuildPostion(), Quaternion.identity);
        Turret = turret;

        turretBluePrint = blueprint;

        //adjust effect rotation
        Quaternion adjustRota = Quaternion.Euler(-90, 0, 0);

        //adjust effect position
        Vector3 adjustPos = new Vector3(transform.position.x, 0.2f, transform.position.z);

        //build effect
        GameObject Effect = Instantiate(buildManager.BuildEffect, adjustPos, adjustRota);
        Destroy(Effect, 5f);

        //play sounds
        soundsManager.BuildingSound();

        Debug.Log("Turret Build!");
    }

    public void UpgradeTurret()
    {
        if (PlayerStats.Money < turretBluePrint.UpgradeCost)
        {
            Debug.Log("Not enough money to upgrade that!");
            return;
        }

        PlayerStats.Money -= turretBluePrint.UpgradeCost;

        //Delect old turret
        Destroy(Turret);

        //adjust effect rotation
        Quaternion adjustRota = Quaternion.Euler(-90, 0, 0);

        //adjust effect position
        Vector3 adjustPos = new Vector3(transform.position.x, 0.2f, transform.position.z);

        //Build a new one
        GameObject turret = (GameObject)Instantiate(turretBluePrint.UpgradePrefab, GetBuildPostion(), Quaternion.identity);
        Turret = turret;

        GameObject Effect = (GameObject)Instantiate(buildManager.BuildEffect, adjustPos, adjustRota);
        Destroy(Effect, 5f);

        IsUpgraded = true;

        Debug.Log("Turret Upgraded!");
    }

    public void SellTurret()
    {
        PlayerStats.Money += turretBluePrint.GetSellAmount();

        GameObject Effect = (GameObject)Instantiate(buildManager.SellEffect, GetBuildPostion(), Quaternion.identity);
        Destroy(Effect, 5f);

        Destroy(Turret);
        turretBluePrint = null;
    }

    void OnMouseEnter()
    {
        if(EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (!buildManager.CanBuild)
        {
            return;
        }

        if (buildManager.HasMoney)
        {
            rend.material = hoverColor;
        }
        else
        {
            rend.material = NotEnoughMoneyColor;
        }

        if (waveSpawner.countdown <= 0)
        {
            rend.material = NotEnoughMoneyColor;
        }
    }

    void OnMouseExit()
    {
        rend.material = StartColor;
    }
}
