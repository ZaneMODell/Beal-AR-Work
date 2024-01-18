using UnityEngine;
using UnityEngine.InputSystem.XR;

/// <summary>
/// Class that manages switching views
/// </summary>
public class ViewManager : MonoBehaviour
{
    #region Enum and Enum Instances
    /// <summary>
    /// Enum class representing the user views
    /// </summary>
    public enum ViewState
    {
        AR,
        Model
    }

    /// <summary>
    /// Instance of the ViewState for the ViewManager
    /// </summary>
    public ViewState m_ViewState;
    #endregion

    #region Class Variables
    /// <summary>
    /// Reference to the main camera
    /// </summary>
    [SerializeField]
    Camera m_MainCamera;

    /// <summary>
    /// TrackedPoseDriver instance used for AR Camera Rotation and Position Tracking
    /// </summary>
    [SerializeField]
    TrackedPoseDriver m_TrackedPoseDriver;

    /// <summary>
    /// Position to lock the camera to during view switch
    /// </summary>
    Vector3 m_CamLockPosition;

    /// <summary>
    /// Rotation to lock the camera to during view switch
    /// </summary>
    Vector3 m_CamLockRotation;
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
            if (go.CompareTag("ARObject"))
            {
                go.transform.localScale = Vector3.zero;
            }
        }
        //Does some camera and state updates
        m_CamLockPosition = m_MainCamera.transform.localPosition;
        m_CamLockRotation = m_MainCamera.transform.rotation.eulerAngles;
        //Disables movement of AR camera in model view
        m_TrackedPoseDriver.enabled = false;
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
            if (go.CompareTag("ARObject"))
            {
                if (go.transform.rotation.eulerAngles.x == -90)
                {
                    go.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                }
                go.transform.localScale = Vector3.one;
            }
        }
        //Does some camera and state updates
        //Enables movement of AR Camera in AR view
        m_TrackedPoseDriver.enabled = true;
        m_MainCamera.enabled = true;
        m_ViewState = ViewState.AR;
        m_MainCamera.transform.SetPositionAndRotation(m_CamLockPosition, Quaternion.Euler(m_CamLockRotation));
    }
    #endregion
    #endregion

}
