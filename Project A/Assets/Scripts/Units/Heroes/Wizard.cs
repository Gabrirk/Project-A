using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : BaseHero
{
    public GameObject fireballPrefab;  // Assign in Inspector
    public GameObject iceShardPrefab;  // Assign in Inspector

    public override void Attack(BaseUnit targetUnit)
    {
        if (hasActed)
        {
            Debug.Log($"{UnitName} has already performed an action this turn.");
            return;
        }

        if (targetUnit == null)
        {
            Debug.LogError("Target unit is null!");
            return;
        }

        // Example UI logic to choose a spell
        // Directly using IceShard here as a placeholder
        CastFireball(targetUnit);

        hasActed = true;
    }
    public void CastFireball(BaseUnit targetUnit)
    {

        if (targetUnit == null)
        {
            Debug.LogError("Target unit is null!");
            return;
        }

        // Calculate a position in front of the mage for the fireball spawn
        Vector3 fireballStartPosition = transform.position + transform.forward * 1.5f; // Adjust distance from mage if necessary

        // Instantiate the fireball at the calculated start position
        GameObject fireballInstance = Instantiate(fireballPrefab, fireballStartPosition, Quaternion.identity);
        FireBall fireball = fireballInstance.GetComponent<FireBall>();

        if (fireball != null)
        {
            // Start moving the fireball towards the target
            StartCoroutine(MoveFireballToTarget(fireballInstance, targetUnit.transform.position));
            fireball.Cast();  // Play the fireball animation if needed
        }
        else
        {
            Debug.LogError("FireBall component is missing on the FireBall prefab!");
        }
    }

    public void CastIceShard(BaseUnit targetUnit)
    {
        if (targetUnit == null)
        {
            Debug.LogError("Target unit is null!");
            return;
        }

        // Instantiate the Ice Shard at the target's position
        GameObject iceShardInstance = Instantiate(iceShardPrefab, targetUnit.transform.position, Quaternion.identity);
        IceShard iceShard = iceShardInstance.GetComponent<IceShard>();

        if (iceShard != null)
        {
            iceShard.Cast();
        }
        else
        {
            Debug.LogError("IceShard component is missing on the IceShard prefab!");
        }

        // Optionally handle additional targeting and effects
        // e.g., iceShard.SetTarget(targetUnit);
    }

    private IEnumerator MoveFireballToTarget(GameObject fireball, Vector3 targetPosition)
    {
        float speed = 10f; // Adjust the speed of the fireball
        while (fireball != null && Vector3.Distance(fireball.transform.position, targetPosition) > 0.1f)
        {
            fireball.transform.position = Vector3.MoveTowards(fireball.transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }


    }
}
