using System;
using UnityEngine;

public class WeaponPickup : MonoBehaviour, IHitable
{
    [SerializeField] private ProjectileWeaponData weaponData;
    [SerializeField] private float rotateSpeed = 45;
    [SerializeField] private AudioGetter pickupSfx;

    private PlayerScript _playerScript;
    
    public void Hit(RaycastHit hit, int damage = 1)
    {
        _playerScript.SwitchWeapon(weaponData);
        AudioPlayer.Instance.PlaySFX(pickupSfx);
        Destroy(gameObject);
    }

    private void Start()
    {
        _playerScript = FindObjectOfType<PlayerScript>();
        Debug.Log(_playerScript.name);    
    }

    private void Update()
    {
        transform.Rotate(Vector3.up,rotateSpeed * Time.deltaTime);
    }
}