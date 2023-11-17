using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(BattleAction))]
public class EnemyActor : BattleActor
{
    // Start is called before the first frame update
    private Animator animator;
    private BattleAction action;
    public int rewardXP;
    void Awake()
    {
        animator = GetComponent<Animator>();
        action = GetComponent<BattleAction>();
        action.target = BattleManager.Instance.player;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitializeEnemy(EnemyData data)
    {
        gameObject.SetActive(true);
        actorName = data.enemyName;
        maxHP = data.maxHP;
        HP = data.maxHP;
        baseATK = data.baseATK;
        baseDEF = data.baseDEF;
        speed = data.speed;
        rewardXP = data.rewardXP;

        animator.runtimeAnimatorController = data.animator;
        actions = data.actions;
    }
    public override void OnTakeDamage(float damage)
    {
        base.OnTakeDamage(damage);
        if (HP <= 0) { OnDeath(); }
    }

    public override void OnDeath()
    {
        base.OnDeath();
        BattleManager.Instance.activeEnemies.Remove(this);
        BattleManager.Instance.OnEnemyDeath();
        GameManager.Instance.player.IncreaseXP(rewardXP);

        var player = BattleManager.Instance.player;
        if (player.pyroRebirth)
        {
            player.OnManaChange(maxHP);
        }
    }

    public override void OnDecide()
    {
        action.data = actions[Random.Range(0, actions.Count)];
        action.Perform();
        //BattleManager.On();
        //Debug.Log(action.data.actionName);
    }
}
