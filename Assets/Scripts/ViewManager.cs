using TMPro;
using UnityEngine;

public class ViewManager : MonoBehaviour
{

    public enum ViewState
    {
        AR,
        Model
    }

    public TextMeshProUGUI testText;

    public ViewState m_viewState;

    public GameObject modelCam;

    public Camera m_MainCamera;

    bool logBool = false;


    void Start()
    {
        m_viewState = ViewState.AR;
    }

    public void SwitchToModelView()
    {
        GameObject[] activeAndInactive = FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (GameObject go in activeAndInactive)
        {
            if (go.tag == "ARObject")
            {
                go.transform.localScale = Vector3.zero;
            }
        }      
        m_MainCamera.enabled = false;
        m_viewState = ViewState.Model;
    }

    public void SwitchToARView()
    {
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
        m_MainCamera.enabled = true;
        m_viewState = ViewState.AR;
    }
}
