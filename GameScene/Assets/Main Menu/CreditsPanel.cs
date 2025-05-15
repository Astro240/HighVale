using UnityEngine;

public class CreditsPanel : MonoBehaviour
{
    public GameObject creditsPanel;

    public void ShowCredits()
    {
        creditsPanel.SetActive(true);
    }

    public void HideCredits()
    {
        creditsPanel.SetActive(false);
    }
}
