using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public int id;
    public bool unlocked;

    //public TMP_Text titleText;
    //public TMP_Text descriptionText;
    public string skillName;
    public string skillDescription;
    public int skillCost;

    public BattleActionData battleSkill;

    public List<Skill> prerequisites;

    public void UpdateUI()
    {
        //update associated ui
    }
}
