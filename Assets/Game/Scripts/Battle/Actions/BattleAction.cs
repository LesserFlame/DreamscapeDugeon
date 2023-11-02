using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAction : MonoBehaviour
{
    [SerializeField] public BattleActionData data;

    public BattleActor target;
    [HideInInspector] public BattleActor owner;

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
        }
        else ApplyDamage();
        if (data.effect != null) SpawnEffect();
    }
    public void ApplyDamage()
    {
        target.OnTakeDamage(owner.baseATK * data.damage);
        //if (owner.gameObject.CompareTag("Player")) BattleManager.Instance.OnPlayerAction(); //Manually change to enemy turn
    }
    public void SpawnEffect()
    {
        Transform spawnTransform = owner.transform;

        if (owner.gameObject.CompareTag("Player")) spawnTransform = owner.GetComponent<PlayerActor>().spellTransform;

        BattleEffect effectRef = Instantiate(data.effect, spawnTransform.position, Quaternion.FromToRotation(owner.transform.position, target.transform.position));
        effectRef.tag = gameObject.tag;
        effectRef.owner = this;
        
        if (effectRef.effectType == BattleEffect.EffectType.PROJECTILE)
        {
            var rb = effectRef.gameObject.GetComponent<Rigidbody2D>();
            Vector2 direction = target.transform.position - owner.transform.position;
            rb.AddForce(direction * data.effect.force, ForceMode2D.Impulse);
        }

        //spawnCount--;
        //if (spawnCount > 0) Invoke("Spawn", spawnDelay);
    }
}
