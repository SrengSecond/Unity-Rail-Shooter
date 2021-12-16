using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] ProjectileWeaponData defaultWeapon;
    private Camera cam;
    private ProjectileWeaponData _currentWeapon;
    
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        SwitchWeapon();
    }

    // Update is called once per frame
    void Update()
    {

        if (_currentWeapon != null)
        {
            Debug.Log("_currentWeapon check");
            _currentWeapon.WeaponUpdate();
        }

    }

    public void SwitchWeapon(ProjectileWeaponData weapon = null)
    {
        _currentWeapon = weapon != null ? weapon : defaultWeapon;
        _currentWeapon.SetupWeapon(cam, this);
    }
    
}