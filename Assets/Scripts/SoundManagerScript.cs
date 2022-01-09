using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{

    public static AudioClip gunshotSound, deathSoundEffect, gunshotEmptySound;
    static AudioSource audioSrc;

    // Start is called before the first frame update
    void Start()
    {
        gunshotSound = Resources.Load<AudioClip>("gunshot");
        gunshotEmptySound = Resources.Load<AudioClip>("gunshot_empty");
        deathSoundEffect = Resources.Load<AudioClip>("deathsound");
      

        audioSrc = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void PlaySound(string clip)
    {
        audioSrc.volume = Settings.GetSoundVolume();
        if (clip == "gunshot")
            audioSrc.PlayOneShot(gunshotSound);

        else if (clip == "gunshot_empty")
            audioSrc.PlayOneShot(gunshotEmptySound);

        else if (clip == "death")
            audioSrc.PlayOneShot(deathSoundEffect);
       
    }

}
