using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObserverPatten : MonoBehaviour
{
    public delegate void _delegate();
    public static _delegate UpdateClick;
    public static Action OnCompleteListener;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            UpdateClick();
        }
    }
}
