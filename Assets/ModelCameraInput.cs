using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Utilities.Tweenables.Primitives;

public class ModelCameraInput : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    [SerializeField]
    private Transform target;

    [SerializeField]
    private ViewManager m_ViewManager;

    [SerializeField]
    private ModelViewManager m_ModelViewManager;

    private Vector3 previousPosition;

    public float zoomOutMin = 50;

    public float zoomOutMax = 500;


    // Update is called once per frame
    void Update()
    {
        if (m_ViewManager.m_ViewState == ViewManager.ViewState.Model)
        {

            if (Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;

                float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

                float difference = currentMagnitude - prevMagnitude;

                Zoom(difference * 0.01f);
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    previousPosition = cam.ScreenToViewportPoint(Input.mousePosition);
                }

                if (Input.GetMouseButton(0))
                {
                    Vector3 direction = previousPosition - cam.ScreenToViewportPoint(Input.mousePosition);

                    cam.transform.position = target.position;
                    float xrot = Mathf.Clamp(cam.transform.eulerAngles.x, 10, 85);
                    cam.transform.rotation = Quaternion.Euler(xrot, cam.transform.eulerAngles.y, cam.transform.eulerAngles.z);

                    cam.transform.Rotate(new Vector3(1, 0, 0), direction.y * 180);
                    cam.transform.Rotate(new Vector3(0, 1, 0), -direction.x * 180);
                    cam.transform.Translate(new Vector3(0, 0, -10));
                    previousPosition = cam.ScreenToViewportPoint(Input.mousePosition);
                }

                //Zoom(Input.GetAxis("Mouse ScrollWheel"));
            }
        }
    }

    void Zoom(float increment)
    {
        cam.fieldOfView = Mathf.Clamp(cam.orthographicSize - increment, zoomOutMin, zoomOutMax);
    }

}
