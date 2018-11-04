using UnityEngine;

namespace Assets.Scripts.MikeHackPad
{
	[RequireComponent( typeof( KeyboardControlCapture ) )]
	[RequireComponent( typeof( CharacterController ) )]
	[RequireComponent( typeof( ThirdPersonCameraController ) )]
	[RequireComponent( typeof( TargetingSystem ) )]
	public class PlayerPrototypeController1 : MonoBehaviour
	{
		[SerializeField]
		private float _runSpeed = 15.0f;
		[SerializeField]
		private float _walkSpeed = 5.0f;
		[SerializeField]
		private float _jumpPower = 10.0f;
		[SerializeField]
		private float _grapplePower = 10.0f;
		[SerializeField]
		private float _windResistance = 1.0f;
		[SerializeField]
		private float _floorSnapping = 1.5f;

		private Vector3 _velocity;
		private Vector3 _heading;
		private Vector3 _grappledPoint;
		private bool _jumpRequested = false;
		private bool _previouslyTouchingFloor = false;
		private bool _isGrappling;

		private KeyboardControlCapture _keyboardController;
		private CharacterController _controllerCollider;
		private ThirdPersonCameraController _cameraSystem;
		private TargetingSystem _targetingSystem;

		private Transform _cameraPivot;

		public void Start()
		{
			_keyboardController = GetComponent<KeyboardControlCapture>();
			_controllerCollider = GetComponent<CharacterController>();
			_cameraSystem = GetComponent<ThirdPersonCameraController>();
			_targetingSystem = GetComponent<TargetingSystem>();

			_cameraPivot = _cameraSystem.GetCameraPivotTransform();
		}

		public void Update()
		{
			if ( _controllerCollider.isGrounded )
			{
				_velocity = Vector3.zero;
				ConnectWithFloor();
			}
			else
			{
				// Apply Earth gravity
				_velocity += ( ( Vector3.up * -1.0f ) * 9.807f ) * Time.deltaTime;
				_velocity += ( -_velocity.normalized * _windResistance ) * Time.deltaTime;
			}


			if ( _keyboardController.IsControllerCaptured() )
			{

				UpdateGrappleMovement();

				if ( !_isGrappling )
				{
					UpdateGroundMovement();
				}

#if UNITY_EDITOR
				Debug.DrawLine( transform.position, transform.position + _velocity * 10, Color.blue );
				Debug.DrawLine( transform.position, transform.position + _heading * 10, Color.red );
#endif
			}

			_controllerCollider.Move( ( _heading + _velocity ) * Time.deltaTime );
		}

		private void LateUpdate()
		{
			float length = ( _controllerCollider.height * 0.5f ) + _floorSnapping;
			if ( !_controllerCollider.isGrounded &&
				_previouslyTouchingFloor &&
				!_jumpRequested &&
				!_isGrappling )
			{
				RaycastHit info;
				Ray ray = new Ray( transform.position, Vector3.down );
				if ( Physics.Raycast( ray, out info, length ) )
				{
					// Stick the controller to the floor
					_controllerCollider.Move( Vector3.down );
				}
			}

#if UNITY_EDITOR
			Debug.DrawLine( transform.position, transform.position + Vector3.down * length, Color.green );
#endif
			_previouslyTouchingFloor = _controllerCollider.isGrounded;
			_jumpRequested = false;
		}

		private void UpdateGrappleMovement()
		{
			if ( Input.GetMouseButton( 0 ) && _targetingSystem.GetTargetGrappleable() )
			{
				if ( !_isGrappling )
				{
					_isGrappling = true;
					_grappledPoint = _targetingSystem.GetTargetedPoint();
				}

				Vector3 headedpoint = _grappledPoint + Vector3.up * ( ( _grappledPoint - transform.position ).magnitude * 0.25f );
				_velocity += ( ( headedpoint - transform.position ).normalized * _grapplePower );

				Debug.DrawLine( _grappledPoint, headedpoint, Color.cyan );
				Debug.DrawLine( transform.position, headedpoint, Color.yellow );
			}
			else if ( _isGrappling )
			{
				_isGrappling = false;
			}
		}

		private void UpdateGroundMovement()
		{
			if ( Input.GetKey( KeyCode.LeftShift ) )
			{
				_heading = _cameraPivot.transform.forward * Input.GetAxis( "Vertical" ) * _runSpeed;
				_heading += _cameraPivot.transform.right * Input.GetAxis( "Horizontal" ) * _runSpeed;
			}
			else
			{
				_heading = _cameraPivot.transform.forward * Input.GetAxis( "Vertical" ) * _walkSpeed;
				_heading += _cameraPivot.transform.right * Input.GetAxis( "Horizontal" ) * _walkSpeed;
			}

			if ( Input.GetKeyDown( KeyCode.Space ) )
			{
				_velocity += Vector3.up * _jumpPower;
				_jumpRequested = true;
			}
		}

		private void ConnectWithFloor()
		{
			RaycastHit info;
			Ray ray = new Ray( transform.position, Vector3.down );
			if ( Physics.Raycast( ray, out info, ( _controllerCollider.height * 0.5f ) + 0.5f ) )
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
}
