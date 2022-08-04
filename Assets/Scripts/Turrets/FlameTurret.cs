using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameTurret : MonoBehaviour
{
    public Transform target;
    private Enemy targetEnemy;

    [Header("General")]
    public float range = 15f;

    [Header("Use Flamethrower")]
    public ParticleSystem FlamethrowerEffect;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip ShotSound;

    [Header("Unity Setup Fields")]
    public string enemyTag = "Enemy";
    public Transform partToRotate;
    public float turnSpeed = 0.1f;

    TurretState turretState;

    // Start is called before the first frame update
    void Start()
    {
        turretState = GetComponent<TurretState>();
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortesDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortesDistance)
            {
                shortesDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortesDistance <= range)
        {
            target = nearestEnemy.transform;
            targetEnemy = nearestEnemy.GetComponent<Enemy>();
        }
        else
        {
            target = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (turretState.IsFrozen == true)
        {
            if (FlamethrowerEffect.isPlaying)
            {
                FlamethrowerEffect.Stop();
                audioSource.Stop();
            }
            return;
        }

        if (target == null)
        {
            if (FlamethrowerEffect.isPlaying)
            {
                FlamethrowerEffect.Stop();
                audioSource.Stop();
            }
            return;
        }
        else
        {
            LockOnTarget();
            FlamethrowerEffect.Play();
        }  
    }


    void LockOnTarget()
    {
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);

        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }
}
