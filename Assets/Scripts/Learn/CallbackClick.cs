using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CallbackClick : MonoBehaviour
{
    public void CallbackFunction()
    {
        Debug.Log("Click Callback");
    }

    public void CallbackAnotherClick()
    {
        Debug.Log("Click Another Callback");
    }

    private void OnEnable()
    {
        ObserverPatten.UpdateClick += CallbackFunction; 
        ObserverPatten.UpdateClick += CallbackAnotherClick; 
    }
}