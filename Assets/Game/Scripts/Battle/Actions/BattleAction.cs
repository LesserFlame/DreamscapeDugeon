using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAction : MonoBehaviour
{
    [SerializeField] public BattleActionData data;

    public BattleActor target;
    [HideInInspector]public BattleActor owner;

    private void Start()
    {
        owner = gameObject.GetComponent<BattleActor>();
    }
    public void Perform()
    {
        if (data.category == BattleActionData.Category.SPELL)
        {
            owner.MP -= data.manaCost;
            if (owner.gameObject.CompareTag("Player")) BattleUIManager.Instance.OnSliderChanged(1, owner.MP, owner.maxMP);

            target.OnTakeDamage(owner.baseATK + (data.damage / 10));
        }
    }
}
