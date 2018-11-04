using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof( KeyboardControlCapture ) )]
[RequireComponent( typeof( CharacterController ) )]
[RequireComponent( typeof( ThirdPersonCameraController ) )]
public class PlayerPrototypeController1 : MonoBehaviour
{
    [SerializeField]
    private float _RunSpeed = 15.0f;
    [SerializeField]
    private float _WalkSpeed = 5.0f;
    [SerializeField]
    private float _JumpPower = 10.0f;
    [SerializeField]
    private float _WindResistance = 1.0f;

    private Vector3 _Velocity;
    private Vector3 _Heading;
    private Vector3 _PreviousVelocity;
    private Vector3 _PreviousHeading;
    private bool _PreviouslyTouchingFloor;

    private KeyboardControlCapture _KeyboardController;
    private CharacterController _ControllerCollider;
    private ThirdPersonCameraController _CameraSystem;

    private Transform CameraPivot;


    public void Start()
    {
        _KeyboardController = GetComponent<KeyboardControlCapture>();
        _ControllerCollider = GetComponent<CharacterController>();
        _CameraSystem = GetComponent<ThirdPersonCameraController>();

        CameraPivot = _CameraSystem.GetCameraPivotTransform();
    }

    public void Update()
    {
        if ( _ControllerCollider.isGrounded )
        {
            _Velocity = Vector3.zero;
            ConnectWithFloor();
        }
        else
        {
            // Apply Earth gravity
            _Velocity += ( ( Vector3.up * -1.0f ) * 9.807f ) * Time.deltaTime;
            _Velocity += ( -_Velocity.normalized * _WindResistance ) * Time.deltaTime;
        }


        if ( _KeyboardController.IsControllerCaptured() )
        {
            if ( Input.GetKey( KeyCode.LeftShift ) )
            {
                _Heading = CameraPivot.transform.forward * Input.GetAxis( "Vertical" ) * _RunSpeed;
                _Heading += CameraPivot.transform.right * Input.GetAxis( "Horizontal" ) * _RunSpeed;
            }
            else
            {
                _Heading = CameraPivot.transform.forward * Input.GetAxis( "Vertical" ) * _WalkSpeed;
                _Heading += CameraPivot.transform.right * Input.GetAxis( "Horizontal" ) * _WalkSpeed;
            }

            if ( Input.GetKeyDown( KeyCode.Space ) )
            {
                _Velocity += Vector3.up * _JumpPower;
            }

#if UNITY_EDITOR
            Debug.DrawLine( transform.position, transform.position + _Velocity * 10, Color.blue );
            Debug.DrawLine( transform.position, transform.position + _Heading * 10, Color.red );
#endif
        }

        _ControllerCollider.Move( ( _Heading + _Velocity ) * Time.deltaTime );
    }

    private void ConnectWithFloor()
    {
        RaycastHit info;
        Ray ray = new Ray( transform.position, Vector3.up * -1.0f );
        if ( Physics.Raycast( ray, out info, ( _ControllerCollider.height * 0.5f ) + 0.5f ) )
        {
            transform.SetParent( info.transform );
            transform.localEulerAngles = new Vector3( 0.0f, transform.localEulerAngles.y, 0.0f );
        }
        else
        {
            transform.SetParent( null );
            transform.localEulerAngles = new Vector3( 0.0f, transform.localEulerAngles.y, 0.0f );
            transform.localScale = new Vector3( 1.0f, 1.0f, 1.0f );
        }
    }
}
