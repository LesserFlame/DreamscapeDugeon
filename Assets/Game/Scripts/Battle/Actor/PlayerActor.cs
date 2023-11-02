using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActor : BattleActor
{
    [HideInInspector] public bool initialized = false;
    public Transform spellTransform;
    private void Start()
    {
        GetComponent<BattleAction>().owner = this;
    }
    public override void OnTakeDamage(float damage)
    {
        base.OnTakeDamage(damage);
        BattleUIManager.Instance.OnSliderChanged(0, HP, maxHP);
        if (HP <= 0) { OnDeath(); }
    }
    public override void OnDeath()
    {
        //base.OnDeath();
        BattleManager.Instance.OnPlayerDeath();
    }

    public override void OnDecide()
    {
        //nothing
    }

    public void OnInitialize(PlayerController player)
    {
        if (!initialized) 
        { 
            HP = player.data.HP;
            maxHP = player.data.HP;
            MP = player.data.MP;
            maxMP = player.data.MP;
            speed = player.data.SP;
            baseATK = player.data.ATK;
            baseDEF = player.data.DEF;

            initialized = true;
        }
    }
    public void OnUpdateStats(PlayerController player)
    {
        HP += player.data.HP - maxHP;
        maxHP = player.data.HP;
        MP += player.data.MP - maxMP;
        maxMP = player.data.MP;
        speed = player.data.SP;
        baseATK = player.data.ATK;
        baseDEF = player.data.DEF;
    }
}
