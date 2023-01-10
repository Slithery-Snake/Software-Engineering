using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] Slider stamina;
    [SerializeField] Slider health;
    [SerializeField] Slider bBar;
    [SerializeField] TextMeshProUGUI messageText;
    PInputManager.UIInfoBoard board;
    PlayerSC sc;
    LerpBar staminaChange;
    HotBarUI[] hotBarUIs;
    [SerializeField] HotBarUI hbarUI;
    [SerializeField] Transform hbarGrid;

    public TextMeshProUGUI MessageText { get => messageText;}

    public static PlayerUI Create(PlayerUI fab, PInputManager.UIInfoBoard board, PlayerSC sc, Transform parent)
    {
        PlayerUI ui = Instantiate(fab, parent);
        ui.board = board;
        ui.sc = sc;
        
        ui.Init();
        return ui;
    }
    
    void Init()
    { staminaChange = LerpBar.Create(stamina, sc.StaminaBarMax, 0, stamina.gameObject, sc.StaminaBarTick);
        board.StaminaChanged +=
        
           staminaChange.StartLerp;

        health.minValue = 0;
        hotBarUIs = new HotBarUI[sc.InventorySlots];

        for(int i = 0; i< sc.InventorySlots; i++)
        {
            hotBarUIs[i] = Instantiate(hbarUI, hbarGrid);
            hotBarUIs[i].SetEmpty();
        }
        health.maxValue = sc.Health;
        health.value = health.maxValue;
        board.HealthChanged += UpdateHealth;
        board.BulletTimeChanged += UpdateBT;
        bBar.minValue = 0;
        bBar.maxValue = sc.SlowBarMax;
        bBar.value = 0;
        board.UnequippedSlot += EmptyHotBar;
        board.EquippedSlot += SetHotBar;
    }
    private void OnDestroy()
    {
        board.StaminaChanged -=

           staminaChange.StartLerp;
        board.HealthChanged -= UpdateHealth;
        board.BulletTimeChanged -= UpdateBT;
        board.UnequippedSlot -= EmptyHotBar;
        board.EquippedSlot -= SetHotBar;
    }
    void UpdateBT(float target)
    {
        bBar.value = target;
    }
   void UpdateHealth(float target)
    {
        health.value = target;
    }
    void SetHotBar(int i, HotBarItemSC str)
    {
        hotBarUIs[i].SetText(str.HotBarName1);
    }
    void EmptyHotBar(int i)
    {
        hotBarUIs[i].SetEmpty();
    }
}

public class LerpBar :MonoBehaviour
{
    Slider slider;
    Coroutine routine;
    MonoBehaviour manager;
        float lerpTime;
    public static LerpBar Create(Slider slider, float maxV, float minV, GameObject obj, float lerpTime)
    {
        LerpBar f = obj.AddComponent<LerpBar>();
        f.slider = slider;
        f.slider.maxValue = maxV;
        f.slider.minValue = minV;
        f.slider.value = maxV;
            f.lerpTime = lerpTime;

        return f;
    }

    public void StartLerp( float targetValue)
    {
        if (routine != null) { StopCoroutine(routine); }
       routine = StartCoroutine(Lerping(slider.value ,targetValue, lerpTime));
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