using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "LevelConfig")]
public class LevelConfig : ScriptableObject
{
    [Header("Animation Curve")]
    public AnimationCurve animationcurve;
    public int maxLVL;
    public int maxRequiredXP;

    public int GetRequiredExp(int level)
    {
        int requiredExperience = Mathf.RoundToInt(animationcurve.Evaluate(Mathf.InverseLerp(0, maxLVL, level)) * maxRequiredXP);
        return requiredExperience;
    }
}
