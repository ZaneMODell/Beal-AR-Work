using System.Collections;
using UnityEngine;

/// <summary>
/// Class that manages all of the input for the model view camera
/// </summary>
public class ModelCameraInput : MonoBehaviour
{
    #region Class Variables
    #region Camera Variables/References
    /// <summary>
    /// Reference to the model camera
    /// </summary>
    [Header("Camera Zoom and Rotation Variables")]
    [SerializeField]
    private Camera m_ModelCam;

    /// <summary>
    /// Speed of the camera when zooming
    /// </summary>
    [Tooltip("Speed in which the camera zooms in and out")]
    public float m_CameraZoomSpeed = 4f;

    /// <summary>
    /// Distance from the camera to the plant that it's looking at
    /// </summary>
    [HideInInspector]
    public float m_CamDistanceFromModel;

    /// <summary>
    /// Distance from the inner bound of the zoom transform to the model
    /// </summary>
    [HideInInspector] 
    public float m_InnerBoundDistanceFromModel;

    /// <summary>
    /// Distance from the inner bound of the zoom transform to the model
    /// </summary>
    [HideInInspector] 
    public float m_OuterBoundDistanceFromModel;

    /// <summary>
    /// Transform to rotate around
    /// </summary>
    [SerializeField]
    [Tooltip("Transform that the camera will rotate around")]
    private Transform m_RotateTransform;

    /// <summary>
    /// Position of the camera on the previous frame
    /// </summary>
    private Vector3 m_PreviousCamPosition;

    /// <summary>
    /// Instance of the zoom input controls
    /// </summary>
    private TouchZoom m_TouchZoom;

    /// <summary>
    /// Coroutine that handles zooming
    /// </summary>
    private Coroutine m_ZoomCoroutine;
    #endregion

    #region Script References
    [Header("Script References")]
    [SerializeField]
    [Tooltip("Reference to the ViewManager instance")]
    private ViewManager m_ViewManager;

    [SerializeField]
    [Tooltip("Reference to the ModelViewManager instance")]
    private ModelViewManager m_ModelViewManager;
    #endregion
    #endregion

    #region Methods

    #region Unity Methods
    /// <summary>
    /// Method that plays when scene loads
    /// </summary>
    private void Awake()
    {
        m_TouchZoom = new TouchZoom();
    }
    /// <summary>
    /// Function that is called when scene loads, called after Awake
    /// </summary>
    private void Start()
    {
        //Syntax used to subscribe to events and call functions, need to look into more
        m_TouchZoom.Touch.SecondaryTouchContact.started += _ => ZoomStart();
        m_TouchZoom.Touch.SecondaryTouchContact.canceled += _ => ZoomEnd();
    }

    /// <summary>
    /// Method that is called when this script is enabled
    /// </summary>
    private void OnEnable()
    {
        //Enabling the input actions class generated from input system
        m_TouchZoom.Enable();
    }

    /// <summary>
    /// Method that is called when this script is disabled
    /// </summary>
    private void OnDisable()
    {
        //Disabling the input actions class generated from input system
        m_TouchZoom.Disable();
    }

