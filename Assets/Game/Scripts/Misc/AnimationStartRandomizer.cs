using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationStartRandomizer : MonoBehaviour
{
    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
        var clip = animator.runtimeAnimatorController.animationClips[0];
        
        var startTime = Random.Range(0, clip.length);

        animator.Play(clip.name, 0, startTime);
    }
}
