// Only added inputs and outputs necessary for testing
// Rumble/vibration output yet to be added
// Analogue stick click yet to be added
// Xbox home button yet to be added
// Just for testing

using UnityEngine;

public class Xbox360_Input_Tester : MonoBehaviour
{
	public Transform _leftStick;
	public Transform _dPad;
	public Transform _rightStick;

	public Transform _back;
	public Transform _start;

	public Transform _buttonA;
	public Transform _buttonB;
	public Transform _buttonX;
	public Transform _buttonY;

	public Transform _rB;
	public Transform _rT;
	public Transform _lB;
	public Transform _lT;

	float _angle = 50.0f;
	Vector3 _activeSpin = new Vector3( 0.0f, 0.0f, 50.0f );
	Vector3 _leftStickStartPos;
	Vector3 _dPadStartPos;
	Vector3 _rightStickStartPos;

	private void Start()
	{
		_leftStickStartPos = _leftStick.position;
		_dPadStartPos = _dPad.position;
		_rightStickStartPos = _rightStick.position;
	}


	void Update()
	{
		// Left stick
		//----------------------------------------------------------

		if ( Input.GetAxis( "GP_Xbox_L_JoystickHorizontal" ) != 0 ||
			Input.GetAxis( "GP_Xbox_L_JoystickVertical" ) != 0 )
		{
			_leftStick.position = _leftStickStartPos + 
				( new Vector3( Input.GetAxis( "GP_Xbox_L_JoystickHorizontal" ) * 2.5f,
				Input.GetAxis( "GP_Xbox_L_JoystickVertical" ) * 2.5f,
				0.0f ) );
		}
		else
		{
			_leftStick.position = _leftStickStartPos;
		}

		// DPad
		//----------------------------------------------------------

		if ( Input.GetAxis( "GP_Xbox_DPadHorizontal" ) != 0 ||
			Input.GetAxis( "GP_Xbox_DPadVertical" ) != 0 )
		{
			_dPad.position = _dPadStartPos +
				( new Vector3( Input.GetAxis( "GP_Xbox_DPadHorizontal" ) * 2.5f,
				Input.GetAxis( "GP_Xbox_DPadVertical" ) * 2.5f,
				0.0f ) );
		}
		else
		{
			_dPad.position = _dPadStartPos;
		}

		// Right stick
		//----------------------------------------------------------

		if ( Input.GetAxis( "GP_Xbox_R_JoystickHorizontal" ) != 0 ||
			Input.GetAxis( "GP_Xbox_R_JoystickVertical" ) != 0 )
		{
			_rightStick.position = _rightStickStartPos +
				( new Vector3( Input.GetAxis( "GP_Xbox_R_JoystickHorizontal" ) * 2.5f,
				Input.GetAxis( "GP_Xbox_R_JoystickVertical" ) * 2.5f,
				0.0f ) );
		}
		else
		{
			_rightStick.position = _rightStickStartPos;
		}

		// Back and Start
		//----------------------------------------------------------

		_back.localEulerAngles = Input.GetAxis( "GP_Xbox_Back" ) != 0 ?
			new Vector3( 0.0f, 0.0f, 0.0f + ( Input.GetAxis( "GP_Xbox_Back" ) * _angle ) ) :
			Vector3.zero;

		_start.localEulerAngles = Input.GetAxis( "GP_Xbox_Start" ) != 0 ?
			new Vector3( 0.0f, 0.0f, 0.0f + ( Input.GetAxis( "GP_Xbox_Start" ) * _angle ) ) :
			Vector3.zero;

		// Action buttons
		//----------------------------------------------------------

		_buttonA.localEulerAngles = Input.GetAxis( "GP_Xbox_AButton" ) != 0 ?
			_activeSpin :
			Vector3.zero;

		_buttonB.localEulerAngles = Input.GetAxis( "GP_Xbox_BButton" ) != 0 ?
			_activeSpin :
			Vector3.zero;

		_buttonX.localEulerAngles = Input.GetAxis( "GP_Xbox_XButton" ) != 0 ?
			_activeSpin :
			Vector3.zero;

		_buttonY.localEulerAngles = Input.GetAxis( "GP_Xbox_YButton" ) != 0 ?
			_activeSpin :
			Vector3.zero;

		// Triggers and bumpers
		//----------------------------------------------------------

		_rB.localEulerAngles = Input.GetAxis( "GP_Xbox_RB" ) != 0 ?
			_activeSpin :
			Vector3.zero;

		_rT.localEulerAngles = Input.GetAxis( "GP_Xbox_RT" ) != 0 ?
			new Vector3( 0.0f, 0.0f, 0.0f + ( Input.GetAxis( "GP_Xbox_RT" ) * _angle ) ) :
			Vector3.zero;

		_lB.localEulerAngles = Input.GetAxis( "GP_Xbox_LB" ) != 0 ?
			_activeSpin :
			Vector3.zero;

		_lT.localEulerAngles = Input.GetAxis( "GP_Xbox_LT" ) != 0 ?
			new Vector3( 0.0f, 0.0f, 0.0f + ( Input.GetAxis( "GP_Xbox_LT" ) * _angle ) ) :
			Vector3.zero;
	}
}
