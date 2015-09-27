﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerCamera : NetworkBehaviour
{
    /// <summary>
    /// Sensitivities used for keyboard movement.
    /// </summary>
    [SerializeField]
    private Vector3 keyboardMovementSensitivity = new Vector3(0.25f, 0.0f, 0.25f);

    /// <summary>
    /// Sensitivities used for keyboard rotation.
    /// </summary>
    [SerializeField]
    private Vector3 keyboardRotationSensitivity = new Vector3(0.0f, 0.0f, 2.5f);

    /// <summary>
    /// Sensitivities used for mouse rotation.
    /// </summary>
    [SerializeField]
    private Vector3 mouseRotationSensitivity = new Vector3(1.0f, 1.0f, 0.0f);

    /// <summary>
    /// The distance to snap to when selecting an object.
    /// </summary>
    [SerializeField]
    private float objectSelectSnapDistance = 10.0f;

    /// <summary>
    /// The player camera object.
    /// </summary>
    [SerializeField]
    private Camera playerCamera;

    // Various private input state variables
    Vector3 keyboardMovement, keyboardRotation, mouseRotation;
    private bool mouseLeftPress, mouseMiddlePress, mouseRightPress;
    private bool mouseLeftHold, mouseMiddleHold, mouseRightHold;
    private bool mouseLeftRelease, mouseMiddleRelease, mouseRightRelease;

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
        playerCamera = GetComponent<Camera>();
        playerCamera.enabled = isLocalPlayer;
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
        ApplyMovement();

        return;
	}


    /// <summary>
    /// Get input from various sources.
    /// </summary>
    private void GetInput()
    {
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
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (SelectObject() && Physics.Raycast(ray, out hitInfo, playerCamera.farClipPlane))
        {
            Transform selectable = hitInfo.transform.FindChild(selectableTag);
            if (selectable != null)
            {
                transform.position = selectable.position - new Vector3(-objectSelectSnapDistance, 0.0f, 0.0f);
            }
        }

        return;
    }


    /// <summary>
    /// Apply the movement determined from user input to the camera.
    /// </summary>
    /// <param name="movement">The movement vector to apply.</param>
    private void ApplyMovement()
    {
        // Translate the camera's movement
        Vector3 cameraTranslation = keyboardMovement;
        cameraTranslation.Scale(keyboardMovementSensitivity);
        transform.Translate(cameraTranslation);

        // Rotate the camera's movement
        Vector3 cameraRotation = keyboardRotation;
        cameraRotation.Scale(keyboardRotationSensitivity);
        if (TiltPanCamera())
        {
            Vector3 cameraMouseRotation = mouseRotation;
            cameraMouseRotation.Scale(mouseRotationSensitivity);
            cameraRotation += cameraMouseRotation;
        }
        transform.Rotate(cameraRotation);

        return;
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
        return mouseLeftHold;
    }

}
