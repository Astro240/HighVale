using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 10f;          // Enemy's health
    public float attackDamage;     // Damage dealt by the enemy
    public float attackRange = 1.5f;     // Range within which the enemy can attack
    public float attackCooldown = 1.5f;  // Time between attacks

    public int money = 50;

    private float lastAttackTime = 0f;  // To track attack timing
    private Transform player;            // Reference to the player
    private Combat combat;
    private Animator anim;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        combat = player.GetComponent<Combat>();
        anim = GetComponent<Animator>();
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
            Debug.Log("Enemy attacked! Animation triggered.");
            anim.SetTrigger("attack");

            // Start a coroutine to handle the delay
            StartCoroutine(DealDamageWithDelay(0.5f)); // Adjust the delay time as needed
        }
    }

    private IEnumerator DealDamageWithDelay(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Deal damage to the player after the delay
        Combat playerScript = player.GetComponent<Combat>();
        if (playerScript != null)
        {
            playerScript.TakeDamage(attackDamage);
            Debug.Log("Player took damage: " + attackDamage);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("Enemy took damage: " + damage + ". Remaining health: " + health);
        if (health <= 0)
        {
            Die();
            if (combat != null)
            {
                combat.SetMoney(this.money);
            }
            else
            {
                Debug.LogError("Combat reference is null!");
            }
        }
    }

    private void Die()
    {
        Debug.Log("Enemy has died!");
        Destroy(gameObject);
    }
}