using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
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
    [SerializeField] protected float speed;

    [HideInInspector] public float tempSpeed;

    //public float Speed
    //{
    //    get { return speed * Random.Range(0, 10); }
    //}

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

    public void GenerateTempSpeed()
    {
        tempSpeed = speed * Random.Range(0, 10);
    }

    public abstract void OnDecide();
}
