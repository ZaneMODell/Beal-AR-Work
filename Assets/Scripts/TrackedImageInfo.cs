using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class TrackedImageInfo : MonoBehaviour
{
    [SerializeField]
    ARTrackedImageManager m_TrackedImageManager;


    [SerializeField]
    GameObject m_PlantModel;

    [SerializeField]
    GameObject m_FrogModel;

    private void Start()
    {
        m_TrackedImageManager.trackedImagePrefab = m_FrogModel;
    }

    void OnEnable() => m_TrackedImageManager.trackedImagesChanged += OnChanged;

    void OnDisable() => m_TrackedImageManager.trackedImagesChanged -= OnChanged;

    void OnChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var newImage in eventArgs.added)
        {
            // Handle added event
        }

        foreach (var updatedImage in eventArgs.updated)
        {
            // Handle updated event
        }

        foreach (var removedImage in eventArgs.removed)
        {
            // Handle removed event
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            ListAllImages();
        }
    }

    void ListAllImages()
    {
        Debug.Log(
            $"There are {m_TrackedImageManager.trackables.count} images being tracked.");

        foreach (var trackedImage in m_TrackedImageManager.trackables)
        {
            Debug.Log($"Image: {trackedImage.referenceImage.name} is at " +
                      $"{trackedImage.transform.position}");

            if (trackedImage.referenceImage.name == "qrcode")
            {
                m_TrackedImageManager.trackedImagePrefab = m_FrogModel;
            }
            Debug.Log(m_TrackedImageManager.trackedImagePrefab.name);
        }
    }
}
