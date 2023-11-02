using UnityEngine;
using UnityEngine.Animations;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    public Transform interactor;

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

    [Header("Battle Stats")]
    public PlayerStatLeveler leveler;
   // public int LVL;
    //public int XP;
    //public int HP;
    //public int MP;
    //public int SP; //battle speed
    //public int ATK;
    //public int DEF;
    public PlayerData data;
    //[SerializeField] private bool hasLeftSprite;
    private int requiredXP;
    [SerializeField] private LevelConfig levelConfig;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        var manager = FindFirstObjectByType<GameManager>();
        if (manager != null)
        {
            manager.player = this;
            LoadStats();
            CalculateRequiredXP();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Collider2D collider = Physics2D.OverlapCircle(interactor.position, 0.1f);
            if (collider != null)
            {
                Debug.Log("Collided with " + collider.name);
                var interactable = collider.GetComponent<Interactable>();
                if (interactable != null) { interactable.OnInteract(); }
            }
        }
        if (Input.GetKeyDown(KeyCode.X)) 
        {
            SaveSystem.DeletePlayer();
            LoadStats();
        }
    }
    private void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        if (x != 0f && y == 0)
        {
            animator.Play("Right");
            sprite.flipX = x < 0;
            if (sprite.flipX) directionId = 3; else directionId = 1;
        }
        else if (y != 0f)
        {
            animator.Play(y > 0 ? "Up" : "Down");
            if (y > 0) directionId = 2; else directionId = 0;
        }

        Vector2 direction = new Vector2(x, y);

        //all directions go at the same speed
        direction.Normalize();

        isSprinting = Input.GetKey(KeyCode.LeftShift);
        rb.velocity = direction * (isSprinting ? sprintSpeed : speed);

        if (direction.magnitude != 0) 
        { 
            animator.speed = (isSprinting ? sprintAnimSpeed : animSpeed);
            isMoving = true;
            //lastClip = GetCurrentClipName();
        }
        else 
        { 
            animator.speed = 0;
            isMoving = false;
            //animator.Play(lastClip);
            Invoke("ResetAnimation", 0.5f);
        }
        animator.SetBool("IsMoving", isMoving);

        //player interactions
        switch(directionId)
        {
            case 0:
                //down
                interactor.localPosition = new Vector3(0, -0.3f); //+ transform.position;
                break;
            case 1:
                //right
                interactor.localPosition = new Vector3(0.5f, 0.2f); //+ transform.position;
                break;
            case 2:
                //up
                interactor.localPosition = new Vector3(0, 0.7f); //+ transform.position;
                break;
            case 3:
                //left
                interactor.localPosition = new Vector3(-0.5f, 0.2f); //+ transform.position;
                break;
        }
    }

    public void LevelUp()
    {
        data.LVL++;
        CalculateRequiredXP();
        LevelUpStats();
        FindAnyObjectByType<PlayerActor>().OnUpdateStats(this);
    }
    public void CalculateRequiredXP()
    {
        requiredXP = levelConfig.GetRequiredExp(data.LVL);
    }
    public void IncreaseXP(int value)
    {
        data.XP += value;

        if (data.XP >= requiredXP) 
        { 
            while (data.XP >= requiredXP)
            {
                data.XP -= requiredXP;
                LevelUp();
            }
        }
    }
    public void LevelUpStats()
    {
        int maxIncrease = 5;
        float ran = Random.Range(0f, 1f);
        data.HP += Mathf.RoundToInt(leveler.HP.baseStatModifier.Evaluate(ran) * maxIncrease);
        ran = Random.Range(0f, 1f);
        data.MP += Mathf.RoundToInt(leveler.MP.baseStatModifier.Evaluate(ran) * maxIncrease);
        ran = Random.Range(0f, 1f);
        data.ATK += Mathf.RoundToInt(leveler.ATK.baseStatModifier.Evaluate(ran) * maxIncrease);
        ran = Random.Range(0f, 1f);
        data.DEF += Mathf.RoundToInt(leveler.DEF.baseStatModifier.Evaluate(ran) * maxIncrease);
        ran = Random.Range(0f, 1f);
        data.SP += Mathf.RoundToInt(leveler.SP.baseStatModifier.Evaluate(ran) * maxIncrease);
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
        Gizmos.DrawWireSphere(interactor.position, 0.1f);
    }

    private void LoadStats()
    {
        data = SaveSystem.LoadPlayer();
        //LVL = data.LVL;
        //XP  = data.XP;
        //HP  = data.HP;
        //MP  = data.MP;
        //SP  = data.SP;
        //ATK = data.ATK;
        //DEF = data.DEF;
    }
}