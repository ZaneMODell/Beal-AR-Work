using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class ViewManager : MonoBehaviour
{

    public enum ViewState
    {
        AR,
        Model
    }

    public ViewState m_viewState;

    public GameObject m_SimulationCamera;

    public GameObject modelCam;

    public Camera m_MainCamera;

    GameObject m_SimulationEnvironment;


    void Start()
    {
        m_viewState = ViewState.AR;
        m_SimulationCamera = GameObject.Find("SimulationCamera");
    }

    private void Update()
    {
        if (!m_SimulationEnvironment)
        {
            if (SceneManager.sceneCount > 1)
            {
                m_SimulationEnvironment = SceneManager.GetSceneAt(1).GetRootGameObjects()[0];
                Debug.Log("Set sim environment");
            }
        }
    }

    public void SwitchToModelView()
    {
        m_SimulationEnvironment.transform.GetComponentInChildren<Transform>().localScale = Vector3.zero;
        m_SimulationCamera.SetActive(false);
        m_MainCamera.enabled = false;
        modelCam.SetActive(true);
        m_viewState = ViewState.Model;
    }

    public void SwitchToARView()
    {
        m_SimulationEnvironment.transform.GetComponentInChildren<Transform>().localScale = Vector3.one;
        m_SimulationCamera.SetActive(true);
        m_MainCamera.enabled = true;
        modelCam.SetActive(false);
        m_viewState = ViewState.AR;
    }
}
