using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour, IHitable
{
    [SerializeField] int maxHealth; //max health to set current enemy health
    [SerializeField] Transform targetPosition; //position enemy walking into
    [SerializeField] private int currentHealth;

    private Vector3 _movementLocal;
    
    private Animator _animator;
    private Transform player;      
    private bool isDead;
    private NavMeshAgent agent;
    private ShootOutPoint shootOutPoint;
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = Camera.main.transform; //get reference to player via main camera
        _animator = GetComponentInChildren<Animator>();
        
        //ignore update rotation when init
        agent.updateRotation = false;
        agent.updatePosition = false;
    }

    void RunBlend()
    {
        if (_animator == null || !_animator.enabled)
            return;
        
        if ((agent.nextPosition - transform.position).sqrMagnitude > 0.01f)
        {
            _movementLocal = transform.InverseTransformDirection(agent.nextPosition - transform.position);
        } 
        
        _animator.SetFloat("Z_Speed",_movementLocal.z);
        _animator.SetFloat("X_Speed",_movementLocal.x);
    }

    // Init is called when enemy first spawn
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

        RunBlend();
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
