using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillTree : Singleton<SkillTree>
{
    //public int[] skillLevels;
    //public int[] skillCaps;
    //public string[] skillNames;
    //public string[] skillDescriptions;

    public List<Skill> skillList;
    //public GameObject skillHolder;

    public int skillPoints;
    public int skillLevel;
    public int skillXP;
    [SerializeField] private int requiredXP;
    [SerializeField] private LevelConfig levelConfig;

    private void Start()
    {
        //if (skillList.Count > 0)
        //{
        //    skillList = skillList.OrderBy(skill => skill.id).ToList();
        //}
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.L)) IncreaseXP(10);
    }
    public void UpdateUI()
    {
        foreach(var skill in skillList) 
        { 
            skill.UpdateUI();
        }
    }
    public void LevelUp()
    {
        skillLevel++;
        CalculateRequiredXP();
        //LevelUpStats();
        skillPoints++;
        //FindAnyObjectByType<PlayerActor>().OnUpdateStats(this);
    }
    public void CalculateRequiredXP()
    {
        requiredXP = levelConfig.GetRequiredExp(skillLevel);
    }
    public void IncreaseXP(int value)
    {
        skillXP += value;

        if (skillXP >= requiredXP)
        {
            while (skillXP >= requiredXP)
            {
                skillXP -= requiredXP;
                LevelUp();
            }
        }
    }
}
