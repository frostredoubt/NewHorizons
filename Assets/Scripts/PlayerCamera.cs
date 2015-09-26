using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerCamera : NetworkBehaviour
{
    /// <summary>
    /// The sensitivity when pressing A-D and left-right on the keyboard.
    /// </summary>
    [SerializeField]
    private float keyboardXSensitivity = 0.25f;

    /// <summary>
    /// The sensitivity when pressing S-W and down-up on the keyboard.
    /// </summary>
    [SerializeField]
    private float keyboardYSensitivity = 0.25f;

    /// <summary>
    /// The sensitivity when moving the mouse left-right.
    /// </summary>
    [SerializeField]
    private float mouseXSensitivity = 0.25f;

    /// <summary>
    /// The sensitivity when moving the mouse down-up.
    /// </summary>
    [SerializeField]
    private float mouseYSensitivity = 0.25f;

    /// <summary>
    /// The sensitivity when using the mouse scrollwheel.
    /// </summary>
    [SerializeField]
    private float mouseScrollSensitivity = 5.0f;

    /// <summary>
    /// The distance to snap to when selecting an object.
    /// </summary>
    [SerializeField]
    private float objectSelectSnapDistance = 5.0f;

    /// <summary>
    /// The player camera object.
    /// </summary>
    public Camera playerCamera;

    /// <summary>
    /// The transform tag that is checked to determine whether or not an object is selectable.
    /// </summary>
    const string selectableTag = "Selectable";

    // Use this for initialization
    [ClientCallback]
    private void Start()
    {
        if (!isLocalPlayer)
        {
            playerCamera.enabled = false;
            Debug.Log("not using camera");
        }
        else
        { //if I am the owner of this prefab
            playerCamera.enabled = true;
            Debug.Log("using camera");
        }

        return;
	}
	
	// Update is called once per frame
    [ClientCallback]
	private void Update()
    {
        if (!isLocalPlayer)
            return;

        // Mouse directional inputs (mouse movement and scrollwheel)
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        float mouseScrollWheel = Input.GetAxis("Mouse ScrollWheel");

        // Keyboard directional inputs (WASD and arrow keys)
        float keyboardX = Input.GetAxis("Horizontal");
        float keyboardY = Input.GetAxis("Vertical");

        // Mouse button click statuses
        bool mouseLeftClick = Input.GetMouseButtonDown(0);
        bool mouseMiddleClick = Input.GetMouseButtonDown(2);
        bool mouseRightClick = Input.GetMouseButtonDown(1);

        // Mouse button hold statuses
        bool mouseLeftHold = Input.GetMouseButton(0);
        bool mouseMiddleHold = Input.GetMouseButton(2);
        bool mouseRightHold = Input.GetMouseButton(1);

        // Mouse button release statuses
        bool mouseLeftRelease = Input.GetMouseButtonUp(0);
        bool mouseMiddleRelease = Input.GetMouseButtonUp(2);
        bool mouseRightRelease = Input.GetMouseButtonUp(1);

        // Set up the definitive input commands for each direction (keyboard taking preference over mouse)
        float definitiveX = keyboardX * keyboardXSensitivity;
        float definitiveY = keyboardY * keyboardYSensitivity;
        if (mouseRightHold)
        {
            if (definitiveX == 0.0f)
            {
                definitiveX = mouseX * mouseXSensitivity;
            }
            if (definitiveY == 0.0f)
            {
                definitiveY = mouseY * mouseYSensitivity;
            }
        }
        float definitiveScroll = mouseScrollWheel * mouseScrollSensitivity;

        // Snap the camera to selectable gameworld objects
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (mouseLeftClick && Physics.Raycast(ray, out hitInfo, playerCamera.farClipPlane))
        {
            Transform selectable = hitInfo.transform.FindChild(selectableTag);
            if (selectable != null)
            {
                transform.position = selectable.position - new Vector3(-objectSelectSnapDistance, 0.0f, 0.0f);
            }
        }

        // After snapping, apply any necessary movement to the camera
        transform.Translate(new Vector3(definitiveX, definitiveY, definitiveScroll));

        return;
	}

}
