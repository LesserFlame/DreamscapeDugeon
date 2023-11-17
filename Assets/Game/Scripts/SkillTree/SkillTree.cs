using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SkillTree : MonoBehaviour//Singleton<SkillTree>
{
    //public int[] skillLevels;
    //public int[] skillCaps;
    //public string[] skillNames;
    //public string[] skillDescriptions;

    public List<Skill> skillList;
    //public GameObject skillHolder;
    public bool detectInput = false;
    //public int skillPoints;
    //public int skillLevel;
    //public int skillXP;
    //[SerializeField] private int requiredXP;
    //[SerializeField] private LevelConfig levelConfig;

    //private SliderTextHandler levelSlider;
    [SerializeField] private TextMeshProUGUI skillNameText;
    [SerializeField] private TextMeshProUGUI skillDescriptionText;
    [SerializeField] private TextMeshProUGUI skillPointsText;
    [SerializeField] private Animator journalAnimator;

    private int selectedSkill = 0;
    private PlayerController player;
    private void Start()
    {
        //if (skillList.Count > 0)
        //{
        //    skillList = skillList.OrderBy(skill => skill.id).ToList();
        //}
        

        //levelSlider = FindAnyObjectByType<SliderTextHandler>();
        UpdateUI();
    }
    public void Awake()
    {
        //base.Awake();
        player = FindAnyObjectByType<PlayerController>();
        //if (player == null)
        //{
        var data = SaveSystem.LoadPlayer();
        foreach (var skill in skillList)
        {
            skill.unlocked = data.SKILLS[skill.id];
            if (skill.id == 0) skill.selected = true;
        }
        //}
        //if (player != null)
        //{
        //    //for (int i = 0; i < skillList.Count; i++)
        //    //{
        //    //    skillList[i].unlocked = player.data.SKILLS[i];
        //    //}
        //    foreach (var skill in skillList)
        //    {
        //        skill.unlocked = player.data.SKILLS[skill.id];
        //        Debug.Log(skill.id + ": " + player.data.SKILLS[skill.id]);
        //    }
        //}
        UpdateUI();
    }
    private void Update()
    {
        if(detectInput) 
        { 
            //if (Input.GetKeyDown(KeyCode.L)) IncreaseXP(10);
            if (Input.GetKeyDown(KeyCode.Z))
            {
                if(skillList[selectedSkill].UnlockSkill())
                {
                    player.data.POINTS -= skillList[selectedSkill].skillCost;
                    UpdateUI();
                }
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                OpenJournal(false);

            }
            if (Input.anyKeyDown)
            {
                var input = Input.GetAxis("Vertical");
                if (input != 0)
                {
                    skillList[selectedSkill].selected = false;
                    if (input > 0) selectedSkill--;
                    else if (input < 0) selectedSkill++;

                    if (selectedSkill < 0) selectedSkill = skillList.Count - 1;
                    else if (selectedSkill >= skillList.Count) selectedSkill = 0;

                    skillList[selectedSkill].selected = true;

                    UpdateUI();
                }
            }
        }
    }
    public void UpdateUI()
    {
        foreach(var skill in skillList) 
        { 
            skill.UpdateUI();
        }
        //levelSlider.OnValueChanged(player.data.SKILLXP, requiredXP);
        skillNameText.text = skillList[selectedSkill].skillName;
        skillDescriptionText.text = skillList[selectedSkill].skillDescription;
        skillPointsText.text = "Skill Points - " + player.data.POINTS;
        //skillText.text = "Skill Points: " + skillPoints;
    }
    //public void LevelUp()
    //{
    //    //skillLevel++;
    //    //CalculateRequiredXP();
    //    ////LevelUpStats();
    //    //skillPoints++;
    //    ////FindAnyObjectByType<PlayerActor>().OnUpdateStats(this);
    //}
    //public void CalculateRequiredXP()
    //{
    //    //requiredXP = levelConfig.GetRequiredExp(skillLevel);
    //}
    //public void IncreaseXP(int value)
    //{
    //    skillXP += value;

    //    if (skillXP >= requiredXP)
    //    {
    //        while (skillXP >= requiredXP)
    //        {
    //            skillXP -= requiredXP;
    //            LevelUp();
    //        }
    //    }

    //    UpdateUI();
    //}

    public void OpenJournal(bool open = true)
    {
        detectInput = open;
        journalAnimator.SetBool("IsOpen", open);
        FindAnyObjectByType<PlayerController>().detectInput = !open;
        if (!open) selectedSkill = 0;
        UpdateUI();
    }
}
