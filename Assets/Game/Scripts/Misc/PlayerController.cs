using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    public Transform interactor;
    public FireLight fireLight;
    public float overlapRadius = 0.1f;
    public bool detectInput = true;
    public bool loadStats = false;
    public float speed = 1f;
    public float sprintSpeed = 2f;
    public float animSpeed = 0.25f;
    public float sprintAnimSpeed = 0.5f;
    private bool isSprinting = false;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator animator;
    //private string lastClip;
    private bool isMoving = false;
    private int directionId = 0;

    [Header("Battle")]
    //public bool[] skills = new bool[6];
    public List<BattleActionData> actions;
    public List<BattleActionData> totalActions;
    public PlayerStatLeveler leveler;
    public int level = 1;
    public int XP;
    public int HP;
    public int MP;
    public int SP; //battle speed
    public int ATK;
    public int DEF;
    public PlayerData data;
    //[SerializeField] private bool hasLeftSprite;
    public int requiredXP;
    private int requiredSkillXP;
    [SerializeField] private LevelConfig levelConfig;
    [SerializeField] private LevelConfig skillLevelConfig;

    [SerializeField] private GameObject failedSound;

    private Vector3 prevPos;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        var manager = FindFirstObjectByType<GameManager>();
        if (manager != null)
        {
            manager.player = this;
        }
        LoadStats();
        CalculateRequiredXP();
        StartingStats();
        GameManager.Instance.ResetCameras();
        BattleManager.Instance.player.OnInitialize(this);
    }

    private void Update()
    {
        if (detectInput)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(interactor.position, overlapRadius);
                if (colliders != null)
                {
                    foreach (var collider in colliders)
                    {
                        var interactable = collider.GetComponent<Interactable>();
                        if (interactable != null) { interactable.OnInteract(); break; }
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                var stats = FindAnyObjectByType<StatsScreenHandler>();
                if (stats != null)
                {
                    stats.OpenStatsMenu();
                    detectInput = false;
                }
                else
                {
                    if (failedSound != null) Instantiate(failedSound);
                }
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                SaveSystem.DeletePlayer();
                LoadStats();
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                data.POINTS += 10;
                SaveSystem.SavePlayer(this);
                LoadStats();
            }
        }
        if (rb.velocity.magnitude < 0.7f) isMoving = false;
        //Debug.Log(rb.velocity.magnitude);
    }
    private void FixedUpdate()
    {
        //if (detectInput)
        //{

            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");
            if (!detectInput)
            {
                x = 0;
                y = 0;
            }
            if (x != 0f && y == 0)
            {
                //animator.Play("RightIdle");
                animator.SetInteger("Direction", 2);
                sprite.flipX = x < 0;
                if (sprite.flipX) directionId = 3; else directionId = 1;
            }
            else if (y != 0f)
            {
                //animator.Play(y > 0 ? "UpIdle" : "DownIdle");
                animator.SetInteger("Direction", y > 0 ? 1 : 0);
                if (y > 0) directionId = 2; else directionId = 0;
            }

            Vector2 direction = new Vector2(x, y);

            //all directions go at the same speed
            direction.Normalize();

            isSprinting = Input.GetKey(KeyCode.LeftShift);
            rb.velocity = direction * (isSprinting ? sprintSpeed : speed);

            var distance = Vector3.Distance(prevPos, transform.position);
            if (rb.velocity.magnitude != 0 && distance > 0.01f)
            {
                animator.speed = (isSprinting ? sprintAnimSpeed : animSpeed);
                isMoving = true;
                //lastClip = GetCurrentClipName();
                //Debug.Log("Moving");
            }
            else
            {
                animator.speed = 1;
                isMoving = false;
            //animator.Play(lastClip);
            //Invoke("ResetAnimation", 0.2f);
                //ResetAnimation();
                //Debug.Log("Not Moving");
            }
            animator.SetBool("IsMoving", isMoving);
            //animator.SetFloat("Speed", distance);

            //player interactions
            switch (directionId)
            {
                case 0:
                    //down
                    interactor.localPosition = new Vector3(0, -0.3f); //+ transform.position;
                    if (fireLight != null) fireLight.destination = new Vector3(0.5f + transform.position.x, 0.2f + transform.position.y);
                    break;
                case 1:
                    //right
                    interactor.localPosition = new Vector3(0.5f, 0.2f); //+ transform.position;
                    if (fireLight != null) fireLight.destination = new Vector3(-0.5f + transform.position.x, 0.2f + transform.position.y);
                break;
                case 2:
                    //up
                    interactor.localPosition = new Vector3(0, 0.7f); //+ transform.position;
                    if (fireLight != null) fireLight.destination = new Vector3(-0.5f + transform.position.x, 0.2f + transform.position.y);
                break;
                case 3:
                    //left
                    interactor.localPosition = new Vector3(-0.5f, 0.2f); //+ transform.position;
                if (fireLight != null) fireLight.destination = new Vector3(0.5f + transform.position.x, 0.2f + transform.position.y);
                break;
            }
        //}
        prevPos = transform.position;
    }


    public void LevelUp()
    {
        level++;
        CalculateRequiredXP();
        LevelUpStats();
        FindAnyObjectByType<PlayerActor>().OnUpdateStats(this);
    }
    public void CalculateRequiredXP()
    {
        requiredXP = levelConfig.GetRequiredExp(level);
    }
    public void IncreaseXP(int value)
    {
        XP += value;

        if (XP >= requiredXP)
        {
            while (XP >= requiredXP)
            {
                XP -= requiredXP;
                LevelUp();
            }
        }
        IncreaseSkillXP(value);
    }
    public void CalculateRequiredSkillXP()
    {
        requiredSkillXP = skillLevelConfig.GetRequiredExp(data.SKILLLVL);
    }
    public void IncreaseSkillXP(int value)
    {
        value /= 4;
        data.SKILLXP += value;

        if (data.SKILLXP >= requiredSkillXP && (data.SKILLLVL < skillLevelConfig.maxLVL))
        {
            while (data.SKILLXP >= requiredSkillXP)
            {
                data.SKILLXP -= requiredSkillXP;
                SkillLevelUp();
            }
        }
    }
    public void SkillLevelUp()
    {
        //if (data.SKILLLVL < skillLevelConfig.maxLVL)
        {
            data.SKILLLVL++;
            data.POINTS++;
            CalculateRequiredSkillXP();
        }
        //LevelUpStats();
        //FindAnyObjectByType<PlayerActor>().OnUpdateStats(this);
    }
    public void StartingStats()
    {
        HP = leveler.HP.baseStatValue;
        MP = leveler.MP.baseStatValue;
        SP = leveler.SP.baseStatValue ;
        ATK = leveler.ATK.baseStatValue ;
        DEF = leveler.DEF.baseStatValue ;
    }
    public void LevelUpStats()
    {
        int maxIncrease = 5;
        float ran = Random.Range(0f, 1f);
        HP += Mathf.RoundToInt(leveler.HP.baseStatModifier.Evaluate(ran) * maxIncrease);
        ran = Random.Range(0f, 1f);
        MP += Mathf.RoundToInt(leveler.MP.baseStatModifier.Evaluate(ran) * maxIncrease);
        ran = Random.Range(0f, 1f);
        ATK += Mathf.RoundToInt(leveler.ATK.baseStatModifier.Evaluate(ran) * maxIncrease);
        ran = Random.Range(0f, 1f);
        DEF += Mathf.RoundToInt(leveler.DEF.baseStatModifier.Evaluate(ran) * maxIncrease);
        ran = Random.Range(0f, 1f);
        SP += Mathf.RoundToInt(leveler.SP.baseStatModifier.Evaluate(ran) * maxIncrease);
    }

    public string GetCurrentClipName()
    {
        int layerIndex = 0;
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(layerIndex);
        if (clipInfo.Length > 0) return clipInfo[0].clip.name;
        else return null;
    }

    [System.Obsolete]
    public void ResetAnimation()
    {
        if (!isMoving && gameObject.active) animator.Play(GetCurrentClipName(), 0, 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(interactor.position, overlapRadius);
    }

    public void LoadStats()
    {
        data = SaveSystem.LoadPlayer();
        if (totalActions != null)
        {
            if (data.SKILLS[2]) actions.Add(totalActions[0]);
            if (data.SKILLS[3]) actions.Add(totalActions[1]);
            if (data.SKILLS[4]) actions.Add(totalActions[2]);
        }
        
        //LVL = data.LVL;
        //XP  = data.XP;
        //HP  = data.HP;
        //MP  = data.MP;
        //SP  = data.SP;
        //ATK = data.ATK;
        //DEF = data.DEF;
    }
    public void OnFootstepSpawn(GameObject go)
    {
        if (go != null)
        {
            //Transform bottom = animator.GetBoneTransform(HumanBodyBones.LeftFoot);
            Vector3 spawnPos = transform.position;
            //spawnPos.y = bottom.position.y;
            Instantiate(go, spawnPos, Quaternion.identity);
        }
    }
}