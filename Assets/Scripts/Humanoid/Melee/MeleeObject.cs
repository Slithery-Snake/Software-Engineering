
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine.Events;
public class MeleeManager : StateManagerComponent
{

    int wait;
    MeleeType.MeleeObject meleeR;
    MeleeType.MeleeObject meleeL;

   
    MeleeType.MeleeObject currentMelee;


    public MeleeType.MeleeObject MeleeSoureOBJ { get => currentMelee; }
    public IMonoCall AttackAttempt { get => attackAttempt; }

    public MeleeManager(MonoCalls.MonoAcessors manager, HumanoidSC sc, Collider l, Collider R, Animator lanim, Animator rAnim, BulletTag tag) : base(manager)
    {
        
       meleeR = MeleeType.MeleeObject.Create( sc, R, rAnim, tag);
        meleeL = MeleeType.MeleeObject.Create( sc, l, lanim, tag);
        


       
    }
    public void SetHand(bool left)
    {
        if (left)
        {
            currentMelee = meleeL;
        } else { currentMelee = meleeR; }


    }
    public void AttackSetUp(MeleeType type)
    {

        currentMelee.SetActive(true);
        currentMelee.SetMelee(type);
        attackAttempt.Call();

    }
    MonoCall attackAttempt = new MonoCall();
         
  
    public void StopAttack()
    {
        currentMelee.SetActive(false);        
    }

    protected override void CleanUp()
    {
    }

    public class MeleeType : Enumeration
    {
        UnityAction<Collider, BulletTag> attackType;
        private readonly MeleeStats stats;

        public MeleeStats Stats => stats;

        public MeleeType(int id, string name, UnityAction<Collider, BulletTag> attackType, MeleeStats stats) : base(id, name)
        {
            this.attackType = attackType;
            this.stats = stats;
        }
     
        public class MeleeObject : MonoBehaviour
        {
            HumanoidSC sc;
            UnityAction<Collider, BulletTag> collided;
            Collider handCollider;
            Animator anim;
            BulletTag bTag;
            public static MeleeObject Create(  HumanoidSC sc, Collider collider, Animator meleeAnim, BulletTag tag)
            {

                MeleeObject hi = collider.transform.gameObject.AddComponent<MeleeObject>();
                
                hi.sc = sc;
                hi.bTag = tag;
                hi.HeavyType = new MeleeType(1, nameof(HeavyType), hi.Heavy, sc.Heavy);
                hi.LightType = new MeleeType(2, nameof(LightType), hi.Light, sc.Light);
                hi.handCollider = collider;
                hi.SetActive(false);
                hi.anim = meleeAnim;

                return hi;

            }
            public MeleeType HeavyType;
            public MeleeType LightType;
             public MeleeType NoneType;

            public Animator Anim { get => anim; }

            public void SetActive(bool f)
            {
                handCollider.enabled = f;

            }
          

            public void SetMelee(MeleeType type)
            {
                collided = type.attackType;
            }
             StatusEffect.StatusEffectManager GetStatus(Collider collision)
            {
                StatusEffect.StatusEffectManager r = collision?.GetComponent<ShootBox>()?.Status?.Status;
                if (r != null)
                {
                    collided = Nothing;
                }

                return r;
            }
            void Nothing(Collider col, BulletTag t) { }
            void Heavy(Collider collision, BulletTag tag)
            {
                StatusEffect.StatusEffectManager s = GetStatus(collision);
                s?.AddStatusEffect(new StatusEffect.StatusEffectManager.Melee(sc.Heavy.dmg), tag);

                s?.AddStatusEffect(new StatusEffect.StatusEffectManager.StunApply(sc.StunTime), tag);
                SoundCentral.Instance.Invoke(transform.position, SoundCentral.SoundTypes.HeavyPunch);
                

            }
            void Light(Collider collision, BulletTag tag)
            {
                GetStatus(collision)?.AddStatusEffect(new StatusEffect.StatusEffectManager.Melee(sc.Light.dmg), tag);

                SoundCentral.Instance.Invoke(transform.position, SoundCentral.SoundTypes.LightPunch);

            }
            private void OnTriggerEnter(Collider other)
            {if (collided != null)
                {
                    collided(other, bTag);
                }
            }
        }
    }
    
}