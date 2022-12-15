using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;
using UnityEngine.Events;
using UnityEngine.Pool;

public class SoundCentral : SingletonBaseClass<SoundCentral>
{
    public static void Create(SoundCentral pref)
    {
       instance = Instantiate(pref);
        instance.Init();

    }
    public delegate void PlaySoundAt(Vector3 v, SoundTypes s);
    public enum SoundTypes {

        PistolShoot, ShottyShoot, SMGShoot
        
    }
    private void Init()
    {
        soundQueue = new Queue<SoundAndLocation>();
        soundToClips = new Dictionary<SoundTypes, AudioClip>();
        for (int i = 0; i < soundsToAudios.Length; i++)
        {
            // IObjectPool<AudioClip> pool = new ObjectPool<AudioClip>();
            soundToClips.Add(soundsToAudios[i].type, soundsToAudios[i].clip);
        }

    }
    [Serializable]
    struct SoundToInt
    {
        public SoundTypes type;
        public AudioClip clip;
      
    }
    struct SoundAndLocation
    {
        public SoundTypes type;
        public Vector3 v;

        public SoundAndLocation(SoundTypes type, Vector3 v)
        {
            this.type = type;
            this.v = v;
        }
    }
    Queue<SoundAndLocation> soundQueue;
    [SerializeField] SoundToInt[] soundsToAudios;
    Dictionary<SoundTypes, AudioClip> soundToClips;

  
      
    

    void ListenEvent(Vector3 v, AudioClip clip)
    {
        AudioSource.PlayClipAtPoint(clip, v);

    }
    private void Update()
    {
        if(soundQueue.Count > 0)
        {
            SoundAndLocation SAL = soundQueue.Dequeue();
            PlaySound(SAL.v, SAL.type);
        }
    }
    UnityAction<Vector3> ListenEvent(SoundTypes type)
    {
        return (Vector3 v) => { PlaySound( v, type); };
    }
   public void Invoke(Vector3 v, SoundTypes i)
    {
       soundQueue.Enqueue(new SoundAndLocation(i, v));
    }
    
    void PlaySound( Vector3 pos, SoundTypes i)
    {
        AudioClip clip = null;
            soundToClips.TryGetValue(i, out clip);
        AudioSource.PlayClipAtPoint(clip, pos);
    }
    
}
