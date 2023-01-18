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
         
        PistolShoot, PistolMag, PistolCharge, SMGShoot, SMGMag, SMGCharge,   ShottyShoot, ShottyMag, ShottyCharge, Sprint, Jump, LightPunch, HeavyPunch, Heal, Message, GunClick, SlowHeartBeat, Drill, DrillBeep

    }
    AudioSource playAtPointFab;
    Queue<SoundAndTransform> tQueue;
    private void Init()
    {
        soundQueue = new Queue<SoundAndLocation>();
        tQueue = new Queue<SoundAndTransform>();

        soundToClips = new Dictionary<SoundTypes, AudioClip>();
        for (int i = 0; i < soundsToAudios.Length; i++)
        {
            // IObjectPool<AudioClip> pool = new ObjectPool<AudioClip>();
            soundToClips.Add(soundsToAudios[i].type, soundsToAudios[i].clip);
        }
        playAtPointFab = new GameObject().AddComponent<AudioSource>();
        playAtPointFab.spatialBlend = 1;
        playAtPointFab.spread = 360;

        

    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        tQueue.Clear();
        soundQueue.Clear(); 
    }

    [Serializable]
    struct SoundToInt
    {
        public SoundTypes type;
        public AudioClip clip;
      
    }
   public struct SoundAndLocation
    {
        public SoundTypes type;
        public Vector3 v;
        public bool twoDSound;

        public SoundAndLocation(SoundTypes type, Vector3 v, bool twoDSound = false)
        {
            this.type = type;
            this.v = v;
            this.twoDSound = twoDSound;
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
        if (tQueue.Count > 0)
        {
            SoundAndTransform SAL = tQueue.Dequeue();
            PlaySound(SAL);
        }
    }

    public void Invoke(Transform v, SoundTypes i)
    {
        tQueue.Enqueue(new SoundAndTransform(i, v));

    }
    public void InvokeWithParent(Transform v, SoundTypes i)
    {
        tQueue.Enqueue(new SoundAndTransform(i, v));

    }
    public void Invoke(Transform v, SoundTypes i, bool twoD)
    {
        tQueue.Enqueue(new SoundAndTransform(i, v, twoD));

    }
    public void Invoke(Vector3 v, SoundTypes i)
    {
       soundQueue.Enqueue(new SoundAndLocation(i, v));
        
    }

   public  struct SoundAndTransform
    {
        public SoundTypes type;
        public Transform v;
        public bool twoDSound;
        public SoundAndTransform(SoundTypes type, Transform v, bool twoDSound = false)
        {
            this.type = type;
            this.v = v;
            this.twoDSound = twoDSound;
        }
    }
    

   public async Task<AudioSource> PlaySound( SoundAndLocation sal)
    {
        AudioClip clip = null;
            soundToClips.TryGetValue(sal.type, out clip);
        AudioSource source = Instantiate(playAtPointFab);
        source.clip = clip;
        source.transform.position = sal.v;
       
        if(sal.twoDSound)
        {
            source.spatialBlend = 0;
        } else
        {
            source.spatialBlend = 1;
        }
     //   source.transform.SetParent(sal.source.transform);
        source.Play();
        await Task.Delay((int)(clip.length * 1000));
        if (source != null)
        {
            Destroy(source.gameObject);
        }
        return source;
    }
    public AudioSource PlaySoundLoop(SoundAndLocation sal)
    {
        
            AudioClip clip = null;
            soundToClips.TryGetValue(sal.type, out clip);
            AudioSource source = Instantiate(playAtPointFab);
            source.clip = clip;
       
        source.transform.position = sal.v;
            //   source.transform.SetParent(sal.source.transform);
            if (sal.twoDSound)
            {
                source.spatialBlend = 0;
            }
            else
            {
                source.spatialBlend = 1;
            }
        source.loop = true;
            source.Play();
            return source;

        
    }
    public async Task<AudioSource> PlaySound(SoundAndTransform sal)
    {
        AudioClip clip = null;
        soundToClips.TryGetValue(sal.type, out clip);
        AudioSource source = Instantiate(playAtPointFab);
        source.clip = clip;
        source.transform.SetParent(sal.v, true);
        //   source.transform.SetParent(sal.source.transform);
        if (sal.twoDSound)
        {
            source.spatialBlend = 0;
        }
        else
        {
            source.spatialBlend = 1;
        }
        source.Play();
        
        await Task.Delay((int)(clip.length * 1000));
        if (source != null)
        {
            Destroy(source.gameObject);
        }
        return source;

    }

}
