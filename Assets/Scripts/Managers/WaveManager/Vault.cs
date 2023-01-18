using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Threading;
using System.Threading.Tasks;
public class Vault : MonoBehaviour, Iinteractable
{
    public int timeLeft;
  //  public bool timerOn = false;
    WaitForSeconds wait = new WaitForSeconds(1);
    event UnityAction WasInteracted;
    [SerializeField] Animator vaultAnim;
    //public Text TimerTxt;
    VaultState vaultState;
    public event UnityAction VaultOpened;
    public event UnityAction EnteredVault;
    [SerializeField] GameObject drill;
    [SerializeField] GameObject interactableObj;
    Interactable interactable;
    [SerializeField] OnColEnterEvent finishCollision;
    [SerializeField] GameObject particles;
    MonoCall startedDrilling = new MonoCall();

    public IMonoCall StartedDrilling { get => startedDrilling;}

    // Start is called before the first frame update
    void Awake()
    {
        vaultState = new VaultState(this);
       interactable = Interactable.Create(interactableObj, this);
        interactable.enabled = false;

    }

    GameObject StartSoundLoop(Vector3 v, SoundCentral.SoundTypes s)
    {
            

                AudioSource funk = SoundCentral.Instance.PlaySoundLoop(new SoundCentral.SoundAndLocation(s, v, false));
   //     funk.Result.loop = true;
        return funk.gameObject;
               
          

    }
 
   

    void UpdateTimer(float currentTime)
    {
        currentTime++;

        float min = Mathf.FloorToInt(currentTime / 60);
        float sec = Mathf.FloorToInt(currentTime % 60);

    //    TimerTxt.txt = string.Format("(0:00) : (1:00) ", min, sec);
    }
    // interacted > start drill, 
    // someitmes drill break, interact to fix
    //on done stop wave spawning
  
    public void StarInteraction()
    {
        interactable.enabled = true;

    }
    public void Interacted(SourceProvider source)
    {
        Debug.Log("the vault was interacted with oh yeah");
        WasInteracted?.Invoke();
    }
    public class VaultState : StateManager
    {
        private readonly Vault parent;
        Closed closed;
        Drilling drillin;
        Broken broken;
        Done done;
        Opening opening;
        Opened opened;
        StatePointer<VaultState, FiniteState<VaultState>> pointer;
        public VaultState(Vault parent)
        {
            this.parent = parent;
            closed = new Closed(this);
            drillin = new Drilling(this,parent.timeLeft);
            broken = new Broken(this);
            opening = new Opening(this);
            pointer = new StatePointer<VaultState, FiniteState<VaultState>>(closed, this);
        }
        public class Closed : FiniteState<VaultState>
        {
            public Closed(VaultState manager) : base(manager)
            {
            }

            public override void EnterState()
            {
                manager.parent.finishCollision.enabled = false;
                manager.parent.WasInteracted += Inter;
            }
            void Inter()
            {
                manager.parent.WasInteracted -= Inter;
                manager.parent.drill.SetActive(true);
                manager.parent.startedDrilling.Call();
                Debug.Log("started drilling");
                manager.ChangeToState(manager.drillin, manager.pointer);
            }

            public override void ExitState()
            {
            }
        }
        public class Drilling : FiniteState<VaultState>
        {
            private int timeLeft;
            WaitForSeconds wait;
            GameObject sound;
            public Drilling(VaultState manager, int timeLeft) : base(manager)
            {
                this.timeLeft = timeLeft;
                wait = new WaitForSeconds(1);
            }
            Coroutine countdownRoutine;
            public override void EnterState()
            {
               sound = manager.parent.StartSoundLoop(manager.parent.transform.position, SoundCentral.SoundTypes.Drill);
                countdownRoutine = manager.parent.StartCoroutine(Tick());
                manager.parent.particles.SetActive(true);
            }
            IEnumerator Tick()
            {
                while (true)
                {
                    if (timeLeft > 0)
                    {
                        timeLeft--;
                        // UpdateTimer(timeLeft);
                        if(Random.Range(1, 25) == 1)
                        {
                            manager.ChangeToState(manager.broken, manager.pointer);
                        }
                    }
                    else
                    {
                        Debug.Log("Timer done");
                        timeLeft = 0;
                        manager.ChangeToState(manager.opening, manager.pointer);
                        
                        break;
                    }
                    yield return wait;
                }

            }

            public override void ExitState()
            {
                manager.parent.particles.SetActive(false);

                if (countdownRoutine != null)
                {
                    manager.parent.StopCoroutine(countdownRoutine);
                }
                if(sound !=null)
                {
                    GameObject.Destroy(sound);
                }
            }
        }

        public class Broken : FiniteState<VaultState>
        {
            GameObject sound;
            public Broken(VaultState manager) : base(manager)
            {
            }

            public override void EnterState()
            {
                sound = manager.parent.StartSoundLoop(manager.parent.transform.position, SoundCentral.SoundTypes.DrillBeep);

                manager.parent.WasInteracted += DrillFix;
            }
            void DrillFix()
            {
                manager.parent.WasInteracted -= DrillFix;

                manager.ChangeToState(manager.drillin, manager.pointer);
            }
            public override void ExitState()
            {
                if (sound != null)
                {
                    GameObject.Destroy(sound);
                }
            }
        }
        public class Done : FiniteState<VaultState>
        {
            public Done(VaultState manager) : base(manager)
            {
            }

            public override void EnterState()
            {
                
            }

            public override void ExitState()
            {
                throw new System.NotImplementedException();
            }
        }
        public class Opening : FiniteState<VaultState>
        {
            OnColEnterEvent enter;
            public Opening(VaultState manager) : base(manager)
            {
                
            }

            public override void EnterState()
            {
                manager.parent.drill.SetActive(false);

                 enter = manager.parent.finishCollision;
                enter.enabled = true;
                enter.TriggerEntered += InvokeFinish;
                
                manager.parent.vaultAnim.SetTrigger("Opened");
                manager.parent.VaultOpened?.Invoke();
            }
            void InvokeFinish(Collider col)
            {
               if(col.gameObject.layer == Constants.playerMask)
                {
                    enter.TriggerEntered -= InvokeFinish;
                    manager.parent.EnteredVault?.Invoke();

                }


            }
            public override void ExitState()
            {
                
            }
        }
        public class Opened : FiniteState<VaultState>
        {
            public Opened(VaultState manager) : base(manager)
            {
            }

            public override void EnterState()
            {
                throw new System.NotImplementedException();
            }

            public override void ExitState()
            {
                throw new System.NotImplementedException();
            }
        }
    }
   
    
}