using UnityEngine;

public class Cat : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("Cat collected!");
        Destroy(gameObject);  // This will destroy the cat object when interacted with
    }
}
