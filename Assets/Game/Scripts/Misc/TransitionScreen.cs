using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionScreen : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private float animationLength = 0;
    //private bool isFading = false;
    //private int currentSceneID = 0;
    private bool isOpen = false;

    private void Start()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (animator != null) animationLength = animator.GetCurrentAnimatorClipInfo(0).Length;
    }
    public void InvokeToggle()
    {
        ToggleActivate();
        Invoke("ToggleActivate", animationLength);
    }
    public void ToggleActivate()
    {
        isOpen = !isOpen;
        if (animator == null) animator = GetComponent<Animator>();
        animator.SetBool("IsOpen", isOpen);
    }
    
}
