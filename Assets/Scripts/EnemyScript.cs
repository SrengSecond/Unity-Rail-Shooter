using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour, IHitable
{
    [SerializeField] int maxHealth;
    [SerializeField] Transform targetPosition;
    
    [SerializeField] private int currentHealth;
    private Transform player;
    private bool isDead;
    private NavMeshAgent agent;
    private ShootOutPoint shootOutPoint;
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = Camera.main.transform;
        
        //ignore update rotation when init
        agent.updateRotation = false;
    }

    public void Init(ShootOutPoint point)
    {
        currentHealth = maxHealth;
        shootOutPoint = point; //set the current shootOutPoint

        if (agent != null)
        {
            agent.SetDestination(targetPosition.position); // Move enemy to target position
        }

    }

    // Update is called once per frame
    void Update()
    {
        //Update rotation of enemy agent to face player camera
        if (player != null && !isDead)
        {
            var direction = player.position - transform.position;
            direction.y = 0f;
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    public void Hit(RaycastHit hit)
    {
        if (isDead)
            return; //if enemy is already dead terminate the function
        
        Debug.Log("Enemy got hit");
        currentHealth--; //remove health when enemy got hit
        if (currentHealth <= 0)
        {
            isDead = true; //set enemy to dead 
            agent.enabled = false; //disable enemy on inspector
            shootOutPoint.EnemyKilled();
            Destroy(gameObject);
        }
    }
}
