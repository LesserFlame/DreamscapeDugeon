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
        else if (data.effect == null) ApplyDamage();
        if (data.effect != null) SpawnEffect();
    }
    public void ApplyDamage()
    {
        target.OnTakeDamage(owner.baseATK * data.damage);
    }
    public void SpawnEffect()
    {
        Transform spawnTransform = owner.transform;

        if (owner.gameObject.CompareTag("Player")) spawnTransform = owner.GetComponent<PlayerActor>().spellTransform;

        BattleEffect effectRef = Instantiate(data.effect, spawnTransform.position, Quaternion.FromToRotation(owner.transform.position, target.transform.position));
        effectRef.tag = gameObject.tag;
        effectRef.owner = this;
        var targetCenter = target.GetComponent<SpriteRenderer>().bounds.center;
        if (!effectRef.ownerTransform) effectRef.transform.position = targetCenter;
        switch (effectRef.effectType)
        {
            case BattleEffect.EffectType.PROJECTILE:
                var targetCollider = target.GetComponent<Collider2D>();
                targetCollider.enabled = true;
                var rb = effectRef.gameObject.GetComponent<Rigidbody2D>();
                Vector2 direction = targetCenter - effectRef.transform.position;
                rb.AddForce(direction * data.effect.force, ForceMode2D.Impulse);
                break;
            case BattleEffect.EffectType.ANIMATION:
                Vector3 rotationDirection = (targetCenter /*+ (Vector3.up * 0.5f)*/) - effectRef.transform.position;
                rotationDirection.Normalize();

                float angle = Mathf.Atan2(rotationDirection.y, rotationDirection.x) * Mathf.Rad2Deg;
                effectRef.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                break;
        }
    }
}
