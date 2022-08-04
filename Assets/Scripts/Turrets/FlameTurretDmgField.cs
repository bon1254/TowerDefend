using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FlameTurretDmgField : MonoBehaviour
{
    public bool IsDoDmg = true;
    public int FlamethrowerDamege;

    private void Start()
    {
        IsDoDmg = true;
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.tag == "Enemy")
        {
            if (IsDoDmg == true)
            {
                IsDoDmg = false;
                collider.GetComponent<Enemy>().TakeDamage(FlamethrowerDamege);
                StartCoroutine(DoFlameDmg());
            }
        }
    }


    private IEnumerator DoFlameDmg()
    {
        yield return new WaitForSeconds(0.5f);
        IsDoDmg = true;
    }
}
