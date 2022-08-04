using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorRadius : MonoBehaviour
{
    public GameObject RangeSphere;
    public bool isClickedOnTurret = false;

    void OnMouseUpAsButton()
    {
        RangeSphere.SetActive(true);
    }

    void OnMouseDown()
    {
        RangeSphere.SetActive(false);
    }
}
