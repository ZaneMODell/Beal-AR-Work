using TMPro;
using UnityEngine;

/// <summary>
/// Class that manages switching views
/// </summary>
public class ViewManager : MonoBehaviour
{
    #region Enum and Enum Instances
    public enum ViewState
    {
        AR,
        Model
    }

    public ViewState m_ViewState;
    #endregion

    #region Class Variables
    /// <summary>
    /// Reference to the main camera
    /// </summary>
    [SerializeField]
    Camera m_MainCamera;

    /// <summary>
    /// Text used for debugging purposes
    /// </summary>
    public TextMeshProUGUI testText;
    #endregion

    #region Methods
    #region Unity Methods
    void Start()
    {
        //Initializes state to AR
        m_ViewState = ViewState.AR;
    }
    #endregion

    #region Custom Methods
    /// <summary>
    /// Function that switches to the plant model view
    /// </summary>
    public void SwitchToModelView()
    {
        //Gets all gameobjects and scales down the AR objects
        GameObject[] activeAndInactive = FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (GameObject go in activeAndInactive)
        {
            if (go.tag == "ARObject")
            {
                go.transform.localScale = Vector3.zero;
            }
        }
        //Does some camera and state updates
        m_MainCamera.enabled = false;
        m_ViewState = ViewState.Model;
    }

    /// <summary>
    /// Function that switches to the general AR view
    /// </summary>
    public void SwitchToARView()
    {
        //Gets all gameobjects and scales down the AR objects
        GameObject[] activeAndInactive = FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (GameObject go in activeAndInactive)
        {
            if (go.tag == "ARObject")
            {
                if (go.transform.rotation.eulerAngles.x == -90)
                {
                    go.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                }
                go.transform.localScale = Vector3.one;
            }
        }
        //Does some camera and state updates
        m_MainCamera.enabled = true;
        m_ViewState = ViewState.AR;
    }
    #endregion
    #endregion

}
