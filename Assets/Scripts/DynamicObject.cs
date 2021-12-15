using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicObject : MonoBehaviour, IHitable
{
    private Rigidbody rb;

    [SerializeField] private float knockBackForce = 50f; 
    // Start is called before the first frame update
    void Start()
    {
        //referent to rb
        rb = GetComponent<Rigidbody>();
        
        //if rb not fount, add rb to the component
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hit(RaycastHit hit, int damage = 1)
    {
        rb.AddForce(-hit.normal * knockBackForce);
    }
}
