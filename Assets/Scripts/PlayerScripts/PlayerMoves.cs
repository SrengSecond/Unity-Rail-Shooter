using System;
using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;

public class PlayerMoves : MonoBehaviour
{
    public static System.Action OnLevelFinished = delegate { };
    
    [SerializeField] private PathCreator pathCreator;
    [SerializeField] private EndOfPathInstruction endPath;
    [SerializeField] private float speed = 1f;
    [SerializeField] bool isMoving = false;
    
    [SerializeField] ShootOutEntry[] ShootOutEntries;

    [Header("Debug")] 
    [SerializeField] private bool enableDebug;
    [SerializeField] private float preViewDistance;
    
    private float _distanceTravel;
    private int areaCleared;
    
        
    void Start()
    {
        foreach (var entry in ShootOutEntries)
        {
            entry.shootOutPoint.Initialize(this);
        }           
    }

    // Update is called once per frame
    void Update()
    {
        //PlayerMovement
        if (pathCreator != null && isMoving)
        {
            _distanceTravel += speed * Time.deltaTime;
            transform.position = pathCreator.path.GetPointAtDistance(_distanceTravel, endPath);
            transform.rotation = pathCreator.path.GetRotationAtDistance(_distanceTravel, endPath);   
        }
        
        //Method Move Camera loop over ShootoutEntries
        for (int i = 0; i < ShootOutEntries.Length; i++)
        {
            //Check Whether the current PlayerCamera Position have react each shootoutPoint 
            if ((pathCreator.path.GetPointAtDistance(ShootOutEntries[i].distance) - transform.position).sqrMagnitude <
                0.01f)
            {
                //check Whether the ShootoutPoint is clear - if so we move on with Return
                if (ShootOutEntries[i].shootOutPoint.AreaCleared)
                    return;

                if (isMoving)
                {
                    ShootOutEntries[i].shootOutPoint.startShootOut(ShootOutEntries[i].areaTimer);
                }

            }
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

    public void AreaCleard()
    {
        areaCleared++;
        if (areaCleared == ShootOutEntries.Length)
        {
            OnLevelFinished();
            return;
        }
        SetMovement(true);
    }

    public void SetMovement(bool isEnabled)
    {
        isMoving = isEnabled;
    }
}

[System.Serializable]
public class ShootOutEntry
{
    public ShootOutPoint shootOutPoint;
    public float distance;
    public float areaTimer = 5f;

}


