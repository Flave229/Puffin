using System;
using UnityEngine;

namespace Assets.Scripts.DavidHackPad.AI.Tasks.Movement
{
	public class Seek : ISteeringBehaviour
	{
		public Vector3 ApplyForce(Vector3 positionOfActor, Vector3 positionOfTarget)
		{
			return Vector3.Normalize(positionOfTarget - positionOfActor);
		}
	}

	public class SeekTask : ITask
	{
		private readonly Rigidbody _actorRigidbody;
		private readonly Transform _actorTransform;
		private readonly Transform _targetTransform;
		private readonly float _seekForce;
		private readonly ISteeringBehaviour _steeringBehaviour;

		private readonly Action _onSuccess;
		private readonly Action _onFail;
		private bool _completed;

		public SeekTask(AgentBehaviour agent, Transform targetTransform, float seekForce, Action onSuccess = null, Action onFail = null)
		{
			_actorRigidbody = agent.GetComponent<Rigidbody>();
			_actorTransform = agent.GetComponent<Transform>();
			_targetTransform = targetTransform;
			_seekForce = seekForce;
			_onSuccess = onSuccess;
			_onFail = onFail;
			_steeringBehaviour = new Seek();
			_completed = false;
		}

		public bool Start()
		{
			return true;
		}

		public void Execute()
		{
			if (_targetTransform == null)
			{
				_onFail?.Invoke();
				Finish();
				return;
			}

			if (Vector3.Distance(_actorTransform.position, _targetTransform.position) < 1)
			{
				_onSuccess?.Invoke();
				Finish();
				return;
			}

			Vector3 force = _steeringBehaviour.ApplyForce(_actorTransform.position, _targetTransform.position) * _seekForce;
			_actorRigidbody.AddForce(force, ForceMode.Force);

			// Hack to limit speed
			if (_actorRigidbody.velocity.magnitude > 2)
			{
				_actorRigidbody.velocity = _actorRigidbody.velocity.normalized * 2;
			}
		}

		public bool Finish()
		{
			_completed = true;
			_onSuccess?.Invoke();
			return true;
		}

		public bool IsComplete()
		{
			return _completed;
		}
	}
}