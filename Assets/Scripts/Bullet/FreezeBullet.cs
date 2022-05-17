using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

public class FreezeBullet : MonoBehaviour
{
    private Transform target;
    public float speed = 70f;

    public int BulletDamage = 50;

    public float ExplosionRadius = 0f;

    public GameObject ImpactEffect;

    public string AttackFrom = "Unknow";

    public void Seek(Transform _target)
    {
        target = _target;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.localPosition - transform.localPosition;
        float distanceThisFrame = speed * Time.deltaTime;

        if(dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target);
    }

    void HitTarget()
    {
        GameObject effectIns = (GameObject)Instantiate(ImpactEffect, transform.localPosition, transform.rotation);
        Destroy(effectIns, 2f);

        if (ExplosionRadius > 0f)
        {
            Explode();
        }
        else
        {
            Damage(target);
        }
        Destroy(gameObject);
    }

    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.localPosition, ExplosionRadius);

        foreach (Collider collider in colliders)
        {          
            if (collider.tag == "Turret" && AttackFrom != "Turret")
            {
                FreezeTurret(collider.transform);
            }
        }
    }

    void FreezeTurret(Transform turretSt)
    {
        TurretState ts = turretSt.GetComponent<TurretState>();

        if (ts != null)
        {
            if (ts.IsFrozen == true)
            {
                return;
            }
            else
            {
                //freeze turret
                ts.IsTurretFrozen();                
            }
        }
    }

    void Damage(Transform enemy)
    {
        Enemy e = enemy.GetComponent<Enemy>();

        if (e != null)
        {
            e.TakeDamage(BulletDamage);
        }    
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.localPosition, ExplosionRadius);
    }
}
