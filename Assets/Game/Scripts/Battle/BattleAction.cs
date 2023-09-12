using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAction : ScriptableObject
{
    string Name;
    float Damage;
    //animation and other stuff
    enum Category
    { 
        ITEM,
        ATTACK,
        SPELL
    }
    Category category;

}
