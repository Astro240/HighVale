using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{

    public GameObject ControlsPanel;
    public void showControls()
    {
        ControlsPanel.SetActive(true);
    }

    public void hideControls()
    {
        ControlsPanel.SetActive(false);
    }
}
