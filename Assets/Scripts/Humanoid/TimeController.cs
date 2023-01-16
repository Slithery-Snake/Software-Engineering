using UnityEngine;
using System;
using EnemyStuff;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.Events;
using System.Collections;
public class TimeNormal : TimeDisabled
{   
    
    public TimeNormal(PInputManager manager, TimeController time) : base(manager, time)
    {
    }
    public override void EnterState()
    {
        time.SetSlow(false);   
    }
    
    public override void HandleKeyDownInput( KeyCode keyCode)
    {
        if(keyCode == KeyCode.V && time.BulletBar > 0)
        {
            manager.ChangeToState(manager.SlowTime, manager.TimeState);   
        }
    }
}
public class TimeSlow : TimeDisabled
{
    static MonoCall timeSlowAttempted = new MonoCall();

    public static IMonoCall TimeSlowAttempted { get => timeSlowAttempted; }
    CancellationTokenSource token;
    public TimeSlow(PInputManager manager, TimeController time) : base(manager, time)
    {
 
    }
    void StartLoop()
    {

   
            token = new CancellationTokenSource();
            Loop(token.Token);
        
     
    }
    void StopLoop()
    {
        token?.Cancel();
        token.Dispose();

    }
    async void Loop(CancellationToken token)
    {
        try
        {
            while (true)
            {
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }
                await SoundCentral.Instance.PlaySound(new SoundCentral.SoundAndTransform(SoundCentral.SoundTypes.SlowHeartBeat, manager.transform, true)); ;
            }
        } catch(OperationCanceledException)
        {

        }
      
    }
    public override void EnterState()
    {
      
            StartLoop();
        
        timeSlowAttempted.Call();
        time.BarZero += Time_BarZero;
       
        time.SetSlow(true);
    }
    
    public override void ExitState()
    {
        time.BarZero -= Time_BarZero;
        StopLoop();

    }
    private void Time_BarZero(object sender, EventArgs e)
    {
        manager.ChangeToState(manager.NormalTime, manager.TimeState);
    }

    public override void HandleKeyDownInput( KeyCode keyCode)
    {
        if (keyCode == KeyCode.V)
        {
            manager.ChangeToState(manager.NormalTime, manager.TimeState);
        }
    }
}
public class TimeDisabled : PlayerState
{
    protected TimeController time;
    public TimeDisabled(PInputManager manager, TimeController time) : base(manager)
    {
        this.time = time;
    }

    public override void EnterState()
    {
    }

    public override void ExitState()
    {
    }

    public override void HandleKeyDownInput( KeyCode keyCode)
    {
    }

    public override void HandleKeyPressedInput( KeyCode keyCode)
    {
    }

    public override void HandleKeyUpInput( KeyCode keyCode)
    {
    }
}
public class TimeController : StateManagerComponent
{
    public static readonly float FixedDeltaDefault = Time.fixedDeltaTime;
    public static readonly float FixedDefaultScale = Time.timeScale;
    float defaultFixedDelta;
     float defaultTimeScale;
    PlayerSC sC;
    static float playerDelta = 1;
    static float slow = 1;
    float bulletBar= 0;
    static int waitTick = 1;
    public event EventHandler BarZero;
    public static float Slow { get => slow; }
    public static float PlayerDelta { get => playerDelta;  }
    public float BulletBar { get => bulletBar; }
    public event UnityAction<float> ValueUpdated;

    Task task;
    CancellationTokenSource source;
    public TimeController(MonoCalls.MonoAcessors manager, PlayerSC sC) : base(manager)
    {
        defaultFixedDelta = FixedDeltaDefault;
        defaultTimeScale = FixedDefaultScale;
        this.sC = sC;
        bulletBar = 0;
        EnemyAI.EnemyKilled +=  EnemyKilled;
    }
    protected override void CleanUp()
    {
        EnemyAI.EnemyKilled -= EnemyKilled;

    }

    void EnemyKilled()
    {
        bulletBar += sC.SlowBarIncrement;

        if (bulletBar > sC.SlowBarMax)
        {
            bulletBar = sC.SlowBarMax;
            
        }
        ValueUpdated?.Invoke( bulletBar);
    }  
    void StartDecrease()
    {
        source = new CancellationTokenSource();
       task = Decrease(source.Token);

    }
     void Stop()
    {
        if (source!= null)
        {
            source.Cancel();
        }
    }
    async Task Decrease(CancellationToken t)
    {
        int wait = (int)(waitTick * 1000);
      //  Debug.Log(wait);
        float decrease = sC.SlowBarDecrement;
    
            try
            {
            while (true)
            {

                if (bulletBar <= 0)
                {
                    bulletBar = 0;
                    BarZero?.Invoke(this, null);
                    Stop();
                //    Debug.Log("bbar less");
                }
                if (t.IsCancellationRequested) { t.ThrowIfCancellationRequested(); }
                bulletBar -= decrease;
                ValueUpdated?.Invoke( bulletBar);




                await Task.Delay(wait);
            }
            }
            catch(OperationCanceledException)
            {
            source.Dispose();
            source = null;
            }

    }
    public void SetSlow(bool toggle)
    {
       
       
        if (toggle)
        {
            EnemyAI.EnemyKilled -= EnemyKilled;

            slow = 1/sC.TimeSlow;
            Time.timeScale = slow;
            Time.fixedDeltaTime = defaultFixedDelta * Time.timeScale;
            TimeController.playerDelta = sC.SlowAdvantage;
            StartDecrease();
        }
        else
        {
            
            EnemyAI.EnemyKilled += EnemyKilled;


            slow = 1;                                               
            Time.fixedDeltaTime = defaultFixedDelta;
            Time.timeScale = defaultTimeScale;
             playerDelta = 1;
            Stop();
        }
    }

   
}
