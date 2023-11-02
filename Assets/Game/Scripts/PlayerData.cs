using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int LVL;
    public int XP;
    public int HP;
    public int MP;
    public int SP;
    public int ATK;
    public int DEF;

    public PlayerData (PlayerController player)
    {
        LVL = player.data.LVL;
        XP = player.data.XP;
        HP = player.data.HP;
        MP = player.data.MP;
        SP = player.data.SP;
        ATK = player.data.ATK;
        DEF = player.data.DEF;
    }
    public PlayerData()
    {
        LVL = 1;
        XP = 0;
        HP = 20; 
        MP = 15;
        SP = 2;
        ATK = 2;
        DEF = 2;
    }
}
