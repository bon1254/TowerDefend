using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshManager : MonoBehaviour
{
    public NavMeshSurface[] surfaces;
    public bool isBaked = false;
    // Update is called once per frame
    public void UpdateBuildNavmesh()

    {   if (isBaked == false)
        {
            for (int i = 0; i < surfaces.Length; i++)
            {
                surfaces[i].BuildNavMesh();
                isBaked = true;
            }
        }     
    }
}
