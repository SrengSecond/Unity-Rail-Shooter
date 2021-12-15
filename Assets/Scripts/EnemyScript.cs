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
        if (_animator == null || !_animator.enabled || isDead || !agent.enabled)
            return;
        
        // if ((agent.nextPosition - transform.position).sqrMagnitude > 0.01f)
        // {
        //     _movementLocal = transform.InverseTransformDirection(agent.nextPosition - transform.position);
        // }
        //
        
        if (agent.remainingDistance > 0.01f)
        {
            _movementLocal = Vector3.Lerp(_movementLocal, transform.InverseTransformDirection(agent.velocity).normalized,2f * Time.deltaTime);
            agent.nextPosition = transform.position;
        }
        else
        {
            _movementLocal = Vector3.zero;
        }

        _animator.SetFloat("X Speed",_movementLocal.x);
        _animator.SetFloat("Z Speed",_movementLocal.z);
        
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

    public void Hit(RaycastHit hit, int damage = 1)
    {
        if (isDead)
            return; //if enemy is already dead terminate the function
        
        Debug.Log("Enemy got hit");
        currentHealth -= damage ; //remove health when enemy got hit
        if (currentHealth <= 0)
        {
            isDead = true; //set enemy to dead 
            agent.enabled = false; //disable enemy on inspector
            shootOutPoint.EnemyKilled();
            _animator.SetTrigger("Dead");
            _animator.SetBool("Is Dead",true);
            Destroy(gameObject,4f);
        }
        else
        {
            _animator.SetTrigger("Shot");
        }
    }
}
