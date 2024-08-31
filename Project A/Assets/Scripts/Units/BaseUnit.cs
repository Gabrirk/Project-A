using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUnit : MonoBehaviour
{
    public Tile OccupiedTile;
    public Faction Faction;
    public string UnitName;

    public int Health;
    public int MaxHealth;
    public int AttackPower;
    public int MovementRange;

    public CharacterAnimator characterAnimator;

    // Stat ranges
    public int MinHealth = 50;
    public int MaxHealthValue = 100;
    public int MinAttackPower = 10;
    public int MaxAttackPower = 20;
    public int MinMovementRange = 1;
    public int MaxMovementRange = 5;

    private void Awake()
    {
        // Initialize or retrieve the CharacterAnimator component
        characterAnimator = GetComponent<CharacterAnimator>();

        // Randomly generate unit stats
        Initialize(
            Random.Range(MinHealth, MaxHealthValue),
            Random.Range(MinAttackPower, MaxAttackPower),
            Random.Range(MinHealth, MaxHealthValue) // Assuming max health is the same as health
        );

        // Randomly set MovementRange
        MovementRange = Random.Range(MinMovementRange, MaxMovementRange);
    }

    public void Initialize(int health, int attackPower, int maxHealth)
    {
        Health = health;
        AttackPower = attackPower;
        MaxHealth = maxHealth;
    }

    public void MoveTo(Tile targetTile)
    {
        // Check if the target tile is different from the current tile
        if (OccupiedTile == targetTile) return;

        // Set the previous tile's OccupiedUnit to null
        if (OccupiedTile != null)
        {
            OccupiedTile.OccupiedUnit = null;
        }

        // Determine direction
        Vector3 direction = targetTile.transform.position - transform.position;

        // Flip the sprite based on the direction
        if (direction.x < 0)
        {
            // Moving left: flip the sprite by inverting the x scale
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (direction.x > 0)
        {
            // Moving right: ensure the sprite is not flipped
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        SetAnimation(CharacterAnimation.Move);
        StartCoroutine(MoveCoroutine(targetTile));
    }

    private IEnumerator MoveCoroutine(Tile targetTile)
    {
        Vector3 start = transform.position;
        Vector3 end = targetTile.transform.position;
        float moveSpeed = 5f; // Movement speed in units per second
        float distance = Vector3.Distance(start, end);
        float moveDuration = distance / moveSpeed; // Calculate the time needed for the move

        float startTime = Time.time;

        while (Time.time < startTime + moveDuration)
        {
            float t = (Time.time - startTime) / moveDuration;
            transform.position = Vector3.Lerp(start, end, t);
            yield return null;
        }

        transform.position = end; // Ensure the final position is accurate
        OccupiedTile = targetTile; // Update the unit's current tile
        targetTile.OccupiedUnit = this; // Set this tile's OccupiedUnit to the unit
        SetAnimation(CharacterAnimation.Idle);
    }

    public void Attack(BaseUnit targetUnit)
    {
        if (targetUnit == null)
        {
            Debug.LogError("Target unit is null!");
            return;
        }

        // Trigger the attack animation and handle damage after animation
        StartCoroutine(HandleAttack(targetUnit));
    }

    private IEnumerator HandleAttack(BaseUnit targetUnit)
    {
        // Trigger the attack animation
        SetAnimation(CharacterAnimation.Attack);

        // Wait for the attack animation to finish
        AnimatorStateInfo stateInfo = characterAnimator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);

        // Apply damage to the target unit
        targetUnit.TakeDamage(AttackPower);

        // Optional: Print attack details
        Debug.Log($"{UnitName} attacks {targetUnit.UnitName} for {AttackPower} damage.");

        // Switch back to idle animation after the attack
        SetAnimation(CharacterAnimation.Idle);
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;

        // Check if the unit is defeated
        if (Health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{UnitName} has been defeated!");
        Destroy(gameObject);
    }

    public void SetAnimation(CharacterAnimation animation)
    {
        if (characterAnimator != null)
        {
            characterAnimator.SetAnimation(animation);
        }
        else
        {
            Debug.LogWarning("CharacterAnimator not set for " + UnitName);
        }
    }
}

public enum CharacterAnimation
{
    Idle,
    Move,
    Attack
}