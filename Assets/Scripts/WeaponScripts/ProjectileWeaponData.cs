using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="CustomWeaponData", menuName ="Weapon Data")]
public class ProjectileWeaponData : ScriptableObject
{
    public System.Action<int> OnWeaponFires = delegate { };

    [SerializeField] private FireType type;
    [SerializeField] float rate  = 0.15f;
    [SerializeField] private int maxAmmo;
    [SerializeField] int damageValue = 0;
    [SerializeField] private bool defaultWeapon;
    
    [SerializeField] private AudioGetter gunshotsfx,reloadSfx, emptySfx, reloadWarningSfx;
    
    [SerializeField] private GameObject muzzleFx;
    [SerializeField] private float fxScale = 0.1f;
    [SerializeField] private Sprite WeaponIcon;

    private ParticleSystem _cachedFX;
    
    private Camera cam;
    private PlayerScript player;
    private int currentAmmo;
    private float nextFireTime;

    public Sprite GetIcon
    {
        get => WeaponIcon;
    }

    public void SetupWeapon(Camera cam, PlayerScript player)
    {
        this.cam = cam;
        this.player = player;
        nextFireTime = 0F;
        currentAmmo = maxAmmo;
        OnWeaponFires(currentAmmo);
        
        if (muzzleFx != null)
        {
            GameObject temp = Instantiate(muzzleFx);
            temp.transform.localScale = Vector3.one * fxScale;
            player.SetMuzzleFx(temp.transform);
            _cachedFX = temp.GetComponent<ParticleSystem>();
        }

    }

    public void playSfxWarning()
    {
        AudioPlayer.Instance.PlaySFX(emptySfx,player.transform);
        AudioPlayer.Instance.PlaySFX(reloadWarningSfx,player.transform);
    }

    public void WeaponUpdate()
    {
        if (type == FireType.SINGLE)
        {
            if (Input.GetMouseButtonDown(0)&& currentAmmo > 0)
            {
                Debug.Log("Mouse Click");
                Fire();
                
                currentAmmo--;
                OnWeaponFires(currentAmmo);
            }
            else if(Input.GetMouseButtonUp(0)&& currentAmmo <= 0)
            {
                playSfxWarning();
            }
        }
        else
        {
            if (Input.GetMouseButton(0) && Time.time > nextFireTime && currentAmmo > 0)
            {
                Debug.Log("Mouse Hold");
                Fire();
                currentAmmo--;
                OnWeaponFires(currentAmmo);
                nextFireTime = Time.time + rate;

            }
            else if (Input.GetMouseButton(0) && Time.time > nextFireTime && currentAmmo <= 0)
            {
                playSfxWarning(); 
            }
        }

        if (defaultWeapon && Input.GetMouseButtonDown(1))
        {
            Debug.Log("click Reload");
            currentAmmo = maxAmmo;
            AudioPlayer.Instance.PlaySFX(reloadSfx,player.transform);
            OnWeaponFires(currentAmmo);
        }
        
        else if (!defaultWeapon && currentAmmo <= 0)
        {
            Debug.Log("Weapon Switch");
            player.SwitchWeapon();
        }
    }

    private void Fire()
    {
        AudioPlayer.Instance.PlaySFX(gunshotsfx,player.transform);
        RaycastHit hitInfo; //RaycastHit hit to store hit collider
        Ray ray = cam.ScreenPointToRay(Input.mousePosition); //Ray ray to store ray where we point the ray out from camera
        
        if(_cachedFX != null)
        {
            Vector3 muzzlePos = cam.ScreenToViewportPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, .2f));
            _cachedFX.transform.position = muzzlePos;
            _cachedFX.transform.rotation = Quaternion.LookRotation(ray.direction);
            _cachedFX.Play();
            Debug.Log("Flash 1");
        }
        if(Physics.Raycast(ray, out hitInfo,50f))
        {
            //hitInfo have collider information
            if (hitInfo.collider != null)
            {
                //find whether collider hit have IHitable script
                IHitable[] hitables = hitInfo.collider.GetComponents<IHitable>();

                if (hitables != null && hitables.Length > 0)
                {
                    foreach (var hitable in hitables)
                    {
                        //if so, trigger function hit inside hitables object
                        hitable.Hit(hitInfo,damageValue);
                        
                        //Bug heritable not call  
                        
                        if (hitable is EnemyScript)
                        {
                            GameManager.Instance.ShotHit(true);
                            return;
                        }

                        GameManager.Instance.ShotHit(false);

                    }
                }
                    
                Debug.Log(hitInfo.collider.gameObject.name);
            }

            return;
        }
        GameManager.Instance.ShotHit(false);
    }
}

public enum FireType 
{
    SINGLE,
    RAPID
}
