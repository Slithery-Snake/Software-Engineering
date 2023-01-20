using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Central : SingletonBaseClass<Central>
{
  public  enum Messages { EnemyKilled, }
    Queue<Packet> queue = new Queue<Packet>();

    Dictionary<int, AudioClip> dictionary;
    [SerializeField] SoundIDpair[] sounds;
    protected override void Awake()
    {
        base.Awake();
        dictionary = new Dictionary<int, AudioClip>();
        for(int i = 0; i < sounds.Length; i++)
        {
            dictionary.Add(sounds[i].id, sounds[i].sound);
        }

    }
    struct Packet
    {
        public int id;
        public Transform transform;
    }

    public void PlaySound(int id, Transform location)
    {
        queue.Enqueue(new Packet { id = id, transform = location });       
    }
    void Process()
    {
        if(queue.Count > 0) {
         Packet packet = queue.Dequeue();
            AudioClip clip = null;
            dictionary.TryGetValue(packet.id, out clip);
    
            AudioSource.PlayClipAtPoint(clip, packet.transform.position);

        }
    }

}