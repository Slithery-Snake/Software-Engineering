using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "playerSC", menuName = "PlayerSC")]
public class PlayerSC : HumanoidSC
{
    [SerializeField] private float timeSlow;
    [SerializeField] private float slowAdvantage;
    [SerializeField] private float slowBarMax;
    [SerializeField] private float slowBarDecrement;
    [SerializeField] private float slowBarIncrement;
    [SerializeField] private float runSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float staminaBarMax;
    [SerializeField] private float staminaBarDecrement;
    [SerializeField] private float staminaBarTick;
    [SerializeField] private float staminaIncrement;
    [SerializeField] private float staminaWaitUntilRegen;
    [SerializeField] private float stunMovementPercentReduction;
    public float TimeSlow { get => timeSlow;  }
    public float SlowAdvantage { get => slowAdvantage; }
    public float SlowBarMax { get => slowBarMax;  }
    public float SlowBarDecrement { get => slowBarDecrement; }
    public float SlowBarIncrement { get => slowBarIncrement;  }
    public float RunSpeed { get => runSpeed;  }
    public float WalkSpeed { get => walkSpeed;  }
    public float StaminaBarMax { get => staminaBarMax; }
    public float StaminaBarDecrement { get => staminaBarDecrement;  }
    public float StaminaBarTick { get => staminaBarTick;  }
    public float StaminaIncrement { get => staminaIncrement; }
    public float StaminaWaitUntilRegen { get => staminaWaitUntilRegen; }
    public float StunMovementPercentReduct { get => stunMovementPercentReduction; }
   
}
