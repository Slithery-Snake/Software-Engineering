using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] Slider stamina;
    [SerializeField] Slider health;
    [SerializeField] Slider bBar;
    PInputManager.UIInfoBoard board;
    PlayerSC sc;
    LerpBar staminaChange;
    public static PlayerUI Create(PlayerUI fab, PInputManager.UIInfoBoard board, PlayerSC sc)
    {
        PlayerUI ui = Instantiate(fab);
        ui.board = board;
        ui.sc = sc;
        ui.Init();
        return ui;
    }
    void Init()
    { staminaChange = LerpBar.Create(stamina, sc.StaminaBarMax, 0, stamina.gameObject);
        board.StaminaChanged += (object t, float target) =>
        {
            staminaChange.StartLerp(target, sc.StaminaBarTick);   };

        health.minValue = 0;
        health.maxValue = sc.Health;
        health.value = health.maxValue;
        board.HealthChanged += (object t, float target) => { health.value = target; };
        board.BulletTimeChanged += (object t, float target) => { bBar.value = target; };
        bBar.minValue = 0;
        bBar.maxValue = sc.SlowBarMax;
        bBar.value = 0;
    }
}
public class LerpBar :MonoBehaviour
{
    Slider slider;
    Coroutine routine;
    MonoBehaviour manager;
    public static LerpBar Create(Slider slider, float maxV, float minV, GameObject obj)
    {
        LerpBar f = obj.AddComponent<LerpBar>();
        f.slider = slider;
        f.slider.maxValue = maxV;
        f.slider.minValue = minV;
        f.slider.value = maxV;

        return f;
    }

    public void StartLerp( float targetValue, float time)
    {
        if (routine != null) { StopCoroutine(routine); }
       routine = StartCoroutine(Lerping(slider.value ,targetValue, time));
    }
    IEnumerator Lerping(float f, float tV, float t)
    { float elapsed = 0;
        do {
            elapsed += Time.deltaTime;
            slider.value = f + (tV - f) * (elapsed / t);
            if(elapsed >= t)
            {
                slider.value = tV;
                break;
            }
            
            yield return new WaitForEndOfFrame();
                } while (true);
    }
    
} 