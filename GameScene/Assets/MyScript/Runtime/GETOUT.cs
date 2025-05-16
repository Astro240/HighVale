using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GETOUT : MonoBehaviour
{
    public AudioClip soundClip; // Assign your audio clip in the Inspector
    public AudioClip knock; // Assign your knock audio clip in the Inspector
    private AudioSource audioSource;
    public float interactionDistance = 3.0f; // Distance to trigger sound
    public Combat Combat;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = soundClip;
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, Combat.transform.position) < interactionDistance)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                StartCoroutine(PlaySounds());
            }
        }
    }

    private IEnumerator PlaySounds()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = knock;
            audioSource.Play();

            // Wait for 1 second
            yield return new WaitForSeconds(0.7f);

            // Play the get out sound
            audioSource.clip = soundClip;
            audioSource.Play();
        }
    }
}