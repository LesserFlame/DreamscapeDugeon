using UnityEngine;
using UnityEngine.Animations;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    public float speed = 1f;
    public float sprintSpeed = 2f;
    public float animSpeed = 0.25f;
    public float sprintAnimSpeed = 0.5f;
    private bool isSprinting = false;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator animator;
    private string lastClip;
    private bool isMoving = false;
    //[SerializeField] private bool hasLeftSprite;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        if (x != 0f && y == 0) { animator.Play("Right"); sprite.flipX = x < 0; }
        else if (y != 0f) animator.Play(y > 0 ? "Up" : "Down");

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
}