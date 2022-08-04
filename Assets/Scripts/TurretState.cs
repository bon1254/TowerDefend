using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretState : MonoBehaviour
{
    [Header("Forzen")]
    public bool IsFrozen = false;
    public GameObject FrozenEffect;

    [Header("Audio")]
    public AudioClip FrozenSound;
    public AudioSource audioSource;

    public void IsTurretFrozen()
    {
        IsFrozen = true;

        //Play sound effect
        FreezeSound();

        //adjust effect localPosition
        Vector3 adjustPos = new Vector3(transform.localPosition.x, 0.2f, transform.localPosition.z);

        //Instantiate effect
        GameObject effectIns = Instantiate(FrozenEffect, adjustPos, transform.localRotation);
        Destroy(effectIns, 5);

        //set timer to unfreeze turret
        Invoke("IsTurretUnFreeze", 5);
    }

    public void FreezeSound()
    {
        audioSource.clip = FrozenSound;
        audioSource.Play();
        audioSource.loop = false;
    }

    private void IsTurretUnFreeze()
    {
        IsFrozen = false;
        audioSource.clip = null;
    }
}
