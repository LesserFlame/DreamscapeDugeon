using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActor : BattleActor
{
    [HideInInspector] public bool initialized = false;
    public Transform spellTransform;
    [HideInInspector] public bool infusion = true;
    [HideInInspector] public bool pyroRebirth = true;
    private void Awake()
    {
        GetComponent<BattleAction>().owner = this;

        actions = FindAnyObjectByType<PlayerController>().actions;//GameManager.Instance.player.actions;
    }
    public override void OnTakeDamage(float damage)
    {
        float tempHP = HP;
        base.OnTakeDamage(damage);
        BattleUIManager.Instance.OnSliderChanged(0, HP, maxHP);

        if (infusion)
        {
            OnManaChange((tempHP - HP));
        }
        
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
    public void OnManaChange(float mana, bool increase = true)
    {
        float temp = MP;
        if (!increase) mana *= -1;
        MP += mana;
        if (MP > maxMP) MP = maxMP;
        if (MP < 0) MP = 0;
        BattleUIManager.Instance.OnSliderChanged(1, MP, maxMP);
        //Debug.Log(temp + " -> " + MP);
    }
    public void OnInitialize(PlayerController player)
    {
        if (!initialized) 
        { 
            HP = player.HP;
            maxHP = player.HP;
            MP = player.MP;
            maxMP = player.MP;
            speed = player.SP;
            baseATK = player.ATK;
            baseDEF = player.DEF;

            infusion = player.data.SKILLS[0];
            pyroRebirth = player.data.SKILLS[1];

            initialized = true;
        }
    }
    public void OnUpdateStats(PlayerController player)
    {
        HP += player.HP - maxHP;
        maxHP = player.HP;
        MP += player.MP - maxMP;
        maxMP = player.MP;
        speed = player.SP;
        baseATK = player.ATK;
        baseDEF = player.DEF;
        infusion = player.data.SKILLS[0];
        pyroRebirth = player.data.SKILLS[1];

        BattleUIManager.Instance.OnSliderChanged(0, HP, maxHP, false);
        BattleUIManager.Instance.OnSliderChanged(1, MP, maxMP, false);
    }
}
