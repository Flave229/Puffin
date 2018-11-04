using UnityEngine;

namespace Assets.Scripts.MikeHackPad
{
	[RequireComponent( typeof( ThirdPersonCameraController ) )]
	public class TargetingSystem : MonoBehaviour
	{
		[SerializeField]
		private float _maxGrappleRange = 2.0f;
		[SerializeField]
		private float _minGrappleRange = 45.0f;

		private GameObject _targetObject;
		private Vector3 _targetedPoint;
		private bool _grapplable;
		private ThirdPersonCameraController _cameraSystem;

		public void Start()
		{
			_cameraSystem = GetComponent<ThirdPersonCameraController>();
		}

		public void Update()
		{
			Transform cameraTransform = _cameraSystem.GetCameraTransform();

			RaycastHit info;
			Ray ray = new Ray( cameraTransform.position, cameraTransform.forward * _maxGrappleRange );
			if ( Physics.Raycast( ray, out info ) )
			{
				_targetObject = info.collider.gameObject;
				_targetedPoint = info.point;
				_grapplable = true;

#if UNITY_EDITOR
				Debug.DrawLine(transform.position, _targetedPoint, Color.green);
#endif
			}
			else
			{
				_targetObject = null;
				_targetedPoint = cameraTransform.position + cameraTransform.forward * _maxGrappleRange;
				_grapplable = false;

#if UNITY_EDITOR
				Debug.DrawLine( transform.position, _targetedPoint, Color.red );
#endif
			}
			
		}

		public Vector3 GetTargetedPoint()
		{
			return _targetedPoint;
		}

		public bool GetTargetGrappleable()
		{
			return _grapplable;
		}
	}
}
