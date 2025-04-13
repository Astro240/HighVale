using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Combat playerCombat; // Reference to the Combat script
    public Slider healthSlider;  // Reference to the UI Slider

    void Start()
    {
        // Initialize health bar
        if (playerCombat != null)
        {
            healthSlider.maxValue = playerCombat.health; // Set max value
            healthSlider.value = playerCombat.health;    // Set initial value
        }
    }

    void Update()
    {
        if (playerCombat != null)
        {
            healthSlider.value = playerCombat.health; // Update the slider value
        }
    }
}
