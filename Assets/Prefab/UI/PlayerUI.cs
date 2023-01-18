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
    LerpBar bBarChange;
    [SerializeField] Image fadeOut;
    public TextMeshProUGUI MessageText { get => messageText;}

    public static PlayerUI Create(PlayerUI fab, PInputManager.UIInfoBoard board, PlayerSC sc, Transform parent)
    {
        PlayerUI ui = Instantiate(fab, parent);
        ui.board = board;
        ui.sc = sc;
        
        ui.Init();
        return ui;
    }
    void UpdateBT(float target)
    {
        bBarChange.StartLerp(target);
    }
   public  Coroutine StartFadeOut()
    {
     return   StartCoroutine(Fade());
    }
    IEnumerator Fade()
    {
        float elapsed = 0;
        while (fadeOut.color.a < 1)
        {
            elapsed += Time.deltaTime;

            Color c = fadeOut.color;
            c.a = 0 + (1 - 0) * (elapsed / 5);
            fadeOut.color = c;         

            yield return new WaitForEndOfFrame();
        }
    }
    void Init()
    { staminaChange = LerpBar.Create(stamina, sc.StaminaBarMax, 0, stamina.gameObject, sc.StaminaBarTick);
        bBarChange = LerpBar.Create(bBar, sc.SlowBarMax, 0, bBar.gameObject, sc.StaminaBarTick);
        board.StaminaChanged +=           staminaChange.StartLerp;
        Color c = fadeOut.color;
        c.a = 0;
        fadeOut.color = c;

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

        board.StaminaChanged -= staminaChange.StartLerp;
        board.HealthChanged -= UpdateHealth;
        board.BulletTimeChanged -= UpdateBT;
        board.UnequippedSlot -= EmptyHotBar;
        board.EquippedSlot -= SetHotBar;
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