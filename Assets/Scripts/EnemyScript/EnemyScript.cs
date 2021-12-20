using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour, IHitable
{
    [SerializeField] int maxHealth; //max health to set current enemy health
    [SerializeField] protected Transform targetPosition; //position enemy walking into
    [SerializeField] public int currentHealth;
    
    [Header("Shooting Properties")]
    [SerializeField] private IntervalRange interval = new IntervalRange(1.5f, 2.7f);
    [SerializeField] private float shootAccuracy = 0.50f;
    [SerializeField] private ParticleSystem shotFx;

    private Vector3 _movementLocal;
    
    private Animator _animator;
    private Transform player;      
    private bool isDead;
    protected NavMeshAgent agent;
    private ShootOutPoint shootOutPoint;
    
    // Start is called before the first frame update
    void Awake()
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
            BehaviourSetup();
        }
    }

    protected virtual void BehaviourSetup()
    {
        agent.SetDestination(targetPosition.position);// Move enemy to target position
        StartCoroutine(Shoot());
        GameManager.Instance.RegisterEnemy();
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
            DeadBehavior();
            _animator.SetTrigger("Dead");
            _animator.SetBool("Is Dead",true);
            Destroy(gameObject,4f);
        }
        
        else
        {
            _animator.SetTrigger("Shot");
        }
    }

    protected virtual void DeadBehavior()
    {
        shootOutPoint.EnemyKilled();
        StopShooting();
        GameManager.Instance.EnemyKilled();
    }
    
    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(0.2f);
        yield return new WaitUntil(() =>
        {
            if (!isDead)
                return agent.remainingDistance < 0.02f;
            
            return true;
        });
        
        while (!isDead)
        {
            shotFx.transform.rotation = Quaternion.LookRotation(transform.forward + Random.insideUnitSphere * 0.1f);
            if (Random.Range(0, .5f) < shootAccuracy)
            {
                shotFx.transform.rotation = Quaternion.LookRotation(player.position - shotFx.transform.position);
                
                GameManager.Instance.playerHit(1f);
                Debug.Log("Player have been hit");
            }
            shotFx.Play();
            yield return new WaitForSeconds(interval.GetValue);
        }
    }

    public void StopShooting()
    {
        StopAllCoroutines();
    }
}

[System.Serializable]
public struct IntervalRange
{
    [SerializeField] private float min, max;

    public IntervalRange(float min, float max)
    {
        this.min = min;
        this.max = max;
    }

    public float GetValue
    {
        get => Random.Range(min, max);
    }
}
