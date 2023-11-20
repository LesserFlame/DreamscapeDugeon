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
    public float delay = 0;
    public bool ownerTransform = true;

    public BattleAction owner;
    [SerializeField] private ParticleSystem deathEffect;
    private Rigidbody2D rb;
    private bool appliedDamage = false;
    private float deathTimer;
    private bool isDead = false;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        deathTimer = delay;
    }
    private void Update()
    {
        if (delay > 0)
        {
            deathTimer -= Time.deltaTime;
            if (deathTimer <= 0) Death();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //apply damage and destroy self
        if (!collision.gameObject.CompareTag(tag)) owner.ApplyDamage(); appliedDamage = true;
        Death();
    }

    private void Death()
    {
        if (!isDead)
        {
            var particle = GetComponent<ParticleSystem>();
            if (particle != null) { particle.Stop(); }
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
            if (!appliedDamage) owner.ApplyDamage();
            isDead = true;
        }
    }

    private void OnDestroy()
    {
        if (!isDead) Death();
        Destroy(gameObject);
    }
}
