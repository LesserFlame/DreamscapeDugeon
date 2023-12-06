using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "BattleActionData_", menuName = "Battle/ActionData")]
public class BattleActionData : ScriptableObject
{
    public string actionName;
    public float damage;
    public float manaCost;
    //public float duration;
    //animation and other stuff
    [Header("Battle Effect")]
    public BattleEffect effect;
    
    public enum Category
    { 
        ITEM,
        ATTACK,
        SPELL
    }
    public Category category;

}
