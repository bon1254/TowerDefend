using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHealthFacingCam : MonoBehaviour
{
    public Camera cameraToLookAt;
    public GameObject UIHpCanvas;

    void Start()
    {
        cameraToLookAt = GameObject.Find("-----SceneObjects-----/Main Camera").GetComponent<Camera>();
    }

    void Update()
    {
        transform.LookAt(transform.position + cameraToLookAt.transform.rotation * Vector3.forward, cameraToLookAt.transform.rotation * Vector3.up);
    }
}