    /// <summary>
    /// Unity function that is called once per frame
    /// </summary>
    void Update()
    {
        if (m_ViewManager.m_ViewState == ViewManager.ViewState.Model)
        {
            //If there are less than 2 touches, we are going to rotate
            if (Input.touchCount < 2)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    //Set previous position to "current" position
                    m_PreviousCamPosition = m_ModelCam.ScreenToViewportPoint(Input.mousePosition);
                }

                if (Input.GetMouseButton(0))
                {
                    //Get the direction that the touch is going
                    Vector3 direction = m_PreviousCamPosition - m_ModelCam.ScreenToViewportPoint(Input.mousePosition);

                    m_ModelCam.transform.position = m_RotateTransform.position;
                    m_ModelViewManager.m_CamZoomInnerBound.position = m_RotateTransform.position;
                    m_ModelViewManager.m_CamZoomOuterBound.position = m_RotateTransform.position;

                    float xrot = m_ModelCam.transform.eulerAngles.x;
                    
                    if (xrot < 0 || xrot > 180)
                    {
                        xrot = 0;
                    }

                    //Setting rotations
                    m_ModelCam.transform.rotation = Quaternion.Euler(xrot, m_ModelCam.transform.eulerAngles.y, m_ModelCam.transform.eulerAngles.z);
                    m_ModelViewManager.m_CamZoomOuterBound.rotation = Quaternion.Euler(xrot, m_ModelCam.transform.eulerAngles.y, m_ModelCam.transform.eulerAngles.z);
                    m_ModelViewManager.m_CamZoomInnerBound.rotation = Quaternion.Euler(xrot, m_ModelCam.transform.eulerAngles.y, m_ModelCam.transform.eulerAngles.z);

                    //Rotating the camera
                    m_ModelCam.transform.Rotate(new Vector3(1, 0, 0), direction.y * 180);
                    m_ModelCam.transform.Rotate(new Vector3(0, 1, 0), -direction.x * 180);
                    m_ModelCam.transform.Translate(new Vector3(0, 0, -m_CamDistanceFromModel));

                    //Rotating the inner bound
                    m_ModelViewManager.m_CamZoomInnerBound.Rotate(new Vector3(1, 0, 0), direction.y * 180);
                    m_ModelViewManager.m_CamZoomInnerBound.Rotate(new Vector3(0, 1, 0), -direction.x * 180);
                    m_ModelViewManager.m_CamZoomInnerBound.Translate(new Vector3(0, 0, -m_InnerBoundDistanceFromModel));

                    //Rotating the outer bound
                    m_ModelViewManager.m_CamZoomOuterBound.Rotate(new Vector3(1, 0, 0), direction.y * 180);
                    m_ModelViewManager.m_CamZoomOuterBound.Rotate(new Vector3(0, 1, 0), -direction.x * 180);
                    m_ModelViewManager.m_CamZoomOuterBound.Translate(new Vector3(0, 0, -m_OuterBoundDistanceFromModel));

                    //Setting the prev position to the current position
                    m_PreviousCamPosition = m_ModelCam.ScreenToViewportPoint(Input.mousePosition);
                }

            }
        }
    }
    #endregion

    #region Custom Methods

    /// <summary>
    /// Function that begins zoom coroutine
    /// </summary>
    private void ZoomStart()
    {
        m_ZoomCoroutine = StartCoroutine(ZoomDetection());
    }

    /// <summary>
    /// Function that ends zoom coroutine
    /// </summary>
    private void ZoomEnd()
    {
        StopCoroutine(m_ZoomCoroutine);
    }

    /// <summary>
    /// Coroutine that detects zoom input and zooms the camera
    /// </summary>
    /// <returns>IEnumerator that yield returns null</returns>
    IEnumerator ZoomDetection()
    {
        float previousDistance = 0f, distance;
        //This coroutine will, when ran, constantly check for multiple inputs
        while (true)
        {
            //Gets touch positions and calculates the distance between them
            Vector2 primaryDistance = m_TouchZoom.Touch.PrimaryFingerPosition.ReadValue<Vector2>();
            Vector2 secondaryDistance = m_TouchZoom.Touch.SecondaryFingerPosition.ReadValue<Vector2>();
            distance = Vector2.Distance(primaryDistance, secondaryDistance);

            //Detection of new input
            //Zoom out
            if (distance > previousDistance)
            {
                //Smoothly interpolates between the camera's current position and it's inner bound
                m_ModelCam.transform.position = Vector3.Slerp(m_ModelCam.transform.position, m_ModelViewManager.m_CamZoomInnerBound.position, Time.deltaTime * m_CameraZoomSpeed);
            }

            //Zoom in
            else if (distance < previousDistance)
            {
                //Smoothly interpolates between the camera's current position and it's outer bound
                m_ModelCam.transform.position = Vector3.Slerp(m_ModelCam.transform.position, m_ModelViewManager.m_CamZoomOuterBound.position, Time.deltaTime * m_CameraZoomSpeed);
            }

            //Updates our previous position and the distance from the camera to the model
            previousDistance = distance;
            m_CamDistanceFromModel = Vector3.Distance(m_ModelCam.transform.position, m_RotateTransform.transform.position); ;
            yield return null;
        }
    }
    #endregion
    #endregion
}
