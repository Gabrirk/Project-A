using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : BaseAbility
{

    public void Cast()
    {
        if (animator != null)
        {
            PlaySpellAnimation("Fire Ball"); // Ensure this matches your animation state name
        }
        else
        {
            Debug.LogError("Animator component is missing on the IceShard prefab!");
        }
    }
}
