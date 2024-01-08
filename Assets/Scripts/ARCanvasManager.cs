using TMPro;
using UnityEngine;

/// <summary>
/// Class that manages the AR canvas and all of its components
/// </summary>
public class ARCanvasManager : MonoBehaviour
{
    #region Class Variables
    [SerializeField]
    [Tooltip("GameObject that contains the Model View Button")]
    GameObject viewButton;

    [SerializeField]
    [Tooltip("GameObject that contains the AR View Button")]
    GameObject arButton;

    [SerializeField]
    [Tooltip("Reference to the ViewManager Class")]
    ViewManager m_ViewManager;

    [SerializeField]
    [Tooltip("Test text for debugging")]
    TextMeshProUGUI testText;
    #endregion

    #region Methods
    #region Custom Methods
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
    #endregion
    #endregion




}
