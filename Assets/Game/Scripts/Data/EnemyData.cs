using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[System.Serializable, CreateAssetMenu(fileName = "EnemyData_", menuName = "Enemies/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public float maxHP = 10;
    public float baseDEF = 4;
    public float baseATK = 5;
    public float speed = 2;

    public AnimatorController animator;
    public List<BattleActionData> actions;
}

