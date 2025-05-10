using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 10f;          // Enemy's health
    public float attackDamage = 5f;     // Damage dealt by the enemy
    public float attackRange = 1.5f;     // Range within which the enemy can attack
    public float attackCooldown = 1.5f;  // Time between attacks

    private float lastAttackTime = 0f;  // To track attack timing
    private Transform player;            // Reference to the player

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; 
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            Attack();
        }
    }

    private void Attack()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            Debug.Log("Enemy attacked! Dealt " + attackDamage + " damage.");
            Combat playerScript = player.GetComponent<Combat>();
            if (playerScript != null)
            {
                playerScript.TakeDamage(attackDamage);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("Enemy took damage: " + damage + ". Remaining health: " + health);
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemy has died!");
        Destroy(gameObject);
    }
}