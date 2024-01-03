using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewerModeButton : MonoBehaviour
{

    [SerializeField]
    GameObject m_TestText;

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
        if (m_TestText.activeSelf)
        {
            m_TestText.SetActive(false);
        }
        else
        {
            m_TestText.SetActive(true);
        }
    }
}
