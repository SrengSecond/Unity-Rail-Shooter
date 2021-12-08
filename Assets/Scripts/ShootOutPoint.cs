using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootOutPoint : MonoBehaviour
{
    [SerializeField] private EnemyEntry[] _enemyList;
    public bool AreaCleared { get; private set; }
    private PlayerMoves playerMove;
    [SerializeField] bool isActivePoint = false;
    private int enemyKilled = 0;

    public void Initialize(PlayerMoves value)
    {
        playerMove = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        
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
        playerMove.SetMovement(true); //Allow player continuous moving.
        AreaCleared = true; //Defined areaClear to move on.
        isActivePoint = false;
    }

    //startShootOut is called when player arrived at shootingPoint. 
    public void startShootOut()
    {
        isActivePoint = true;
        playerMove.SetMovement(false); //make player stop
        StartCoroutine(sendEnemies()); //start spawn enemy 
    }

    IEnumerator sendEnemies()
    {
        foreach (var enemy in _enemyList)
        {
            yield return new WaitForSeconds(enemy.delay); //delay before spawn
            enemy.enemyScript.Init(this); // init function spawn
            Debug.Log(enemy.enemyScript.gameObject.name + "Enemy Spawned");
        }
    }

    public void EnemyKilled()
    {
        enemyKilled++;
        if (enemyKilled == _enemyList.Length)
        {
            moveNextPoint();
        }
    }
}

[System.Serializable]
public class EnemyEntry
{
    public EnemyScript enemyScript;
    public float delay;
}
