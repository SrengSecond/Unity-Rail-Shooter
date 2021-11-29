using System;
using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;

public class PlayerMoves : MonoBehaviour
{
    [SerializeField] private PathCreator pathCreator;
    [SerializeField] private EndOfPathInstruction endPath;
    [SerializeField] private float speed = 1f;
    public bool isMoving = false;
    private float _distanceTravel;

    [Header("Debug")] 
    [SerializeField] private bool enableDebug;
    [SerializeField] private float preViewDistance;
    
    
        
    void Start()
    {
                
    }

    // Update is called once per frame
    void Update()
    {

        if (pathCreator != null && isMoving)
        {
            _distanceTravel += speed * Time.deltaTime;
            transform.position = pathCreator.path.GetPointAtDistance(_distanceTravel, endPath);
            transform.rotation = pathCreator.path.GetRotationAtDistance(_distanceTravel, endPath);   
        }
    }

    private void OnValidate()
    {
        if (enableDebug)
        {
            transform.position = pathCreator.path.GetPointAtDistance(preViewDistance, endPath);
            transform.rotation = pathCreator.path.GetRotationAtDistance(preViewDistance, endPath);
        }
    }
}
