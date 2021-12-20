using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class UiManager
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private RectTransform hoststageKilledText;

    [Header("weapon HUD")] 
    [SerializeField] private Image weaponIcon;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private GameObject reloadWarning;
    [SerializeField] private RectTransform crossHair;

    [Header("Score Propertiesse")] 
    [SerializeField] private TextMeshProUGUI enemyKilled;
    [SerializeField] private TextMeshProUGUI HostageKilled;
    [SerializeField] private TextMeshProUGUI shots;
    [SerializeField] private TextMeshProUGUI hit;
    [SerializeField] private TextMeshProUGUI accuracy;
    [SerializeField] private GameObject endScreenPanel;
    
    private ProjectileWeaponData currentWeapon;
    
    public void Init(float maxHealth)
    {
        if (crossHair != null)
            Cursor.visible = false;
        hoststageKilledText.gameObject.SetActive(false);
        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth;
        PlayerScript.onWeaponDataChanged += UpdateWeapon;
        TimerObject.OnTimerChanged += UpdateTimer;
    }

    public void UpdateTimer(int currrentTimer)
    {
        timerText.SetText(currrentTimer.ToString("00"));
    }

    public void RemoveEvent()
    {
        PlayerScript.onWeaponDataChanged -= UpdateWeapon;
        TimerObject.OnTimerChanged -= UpdateTimer;
        // currentWeapon.OnWeaponFires -= updateAmmo;
    }

    private void UpdateWeapon(ProjectileWeaponData obj)
    {
        if (currentWeapon != null)
        {
            currentWeapon.OnWeaponFires -= updateAmmo;
        }

        currentWeapon = obj;
        currentWeapon.OnWeaponFires += updateAmmo;
        weaponIcon.sprite = currentWeapon.GetIcon;
        
        Debug.Log(obj.name);
    }

    public void UpdateHealthBar(float value)
    {
        healthBar.value = value;
    }

    void updateAmmo(int ammo)
    {
        reloadWarning.SetActive(ammo <= 0);
        ammoText.SetText(ammo.ToString("00"));
    }

    public void showHostStageKilled(Vector3 pos, bool show)
    {
        hoststageKilledText.gameObject.SetActive(show);
        if (!show)
            return;
        hoststageKilledText.position = pos;
        Vector2 adjustPos = Extensions.GetPositionInsideScreen(new Vector2(1920f, 1080f), hoststageKilledText, 25f);
        hoststageKilledText.anchoredPosition = adjustPos;
    }

    public void ShowEndScreen(int enemyKill, int totalEnemies, int hostagekill, int totalShots, int totalHit)
    {
        endScreenPanel.SetActive(true);
        enemyKilled.SetText(((enemyKill/(float)totalEnemies) * 100f).ToString("00") + "%");
        HostageKilled.SetText(hostagekill.ToString());
        shots.SetText(totalShots.ToString());
        hit.SetText(totalHit.ToString());
        accuracy.SetText(((totalHit/(float)totalShots)*100f).ToString("00") + "%");
    }

    public void MoveCrossHair(Vector3 mousePosition)
    {
        if (crossHair != null)
        {
            crossHair.position = mousePosition;
            
        }
    }
}
