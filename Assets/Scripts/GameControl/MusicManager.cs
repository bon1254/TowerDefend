using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] audioClips;

    private static MusicManager instance = null;
    public void Awake()
    {
        // If there is not already an instance of SoundManager, set it to this.
        if (instance == null)
        {
            instance = this;
        }
        //If an instance already exists, destroy whatever this object is to enforce the singleton.
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        BGMLevel00();
    }

    public void BGMLevel00()
    {
        audioSource.clip = audioClips[0];
        audioSource.Play();
    }

    public void BGMLevel01()
    {
        audioSource.clip = audioClips[1];
        audioSource.Play();
    }
}
