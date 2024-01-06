using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ViewManager : MonoBehaviour
{

    public enum ViewState
    {
        AR,
        Model
    }

    public TextMeshProUGUI testText;

    public ViewState m_viewState;

    public GameObject m_SimulationCamera;

    public GameObject modelCam;

    public Camera m_MainCamera;

    public GameObject m_SimulationPrefab;

    GameObject m_SimulationEnvironment;

    bool logBool = false;


    void Start()
    {
        m_viewState = ViewState.AR;
        m_SimulationCamera = GameObject.Find("SimulationCamera");
        testText.text = SceneManager.GetSceneAt(0).name;
    }

    private void Update()
    {
        //if (!m_SimulationEnvironment)
        //{
        //    if (SceneManager.sceneCount > 1)
        //    {
        //        m_SimulationEnvironment = SceneManager.GetSceneAt(1).GetRootGameObjects()[0];
        //        Debug.Log(m_SimulationEnvironment.name);
        //        testText.text = "Set sim environment: " + m_SimulationEnvironment.name;
        //    }
        //}

        if (!logBool)
        {
            GameObject[] allObjects = FindObjectsOfType<GameObject>();
            foreach (GameObject go in allObjects)
                if (go.activeInHierarchy)
                    testText.text += go.name + "/";
            if (m_SimulationEnvironment)
            {
                testText.text += m_SimulationEnvironment.name + "(Environment was already found)/";
                logBool = true;
            }
            else
            {
                if (GameObject.Find("SimEnvironmentProbe"))
                {
                    m_SimulationEnvironment = GameObject.Find("SimEnvironmentProbe").transform.parent.gameObject;
                    testText.text += m_SimulationEnvironment.name + "/";
                    logBool = true;
                }
            }
            
        }
        
    }

    public void SwitchToModelView()
    {
        testText.text = "Starting model view switch";
        m_SimulationEnvironment.transform.GetComponentInChildren<Transform>().localScale = Vector3.zero;
        m_SimulationCamera.SetActive(false);
        m_MainCamera.enabled = false;
        modelCam.SetActive(true);
        m_viewState = ViewState.Model;
        testText.text = "ending model view switch";
    }

    public void SwitchToARView()
    {
        testText.text = "Starting AR view switch";
        m_SimulationEnvironment.transform.GetComponentInChildren<Transform>().localScale = Vector3.one;
        m_SimulationCamera.SetActive(true);
        m_MainCamera.enabled = true;
        modelCam.SetActive(false);
        m_viewState = ViewState.AR;
        testText.text = "ending AR view switch";
    }
}
