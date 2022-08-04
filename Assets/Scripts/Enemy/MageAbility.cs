using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageAbility : MonoBehaviour
{
    public Transform target;
    public Vector3 PositionOffset;

    [Header("General")]
    public float range = 15f;

    [Header("Use Bullets (Freeze)")]
    public GameObject BulletPrefab;
    public float fireRate = 1f;
    private float fireCountDown = 0f;

    [HideInInspector]
    public GameObject Turret;
    [HideInInspector]
    public TurretBluePrint turretBluePrint;

    [Header("Unity Setup Fields")]
    public string TurretTag = "Turret";

    public Transform firePoint;
    public Transform partToRotate;
    public float turnSpeed = 10f;

    public TurretState targetTurretState;
    public Turret targetTurret;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {        
            return;
        }

        //if target is frozen then return
        if (targetTurret != null && targetTurretState.IsFrozen == true)
        {
            return;
        }
        else
        {
            LockOnTarget();

            if (fireCountDown <= 0f)
            {
                Shoot();
                fireCountDown = 1f / fireRate;
            }

            fireCountDown -= Time.deltaTime;
        }
    }

    public Vector3 GetBuildPostion()
    {
        return transform.position + PositionOffset;
    }

    void UpdateTarget()
    {
        GameObject[] turrets = GameObject.FindGameObjectsWithTag(TurretTag);

        float shortesDistance = Mathf.Infinity;
        GameObject nearestTurret = null;

        foreach (GameObject turret in turrets)
        {                 
            float distanceToTurret = Vector3.Distance(transform.position, turret.transform.position);
            if (distanceToTurret < shortesDistance)
            {
                shortesDistance = distanceToTurret;
                nearestTurret = turret;
            }
        }

        if (nearestTurret != null && shortesDistance <= range)
        {
            target = nearestTurret.transform;
            targetTurret = nearestTurret.GetComponent<Turret>();
            targetTurretState = nearestTurret.GetComponent<TurretState>();
        }
        else
        {
            target = null;
        }
    }

    void LockOnTarget()
    {
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);

        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    void Shoot()
    {        
        GameObject BulletGo = Instantiate(BulletPrefab, firePoint.position, firePoint.rotation);

        FreezeBullet freezebullet = BulletGo.GetComponent<FreezeBullet>();

        if (freezebullet != null)
        {
            Debug.Log("target.name..." + target.name);
            freezebullet.Seek(target);
        }
    }
}
