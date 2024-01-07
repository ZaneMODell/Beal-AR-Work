using TMPro;
using UnityEngine;


public class ARCanvasManager : MonoBehaviour
{
    public GameObject viewButton;

    public GameObject arButton;

    public ViewManager m_ViewManager;

    public TextMeshProUGUI testText;

    public void EnableViewerMode()
    {
        m_ViewManager.SwitchToModelView();
        viewButton.SetActive(false);
        arButton.SetActive(true);
        //testText.text = "Made it through viewer mode";
    }

    public void EnableARMode()
    {
        m_ViewManager.SwitchToARView();
        viewButton.SetActive(true);
        arButton.SetActive(false);
        //testText.text = "Made it through ar mode";
    }
}
