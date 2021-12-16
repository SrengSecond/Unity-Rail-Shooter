using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="CustomWeaponData", menuName ="Weapon Data")]
public class ProjectileWeaponData : ScriptableObject 
{
    [SerializeField] private FireType type;
    [SerializeField] float rate  = 0.15f;
    [SerializeField] private int maxAmmo;
    [SerializeField] int damageValue = 0;
    [SerializeField] private bool defaultWeapon;

    private Camera cam;
    private PlayerScript player;
    private int currentAmmo;
    private float nextFireTime;
    
    public void SetupWeapon(Camera cam, PlayerScript player)
    {
        this.cam = cam;
        this.player = player;
        nextFireTime = 0F;
        currentAmmo = maxAmmo;
    }

    public void WeaponUpdate()
    {
        if (type == FireType.SINGLE)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Mouse Click");
                Fire();
                currentAmmo--;
            }
        }
        else
        {
            if (Input.GetMouseButton(0) && Time.time > nextFireTime && currentAmmo > 0)
            {
                Debug.Log("Mouse Hold");
                Fire();
                currentAmmo--;
                nextFireTime = Time.time + rate;
            }
            else if (currentAmmo <= 0)
            {
                Debug.Log(("Ammo runs out, please reload"));
            }
            Debug.Log("bullet" + currentAmmo);
        }

        if (defaultWeapon && Input.GetMouseButtonDown(1))
        {
            currentAmmo = maxAmmo;
        }
        else if (!defaultWeapon && currentAmmo <= 0)
        {
            Debug.Log("Weapon Switch");
            player.SwitchWeapon();
        }
    }

    private void Fire()
    {
        RaycastHit hitInfo; //RaycastHit hit to store hit collider
        Ray ray = cam.ScreenPointToRay(Input.mousePosition); //Ray ray to store ray where we point the ray out from camera
            
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
                    }
                }
                    
                Debug.Log(hitInfo.collider.gameObject.name);
            }
        }
    }
}

public enum FireType 
{
    SINGLE,
    RAPID
}
