using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Utilities.Tweenables.Primitives;

public class ModelCameraInput : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    private float cameraSpeed = 4f;


    public float camDistanceFromModel;

    public float m_InnerBoundDistanceFromModel;

    public float m_OuterBoundDistanceFromModel;

    [SerializeField]
    private Transform target;

    [SerializeField]
    private ViewManager m_ViewManager;

    [SerializeField]
    private ModelViewManager m_ModelViewManager;

    private Vector3 previousPosition;

    public float zoomOutMin = 50;

    public float zoomOutMax = 500;

    private TouchZoom touchZoom;

    private Coroutine zoomCoroutine;

    private Vector2 primaryDelta, secondaryDelta;


    private void Awake()
    {
        touchZoom = new TouchZoom();
    }

    private void OnEnable()
    {
        touchZoom.Enable();
    }

    private void OnDisable()
    {
        touchZoom.Disable();
    }

    private void Start()
    {
        //Syntax used to subscribe to events and call functions, need to look into more to fully understand
        touchZoom.Touch.SecondaryTouchContact.started += _ => ZoomStart();
        touchZoom.Touch.SecondaryTouchContact.canceled += _ => ZoomEnd();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_ViewManager.m_ViewState == ViewManager.ViewState.Model)
        {
            if (Input.touchCount < 2)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    previousPosition = cam.ScreenToViewportPoint(Input.mousePosition);
                }

                if (Input.GetMouseButton(0))
                {
                    Vector3 direction = previousPosition - cam.ScreenToViewportPoint(Input.mousePosition);

                    cam.transform.position = target.position;
                    float xrot = cam.transform.eulerAngles.x;
                    
                    if (xrot < 0 || xrot > 180)
                    {
                        xrot = 0;
                    }

                    cam.transform.rotation = Quaternion.Euler(xrot, cam.transform.eulerAngles.y, cam.transform.eulerAngles.z);

                    cam.transform.Rotate(new Vector3(1, 0, 0), direction.y * 180);
                    cam.transform.Rotate(new Vector3(0, 1, 0), -direction.x * 180);
                    cam.transform.Translate(new Vector3(0, 0, -camDistanceFromModel));

                    m_ModelViewManager.m_CamZoomInnerBound.Rotate(new Vector3(1, 0, 0), direction.y * 180);
                    m_ModelViewManager.m_CamZoomInnerBound.Rotate(new Vector3(0, 1, 0), -direction.x * 180);
                    m_ModelViewManager.m_CamZoomInnerBound.Translate(new Vector3(0, 0, -m_InnerBoundDistanceFromModel));
                    m_ModelViewManager.m_CamZoomOuterBound.Rotate(new Vector3(1, 0, 0), direction.y * 180);
                    m_ModelViewManager.m_CamZoomOuterBound.Rotate(new Vector3(0, 1, 0), -direction.x * 180);
                    m_ModelViewManager.m_CamZoomOuterBound.Translate(new Vector3(0, 0, -m_OuterBoundDistanceFromModel));

                    
                    previousPosition = cam.ScreenToViewportPoint(Input.mousePosition);
                }

            }
        }
    }


    private void ZoomStart()
    {
        zoomCoroutine = StartCoroutine(ZoomDetection());
    }

    private void ZoomEnd()
    {
        StopCoroutine(zoomCoroutine);
    }

    IEnumerator ZoomDetection()
    {
        float previousDistance = 0f, distance;
        while (true)
        {
            Vector2 primaryDistance = touchZoom.Touch.PrimaryFingerPosition.ReadValue<Vector2>();
            Vector2 secondaryDistance = touchZoom.Touch.SecondaryFingerPosition.ReadValue<Vector2>();
            distance = Vector2.Distance(primaryDistance, secondaryDistance);

            primaryDelta = primaryDistance - primaryDelta;

            secondaryDelta = secondaryDistance - secondaryDelta;

            //Detection
            //Zoom out
            if (distance > previousDistance && Vector2.Dot(primaryDelta, secondaryDelta) < -.9f)
            {
                cam.transform.position = Vector3.Slerp(cam.transform.position, m_ModelViewManager.m_CamZoomInnerBound.position, Time.deltaTime * cameraSpeed);
            }
            //Zoom in
            else if (distance < previousDistance && Vector2.Dot(primaryDelta, secondaryDelta) > .9f)
            {
                cam.transform.position = Vector3.Slerp(cam.transform.position, m_ModelViewManager.m_CamZoomOuterBound.position, Time.deltaTime * -cameraSpeed);
            }
            previousDistance = distance;
            camDistanceFromModel = Mathf.Abs(cam.transform.position.z - target.transform.position.z);
            yield return null;
        }
    }

}
