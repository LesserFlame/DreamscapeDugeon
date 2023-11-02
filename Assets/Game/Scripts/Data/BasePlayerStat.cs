using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BasePlayerStat
{
    [SerializeField]
    public int baseStatValue;

    [SerializeField]
    public AnimationCurve baseStatModifier;
}
