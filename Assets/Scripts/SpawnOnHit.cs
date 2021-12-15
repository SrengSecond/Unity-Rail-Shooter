using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnHit : MonoBehaviour, IHitable
{
    [SerializeField] private GameObject prefabToSpawn;

    [SerializeField] private bool destroyOnHit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hit(RaycastHit hit, int damage = 1)
    {
        if (prefabToSpawn != null)
        {
            Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
        }
        if(destroyOnHit)
            Destroy(gameObject);
    }
}
