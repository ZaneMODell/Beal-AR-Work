using UnityEngine;


/// <summary>
/// Class that manages the model view
/// </summary>
public class ModelViewManager : MonoBehaviour
{

    #region Class Variables
    [SerializeField]
    Transform m_PlantInstantiationPoint;

    public GameObject m_PlantPrefab;

    GameObject m_InstantiatedPlantPrefab;

    bool m_ModelSet;

    [SerializeField]
    ViewManager m_ViewManager;

    [SerializeField]
    GameObject m_MainCamera;

    Vector3 camPosition;

    Quaternion camRotation;
    #endregion

    #region Methods
    public void SetModel(GameObject plantPrefab)
    {
        if (!m_ModelSet && m_ViewManager.m_ViewState == ViewManager.ViewState.Model)
        {
            m_PlantPrefab = plantPrefab;
            m_InstantiatedPlantPrefab = Instantiate(m_PlantPrefab, m_PlantInstantiationPoint.position, m_PlantInstantiationPoint.rotation, m_PlantInstantiationPoint);
            m_InstantiatedPlantPrefab.transform.localScale = Vector3.one * 20;
            m_InstantiatedPlantPrefab.transform.eulerAngles = new Vector3(m_InstantiatedPlantPrefab.transform.eulerAngles.x - 90,
                m_InstantiatedPlantPrefab.transform.eulerAngles.y, m_InstantiatedPlantPrefab.transform.eulerAngles.z);
            m_ModelSet = true;
        }
    }

    public void ClearModel()
    {
        if (m_ModelSet && m_ViewManager.m_ViewState == ViewManager.ViewState.AR)
        {
            Destroy(m_InstantiatedPlantPrefab);
            m_InstantiatedPlantPrefab = null;
            m_PlantPrefab = null;
            m_ModelSet = false;
        }
    }
    #endregion

}
