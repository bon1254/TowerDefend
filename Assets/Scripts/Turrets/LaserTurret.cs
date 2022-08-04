using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTurret : MonoBehaviour
{
    public Transform target;
    private Enemy targetEnemy;

    [Header("General")]
    public float range = 15f;

    [Header("Use Laser")]
    public int Laser_DamegeOverTime = 30;
    public float SlowAmount = 0.5f;
    public LineRenderer lineRenderer;
    public ParticleSystem ImpactEffect;
    public ParticleSystem LaserBeam;
    public Light LaserBeamLight;
    public Light ImpactLight;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip LaserSound;

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
            targetEnemy = nearestEnemy.GetComponent<Enemy>();
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
        Gizmos.DrawWireSphere(transform.position, range);
    }

    // Update is called once per frame
    void Update()
    {
        if (turretState.IsFrozen == true)
        {         
            if (lineRenderer.enabled)
            {
                lineRenderer.enabled = false;
                ImpactEffect.Stop();
                LaserBeam.Stop();
                LaserBeamLight.enabled = false;
                ImpactLight.enabled = false;
                audioSource.Stop();
            }
            return;
        }

        if (target == null)
        {
            if (lineRenderer.enabled)
            {
                lineRenderer.enabled = false;
                ImpactEffect.Stop();
                LaserBeam.Stop();
                LaserBeamLight.enabled = false;
                ImpactLight.enabled = false;
                audioSource.Stop();
            }
            return;
        }

        LockOnTarget();
        Laser();  
    }

    void Laser()
    {
        if (targetEnemy != null)
        {
            targetEnemy.GetComponent<Enemy>().TakeDamage(Laser_DamegeOverTime * Time.deltaTime);
            targetEnemy.Slow(SlowAmount);
        }

        LaserBeam.transform.position = firePoint.position;

        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
            ImpactEffect.Play();
            LaserBeam.Play();
            LaserBeamLight.enabled = true;
            ImpactLight.enabled = true;

            //Sound effect
            audioSource.clip = LaserSound;
            audioSource.Play();
            audioSource.loop = true;
        }

        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.position);

        Vector3 dir = firePoint.position - target.position;

        ImpactEffect.transform.position = target.position + dir.normalized * 0.8f;

        ImpactEffect.transform.rotation = Quaternion.LookRotation(dir);
    }

    void LockOnTarget()
    {
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);

        Vector3 rotation = Quaternion.Lerp(a: partToRotate.rotation, b: lookRotation, t: Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }
}
