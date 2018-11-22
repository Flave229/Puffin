using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.MikeHackPad
{
	[RequireComponent( typeof( CharacterController ) )]
	public class PlayerPrototype2 : MonoBehaviour
	{
		[SerializeField]
		private SpriteRenderer _standingSprite;
		[SerializeField]
		private SpriteRenderer _walkingSprite;
		[SerializeField]
		private SpriteRenderer _runningSprite;
		[SerializeField]
		private SpriteRenderer _jumpingSprite;
		[SerializeField]
		private SpriteRenderer _fallingSprite;

		[SerializeField]
		private float _runSpeed = 2.0f;
		[SerializeField]
		private float _walkSpeed = 10.0f;
		[SerializeField]
		private float _jumpPower = 10.0f;
		[SerializeField]
		private float _windResistance = 1.0f;
		[SerializeField]
		private float _floorSnapping = 1.5f;

		private Vector2 _velocity;
		private Vector2 _heading;
		private bool _jumpRequested = false;
		private bool _previouslyTouchingFloor = false;

		private bool _isStanding;
		private bool _isWalking;
		private bool _isRunning;

		private CharacterController _controllerCollider;

		// Use this for initialization
		void Start()
		{
			_controllerCollider = GetComponent<CharacterController>();
		}

		// Update is called once per frame
		void Update()
		{
			_isStanding = false;
			_isWalking = false;
			_isRunning = false;

			_standingSprite.enabled = false;
			_walkingSprite.enabled = false;
			_runningSprite.enabled = false;
			_jumpingSprite.enabled = false;
			_fallingSprite.enabled = false;

			if ( _controllerCollider.isGrounded )
			{
				_velocity = Vector3.zero;
				ConnectWithFloor();
			}
			else
			{
				// Apply Earth gravity
				_velocity += ( ( Vector2.up * -1.0f ) * 9.807f ) * Time.deltaTime;
				_velocity += ( -_velocity.normalized * _windResistance ) * Time.deltaTime;
			}

			UpdateGroundMovement();

			_controllerCollider.Move( ( _heading + _velocity ) * Time.deltaTime );
		}

		private void LateUpdate()
		{
			float length = ( _controllerCollider.height * 0.5f ) + _floorSnapping;
			if ( !_controllerCollider.isGrounded &&
				_previouslyTouchingFloor &&
				!_jumpRequested )
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

			_isStanding = _controllerCollider.isGrounded;

			UpdateSprites();
		}

		private void UpdateGroundMovement()
		{
			_heading = Vector2.zero;

			if ( Input.GetAxis( "GP_Xbox_AButton" ) != 0 && _controllerCollider.isGrounded )
			{
				_velocity += Vector2.up * _jumpPower;
				_jumpRequested = true;
			}

			float leftStickX = Input.GetAxis( "GP_Xbox_L_JoystickHorizontal" );
			Debug.Log( leftStickX );
			leftStickX = ( ( leftStickX < 0.0175f ) && ( leftStickX > -0.0175f ) ) ? 0.0f : leftStickX;

			if ( leftStickX != 0 )
			{
				if ( Input.GetAxis( "GP_Xbox_LB" ) != 0 )
				{
					_heading.x += leftStickX * _runSpeed;
					_isRunning = true;
				}
				else
				{
					_heading.x += leftStickX * _walkSpeed;
					_isWalking = true;
				}
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

		private void UpdateSprites()
		{
			bool flipped = _heading.x < 0;
			if ( !_isStanding )
			{
				if ( _velocity.y > 0 )
				{
					_jumpingSprite.enabled = true;
					_jumpingSprite.flipX = flipped;
				}
				else
				{
					_fallingSprite.enabled = true;
					_fallingSprite.flipX = flipped;
				}
			}
			else
			{
				if ( _isWalking )
				{
					_walkingSprite.enabled = true;
					_walkingSprite.flipX = flipped;
					return;
				}
				if ( _isRunning )
				{
					_runningSprite.enabled = true;
					_runningSprite.flipX = flipped;
					return;
				}

				_standingSprite.enabled = true;
				_standingSprite.flipX = flipped;
			}
		}
	}
}