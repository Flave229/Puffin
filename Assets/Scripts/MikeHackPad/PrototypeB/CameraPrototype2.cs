using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.MikeHackPad
{
	public class CameraPrototype2 : MonoBehaviour
	{
		private Vector3 _offset;
		[SerializeField]
		private PlayerPrototype2 _playerCharacter;

		// Use this for initialization
		void Start()
		{
			_offset = transform.position - _playerCharacter.transform.position;
		}

		// Update is called once per frame
		void LateUpdate()
		{
			transform.position = _playerCharacter.transform.position + _offset;
		}
	}
}