using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class that manages the AR canvas and all of its components
/// </summary>
public class ARCanvasManager : MonoBehaviour
{
    #region Class Variables
    /// <summary>
    /// Reference to the view button on the AR Canvas
    /// </summary>
    [Header("Canvas Elements")]
    [SerializeField]
    [Tooltip("GameObject that contains the Model View Button")]
    GameObject m_ViewButton;

    /// <summary>
    /// Reference to the AR button on the AR Canvas
    /// </summary>
    [SerializeField]
    [Tooltip("GameObject that contains the AR View Button")]
    GameObject m_ARButton;

    /// <summary>
    /// Reference to the conservation resources button on the AR Canvas
    /// </summary>
    [SerializeField]
    [Tooltip("GameObject that contains the Conservation Resources Button")]
    GameObject m_ResourceButton;

    /// <summary>
    /// Reference to the plant map button on the AR Canvas
    /// </summary>
    [SerializeField]
    [Tooltip("GameObject that contains the plant map Button")]
    GameObject m_MapButton;

    /// <summary>
    /// Reference to the test text on the AR Canvas
    /// </summary>
    [SerializeField]
    [Tooltip("Test text for debugging")]
    TextMeshProUGUI m_TestText;

    /// <summary>
    /// Image used for a fade in effect upon app start
    /// </summary>
    [SerializeField]
    [Tooltip("UI image used for fade in on app start")]
    Image m_FadeImage;

    /// <summary>
    /// Alpha variable used for fade effect
    /// </summary>
    private float alpha = 1;

    #region Script References
    /// <summary>
    /// Reference to the ViewManager instance
    /// </summary>
    [Header("Script References")]
    [SerializeField]
    [Tooltip("Reference to the ViewManager Class")]
    private ViewManager m_ViewManager;

    /// <summary>
    /// Reference to the ModelViewManager instance
    /// </summary>
    [SerializeField]
    [Tooltip("Reference to the ModelViewManager Class")]
    private ModelViewManager m_ModelViewManager;

    public ResourceManager resourceManager;

    #endregion
    #endregion

    #region Methods
    #region Unity Methods
    /// <summary>
    /// Unity method called on scene load
    /// </summary>
    private void Awake()
    {
        //Sets Color to be black with 100% alpha
        m_FadeImage.color = new Color(0, 0, 0, alpha);
    }
    /// <summary>
    /// Method called once every frame
    /// </summary>
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
        m_ResourceButton.SetActive(true);
        m_MapButton.SetActive(true);
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
        m_ResourceButton.SetActive(false);
        m_MapButton.SetActive(false);
    }

    public void OpenURL(string url)
    {
        Application.OpenURL(url);
    }

    public void OpenMapURL()
    {
        OpenURL(resourceManager.currentMapLink);
    }
    #endregion
    #endregion

}
