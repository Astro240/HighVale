using System.Collections;
using UnityEngine;

public class RestoreHealth : MonoBehaviour
{
    public int healthAmount = 20; // Amount of health to restore each interval
    public float restoreInterval = 1f; // How often to restore health (in seconds)

    private bool isPlayerInTrigger = false;

    // This method is called when another collider enters the trigger collider attached to this object
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = true;
            StartCoroutine(RestoreHealthCoroutine(other.GetComponent<Combat>()));
        }
    }

    // This method is called when another collider exits the trigger collider
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
            StopAllCoroutines(); // Stop restoring health when player exits
        }
    }

    private IEnumerator RestoreHealthCoroutine(Combat playerHealth)
    {
        while (isPlayerInTrigger)
        {
            if (playerHealth != null)
            {
                playerHealth.RestoreHealth(healthAmount);
                yield return new WaitForSeconds(restoreInterval); // Wait for the specified interval
            }
            else
            {
                break; // Exit if the playerHealth component is no longer available
            }
        }
    }
}