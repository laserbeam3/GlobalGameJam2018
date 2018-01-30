using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundEffects : MonoBehaviour
{
    public AudioSource audio;
    public AudioClip[] yodleSounds;
    public AudioClip[] growls;
    public AudioClip[] boulderRolls;

    public void PlayYodle() {
        if (audio.isPlaying) return;

        audio.PlayOneShot(yodleSounds[Random.Range(0, yodleSounds.Length)]);
    }
}
