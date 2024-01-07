using TMPro;
using UnityEngine;


public class ARCanvasManager : MonoBehaviour
{
    public GameObject viewButton;

    public GameObject arButton;

    public ViewManager m_ViewManager;

    public TextMeshProUGUI testText;

    /// <summary>
    /// Function that enables the model viewer mode via a UI Button Press
    /// </summary>
    public void EnableViewerMode()
    {
        m_ViewManager.SwitchToModelView();
        viewButton.SetActive(false);
        arButton.SetActive(true);
    }

    /// <summary>
    /// Function that enables the AR viewer mode via a UI Button Press
    /// </summary>
    public void EnableARMode()
    {
        m_ViewManager.SwitchToARView();
        viewButton.SetActive(true);
        arButton.SetActive(false);
    }
}
