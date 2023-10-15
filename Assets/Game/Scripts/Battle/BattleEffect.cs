using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEffect : MonoBehaviour
{
    public enum EffectType
    {
        PROJECTILE,
        CONTINUOUS,
        ANIMATION
    }
    public EffectType effectType = EffectType.PROJECTILE;
    public float force = 1;
    public float spawnCount = 1;
    public float spawnDelay = 0;
    public float deathDelay = 1;

    public BattleAction owner;
    [SerializeField] private ParticleSystem deathEffect;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //apply damage and destroy self
        if (!collision.gameObject.CompareTag(tag)) owner.ApplyDamage();
        Death();
    }

    private void Death()
    {
        GetComponent<ParticleSystem>().Stop();
        if (rb != null) 
        { 
            //rb.velocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            gameObject.GetComponent<Collider2D>().enabled = false;
        }
        if (deathEffect != null)
        {
            Instantiate(deathEffect, gameObject.transform);
        }
        Invoke("OnDestroy", deathDelay);
    }

    private void OnDestroy()
    {
        Destroy(gameObject);
    }
}
