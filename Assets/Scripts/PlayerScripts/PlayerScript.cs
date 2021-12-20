using System.Collections;
using System.Collections.Generic;
using Kino;
using UnityEngine;


public class PlayerScript : MonoBehaviour
{
    public static System.Action<ProjectileWeaponData> onWeaponDataChanged = delegate { };
    
    
    [SerializeField] ProjectileWeaponData defaultWeapon;
    [SerializeField] private AnimationCurve _glitchFxCurve;
    [SerializeField] private ProjectileWeaponData _currentWeapon;
    [SerializeField] private AudioGetter damageSfx;
    
    private Camera _cam;
    private Transform _childFx;
    private DigitalGlitch _glitchFx;
    
    // Start is called before the first frame update
    void Start()
    {
        this.DelayAction(delegate { Debug.Log("Debugged Action run after 5 second"); { Debug.Log("Start function called"); } },5f);
        _cam = GetComponent<Camera>();
        _glitchFx = GetComponent<DigitalGlitch>();
        this.DelayAction(delegate { SwitchWeapon();},0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentWeapon != null)
        {
            // Debug.Log("_currentWeapon check");
            _currentWeapon.WeaponUpdate();
        }
    }
    
    public void SwitchWeapon(ProjectileWeaponData weapon = null)
    {
        _currentWeapon = weapon != null ? weapon : defaultWeapon;
        onWeaponDataChanged(_currentWeapon);
        _currentWeapon.SetupWeapon(_cam, this);
    }
    
    public void SetMuzzleFx(Transform fx) //set where place where muzzle should be spawn
    {
        if(_childFx != null)
            Destroy(_childFx.gameObject);
        
        fx.SetParent(transform);
        _childFx = fx;
    }

    IEnumerator DoCameraShake(float timer, float amp, float freq)
    {
        AudioPlayer.Instance.PlaySFX(damageSfx,transform);
        Vector3 initPos = transform.position;
        Vector3 newPos = transform.position;
        float duration = timer;
        yield return new WaitForSeconds(0.2f);
        
        while (duration >0f)
        {
            if ((newPos - transform.position).sqrMagnitude < 0.01f)
            {
                newPos = initPos;
                newPos.x += Random.Range(-.5f, .5f) * amp;
                newPos.y += Random.Range(-.5f, .5f) * amp;
                
            }
            transform.position = Vector3.Lerp(transform.position,newPos,freq);

            _glitchFx.intensity = _glitchFxCurve.Evaluate(duration / timer);
            
            duration -= Time.deltaTime;
            yield return null;
        }

        _glitchFx.intensity = 0;
        transform.position = initPos;
        
    }

    public void shakeCamera(float timer, float amp, float freq)
    {
        StartCoroutine(DoCameraShake(timer,amp,freq));
    }
}