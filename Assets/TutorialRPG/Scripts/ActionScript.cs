using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionScript : MonoBehaviour
{
    public GameObject owner;

    [SerializeField] private string animationName;

    [SerializeField] private bool magicAttack;
    [SerializeField] private float magicCost;

    [SerializeField] private float minAttackMultiplier;
    [SerializeField] private float maxAttackMultiplier;

    [SerializeField] private float minDefenseMultiplier;
    [SerializeField] private float maxDefenseMultiplier;

    private ActorStats attackerStats;
    private ActorStats targetStats;
    private float damage = 0.0f;

    public void Attack(GameObject target)
    {
        attackerStats = owner.GetComponent<ActorStats>();
        targetStats = target.GetComponent<ActorStats>();

        if (attackerStats.magic >= magicCost)
        {
            float multiplier = Random.Range(minAttackMultiplier, maxAttackMultiplier);

            damage = multiplier * attackerStats.melee;
            if (magicCost > 0) attackerStats.UpdateMagicFill(magicCost);
            if (magicAttack)
            {
                damage = multiplier * attackerStats.spell;
                attackerStats.magic = attackerStats.magic - magicCost;
            }

            float defenseMultiplier = Random.Range(minDefenseMultiplier, maxDefenseMultiplier);
            damage = Mathf.Max(0, damage - (defenseMultiplier * targetStats.defense));
            owner.GetComponent<Animator>().Play(animationName);
            targetStats.ReceiveDamage(Mathf.CeilToInt(damage));
        }
        else targetStats.ReceiveDamage(0);
    }
}