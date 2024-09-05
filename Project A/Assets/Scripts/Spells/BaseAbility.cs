using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAbility : MonoBehaviour
{
    public string AbilityName;

    protected Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Method to trigger a spell animation
    protected void PlaySpellAnimation(string animationName)
    {
        if (animator != null)
        {
            animator.Play(animationName);
            StartCoroutine(DestroyAfterAnimation(animationName));
        }
        else
        {
            Debug.LogWarning("Animator component not found on " + gameObject.name);
        }
    }

    private IEnumerator DestroyAfterAnimation(string animationName)
    {
        // Get the animation state info
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // Wait until the animation starts
        yield return new WaitUntil(() => stateInfo.IsName(animationName));

        // Wait for the duration of the animation
        yield return new WaitForSeconds(stateInfo.length);

        // Destroy the GameObject after the animation completes
        Destroy(gameObject);
    }


}
