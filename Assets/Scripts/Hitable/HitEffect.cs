using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour,IHitable
{
    [SerializeField] private GameObject effectsPrefabs;
    [SerializeField] private AudioGetter hitSfx;

    private ParticleSystem effectsCache;
    private ParticleSystem DeadEffectsCache;
    
    // Start is called before the first frame update
    void Start()
    {
        if (effectsPrefabs != null)
        {
            GameObject effectTemp = Instantiate(effectsPrefabs, transform);
            effectsCache = effectTemp.GetComponent<ParticleSystem>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hit(RaycastHit hit, int damage = 1)
    {
        if (effectsCache != null)
        {
            Debug.Log("Play Effect");
            effectsCache.transform.position = hit.point;
            effectsCache.transform.rotation = Quaternion.LookRotation(hit.normal);
            AudioPlayer.Instance.PlaySFX(hitSfx, effectsCache.transform);
            effectsCache.Play();
        }
    }
}
