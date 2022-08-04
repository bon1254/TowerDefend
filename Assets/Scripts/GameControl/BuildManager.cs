using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;

    void Awake()
    {
        if (instance = null)
        {
            Debug.LogError("More than One BuildManager here");
            return;
        }
        instance = this;
    }

    public GameObject BuildEffect;
    public GameObject SellEffect;

    private TurretBluePrint TurretToBuild;
    private Node SelectedNode;

    public NodeUI nodeUI;

    private Node node;

    public bool CanBuild
    { 
        get 
        { 
            return TurretToBuild != null;
        } 
    }
    public bool HasMoney
    {
        get
        { 
            return PlayerStats.Money >= TurretToBuild.cost; 
        }
    }


    public void SelectNode(Node node)
    {
        if (SelectedNode == node)
        {
            DeselectNode();
            return;
        }

        SelectedNode = node;
        TurretToBuild = null;

        nodeUI.SetTarget(node);
    }

    public void DeselectNode()
    {
        SelectedNode = null;
        nodeUI.Hide();
    }

    public void SelectTurretToBuild(TurretBluePrint turret)
    {
        TurretToBuild = turret;
        DeselectNode();
    }

    public TurretBluePrint GetTurretToBuild()
    {
        return TurretToBuild;
    }
}
