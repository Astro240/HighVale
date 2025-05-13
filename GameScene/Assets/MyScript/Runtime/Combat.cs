using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class Combat : MonoBehaviour
{
    public float health = 100f;
    public float stamina = 100f;
    public float attackDamage = 10f;
    public float staminaCost = 20f;
    public float attackRange = 2f;
    public float regenAmount;
    public float regenInterval = 1f;
    private Coroutine regenCoroutine;
    private Animator mAnimator;
    private bool isDead = false; // Track if the player is dead
    private StarterAssets.ThirdPersonController pl;
    public bool slice = false; // Track if the player is dead
    public bool isAttacking = false; // Track if the player is currently attacking

    void Start()
    {
        pl = GetComponent<ThirdPersonController>();
        mAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDead) return; // Stop processing input if dead

        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            Attack();
        }
    }

    public void Attack()
    {
        if (isDead || pl.isDodge || slice || isAttacking) return; // Prevent attack if dead, dodging, or already attacking

        if (stamina >= staminaCost)
        {
            stamina -= staminaCost;

            // Stop regenerating stamina while attacking
            if (regenCoroutine != null)
            {
                StopCoroutine(regenCoroutine);
                regenCoroutine = null;
            }
        }
        else
        {
            Debug.Log("Not enough stamina to attack!");
            return;
        }

        mAnimator.SetTrigger("Attack1");
        isAttacking = true; // Set attacking state

        // Start stamina regeneration after a delay
        if (regenCoroutine == null)
        {
            regenCoroutine = StartCoroutine(RegenerateStamina(100f, regenAmount)); // Adjust the regenSpeed as needed
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                DamageEnemy(hitCollider.gameObject);
                break; // Exit after damaging the first enemy
            }
        }

        Debug.Log("No enemies in range.");
        StartCoroutine(ResetAttack()); // Start the coroutine to reset the attack state
    }

    private void DamageEnemy(GameObject target)
    {
        Debug.Log("Attacked! Dealt " + attackDamage + " damage.");

        Enemy targetCombat = target.GetComponent<Enemy>();
        if (targetCombat != null)
        {
            targetCombat.TakeDamage(attackDamage);
        }
        else
        {
            Debug.Log("Target does not have an Enemy component.");
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead || pl.isDodge) return; // Prevent taking damage if dead or dodging
        health -= damage;
        Debug.Log("Took damage: " + damage + ". Remaining health: " + health);
        mAnimator.SetTrigger("hit");

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (mAnimator != null)
        {
            mAnimator.SetTrigger("Death");
        }
        isDead = true; // Set dead state
        Debug.Log("Player has died!");
    }

    private IEnumerator RegenerateStamina(float targetStamina, float regenSpeed)
    {
        yield return new WaitForSeconds(1f); // Wait for 1 second before starting regeneration

        while (stamina < targetStamina)
        {
            stamina += regenSpeed * Time.deltaTime; // Smoothly increase stamina
            stamina = Mathf.Clamp(stamina, 0, targetStamina);
            yield return null; // Wait for the next frame
        }
        regenCoroutine = null; // Reset the coroutine reference when done
    }

    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(0.75f); // Wait for 1 second before allowing another attack
        isAttacking = false; // Reset attacking state
    }
}