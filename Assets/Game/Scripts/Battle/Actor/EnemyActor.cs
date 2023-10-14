using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyActor : BattleActor
{
    // Start is called before the first frame update
    private Animator animator;
    void Awake()
    {
        animator = GetComponent<Animator>();
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

        animator.runtimeAnimatorController = data.animator;
    }
    public override void OnTakeDamage(float damage)
    {
        base.OnTakeDamage(damage);
        if (HP < 0) { OnDeath(); }
    }

    public override void OnDeath()
    {
        base.OnDeath();
        BattleManager.Instance.activeEnemies.Remove(this);
    }
}
