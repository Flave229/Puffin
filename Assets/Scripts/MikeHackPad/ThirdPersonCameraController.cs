using UnityEngine;

namespace Assets.Scripts.MikeHackPad
{
	[RequireComponent(typeof(KeyboardControlCapture))]
	public class ThirdPersonCameraController : MonoBehaviour
	{
		[SerializeField]
		private Transform _cameraPivot;
		[SerializeField]
		private Transform _cameraPitch;
		[SerializeField]
		private Camera _camera;
		private KeyboardControlCapture _keyboardController;

		private float _overheadClampAngle = 90.0f;
		private float _underCharacterClampAngle = 270.0f;

		[SerializeField]
		public float MouseSensitivity;

		public void Start()
		{
			_keyboardController = GetComponent<KeyboardControlCapture>();
		}


		public void Update()
		{
			if (!_keyboardController.IsControllerCaptured())
			{
				return;
			}

			Vector3 cameraPitch = _cameraPitch.transform.localEulerAngles;
			Vector3 cameraPivot = _cameraPivot.transform.localEulerAngles;

			cameraPitch.x += Input.GetAxis("Mouse Y") * MouseSensitivity * -1.0f * Time.deltaTime;
			cameraPivot.y += Input.GetAxis("Mouse X") * MouseSensitivity * Time.deltaTime;

			if (cameraPitch.x > _overheadClampAngle && cameraPitch.x < _underCharacterClampAngle)
			{
				if (cameraPitch.x < 180.0f)
				{
					cameraPitch.x = _overheadClampAngle;
				}
				else
				{
					cameraPitch.x = _underCharacterClampAngle;
				}
			}

			_cameraPitch.transform.localEulerAngles = cameraPitch;
			_cameraPivot.transform.localEulerAngles = cameraPivot;
		}

		public Transform GetCameraPivotTransform()
		{
			return _cameraPivot;
		}

		public Transform GetCameraPitchTransform()
		{
			return _cameraPitch;
		}

		public Vector3 GetCameraFlatForward()
		{
			return _cameraPivot.forward;
		}

		public void EnableCamera(bool isEnabled)
		{
			_camera.enabled = isEnabled;
			_camera.GetComponent<AudioListener>().enabled = isEnabled;
		}
	}
}