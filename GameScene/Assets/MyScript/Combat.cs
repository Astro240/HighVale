using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    public float health = 100f;
    public float stamina = 100f;      
    public float attackDamage = 10f;  
    public float staminaCost = 20f;    
    public float attackRange = 2f;     

    void Start()
    {
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    public void Attack()
    {
        if (stamina >= staminaCost)
        {
            stamina -= staminaCost;
        }
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                DamageEnemy(hitCollider.gameObject);
                return;
            }
        }

        Debug.Log("No enemies in range.");
    }

    private void DamageEnemy(GameObject target)
    {
        if (stamina >= staminaCost)
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
        else
        {
            Debug.Log("Not enough stamina to attack!");
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("Took damage: " + damage + ". Remaining health: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player has died!");
    }

    private IEnumerator RegenerateStamina(float regenAmount, float interval)
    {
        while (stamina < 100f)
        {
            stamina += regenAmount;
            stamina = Mathf.Clamp(stamina, 0, 100f);
            yield return new WaitForSeconds(interval);
        }
    }

}