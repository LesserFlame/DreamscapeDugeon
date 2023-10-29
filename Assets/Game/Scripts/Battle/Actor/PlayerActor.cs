using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActor : BattleActor
{
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
}
