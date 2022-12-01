using UnityEngine;
using System;
using EnemyStuff;
using System.Threading;
using System.Threading.Tasks;
public class TimeNormal : TimeDisabled
{
    public TimeNormal(PInputManager manager, TimeController time) : base(manager, time)
    {
    }
    public override void EnterState(PInputManager stateManager)
    {
        time.SetSlow(false);   
    }
    
    public override void HandleKeyDownInput(PInputManager stateManager, KeyCode keyCode)
    {
        if(keyCode == KeyCode.V)
        {
            manager.ChangeToNewState(manager.SlowTime, manager.TimeState);   
        }
    }
}
public class TimeSlow : TimeDisabled
{
    public TimeSlow(PInputManager manager, TimeController time) : base(manager, time)
    {
 
    }

    public override void EnterState(PInputManager stateManager)
    {
        time.BarZero += Time_BarZero;

        time.SetSlow(true);
    }
    public override void ExitState(PInputManager stateManager)
    {
        time.BarZero -= Time_BarZero;
    }
    private void Time_BarZero(object sender, EventArgs e)
    {
        manager.ChangeToNewState(manager.NormalTime, manager.TimeState);
    }

    public override void HandleKeyDownInput(PInputManager stateManager, KeyCode keyCode)
    {
        if (keyCode == KeyCode.V)
        {
            manager.ChangeToNewState(manager.NormalTime, manager.TimeState);
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

    public override void EnterState(PInputManager stateManager)
    {
    }

    public override void ExitState(PInputManager stateManager)
    {
    }

    public override void HandleKeyDownInput(PInputManager stateManager, KeyCode keyCode)
    {
    }

    public override void HandleKeyPressedInput(PInputManager stateManager, KeyCode keyCode)
    {
    }

    public override void HandleKeyUpInput(PInputManager stateManager, KeyCode keyCode)
    {
    }
}
public class TimeController : StateManagerComponent
{
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
    public event EventHandler<float> ValueUpdated;

    Task task;
    CancellationTokenSource source;
    public TimeController(MonoCalls.MonoAcessors manager, PlayerSC sC) : base(manager)
    {
        defaultFixedDelta = Time.fixedDeltaTime;
        defaultTimeScale = Time.timeScale;
        this.sC = sC;
        bulletBar = 0;
        EnemyAI.EnemyKilled += (object j, EventArgs f) => { EnemyKilled(); };
    }
    void EnemyKilled()
    {
        bulletBar += sC.SlowBarIncrement;

        if (bulletBar > sC.SlowBarMax)
        {
            bulletBar = sC.SlowBarMax;
            
        }
        ValueUpdated?.Invoke(this, bulletBar);
        Debug.Log(bulletBar + " bbar");
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
                ValueUpdated?.Invoke(this, bulletBar);




                await Task.Delay(wait);
            }
            }
            catch(OperationCanceledException)
            {
            source.Dispose();

        }

    }
    public void SetSlow(bool toggle)
    {
       
       
        if (toggle)
        {
            slow = 1/sC.TimeSlow;
            Time.timeScale = slow;
            Time.fixedDeltaTime = defaultFixedDelta * Time.timeScale;
            TimeController.playerDelta = sC.SlowAdvantage;
            StartDecrease();
        }
        else
        {
            slow = 1;                                               
            Time.fixedDeltaTime = defaultFixedDelta;
            Time.timeScale = defaultTimeScale;
             playerDelta = 1;
            Stop();
        }
    }

    
}
