    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;
using UnityEngine.Events;
using UnityEngine.Pool;
using System.Threading;
using System.Threading.Tasks;
public class SoundCentral : SingletonBaseClass<SoundCentral>
{
    public static void Create(SoundCentral pref)
    {
       instance = Instantiate(pref);
        instance.Init();

    }
    public delegate void PlaySoundAt(Vector3 v, SoundTypes s);
    public enum SoundTypes {
         
        PistolShoot, PistolMag, PistolCharge, SMGShoot, SMGMag, SMGCharge,   ShottyShoot, ShottyMag, ShottyCharge, Sprint, Jump, LightPunch, HeavyPunch, Heal

    }
    AudioSource playAtPointFab;

    private void Init()
    {
        soundQueue = new Queue<SoundAndLocation>();
        soundToClips = new Dictionary<SoundTypes, AudioClip>();
        for (int i = 0; i < soundsToAudios.Length; i++)
        {
            // IObjectPool<AudioClip> pool = new ObjectPool<AudioClip>();
            soundToClips.Add(soundsToAudios[i].type, soundsToAudios[i].clip);
        }
        playAtPointFab = new GameObject().AddComponent<AudioSource>();

        

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
        public GameObject source;
        public SoundAndLocation(SoundTypes type, Vector3 v, GameObject source)
        {
            this.type = type;
            this.v = v;
            this.source = source;
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
            PlaySound(SAL);
        }
    }
  
   public void Invoke(Vector3 v, SoundTypes i)
    {
       soundQueue.Enqueue(new SoundAndLocation(i, v, null));
        
    }
    
    async void PlaySound( SoundAndLocation sal)
    {
        AudioClip clip = null;
            soundToClips.TryGetValue(sal.type, out clip);
        AudioSource source = Instantiate(playAtPointFab);
        source.clip = clip;
        source.transform.position = sal.v;
     //   source.transform.SetParent(sal.source.transform);
        source.Play();
        await Task.Delay((int)(clip.length * 1000));
        if (source != null)
        {
            Destroy(source.gameObject);
        }

    }

}
