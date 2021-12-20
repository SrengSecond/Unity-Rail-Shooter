using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnHit : MonoBehaviour, IHitable
{
    [SerializeField] private GameObject prefabToSpawn;
    [SerializeField] private float spawnOffset = .5f;
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
            Instantiate(prefabToSpawn, transform.position + (Vector3.up * spawnOffset), Quaternion.identity);
        }
        
        if(destroyOnHit)
            Destroy(gameObject);
    }
}
