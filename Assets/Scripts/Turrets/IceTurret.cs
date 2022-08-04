﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceTurret : MonoBehaviour
{
    public Transform target;
    //private Enemy targetEnemy;

    [Header("General")]
    public float range = 15f;
    public GameObject BulletPrefab;
    public float fireRate = 1f;
    private float fireCountDown = 0f;

    //[Header("Use Muzzle")]
    //public GameObject MuzzleEffect;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip ShotSound;

    [Header("Unity Setup Fields")]
    public string enemyTag = "Enemy";
    public Transform firePoint;
    public Transform partToRotate;
    public float turnSpeed = 50f;

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
            //targetEnemy = nearestEnemy.GetComponent<Enemy>();
        }
        else
        {
            target = null;
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, range);
    }

    // Update is called once per frame
    void Update()
    {
        if (turretState.IsFrozen == true)
        {  
            return;
        }

        if (target == null)
        {
            return;
        }

        LockOnTarget();
        
        if (fireCountDown <= 0f)
        {
            Shoot();
            fireCountDown = 1f / fireRate;
        }

        fireCountDown -= Time.deltaTime;       
    }

    void LockOnTarget()
    {
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);

        Vector3 rotation = Quaternion.Lerp(a: partToRotate.rotation, b: lookRotation, t: Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    void Shoot()
    {
        audioSource.clip = ShotSound;
        audioSource.Play();

        //if (MuzzleEffect != null)
        //{
        //    GameObject muzzleVFX = Instantiate(MuzzleEffect, firePoint.position, firePoint.rotation);

        //    Destroy(muzzleVFX, 3);
        //}

        GameObject BulletGo = Instantiate(BulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = BulletGo.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.Seek(target);
        }
    }
}
