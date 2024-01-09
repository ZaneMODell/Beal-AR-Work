using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


/// <summary>
/// Class that manages all tracked images
/// </summary>
public class ImageTrackingObjectManager : MonoBehaviour
{
    #region Class Variables
    #region Image Related Information
    [SerializeField]
    [Tooltip("Image manager on the AR Session Origin")]
    ARTrackedImageManager m_ImageManager;

    /// <summary>
    /// Get the <c>ARTrackedImageManager</c>
    /// </summary>
    public ARTrackedImageManager ImageManager
    {
        get => m_ImageManager;
        set => m_ImageManager = value;
    }

    [SerializeField]
    [Tooltip("Reference Image Library")]
    XRReferenceImageLibrary m_ImageLibrary;

    /// <summary>
    /// Get the <c>XRReferenceImageLibrary</c>
    /// </summary>
    public XRReferenceImageLibrary ImageLibrary
    {
        get => m_ImageLibrary;
        set => m_ImageLibrary = value;
    }

    int m_NumberOfTrackedImages;

    static Guid s_FirstImageGUID;
    static Guid s_SecondImageGUID;
    #endregion

    #region Prefab References
    [SerializeField]
    [Tooltip("Prefab for tracked 1 image")]
    GameObject m_PlantPrefab;

    /// <summary>
    /// Get the one prefab
    /// </summary>
    public GameObject PlantPrefab
    {
        get => m_PlantPrefab;
        set => m_PlantPrefab = value;
    }

    /// <summary>
    /// Spawned plant prefab
    /// </summary>
    GameObject m_SpawnedPlantPrefab;

    /// <summary>
    /// get the spawned one prefab
    /// </summary>
    public GameObject SpawnedPlantPrefab
    {
        get => m_SpawnedPlantPrefab;
        set => m_SpawnedPlantPrefab = value;
    }

    [SerializeField]
    [Tooltip("Prefab for tracked 2 image")]
    GameObject m_FrogPrefab;

    /// <summary>
    /// get the two prefab
    /// </summary>
    public GameObject FrogPrefab
    {
        get => m_FrogPrefab;
        set => m_FrogPrefab = value;
    }

    /// <summary>
    /// Spawned frog prefab
    /// </summary>
    GameObject m_SpawnedFrogPrefab;

    /// <summary>
    /// get the spawned two prefab
    /// </summary>
    public GameObject SpawnedFrogPrefab
    {
        get => m_SpawnedFrogPrefab;
        set => m_SpawnedFrogPrefab = value;
    }
    #endregion

    #region AR Canvas
    [SerializeField]
    [Tooltip("GameObject that is the AR Canvas")]
    GameObject m_ARCanvas;
    #endregion

    #region ViewManager
    [SerializeField]
    [Tooltip("Reference to the View Manager")]
    ViewManager m_ViewManager;
    #endregion

    #region ModelViewManager
    [SerializeField]
    [Tooltip("Reference to the Model View Manager")]
    ModelViewManager m_ModelViewManager;
    #endregion
    #endregion

    #region Methods
    #region Unity Methods
    /// <summary>
    /// Unity method that is called when this object is active and enabled
    /// </summary>
    void OnEnable()
    {
        if (s_FirstImageGUID != m_ImageLibrary[0].guid || s_SecondImageGUID != m_ImageLibrary[1].guid)
        {
            s_FirstImageGUID = m_ImageLibrary[0].guid;
            s_SecondImageGUID = m_ImageLibrary[1].guid;
        };
        m_ImageManager.trackedImagesChanged += ImageManagerOnTrackedImagesChanged;
    }

    /// <summary>
    /// Unity method that is called when this object is disabled
    /// </summary>
    void OnDisable()
    {
        m_ImageManager.trackedImagesChanged -= ImageManagerOnTrackedImagesChanged;
    }

    /// <summary>
    /// Unity method that is called once each frame (Set in Project Settings at 60 FPS)
    /// </summary>
    private void FixedUpdate()
    {
        if (NumberOfTrackedImages() > 0 || m_ViewManager.m_ViewState == ViewManager.ViewState.Model)
        {
            m_ARCanvas.SetActive(true);
        }
        else
        {
            m_ARCanvas.SetActive(false);
        }
    }

    #endregion

    #region Custom Methods

    /// <summary>
    /// Function that is called when the AR Tracked Images' states are changed in some way
    /// </summary>
    /// <param name="obj">Object representing the changed state of the AR Tracked Images</param>
    void ImageManagerOnTrackedImagesChanged(ARTrackedImagesChangedEventArgs obj)
    {
        // added, spawn prefab
        foreach (ARTrackedImage image in obj.added)
        {
            if (image.referenceImage.guid == s_FirstImageGUID)
            {
                m_SpawnedPlantPrefab = Instantiate(m_PlantPrefab, image.transform.position, image.transform.rotation);
            }
            else if (image.referenceImage.guid == s_SecondImageGUID)
            {
                m_SpawnedFrogPrefab = Instantiate(m_FrogPrefab, image.transform.position, image.transform.rotation);
            }
        }

        // updated, set prefab position and rotation
        foreach (ARTrackedImage image in obj.updated)
        {
            // image is tracking or tracking with limited state, show visuals and update it's position and rotation
            if (image.trackingState == TrackingState.Tracking)
            {
                if (image.referenceImage.guid == s_FirstImageGUID)
                {
                    m_SpawnedPlantPrefab.transform.SetPositionAndRotation(image.transform.position, image.transform.rotation);
                    m_ModelViewManager.SetModel(m_PlantPrefab);
                    
                    m_SpawnedPlantPrefab.SetActive(true);
                }
                else if (image.referenceImage.guid == s_SecondImageGUID)
                {
                    m_SpawnedFrogPrefab.transform.SetPositionAndRotation(image.transform.position, image.transform.rotation);
                    m_ModelViewManager.SetModel(m_FrogPrefab);
                    m_SpawnedFrogPrefab.SetActive(true);
                }
            }
            // image is no longer tracking, disable visuals TrackingState.Limited TrackingState.None
            else
            {
                if (image.referenceImage.guid == s_FirstImageGUID)
                {
                    m_ModelViewManager.ClearModel();
                    m_SpawnedPlantPrefab.SetActive(false);
                }
                else if (image.referenceImage.guid == s_SecondImageGUID)
                {
                    m_ModelViewManager.ClearModel();
                    m_SpawnedFrogPrefab.SetActive(false);
                }
            }
        }

        // removed, destroy spawned instance
        foreach (ARTrackedImage image in obj.removed)
        {
            if (image.referenceImage.guid == s_FirstImageGUID)
            {
                Destroy(m_SpawnedPlantPrefab);
            }
            else if (image.referenceImage.guid == s_SecondImageGUID)
            {
                Destroy(m_SpawnedFrogPrefab);
            }
        }
    }

    /// <summary>
    /// Function that returns the number of currently tracked images
    /// </summary>
    /// <returns>Returns an int that is the number of currently tracked images</returns>
    public int NumberOfTrackedImages()
    {
        m_NumberOfTrackedImages = 0;
        foreach (ARTrackedImage image in m_ImageManager.trackables)
        {
            if (image.trackingState == TrackingState.Tracking)
            {
                m_NumberOfTrackedImages++;
            }
        }
        return m_NumberOfTrackedImages;
    }
    #endregion
    #endregion

}