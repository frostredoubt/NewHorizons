using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerCamera : NetworkBehaviour
{

    /// <summary>
    /// Sensitivities used for keyboard movement.
    /// </summary>
    [SerializeField]
    private Vector3 keyboardMovementSensitivity = new Vector3(20.0f, 0.0f, 20.0f);

    /// <summary>
    /// Sensitivities used for keyboard rotation.
    /// </summary>
    [SerializeField]
    private Vector3 keyboardRotationSensitivity = new Vector3(0.0f, 0.0f, 100.0f);

    /// <summary>
    /// Sensitivities used for mouse movement.
    /// </summary>
    [SerializeField]
    private Vector3 mouseMovementSensitivity = new Vector3(0.0f, 200.0f, 0.0f);

    /// <summary>
    /// Sensitivities used for mouse rotation.
    /// </summary>
    [SerializeField]
    private Vector3 mouseRotationSensitivity = new Vector3(100.0f, 100.0f, 0.0f);

    /// <summary>
    /// The distance to snap to when selecting an object.
    /// </summary>
    [SerializeField]
    private float objectSelectDistance = 15.0f;

    /// <summary>
    /// The speed to use when zooming in on a selected object.
    /// </summary>
    [SerializeField]
    private float objectSelectMoveSpeed = 5.0f;

    /// <summary>
    /// The speed to use when rotating towards a selected object.
    /// </summary>
    [SerializeField]
    private float objectSelectRotationSpeed = 5.0f;

    /// <summary>
    /// The player camera object.
    /// </summary>
    private Camera playerCamera;


    /// <summary>
    /// The state of object selection tracking that the camera is currently in.
    /// </summary>
    private enum ObjectSelectionTrackingState
    {
        /// <summary>
        /// The camera is not currently tracking towards an object.
        /// </summary>
        None,

        /// <summary>
        /// The camera is currently tracking the object.
        /// </summary>
        Tracking

    }

    // Various private input state variables
    Vector3 keyboardMovement, keyboardRotation, mouseMovement, mouseRotation;
    private bool mouseLeftPress, mouseMiddlePress, mouseRightPress;
    private bool mouseLeftHold, mouseMiddleHold, mouseRightHold;
    private bool mouseLeftRelease, mouseMiddleRelease, mouseRightRelease;
    private ObjectSelectionTrackingState objectSelectionTrackingState; // The current state of object selection tracking
    private Transform lastSelectedObject; // The last object that was selected


    /// <summary>
    /// The transform tag that is checked to determine whether or not an object is selectable.
    /// </summary>
    const string selectableTag = "Selectable";


    /// <summary>
    /// Perform initialization on the camera object.
    /// </summary>
    [ClientCallback]
    private void Start()
    {
        GameObject.Find("CustomNetworkMgr").GetComponent<NetworkManagerHUD>().showGUI = false;
        playerCamera = GetComponent<Camera>();
        playerCamera.enabled = isLocalPlayer;
        GetComponent<AudioListener>().enabled = isLocalPlayer;
        objectSelectionTrackingState = ObjectSelectionTrackingState.None;
        lastSelectedObject = null;

        if (isLocalPlayer)
        {
            Game.singleton.local_player = gameObject;
        }

        return;
    }


    /// <summary>
    /// Run an update once per game frame.
    /// </summary>
    [ClientCallback]
    private void Update()
    {
        if (!isLocalPlayer) // If we're not updating a local player, return
        {
            return;
        }

        GetInput();
        CheckForObjectSelect();
        UpdateCamera();

        // Fil: putting this here for now
        if (Input.GetKeyDown(KeyCode.Return))
        {
            CmdStartGame();
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            bool current = GameObject.Find("CustomNetworkMgr").GetComponent<NetworkManagerHUD>().showGUI;
            GameObject.Find("CustomNetworkMgr").GetComponent<NetworkManagerHUD>().showGUI = !current;
        }

        return;
    }

    [Command]
    private void CmdStartGame()
    {
        Game.singleton.StartGame();
    }


    /// <summary>
    /// Get input from various sources.
    /// </summary>
    private void GetInput()
    {
        if (objectSelectionTrackingState != ObjectSelectionTrackingState.None) // Ignore input while tracking
        {
            return;
        }

        // Mouse button press statuses
        mouseLeftPress = Input.GetMouseButtonDown(0);
        mouseMiddlePress = Input.GetMouseButtonDown(2);
        mouseRightPress = Input.GetMouseButtonDown(1);

        // Mouse button hold statuses
        mouseLeftHold = Input.GetMouseButton(0);
        mouseMiddleHold = Input.GetMouseButton(2);
        mouseRightHold = Input.GetMouseButton(1);

        // Mouse button release statuses
        mouseLeftRelease = Input.GetMouseButtonUp(0);
        mouseMiddleRelease = Input.GetMouseButtonUp(2);
        mouseRightRelease = Input.GetMouseButtonUp(1);

        // Movement and rotational inputs
        keyboardMovement = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        mouseMovement = new Vector3(0.0f, Input.GetAxis("Mouse ScrollWheel"), 0.0f);
        keyboardRotation = new Vector3(0.0f, 0.0f,
            (Input.GetKey(KeyCode.Q) ? 1.0f : 0.0f) + (Input.GetKey(KeyCode.E) ? -1.0f : 0.0f));
        mouseRotation = new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0.0f);

        return;
    }


    /// <summary>
    /// Check if the user has selected a selectable item and snap the camera to it.
    /// </summary>
    private void CheckForObjectSelect()
    {
        if (objectSelectionTrackingState != ObjectSelectionTrackingState.None) // Ignore selection while tracking
        {
            return;
        }

        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (SelectObject() && Physics.Raycast(ray, out hitInfo, playerCamera.farClipPlane))
        {
            lastSelectedObject = hitInfo.transform.FindChild(selectableTag);
            objectSelectionTrackingState = ObjectSelectionTrackingState.Tracking;
        }

        return;
    }


    /// <summary>
    /// Apply the movement determined from user input to the camera.
    /// </summary>
    private void UpdateCamera()
    {
        switch (objectSelectionTrackingState)
        {
            case ObjectSelectionTrackingState.None:
                MoveAndRotateCamera();
                break;

            case ObjectSelectionTrackingState.Tracking:
                if (lastSelectedObject == null) // If somehow we aren't tracking anything, abort tracking
                {
                    goto default;
                }
                bool finishedRotating = RotateTowardsSelectedObject();
                bool finishedMoving = MoveTowardsSelectedObject();

                if (finishedRotating && finishedMoving)
                {
                    objectSelectionTrackingState = ObjectSelectionTrackingState.None;
                }
                break;

            default: // Something crazy happened, abort any tracking
                objectSelectionTrackingState = ObjectSelectionTrackingState.None;
                break;
        }

        return;
    }


    /// <summary>
    /// Apply movement and rotation directly to the camera based on the user's input.
    /// </summary>
    private void MoveAndRotateCamera()
    {
        // Translate the camera's movement
        Vector3 cameraTranslation = keyboardMovement;
        cameraTranslation.Scale(keyboardMovementSensitivity);
        Vector3 cameraMouseTranslation = mouseMovement;
        cameraMouseTranslation.Scale(mouseMovementSensitivity);
        cameraTranslation += cameraMouseTranslation;
        transform.Translate(Time.deltaTime * cameraTranslation);

        // Rotate the camera's movement
        Vector3 cameraRotation = keyboardRotation;
        cameraRotation.Scale(keyboardRotationSensitivity);
        if (TiltPanCamera())
        {
            Vector3 cameraMouseRotation = mouseRotation;
            cameraMouseRotation.Scale(mouseRotationSensitivity);
            cameraRotation += cameraMouseRotation;
        }
        transform.Rotate(Time.deltaTime * cameraRotation);

        return;
    }

    /// <summary>
    /// Rotate the camera towards a selected object.
    /// </summary>
    /// <returns>A boolean representing whether or not the rotation has completed.</returns>
    private bool RotateTowardsSelectedObject()
    {
        // Rotate our object towards the selected object
        Quaternion lastCameraRotation = transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation(lastSelectedObject.position - transform.position),
            objectSelectRotationSpeed * Time.deltaTime);

        return (transform.rotation == lastCameraRotation);
    }


    /// <summary>
    /// Move the camera along a ray towards the selected object.
    /// </summary>
    /// <returns>A boolean representing whether or not the movement has completed.</returns>
    private bool MoveTowardsSelectedObject()
    {
        // Set the upper bound for stopping
        float objectSelectDistanceLower = objectSelectDistance - 1.0f;
        if (objectSelectDistanceLower < 0.0f)
        {
            objectSelectDistanceLower = 0.0f;
        }

        // Set the lower bound for stopping
        float objectSelectDistanceUpper = objectSelectDistance + 1.0f;

        Vector3 objectDistance = (lastSelectedObject.transform.position - transform.position);
        float objectMagnitude = objectDistance.magnitude;
        if (objectMagnitude < objectSelectDistanceLower)
        {
            objectDistance *= -1.0f;
        }
        else if (objectMagnitude <= objectSelectDistanceUpper)
        {
            return true;
        }

        Ray ray = new Ray(transform.position, objectDistance.normalized);
        float moveModifier = objectMagnitude / 2.0f;
        transform.Translate(ray.direction * objectSelectMoveSpeed * moveModifier * Time.deltaTime, Space.World);

        float newObjectMagnitude = (lastSelectedObject.transform.position - transform.position).magnitude;
        return (newObjectMagnitude >= objectSelectDistanceLower && newObjectMagnitude <= objectSelectDistanceUpper);
    }


    /// <summary>
    /// Check if the user's input denotes a desire to select a game object.
    /// </summary>
    /// <returns>A boolean representing whether or not the user used the game object selection input.</returns>
    private bool SelectObject()
    {
        return mouseLeftPress;
    }


    /// <summary>
    /// Check if the user's input denotes a desire to tilt or pan the camera.
    /// </summary>
    /// <returns>A boolean representing whether or not the user used the camera rotation input.</returns>
    private bool TiltPanCamera()
    {
        return mouseRightHold;
    }

}