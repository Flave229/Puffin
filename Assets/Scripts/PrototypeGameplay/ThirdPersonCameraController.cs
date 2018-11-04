using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof( KeyboardControlCapture ) )]
public class ThirdPersonCameraController : MonoBehaviour
{
    [SerializeField]
    private Transform _CameraPivot;
    [SerializeField]
    private Transform _CameraPitch;
    [SerializeField]
    private Camera _Camera;
    private KeyboardControlCapture _KeyboardController;

    private float _OverheadClampAngle = 90.0f;
    private float _UnderCharacterClampAngle = 270.0f;

    public float _MouseSensitivity;


    public void Start()
    {
        _KeyboardController = GetComponent<KeyboardControlCapture>( );
    }


    public void Update()
    {
        if ( !_KeyboardController.IsControllerCaptured( ) )
        {
            return;
        }

        Vector3 cameraPitch = _CameraPitch.transform.localEulerAngles;
        Vector3 cameraPivot = _CameraPivot.transform.localEulerAngles;

        cameraPitch.x += Input.GetAxis( "Mouse Y" ) * _MouseSensitivity * -1.0f * Time.deltaTime;
        cameraPivot.y += Input.GetAxis( "Mouse X" ) * _MouseSensitivity * Time.deltaTime;
        
        if ( cameraPitch.x > _OverheadClampAngle && cameraPitch.x < _UnderCharacterClampAngle )
        {
            if ( cameraPitch.x < 180.0f )
            {
                cameraPitch.x = _OverheadClampAngle;
            }
            else
            {
                cameraPitch.x = _UnderCharacterClampAngle;
            }
        }

        _CameraPitch.transform.localEulerAngles = cameraPitch;
        _CameraPivot.transform.localEulerAngles = cameraPivot;
    }

    public Transform GetCameraPivotTransform()
    {
        return _CameraPivot;
    }

    public Transform GetCameraPitchTransform()
    {
        return _CameraPitch;
    }

    public Vector3 GetCameraFlatForward()
    {
        return _CameraPivot.forward;
    }


    public void EnableCamera( bool isEnabled )
    {
        _Camera.enabled = isEnabled;
        _Camera.GetComponent<AudioListener>( ).enabled = isEnabled;
    }
}
