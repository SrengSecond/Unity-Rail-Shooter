using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplotionDamage : MonoBehaviour
{
    [SerializeField] private float damageRadius;
    [SerializeField] private float delayUnitDestroy;
    
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject,delayUnitDestroy);
        DamageNearByObject();
    }

    void DamageNearByObject()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (var col in cols)
        {
            IHitable[] hitables = col.GetComponents<IHitable>();
            RaycastHit hit;
            if(Physics.Raycast(transform.position,col.transform.position - transform.position, out hit))
                if (hitables != null && hitables.Length > 0)
                {
                    foreach (var hitable in hitables)
                    {
                        //if so, trigger function hit inside hitables object
                        hitable.Hit(hit,100);
                    }
                }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,damageRadius);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
