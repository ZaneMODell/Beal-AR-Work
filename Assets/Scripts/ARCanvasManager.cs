using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    [Tooltip("Reference to the ModelViewManager Class")]
    ModelViewManager m_ModelViewManager;

    [SerializeField]
    [Tooltip("Test text for debugging")]
    TextMeshProUGUI testText;

    [SerializeField]
    Image BlackImage;

    float alpha = 1;
    #endregion

    #region Methods

    #region Unity Methods
    private void Awake()
    {
        BlackImage.color = new Color(0, 0, 0, alpha);
    }
    private void Update()
    {
        if (BlackImage)
        {
            if (Time.time > 1)
            {
                BlackImage.color = new Color(0, 0, 0, alpha);
                alpha -= Time.deltaTime * 5;
            }
            if (alpha <= 0)
            {
                BlackImage = null;
            }
        }
        

    }
    #endregion
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
        m_ModelViewManager.ClearModel();
        viewButton.SetActive(true);
        arButton.SetActive(false);
    }
    #endregion
    #endregion

}
