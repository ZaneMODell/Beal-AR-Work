using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ARCanvasManager : MonoBehaviour
{
    public GameObject viewButton;

    public GameObject arButton;

    public ViewManager m_ViewManager;

    public void EnableViewerMode()
    {
        m_ViewManager.SwitchToModelView();
        viewButton.SetActive(false);
        arButton.SetActive(true);
    }

    public void EnableARMode()
    {
        m_ViewManager.SwitchToARView();
        viewButton.SetActive(true);
        arButton.SetActive(false);
    }
}
