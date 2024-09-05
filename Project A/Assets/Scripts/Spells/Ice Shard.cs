using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceShard : BaseAbility
{

    public void Cast()
    {
        if (animator != null)
        {
            PlaySpellAnimation("Ice Shard"); // Ensure this matches your animation state name
        }
        else
        {
            Debug.LogError("Animator component is missing on the IceShard prefab!");
        }
    }

}

