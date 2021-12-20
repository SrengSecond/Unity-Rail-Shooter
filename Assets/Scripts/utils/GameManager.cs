using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameState state;
    [SerializeField] PlayerMoves playerMoves;
    [SerializeField] private PlayerScript _playerScript;
    [SerializeField] private int playerHealth = 10;

    [SerializeField] private float currentHealth;
    [SerializeField] private int enemyHit, shotsFired,totalEnemy,hostageKilled,enemyKilled;
        
    [SerializeField] private UiManager UiManager = new UiManager();

    private TimerObject _timerObject = new TimerObject();
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SwitchState(GameState.Start);
        Init();
    }

    private void OnDisable()
    {
        UiManager.RemoveEvent();
        PlayerMoves.OnLevelFinished -= ShowEndScreen;
    }

    void Init()
    {
        currentHealth = playerHealth;
        UiManager.Init(currentHealth);
        PlayerMoves.OnLevelFinished += ShowEndScreen;
    }

    public void SwitchState(GameState newState)
    {
        if (state == newState)
            return;
        state = newState;
        switch (state)
        {
            case GameState.Start:
                Debug.Log("GameStart");
                playerMoves.enabled = false;
                this.DelayAction(delegate { SwitchState(GameState.Gameplay); },3f );
                break;
            
            case GameState.Gameplay:
                Debug.Log("state: Gameplay" + Time.time);
                playerMoves.enabled = true;
                break;
            
            case GameState.Levelend:
                break;
        }
    }

    public void ShotHit(bool hit)
    {
        if (hit)
            enemyHit++;
        
        shotsFired++;
    }

    public void playerHit(float damage)
    {
        currentHealth -= damage;
        UiManager.UpdateHealthBar(currentHealth);
        _playerScript.shakeCamera(.5f,.25f,5f);
    }

    public void StartTimer(float duration)
    {
        _timerObject.StartTimer(this,duration);
    }

    public void stopTimer()
    {
        _timerObject.stopTimer(this);
    }

    public void RegisterEnemy()
    {
        totalEnemy++;
    }

    public void HostageKilled(Vector3 worldPos)
    {
        hostageKilled++;
        ShowHostStageKilled(worldPos, true);
        this.DelayAction(delegate { ShowHostStageKilled(worldPos, false); },3f);
    }

    public void EnemyKilled()
    {
        enemyKilled++;
    }

    void ShowHostStageKilled(Vector3 pos, bool show)
    {
        Vector3 screenPos = playerMoves.GetComponent<Camera>().WorldToScreenPoint(pos);
        UiManager.showHostStageKilled(screenPos,show);
    }

    void ShowEndScreen()
    {
        this.DelayAction(delegate {UiManager.ShowEndScreen(enemyKilled,totalEnemy,hostageKilled,shotsFired,enemyHit); }, 0.2f);
    }

    private void Update()
    {
        UiManager.MoveCrossHair(Input.mousePosition);
    }
}
    

public enum GameState
{
    Default,
    Start,
    Gameplay,
    Levelend
}