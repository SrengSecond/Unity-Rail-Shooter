using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootOutPoint : MonoBehaviour
{
    [SerializeField] private EnemyEntry[] _enemyList;
    public bool AreaCleared { get; private set; }
    private PlayerMoves playerMove;
    [SerializeField] bool isActivePoint = false;
    private int enemyKilled, totalEnemies;
    

    public void Initialize(PlayerMoves value)
    {
        playerMove = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (var enemy in _enemyList)
        {
            enemy.enemyScript.gameObject.SetActive(false);
            totalEnemies = !(enemy.enemyScript is hostageScript) ? totalEnemies + 1 : totalEnemies + 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerMove.SetMovement(false);
        }

        if (Input.GetKeyDown(KeyCode.Return) && isActivePoint)
        {
            moveNextPoint();
        }

    }

    public void moveNextPoint()
    {
        Debug.Log("Cleared!");
        playerMove.AreaCleard(); //Allow player continuous moving.
        AreaCleared = true; //Defined areaClear to move on.
        isActivePoint = false;
    }

    //startShootOut is called when player arrived at shootingPoint. 
    public void startShootOut(float timer)
    {
        isActivePoint = true;
        playerMove.SetMovement(false); //make player stop
        StartCoroutine(sendEnemies()); //start spawn enemy
        this.DelayAction(SetAreaCleared,timer);
        GameManager.Instance.StartTimer(timer);
    }

    IEnumerator sendEnemies()
    {
        foreach (var enemy in _enemyList)
        {
            // totalEnemies = !(enemy.enemyScript is hostageScript) ? totalEnemies + 1 : totalEnemies + 0;
            yield return new WaitForSeconds(enemy.delay); //delay before spawn
            enemy.enemyScript.gameObject.SetActive(true);
            enemy.enemyScript.Init(this); // init function spawn
            
            Debug.Log(enemy.enemyScript.gameObject.name + "Enemy Spawned");
        }
    }

    public void EnemyKilled()
    {
        enemyKilled++;
        
        // if (enemyKilled == _enemyList.Length)
        if (enemyKilled == totalEnemies)
        {
            moveNextPoint();
            GameManager.Instance.stopTimer();
        }
    }

    public void SetAreaCleared()
    {
        if (AreaCleared) 
            return;
        AreaCleared = true;
        // playerMove.SetMovement(true);
        playerMove.AreaCleard();
        foreach (var enemy in _enemyList)
        {
            if(enemy.enemyScript == null)
                continue;
            
            enemy.enemyScript.StopShooting();
        }
    }
}

[System.Serializable]
public class EnemyEntry
{
    public EnemyScript enemyScript;
    public float delay;
}
