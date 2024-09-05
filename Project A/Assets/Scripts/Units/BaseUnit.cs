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

    public bool hasMoved;
    public bool hasActed;

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

    public void StartTurn()
    {
        hasMoved = false;
        hasActed = false;
    }

    public void EndTurn()
    {
        hasMoved = true;
        hasActed = true;
    }

    public void MoveTo(Tile targetTile)
    {
        if (hasMoved)
        {
            Debug.Log($"{UnitName} has already moved this turn.");
            return;
        }

        if (OccupiedTile == targetTile) return;

        if (!Tile.IsHighlighted(targetTile))
        {
            Debug.Log("Target tile is out of range!");
            return;
        }

        if (OccupiedTile != null)
        {
            OccupiedTile.OccupiedUnit = null;
        }

        Vector3 direction = targetTile.transform.position - transform.position;

        if (direction.x < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (direction.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        SetAnimation(CharacterAnimation.Move);
        StartCoroutine(MoveCoroutine(targetTile));
        hasMoved = true;
    }

    private IEnumerator MoveCoroutine(Tile targetTile)
    {
        Vector3 start = transform.position;
        Vector3 end = targetTile.transform.position;
        float moveSpeed = 5f;
        float distance = Vector3.Distance(start, end);
        float moveDuration = distance / moveSpeed;

        float startTime = Time.time;

        while (Time.time < startTime + moveDuration)
        {
            float t = (Time.time - startTime) / moveDuration;
            transform.position = Vector3.Lerp(start, end, t);
            yield return null;
        }

        transform.position = end;
        OccupiedTile = targetTile;
        targetTile.OccupiedUnit = this;
        SetAnimation(CharacterAnimation.Idle);
    }

    public virtual void Attack(BaseUnit targetUnit)
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

        StartCoroutine(HandleAttack(targetUnit));
        hasActed = true;
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

    public bool IsTargetInRange(BaseUnit targetUnit, int spellRange)
    {
        // Calculate the distance between this unit and the target unit
        float distance = Vector3.Distance(transform.position, targetUnit.transform.position);

        // Convert distance from world units to tile-based units (assuming 1 unit = 1 tile)
        int tileDistance = Mathf.RoundToInt(distance);

        // Return true if the target is within the spell range
        return tileDistance <= spellRange;
    }
}

public enum CharacterAnimation
{
    Idle,
    Move,
    Attack
}