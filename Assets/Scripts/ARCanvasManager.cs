using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class that manages the AR canvas and all of its components
/// </summary>
public class ARCanvasManager : MonoBehaviour
{
    #region Class Variables
    [Header("Canvas Elements")]
    [SerializeField]
    [Tooltip("GameObject that contains the Model View Button")]
    GameObject m_ViewButton;

    [SerializeField]
    [Tooltip("GameObject that contains the AR View Button")]
    GameObject m_ARButton;

    [SerializeField]
    [Tooltip("Test text for debugging")]
    TextMeshProUGUI m_TestText;

    /// <summary>
    /// Image used for a fade in effect upon app start
    /// </summary>
    [SerializeField]
    Image m_FadeImage;

    /// <summary>
    /// Alpha variable used for fade effect
    /// </summary>
    float alpha = 1;

    #region Script References
    [Header("Script References")]
    [SerializeField]
    [Tooltip("Reference to the ViewManager Class")]
    ViewManager m_ViewManager;

    [SerializeField]
    [Tooltip("Reference to the ModelViewManager Class")]
    ModelViewManager m_ModelViewManager;

    #endregion
    #endregion

    #region Methods
    #region Unity Methods
    private void Awake()
    {
        //Sets Color to be black with 100% alpha
        m_FadeImage.color = new Color(0, 0, 0, alpha);
    }
    private void Update()
    {
        //Handles image fade in
        if (m_FadeImage)
        {
            if (Time.time > 1)
            {
                m_FadeImage.color = new Color(0, 0, 0, alpha);
                alpha -= Time.deltaTime * 5;
            }
            if (alpha <= 0)
            {
                m_FadeImage = null;
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
        m_ViewButton.SetActive(false);
        m_ARButton.SetActive(true);
    }

    /// <summary>
    /// Function that enables the AR viewer mode via a UI Button Press
    /// </summary>
    public void EnableARMode()
    {
        m_ViewManager.SwitchToARView();
        m_ModelViewManager.ClearModel();
        m_ViewButton.SetActive(true);
        m_ARButton.SetActive(false);
    }
    #endregion
    #endregion

}
