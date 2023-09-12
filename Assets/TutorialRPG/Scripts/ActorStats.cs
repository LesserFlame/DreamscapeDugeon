using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ActorStats : MonoBehaviour, IComparable
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject healthFill;
    [SerializeField] private GameObject magicFill;

    [Header("Stats")]
    public float health;
    public float magic;
    public float melee;
    public float spell;
    public float defense;
    public float speed;
    public float experience;
    public bool isDead = false;

    private float startHealth;
    private float startMagic;

    [HideInInspector] public int nextActorTurn;
    //Resize health and magic bars
    private Transform healthTransform;
    private Transform magicTransform;

    private Vector2 healthScale;
    private Vector2 magicScale;

    private float xNewHealthScale;
    private float xNewMagicScale;

    private BattleController battleController;

    void Awake()
    {
        healthTransform = healthFill.GetComponent<RectTransform>();
        healthScale = healthFill.transform.localScale;

        magicTransform = magicFill.GetComponent<RectTransform>();
        magicScale = magicFill.transform.localScale;

        startHealth = health;
        startMagic = magic;

        battleController = FindFirstObjectByType<BattleController>();
    }

    public void ReceiveDamage(float damage)
    {
        health = health - damage;
        animator.Play("Hurt");

        //set damage text

        if (health <= 0)
        {
            isDead = true;
            gameObject.tag = "Dead";
            Destroy(healthFill);
            Destroy(gameObject);
        }
        else if (damage > 0)
        {
            xNewHealthScale = healthScale.x * (health / startHealth);
            healthFill.transform.localScale = new Vector2(xNewHealthScale, healthScale.y);
        }

        if (damage > 0)
        {
            battleController.GetComponent<BattleController>().damageText.gameObject.SetActive(true);
            battleController.GetComponent<BattleController>().damageText.text = damage.ToString();
        }
        Invoke("ContinueGame", 2);
    }

    public void UpdateMagicFill(float cost)
    {
        if (cost > 0)
        {
            magic = magic - cost;
            xNewMagicScale = magicScale.x * (magic / startMagic);
            magicFill.transform.localScale = new Vector2(xNewMagicScale, magicScale.y);
        }
    }

    void ContinueGame()
    {
        FindFirstObjectByType<BattleController>().NextTurn();
    }

    public bool GetDead()
    {
        return isDead;
    }

    public void CalculateNextTurn(int currentTurn)
    {
        nextActorTurn = currentTurn + Mathf.CeilToInt(100f / speed);
    }

    public int CompareTo(object otherStats)
    {
        int next = nextActorTurn.CompareTo(((ActorStats)otherStats).nextActorTurn);
        return next;
    }
}
