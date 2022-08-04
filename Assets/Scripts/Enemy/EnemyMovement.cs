using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Enemy))]
public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent navAgeng;

    private Transform target;

    private Enemy enemy;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("-----SceneObjects-----/EndPlane").transform;
        enemy = GetComponent<Enemy>();
        navAgeng = GetComponent<NavMeshAgent>();
    }

    void FixedUpdate()
    {
        navAgeng.SetDestination(target.position);

        enemy.speed = enemy.StartSpeed;
    }

    void EndPath()
    {
        PlayerStats.Lives--;
        WaveSpawner.EnemiesAlive--;
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EndPlane")
        {
            EndPath();
        }    
    }
}
