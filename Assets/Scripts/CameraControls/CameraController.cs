using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private Transform cameraTransform; // May switch to Cinemachine if i have time - Switch to cinemachine and regret it...oh well its cute
    public Transform followTarget;

    [Header("Camera HandleFindPath")]
    [SerializeField] private float shiftSpeed;
    [SerializeField] private float Normalspeed;
    float currentSpeed;
    [SerializeField] private float movTime;

    [Header("Camera Rotation")]
    [SerializeField] private float rotationAmount;
    [SerializeField] private bool invertedControls;

    [Header("Camera Zoom")]
    [SerializeField] private Vector3 zoomAmount;
    [SerializeField] private float zoomSpeed;

    [SerializeField] private float fieldOfViewMin;
    [SerializeField] private float fieldOfViewMax;

    [Header("Camera Controls - Mouse")]
    //Mouse dragging
    Vector3 dragStartPosition;
    Vector3 dragCurrentPosition;
    [SerializeField] private Vector3 zoomAmountMouse;

    //Mouse rotation
    [SerializeField] private Vector3 rotationStartPosition;
    [SerializeField] private Vector3 rotationCurrentPosition;

    bool isDragging = false;

    Vector3 newPosition;
    Quaternion newrotation;
    float fieldOfView;

    //
    RaycastHit m_HitInfo = new RaycastHit();
    private HexTile target;

    private void Awake()
    {
        instance = this;
        cameraTransform = Camera.main.transform;
    }

    void Start()
    {
        newPosition = transform.position;
        newrotation = transform.rotation;
    }

    void Update()
    {
        // TO DO
        //Will come back eventually to fix this ... not working - Add new camera for this to work and its too much effort
        if (followTarget != null)
        {
            transform.position = followTarget.position;
        }
        else
        {
            CameraControlMouse();
            CameraControlsKeyBoard();
        }

        //TO DO
        //Change this to all wasd input
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            followTarget = null;
        }

        //Track Mouse
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray,out m_HitInfo) && m_HitInfo.collider.gameObject.layer == 6)
        {
            Transform objectHit = m_HitInfo.transform;

            if(m_HitInfo.collider.gameObject.TryGetComponent<HexTile>(out target))
            {
                target.OnHighlightTile();
            }
        }
    }

    // I would do it with the new system but i can't be bothered to implement for the camera
    // Please dont judge 

    void CameraControlsKeyBoard()
    {
        HandleMovementInput();
        HandleRotationInput();
        HandleZoomControls();
    }
    
    void CameraControlMouse()
    {
        HandleMouseInput();
        HandleMouseRotation();
    }

    void HandleMovementInput()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = shiftSpeed;
        }
        else
        {
            currentSpeed = Normalspeed;
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            newPosition += (transform.forward * currentSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            newPosition += (transform.forward * -currentSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            newPosition += (transform.right * -currentSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            newPosition += (transform.right * currentSpeed * Time.deltaTime);
        }

        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movTime);
    }

    void HandleRotationInput()
    {
        if (invertedControls)
        {
            if (Input.GetKey(KeyCode.Q))
            {
                newrotation *= Quaternion.Euler(Vector3.up * rotationAmount);
            }
            if (Input.GetKey(KeyCode.E))
            {
                newrotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.Q))
            {
                newrotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
            }
            if (Input.GetKey(KeyCode.E))
            {
                newrotation *= Quaternion.Euler(Vector3.up * rotationAmount);
            }
        }


        transform.rotation = Quaternion.Lerp(transform.rotation, newrotation, Time.deltaTime * movTime);
    }

    void HandleZoomControls()
    {
        #region Old Zoom Controls
        ////Keyboard
        //if(Input.GetKey(KeyCode.R))
        //{
        //    newZoom += zoomAmount;
        //}
        //if(Input.GetKey(KeyCode.F))
        //{
        //    newZoom += -zoomAmount;
        //}

        //cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * zoomSpeed);

#endregion

        float fieldOfViewIncreaseAmount = 2f;

        if (invertedControls)
        {


            if (Input.mouseScrollDelta.y > 0)
            {
                fieldOfView += fieldOfViewIncreaseAmount;
            }
            if (Input.mouseScrollDelta.y < 0)
            {
                fieldOfView -= fieldOfViewIncreaseAmount;
            }
        }
        else
        {
            if (Input.mouseScrollDelta.y < 0)
            {
                fieldOfView += fieldOfViewIncreaseAmount;
            }
            if (Input.mouseScrollDelta.y > 0)
            {
                fieldOfView -= fieldOfViewIncreaseAmount;
            }
        }

        fieldOfView = Mathf.Clamp(fieldOfView, fieldOfViewMin, fieldOfViewMax);
        
        virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(virtualCamera.m_Lens.FieldOfView, fieldOfView, Time.deltaTime * zoomSpeed); 

    }

    //TO DO
    // CHANGE THE MOUSE INPUT ON Y TO STAY THE SAME !!!!!
    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            // Raycast from the camera
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.layer == 6)
            {
                isDragging = true;
                dragStartPosition = hit.point;
            }
        }

        if (Input.GetMouseButton(1) && isDragging)
        {
            // Raycast from the camera
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.layer == 6)
            {
                dragCurrentPosition = hit.point;
                newPosition = transform.position + dragStartPosition - dragCurrentPosition;

                // Smoothly move the object to the new position
                transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movTime);
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            // Stop dragging
            isDragging = false;
        }
    }

    void HandleMouseRotation()
    {
        if (Input.GetMouseButtonDown(2))
        {
            rotationStartPosition = Input.mousePosition;
        }

        if(Input.GetMouseButton(2))
        {
            rotationCurrentPosition = Input.mousePosition;

            Vector3 horizontalDifference = rotationStartPosition - rotationCurrentPosition;
            Vector3 verticalDifference = rotationCurrentPosition - rotationStartPosition;


            rotationStartPosition = rotationCurrentPosition;

            // Rotate horizontally around the up axis.
            Quaternion horizontalRotation = Quaternion.Euler(Vector3.up * (-horizontalDifference.x / 5f));

            // Rotate vertically around the right axis.
            Quaternion verticalRotation = Quaternion.Euler(Vector3.right * (-verticalDifference.y / 5f));

            newrotation *= horizontalRotation;

            //// Apply vertical rotation and clamp it to a desired vertical range (e.g., -45 to 45 degrees).
            //newrotation *= verticalRotation;
            //newrotation.eulerAngles = new Vector3(Mathf.Clamp(newrotation.eulerAngles.x, 0, 60), newrotation.eulerAngles.z, newrotation.eulerAngles.z);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, newrotation, Time.deltaTime * movTime);
    }
}
