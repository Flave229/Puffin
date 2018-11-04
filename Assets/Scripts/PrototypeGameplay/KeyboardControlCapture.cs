using UnityEngine;

namespace Assets.Scripts.PrototypeGameplay
{
	public class KeyboardControlCapture : MonoBehaviour
	{
		private bool _isControllerCaptured = false;

		void Update()
		{
			if (Input.GetMouseButton(0))
			{
				_isControllerCaptured = true;
				Cursor.lockState = CursorLockMode.Locked;
			}
			if (Input.GetKey(KeyCode.Escape))
			{
				_isControllerCaptured = false;
				Cursor.lockState = CursorLockMode.None;
			}
		}

		public bool IsControllerCaptured()
		{
			return _isControllerCaptured;
		}
	}
}