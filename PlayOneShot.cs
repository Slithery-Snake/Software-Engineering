using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOneShot : MonoBehaviour {
    public string fileName;
    void Update()
    {
        audioSource.PlayOneShot(fileName[Random.Range(0, fileName.length - 1)], Random.Range(0.75f, 1));
    }
}
