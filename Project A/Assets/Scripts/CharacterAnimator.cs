using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    private Animator animator; // Reference to the Animator component

    [SerializeField] private string idleAnimationName;
    [SerializeField] private string moveAnimationName;
    [SerializeField] private string attackAnimationName;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetAnimation(CharacterAnimation animation)
    {
        switch (animation)
        {
            case CharacterAnimation.Idle:
                animator.Play(idleAnimationName); // Use unique idle animation name
                break;
            case CharacterAnimation.Move:
                animator.Play(moveAnimationName); // Use unique move animation name
                break;
            case CharacterAnimation.Attack:
                animator.Play(attackAnimationName); // Use unique attack animation name
                break;
        }
    }

    public AnimatorStateInfo GetCurrentAnimatorStateInfo(int layerIndex)
    {
        return animator.GetCurrentAnimatorStateInfo(layerIndex);
    }
}
