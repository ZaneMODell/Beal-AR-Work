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

    [SerializeField]
    GameObject m_ModelCamera;

    public float modelHeight;

    public float modelWidth;

    public Transform m_CamZoomInnerBound;

    public Transform m_CamZoomOuterBound;

    [SerializeField] private ModelCameraInput m_ModelCameraInput;
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
            MeshFilter meshFilter = m_InstantiatedPlantPrefab.GetComponentInChildren<MeshFilter>();
            modelHeight = meshFilter.mesh.bounds.extents.y;
            modelWidth = meshFilter.mesh.bounds.extents.x;
            m_ModelCamera.transform.position = m_InstantiatedPlantPrefab.transform.position - new Vector3(0, 0, modelHeight * 1000);
            m_CamZoomInnerBound.position = m_InstantiatedPlantPrefab.transform.position - new Vector3(0, 0, modelHeight * 100);
            m_CamZoomOuterBound.position = m_InstantiatedPlantPrefab.transform.position - new Vector3(0, 0, modelHeight * 3000);

            m_ModelCameraInput.camDistanceFromModel = Mathf.Abs(m_ModelCamera.transform.position.z - m_InstantiatedPlantPrefab.transform.position.z);
            m_ModelCameraInput.m_InnerBoundDistanceFromModel = Mathf.Abs(m_CamZoomInnerBound.position.z - m_InstantiatedPlantPrefab.transform.position.z);
            m_ModelCameraInput.m_OuterBoundDistanceFromModel = Mathf.Abs(m_CamZoomOuterBound.position.z - m_InstantiatedPlantPrefab.transform.position.z);

            m_ModelCamera.transform.eulerAngles = new Vector3(0, 0, 5);
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
