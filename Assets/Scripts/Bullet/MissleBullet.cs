using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissleBullet : MonoBehaviour
{
    //發射
    //期間 isgrounded = false
    //Launch 跑出弧度 
    //碰到target
    //爆炸 造成傷害
   
    public float speed = 70f;
    public int BulletDamage = 50;
    public float ExplosionRadius = 0f;

    public GameObject ImpactEffect;
    public GameObject ImpactSound_missle;
    public string AttackFrom = "Unknow";

    // launch variables
    private Transform target;
    [Range(1.0f, 15.0f)] public float TargetRadius;
    [Range(20.0f, 75.0f)] public float LaunchAngle;
    [Range(0.0f, 10.0f)] public float TargetHeightOffsetFromGround;
    public bool RandomizeHeightOffset;

    // cache 
    private Rigidbody rb;
    //private Vector3 initiallocalPosition;
    private Quaternion initialRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //initiallocalPosition = transform.localPosition;
        initialRotation = transform.rotation;
    }

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
        else
        {
            Launch();
        }
  
        Vector3 dir = target.localPosition - transform.localPosition;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }  

        // update the rotation of the projectile during trajectory motion
        transform.rotation = Quaternion.LookRotation(rb.velocity) * initialRotation;
        transform.LookAt(target);
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    // launches the object towards the TargetObject with a given LaunchAngle
    void Launch()
    {
        // think of it as top-down view of vectors: 
        //   we don't care about the y-component(height) of the initial and target localPosition.
        Vector3 projectileXZPos = new Vector3(transform.localPosition.x, 0.0f, transform.localPosition.z);
        Vector3 targetXZPos = new Vector3(target.localPosition.x, 0.0f, target.localPosition.z);

        // shorthands for the formula
        float R = Vector3.Distance(projectileXZPos, targetXZPos);
        float G = Physics.gravity.y;
        float tanAlpha = Mathf.Tan(LaunchAngle * Mathf.Deg2Rad);
        float H = target.localPosition.y - transform.localPosition.y;

        // calculate the local space components of the velocity 
        // required to land the projectile on the target object 
        float Vz = Mathf.Sqrt(G * R * R / (2.0f * (H - R * tanAlpha)));
        float Vy = tanAlpha * Vz;

        // create the velocity vector in local space and get it in global space
        Vector3 localVelocity = new Vector3(0f, Vy, Vz);
        Vector3 globalVelocity = transform.TransformDirection(localVelocity);

        // launch the object by setting its initial velocity and flipping its state
        rb.velocity = globalVelocity;
    }

    void HitTarget()
    {
        //Instantiate soundobj
        GameObject soundobj = Instantiate(ImpactSound_missle, transform.localPosition, Quaternion.identity);
        Destroy(soundobj, 1.5f);

        //Instantiate impacteffect
        GameObject effectIns = Instantiate(ImpactEffect, transform.localPosition, transform.rotation);
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
            if (collider.tag == "Enemy" && AttackFrom != "Enemy")
            {
                Damage(collider.transform);
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
}
