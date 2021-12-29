using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{

    public static AudioClip gunshotSound, deathSoundEffect;
    static AudioSource audioSrc;

    // Start is called before the first frame update
    void Start()
    {
        gunshotSound = Resources.Load<AudioClip>("gunshot");
        deathSoundEffect = Resources.Load<AudioClip>("deathsound");

        audioSrc = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void PlaySound(string clip)
    {
        if (clip == "gunshot")
            audioSrc.PlayOneShot(gunshotSound);

        else if (clip == "death")
            audioSrc.PlayOneShot(deathSoundEffect);
    }

}
