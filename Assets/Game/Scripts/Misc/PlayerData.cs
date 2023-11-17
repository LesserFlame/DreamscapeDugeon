using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int SKILLLVL;
    public int SKILLXP;
    //public int HP;
    //public int MP;
    //public int SP;
    //public int ATK;
    //public int DEF;
    public bool[] SKILLS;
    public int POINTS;

    public PlayerData (PlayerController player)
    {
        SKILLLVL = player.data.SKILLLVL;
        SKILLXP = player.data.SKILLXP;
        //HP = player.data.HP;
        //MP = player.data.MP;
        //SP = player.data.SP;
        //ATK = player.data.ATK;
        //DEF = player.data.DEF;
        SKILLS = player.data.SKILLS;
        POINTS = player.data.POINTS;
    }
    public PlayerData()
    {
        SKILLLVL = 1;
        SKILLXP = 0;
        //HP = 20; 
        //MP = 30;
        //SP = 2;
        //ATK = 2;
        //DEF = 2;
        SKILLS = new bool[6];
        SKILLS[2] = true;
        POINTS = 0;
        //inferno infusion, pyroclasmic rebirth, flame spark, pyroquake, emberstream, flareheal
    }
}
