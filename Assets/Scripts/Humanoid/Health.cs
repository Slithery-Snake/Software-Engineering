
using System;
using UnityEngine.Events;
public class Health : IShootable
{
    
    protected float currentHealth;
    public event EventHandler<float> HealthChanged;

    public event UnityAction HealthBelowZero;
    protected HumanoidSC sC;


    public Health(HumanoidSC sC)
    {
        this.sC = sC;
        currentHealth = sC.Health;
    }

    public void ShotAt(BulletSC bullet)
    {
        currentHealth -= bullet.Damage;
        HealthChanged?.Invoke(this, currentHealth);
        if (currentHealth <= 0 )
        {
            HealthZero();
        }
        
    }

    protected void HealthZero()
    {
        HealthBelowZero();

        
    }
  
}
             