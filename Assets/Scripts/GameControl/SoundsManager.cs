using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] ButtonSounds;
    public AudioClip[] BuildSound;

    public void ButtonClick()
    {
        audioSource.clip = ButtonSounds[0];
        audioSource.Play();
    }

    public void ButtonClick02()
    {
        //Turret btn sound
        audioSource.clip = ButtonSounds[1];
        audioSource.Play();
    }

    public void BuildingSound()
    {
        audioSource.clip = BuildSound[0];
        audioSource.Play();
    }
}
