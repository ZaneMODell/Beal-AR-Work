using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ARCanvasManager : MonoBehaviour
{

    [SerializeField]
    TextMeshProUGUI m_TestText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnableViewerMode()
    {
        if (m_TestText.gameObject.activeSelf)
        {
            m_TestText.gameObject.SetActive(false);
        }
        else
        {
            m_TestText.text = "Hey, Viewer mode has been enabled!";
            m_TestText.gameObject.SetActive(true);
        }
    }

    public void EnableARMode()
    {
        if (m_TestText.gameObject.activeSelf)
        {
            m_TestText.gameObject.SetActive(false);
        }
        else
        {
            m_TestText.text = "Hey, AR mode has been enabled!";
            m_TestText.gameObject.SetActive(true);
        }
    }
}
