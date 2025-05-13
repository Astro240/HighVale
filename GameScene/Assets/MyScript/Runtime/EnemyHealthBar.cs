using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Enemy EnemyHealth; // Reference to the Combat script
    public Slider healthSlider;  // Reference to the UI Slider
    public Slider easeHealthSlider;
    public float lerpSpeed = 0.05f;

    void Start()
    {
        // Initialize health bar
        if (EnemyHealth != null)
        {
            healthSlider.maxValue = EnemyHealth.health; // Set max value
            healthSlider.value = EnemyHealth.health;    // Set initial value
        }
    }

    void Update()
    {
        if (EnemyHealth != null)
        {
            healthSlider.value = EnemyHealth.health; // Update the slider value
        }
        easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, EnemyHealth.health,lerpSpeed);
    }
}
