using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [Header("Music")]
    [SerializeField] AudioClip music;
    [SerializeField] AudioSource musicSource;

    public enum BattleState
    { 
        BATTLE_START,
        BATTLE_STOP, 
        BATTLE_WON,
        BATTLE_LOST,
        PLAYER_DECISION,
        PLAYER_ACTION,
        ENEMY_DECISION,
        ENEMY_ACTION
    }

    BattleState state = BattleState.BATTLE_START;
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
                break;

            case BattleState.PLAYER_ACTION:
                //wait for player action
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
        state = BattleState.PLAYER_DECISION;
    }
}