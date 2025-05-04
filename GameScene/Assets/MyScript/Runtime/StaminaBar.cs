using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Combat playerCombat; // Reference to the Combat script
    public Slider staminaSlider;  // Reference to the UI Slider
    public Slider easeStaminaSlider;
    public float lerpSpeed = 0.05f;

    void Start()
    {
        // Initialize health bar
        if (playerCombat != null)
        {
            staminaSlider.maxValue = playerCombat.stamina; // Set max value
            staminaSlider.value = playerCombat.stamina;    // Set initial value
        }
    }

    void Update()
    {
        if (playerCombat != null)
        {
            staminaSlider.value = playerCombat.stamina; // Update the slider value
        }
        easeStaminaSlider.value = Mathf.Lerp(easeStaminaSlider.value, playerCombat.stamina,lerpSpeed);
    }
}
