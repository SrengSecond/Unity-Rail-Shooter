using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hostageScript : EnemyScript
{

    protected override void BehaviourSetup()
    {
        agent.SetDestination(targetPosition.position);
    }

    protected override void DeadBehavior()
    {
        GameManager.Instance.playerHit(1);
        GameManager.Instance.HostageKilled(transform.position + Vector3.up * 1.2f);
    }
}
