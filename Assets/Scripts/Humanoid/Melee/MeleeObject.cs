
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

    public MeleeManager(MonoCalls.MonoAcessors manager, HumanoidSC sc, Collider l, Collider R, Animator lanim, Animator rAnim) : base(manager)
    {
        
       meleeR = MeleeType.MeleeObject.Create( sc, R, rAnim);
        meleeL = MeleeType.MeleeObject.Create( sc, l, lanim);
        


       
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

    }
 
  
    public void StopAttack()
    {
        currentMelee.SetActive(false);        
    }

    public class MeleeType : Enumeration
    {
        UnityAction<Collider> attackType;
        private readonly MeleeStats stats;

        public MeleeStats Stats => stats;

        public MeleeType(int id, string name, UnityAction<Collider> attackType, MeleeStats stats) : base(id, name)
        {
            this.attackType = attackType;
            this.stats = stats;
        }
     
        public class MeleeObject : MonoBehaviour
        {
            HumanoidSC sc;
            UnityAction<Collider> collided;
            Collider handCollider;
            Animator anim;

            public static MeleeObject Create(  HumanoidSC sc, Collider collider, Animator meleeAnim)
            {

                MeleeObject hi = collider.transform.gameObject.AddComponent<MeleeObject>();
                
                hi.sc = sc;
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
            static StatusEffect.StatusEffectManager GetStatus(Collider collision)
            {
                return collision?.GetComponent<ShootBox>()?.Status?.Status;
            }
           
            void Heavy(Collider collision)
            {
                GetStatus(collision)?.AddStatusEffect(new StatusEffect.StatusEffectManager.Melee(sc.Heavy.dmg));
                
                collision?.GetComponent<ShootBox>()?.Status?.Status.AddStatusEffect(new StatusEffect.StatusEffectManager.StunApply(sc.StunTime));
                SoundCentral.Instance.Invoke(transform.position, SoundCentral.SoundTypes.HeavyPunch);

            }
            void Light(Collider collision)
            {
                GetStatus(collision)?.AddStatusEffect(new StatusEffect.StatusEffectManager.Melee(sc.Light.dmg));
                SoundCentral.Instance.Invoke(transform.position, SoundCentral.SoundTypes.LightPunch);

            }
            private void OnTriggerEnter(Collider other)
            {if (collided != null)
                {
                    collided(other);
                }
            }
        }
    }
    
}