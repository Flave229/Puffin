using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.MikeHackPad
{
	[RequireComponent( typeof( CharacterController ) )]
	[RequireComponent( typeof( CharacterSpriteRenderer ) )]
	public class PlayerPrototype2 : MonoBehaviour
	{

		[SerializeField]
		private float _runSpeed = 8.0f;
		[SerializeField]
		private float _walkSpeed = 2.0f;
		[SerializeField]
		private float _jumpPower = 10.0f;
		[SerializeField]
		private float _windResistance = 1.0f;
		[SerializeField]
		private float _floorSnapping = 1.5f;
		[SerializeField]
		private float _jumpingGravity = 8.0f;
		[SerializeField]
		private float _endOfJumpGravity = 12.0f;

		private Vector2 _velocity;
		private Vector2 _heading;
		private bool _jumpRequested = false;
		private bool _previouslyTouchingFloor = false;
		private float _currentGravity;
		
		private bool _isJumping;

		private CharacterController _controllerCollider;
		private CharacterSpriteRenderer _characterSpriteRenderer;

		void Start()
		{
			_controllerCollider = GetComponent<CharacterController>();
			_characterSpriteRenderer = GetComponent<CharacterSpriteRenderer>();
		}

		void Update()
		{

			if ( _controllerCollider.isGrounded )
			{
				_velocity = Vector3.zero;
				ConnectWithFloor();
			}
			else
			{
				// Earth gravity (9.807f)
				_velocity += ( Vector2.down * _currentGravity ) * Time.deltaTime;
				_velocity += ( -_velocity.normalized * _windResistance ) * Time.deltaTime;
			}

			UpdateMovement();

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
		}

		private void UpdateMovement()
		{
			UpdateGroundMovement();
			UpdateAirMovement();
		}

		private void UpdateGroundMovement()
		{
			_heading = Vector2.zero;
			_characterSpriteRenderer._currentSprite = ECharacterSprite.IS_STANDING;
			
			float leftStickX = Input.GetAxis( "GP_Xbox_L_JoystickHorizontal" );
			Debug.Log( leftStickX );
			leftStickX = ( ( leftStickX < 0.2f ) && ( leftStickX > -0.2f ) ) ? 0.0f : leftStickX;

			if ( leftStickX != 0 )
			{
				if ( Input.GetAxis( "GP_Xbox_LB" ) != 0 )
				{
					_characterSpriteRenderer._currentSprite = ECharacterSprite.IS_RUNNING;
					_heading.x += leftStickX * _runSpeed;
				}
				else
				{
					_characterSpriteRenderer._currentSprite = ECharacterSprite.IS_WALKING;
					_heading.x += leftStickX * _walkSpeed;
				}

				//_isSpriteXFlipped = _heading.x < 0;
				_characterSpriteRenderer._isXFlipped = _heading.x < 0;
			}
		}

		private void UpdateAirMovement()
		{
			if ( _controllerCollider.isGrounded )
			{
				_isJumping = false;
			}

			if ( Input.GetAxis( "GP_Xbox_AButton" ) != 0 && _controllerCollider.isGrounded )
			{
				_velocity += Vector2.up * _jumpPower;
				_jumpRequested = true;
				_isJumping = true;
			}

			if ( _velocity.y < 0.0f && _isJumping )
			{
				_currentGravity = _endOfJumpGravity;
			}
			else if ( _velocity.y > 0.0f && Input.GetAxis( "GP_Xbox_AButton" ) != 0 )
			{
				_currentGravity = _jumpingGravity;
			}
			else if ( !_controllerCollider.isGrounded )
			{
				_currentGravity = -Physics.gravity.y;
			}

			if ( !_controllerCollider.isGrounded )
			{
				if ( _velocity.y > 0.0f )
				{
					_characterSpriteRenderer._currentSprite = ECharacterSprite.IS_JUMPING;
				}
				else
				{
					_characterSpriteRenderer._currentSprite = ECharacterSprite.IS_FALLING;
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
	}
}