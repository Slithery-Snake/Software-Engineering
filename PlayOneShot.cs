using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayAudio : MonoBehaviour
{
    public AudioClip clip;
    public float volume = 1;
    void Start()
    {
        AudioSource.PlayClipAtPoint(clip, transform.position, volume);
    }
}
