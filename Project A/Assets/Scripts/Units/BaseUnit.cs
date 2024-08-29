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

    public void Initialize(int health, int attackPower, int maxHealth)
    {
        Health = health;
        AttackPower = attackPower;
        MaxHealth = maxHealth;
    }

    public void Attack(BaseUnit targetUnit)
    {
        if (targetUnit == null)
        {
            Debug.LogError("Target unit is null!");
            return;
        }

        // Apply damage to the target unit
        targetUnit.TakeDamage(AttackPower);

        // Optional: Print attack details
        Debug.Log($"{UnitName} attacks {targetUnit.UnitName} for {AttackPower} damage.");
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

    // Method to handle unit death
    private void Die()
    {
        // Optional: Add any additional logic for when the unit dies (e.g., play animation, drop loot)
        Debug.Log($"{UnitName} has been defeated!");
        Destroy(gameObject);
    }


}
