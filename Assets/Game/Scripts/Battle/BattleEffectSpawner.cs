using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEffectSpawner : MonoBehaviour
{
    

    [SerializeField] private float force = 1;
    [SerializeField] private float spawnCount = 1;
    [SerializeField] private float spawnDelay = 0;
    private GameObject target;
    private BattleEffect effect;

    private void Start()
    {
        Invoke("Spawn", spawnDelay);
    }

    public void Spawn()
    {
        BattleEffect effectRef = Instantiate(effect);
        if (effectRef.effectType == BattleEffect.EffectType.PROJECTILE)
        {
            var rb = effectRef.gameObject.GetComponent<Rigidbody2D>();
            Vector2 direction = target.transform.position - effectRef.transform.position;
            rb.AddForce(direction * force);
        }

        spawnCount--;
        if (spawnCount > 0) Invoke("Spawn", spawnDelay);
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    
}
