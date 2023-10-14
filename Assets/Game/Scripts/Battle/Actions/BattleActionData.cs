using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "BattleActionData_", menuName = "Battle/ActionData")]
public class BattleActionData : ScriptableObject
{
    public string actionName;
    public float damage;
    public float manaCost;
    //animation and other stuff
    public enum Category
    { 
        ITEM,
        ATTACK,
        SPELL
    }
    public Category category;

}
