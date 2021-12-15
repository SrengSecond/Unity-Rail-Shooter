using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour,IHitable
{
    [SerializeField] private GameObject effectsPrefabs;
    private ParticleSystem effectsCache;
    
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
            effectsCache.transform.position = hit.point;
            effectsCache.transform.rotation = Quaternion.LookRotation(hit.normal);
            effectsCache.Play();
        }
    }
}
