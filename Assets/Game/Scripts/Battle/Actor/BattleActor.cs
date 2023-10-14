using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleActor : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public List<BattleActionData> actions;

    [SerializeField] public string actorName;
    [SerializeField] public float HP;
    [SerializeField] public float maxHP;
    [SerializeField] public float baseATK;
    [SerializeField] public float baseDEF;
    [SerializeField] public float MP;
    [SerializeField] public float maxMP;

    public virtual void OnTakeDamage(float baseDamgage)
    {
        float trueDamage = baseDamgage - baseDEF;
        if (trueDamage < 0) { trueDamage = 0; }
        HP -= trueDamage;
    }

    public virtual void OnDeath()
    {
        gameObject.SetActive(false);
    }
}
