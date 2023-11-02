using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatLeveler", menuName = "PlayerStatLeveler")]
public class PlayerStatLeveler : ScriptableObject
{
    public BasePlayerStat HP;
    public BasePlayerStat MP;
    public BasePlayerStat ATK;
    public BasePlayerStat DEF;
    public BasePlayerStat SP;
}
