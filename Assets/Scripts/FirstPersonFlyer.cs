using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;



[RequireComponent(typeof(CharacterController))]
public class FirstPersonFlyer : MonoBehaviour
{
    [SerializeField]
    private bool m_IsWalking;
    [SerializeField]
    private float m_WalkSpeed;
    [SerializeField]
    private float m_RunSpeed;
    [SerializeField]
    [Range(0f, 1f)]
    private float m_RunstepLenghten;
    [SerializeField]
    private float m_JumpSpeed;
    [SerializeField]
    private MouseLook m_MouseLook;
    [SerializeField]
    private float m_StepInterval;


    private Camera m_Camera;
    private float m_YRotation;
    private Vector2 m_Input;
    private Vector3 m_MoveDir = Vector3.zero;
    private CharacterController m_CharacterController;
    private CollisionFlags m_CollisionFlags;
    private Vector3 m_OriginalCameraPosition;
    private float m_StepCycle;

    // Use this for initialization
    private void Start()
    {
        m_CharacterController = GetComponent<CharacterController>();
        m_Camera = Camera.main;
        m_OriginalCameraPosition = m_Camera.transform.localPosition;
        m_StepCycle = 0f;
        m_MouseLook.Init(transform, m_Camera.transform);
    }


    // Update is called once per frame
    private void Update()
    {
        RotateView();
    }


    // Run the movement update any time we update physics (as it's based on force application)
    private void FixedUpdate()
    {
        float speed;
        GetInput(out speed);
        // always move along the camera forward as it is the direction that it being aimed at
        Vector3 desiredMove = transform.forward * m_Input.y + transform.right * m_Input.x;

        // get a normal for the surface that is being touched to move along it
        //RaycastHit hitInfo;
        //Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
        //                   m_CharacterController.height / 2f);
        //desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

        m_MoveDir.x = desiredMove.x * speed;
        m_MoveDir.z = desiredMove.z * speed;

        m_CollisionFlags = m_CharacterController.Move(m_MoveDir * Time.fixedDeltaTime);
    }


    // Get input based on the horizontal and vertical axes
    private void GetInput(out float speed)
    {
        // Read input
        float x = CrossPlatformInputManager.GetAxis("Horizontal");
        float y = CrossPlatformInputManager.GetAxis("Vertical");

#if !MOBILE_INPUT
        // On standalone builds, walk/run speed is modified by a key press.
        // keep track of whether or not the character is walking or running
        m_IsWalking = !Input.GetKey(KeyCode.LeftShift);
#endif
        // set the desired speed to be walking or running
        speed = m_IsWalking ? m_WalkSpeed : m_RunSpeed;
        m_Input = new Vector2(x, y);

        // normalize input if it exceeds 1 in combined length:
        if (m_Input.sqrMagnitude > 1)
        {
            m_Input.Normalize();
        }
        return;
    }


    // Rotate the view using the mouse
    private void RotateView()
    {
        m_MouseLook.LookRotation(transform, m_Camera.transform);
    }

}
