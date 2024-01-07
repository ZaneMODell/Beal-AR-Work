using System;
using UnityEngine;
//using UnityEngine.Formats.Alembic.Importer;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageTrackingObjectManager : MonoBehaviour
{
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

    GameObject m_SpawnedFrogPrefab;

    /// <summary>
    /// get the spawned two prefab
    /// </summary>
    public GameObject SpawnedFrogPrefab
    {
        get => m_SpawnedFrogPrefab;
        set => m_SpawnedFrogPrefab = value;
    }

    int m_NumberOfTrackedImages;

    static Guid s_FirstImageGUID;
    static Guid s_SecondImageGUID;

    [SerializeField]
    GameObject m_ARCanvas;

    public ViewManager m_ViewManager;

    void OnEnable()
    {
        if (s_FirstImageGUID != m_ImageLibrary[0].guid || s_SecondImageGUID != m_ImageLibrary[1].guid)
        {
            s_FirstImageGUID = m_ImageLibrary[0].guid;
            s_SecondImageGUID = m_ImageLibrary[1].guid;
        };
        m_ImageManager.trackedImagesChanged += ImageManagerOnTrackedImagesChanged;
    }

    void OnDisable()
    {
        m_ImageManager.trackedImagesChanged -= ImageManagerOnTrackedImagesChanged;
    }

    void ImageManagerOnTrackedImagesChanged(ARTrackedImagesChangedEventArgs obj)
    {
        // added, spawn prefab
        foreach (ARTrackedImage image in obj.added)
        {
            if (image.referenceImage.guid == s_FirstImageGUID)
            {
                m_SpawnedPlantPrefab = Instantiate(m_PlantPrefab, image.transform.position, image.transform.rotation);
                //m_SpawnedPlantPrefab = Instantiate(m_PlantPrefab, image.transform.position, Quaternion.identity);
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
                    //m_SpawnedPlantPrefab.transform.position = image.transform.position;
                    m_SpawnedPlantPrefab.transform.SetPositionAndRotation(image.transform.position, image.transform.rotation);
                    m_SpawnedPlantPrefab.SetActive(true);
                    //m_SpawnedPlantPrefab.GetComponent<AlembicStreamPlayer>().CurrentTime += Time.deltaTime * 5;
                }
                else if (image.referenceImage.guid == s_SecondImageGUID)
                {
                    m_SpawnedFrogPrefab.transform.SetPositionAndRotation(image.transform.position, image.transform.rotation);
                    m_SpawnedFrogPrefab.SetActive(true);
                }
            }
            // image is no longer tracking, disable visuals TrackingState.Limited TrackingState.None
            else
            {
                if (image.referenceImage.guid == s_FirstImageGUID)
                {
                    m_SpawnedPlantPrefab.SetActive(false);
                    //m_SpawnedPlantPrefab.GetComponent<AlembicStreamPlayer>().CurrentTime = 0;
                }
                else if (image.referenceImage.guid == s_SecondImageGUID)
                {
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

    private void Update()
    {
        if (NumberOfTrackedImages() > 0 || m_ViewManager.m_viewState == ViewManager.ViewState.Model)
        {
            m_ARCanvas.SetActive(true);
        }
        else
        {
            m_ARCanvas.SetActive(false);
        }
    }
}