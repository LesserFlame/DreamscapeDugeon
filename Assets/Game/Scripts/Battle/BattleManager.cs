using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : Singleton<BattleManager>
{
    [Header("Music")]
    [SerializeField] AudioClip music;
    [SerializeField] AudioSource musicSource;

    [Header("Actors")]
    [SerializeField] public PlayerActor player;
    [SerializeField] public List<EnemyActor> enemies;
    [SerializeField] public List<EnemyActor> activeEnemies;

    [Header("UI")]
    [SerializeField] private BattleUIManager ui;

    //private int enemyCount = 0;

    public enum BattleState
    { 
        BATTLE_INACTIVE,
        BATTLE_START,
        BATTLE_STOP, 
        BATTLE_WON,
        BATTLE_LOST,
        PLAYER_DECISION,
        PLAYER_ACTION,
        ENEMY_DECISION,
        ENEMY_ACTION
    }

    BattleState state = BattleState.BATTLE_INACTIVE;
    float stateTimer = 0;

    void Start()
    {
        if (musicSource == null)
        {
            musicSource = new AudioSource();
        }
    }

    void Update()
    {
        switch (state)
        {
            case BattleState.BATTLE_INACTIVE:
                {
                    //nothing
                }
                break;
            case BattleState.BATTLE_START:
                if (music != null)
                {
                    musicSource.clip = music;
                    musicSource.Play();
                }
                state = BattleState.PLAYER_DECISION;

                break;

            case BattleState.PLAYER_DECISION:
                //wait for user input
                ui.detectInput = true;
                if(Input.GetKeyDown(KeyCode.Z))
                {
                    ui.OnShowButtons(false);
                    ui.OnShowOptions(true);
                }
                break;

            case BattleState.PLAYER_ACTION:
                //wait for player action
                ui.HideAll();
                ui.detectInput = false;
                if (stateTimer <= 0) OnPlayerAction();
                break;

            case BattleState.ENEMY_DECISION:
                //wait for enemy to choose action
                if (stateTimer <= 0) OnEnemyDecide();
                break;

            case BattleState.ENEMY_ACTION:
                //wait for enemy action
                if (stateTimer <= 0) OnEnemyAction();
                break;

            case BattleState.BATTLE_STOP:
                //decide win / loss
                break;

            case BattleState.BATTLE_WON:
                //give player xp / gold, send to overworld position
                break;

            case BattleState.BATTLE_LOST:
                //game over, send back to home
                break;

            default:
                break;
        }
        if (stateTimer > 0 ) stateTimer -= Time.deltaTime;
    }
    public void OnStartBattle(List<EnemyData> enemiesData) 
    {
        activeEnemies.Clear();
        int tempEnemy = 0;
        foreach (var enemy in enemiesData) 
        {
            if (tempEnemy < 3) enemies[tempEnemy].InitializeEnemy(enemy);
            activeEnemies.Add(enemies[tempEnemy]);
            tempEnemy++;
        }
        for (int i = tempEnemy; i < enemies.Count; i++) enemies[i].gameObject.SetActive(false);

        ui.OnShowButtons();
        ui.OnSliderChanged(0, 102, 124);
        ui.OnSliderChanged(1, 15, 32);
        ui.OnSelectButton(0);
        ui.OnInitialize();
        state = BattleState.BATTLE_START;
    }
    public void OnPlayerDecide()
    {
        state = BattleState.PLAYER_ACTION;
        stateTimer = 1;
    }
    public void OnPlayerAction()
    {
        state = BattleState.ENEMY_DECISION;
        stateTimer = 1;
    }
    public void OnEnemyDecide()
    {
        state = BattleState.ENEMY_ACTION;
        stateTimer = 1;
    }
    public void OnEnemyAction()
    {
        ui.OnShowButtons(true);
        ui.OnSelectButton(0);
        state = BattleState.PLAYER_DECISION;
    }

    internal void OnPlayerDeath()
    {
        throw new NotImplementedException();
    }
}