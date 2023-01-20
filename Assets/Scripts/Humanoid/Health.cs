
using System;
using UnityEngine.Events;
public class Health : IShootable
{
    
    protected float currentHealth;
    public event UnityAction<float> HealthChanged;
   
    public event UnityAction HealthBelowZero;
    protected HumanoidSC sC;


    public Health(HumanoidSC sC)
    {
        this.sC = sC;
        currentHealth = sC.Health;
        HealthChanged += HealthZero;
    }

    public void ShotAt(BulletSC bullet)
    {
        currentHealth -= bullet.Damage;

        InvokeHealthChanged();
        
        
    }
    void InvokeHealthChanged()
    {
        HealthChanged?.Invoke( currentHealth);
    }
    public void AddHealth(int i)
    {
        currentHealth += i;
        if(currentHealth > sC.Health)
        {
            currentHealth = sC.Health;
        }
        InvokeHealthChanged();
    }
    public void Remove(int i)
    {
        currentHealth -= i;
        if (currentHealth > sC.Health)
        {
            currentHealth = sC.Health;
        }
        InvokeHealthChanged();
    }
    protected void HealthZero(float i)
    {
        if (i <= 0)
        {
            HealthBelowZero?.Invoke();
            HealthChanged -= HealthZero;

        }
       

        
    }
  
}
             