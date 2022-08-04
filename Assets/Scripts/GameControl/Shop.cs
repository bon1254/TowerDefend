using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [Header("TurretBluePrints")]
    public TurretBluePrint StandardTurret;
    public TurretBluePrint IceTurret;
    public TurretBluePrint FlameTurret;
    public TurretBluePrint LaserTurret;
    public TurretBluePrint MissleTurret;

    [Header("Purchase")]
    public Image[] CostImages;
    public Color HasMoney;
    public Color NotEnoughMoney;

    BuildManager buildManager;
    void Start()
    {
        buildManager = BuildManager.instance;
    }

    void Update()
    {
        if (PlayerStats.Money >= 100) //StandardTurret
        {
            CostImages[0].color = HasMoney;
        }
        else
        {
            CostImages[0].color = NotEnoughMoney;
        }
        
        if (PlayerStats.Money >= 150) //IceTurret
        {
            CostImages[1].color = HasMoney;
        }
        else
        {
            CostImages[1].color = NotEnoughMoney;
        }

        if (PlayerStats.Money >= 200) //FlameThrower
        {
            CostImages[2].color = HasMoney;
        }
        else
        {
            CostImages[2].color = NotEnoughMoney;
        }

        if (PlayerStats.Money >= 250) //LaserTurret
        {
            CostImages[3].color = HasMoney;
        }
        else
        {
            CostImages[3].color = NotEnoughMoney;
        }

        if (PlayerStats.Money >= 350) //MissleTurret
        {
            CostImages[4].color = HasMoney;
        }
        else
        {
            CostImages[4].color = NotEnoughMoney;
        }
    }

    public void SelectStandardTurret()
    {
        buildManager.SelectTurretToBuild(StandardTurret);
    }

    public void SelectIceTurret()
    {
        buildManager.SelectTurretToBuild(IceTurret);
    }

    public void SelectFlameTurret()
    {
        buildManager.SelectTurretToBuild(FlameTurret);
    }

    public void SelectLaserTurret()
    {
        buildManager.SelectTurretToBuild(LaserTurret);
    }

    public void SelectMissleTurret()
    {
        buildManager.SelectTurretToBuild(MissleTurret);
    }
}
