using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.MikeHackPad
{
	[RequireComponent( typeof( CharacterController ) )]
	public class PlayerPrototype2 : MonoBehaviour
	{
		[SerializeField]
		private float _runSpeed;
		[SerializeField]
		private float _walkSpeed;
		[SerializeField]
		private float _floorSnapping = 1.5f;

		private Vector2 _velocity;
		private Vector2 _heading;
		private bool _jumpRequested = false;
		private bool _previouslyTouchingFloor = false;

		private CharacterController _controllerCollider;

		// Use this for initialization
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{

		}

		private void LateUpdate()
		{
			//float length = ( _controllerCollider.height * 0.5f ) + _floorSnapping;
			//if ( !_controllerCollider.isGrounded &&
			//	_previouslyTouchingFloor &&
			//	!_jumpRequested )
			//{
			//	RaycastHit info;
			//	Ray ray = new Ray( transform.position, Vector3.down );
			//	if ( Physics.Raycast( ray, out info, length ) )
			//	{
			//		// Stick the controller to the floor
			//		_controllerCollider.Move( Vector3.down );
			//	}
			//}
		}

		private void UpdateGroundMovement()
		{
			if ( Input.GetAxis( "GP_Xbox_AButton" ) != 0 )
			{

			}

			if ( Input.GetAxis( "GP_Xbox_LB" ) != 0 )
			{
				if ( Input.GetAxisRaw( "GP_Xbox_L_JoystickHorizontal" ) != 0 )
				{

				}
			}
			else
			{
				if ( Input.GetAxis( "GP_Xbox_L_JoystickHorizontal" ) != 0 )
				{

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