using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.DavidHackPad.AI.Tasks
{
	public class FindTask<T> : ITask where T : MonoBehaviour
	{
		private readonly Action<Transform, T> _onSuccess;
		private readonly Action _onFail;
		private readonly AgentBehaviour _agent;
		private bool _completed;
		private Transform _target;

		public FindTask(AgentBehaviour agent, Action<Transform, T> onSuccess = null, Action onFail = null)
		{
			_onSuccess = onSuccess;
			_onFail = onFail;
			_agent = agent;
			_completed = false;
		}

		public bool Start()
		{
			return true;
		}

		public void Execute()
		{
			T[] validObjects = Object.FindObjectsOfType<T>();

			if (validObjects.Length == 0)
			{
				_onFail?.Invoke();
				Finish();
				return;
			}

			T target = validObjects[0];
			float closestDistance = Mathf.Infinity;
			for (int index = 1; index < validObjects.Length; ++index)
			{
				T currentTarget = validObjects[index];
				Vector3 directionTowardsTarget = currentTarget.transform.position - _agent.transform.position;
				float squareDistanceToTarget = directionTowardsTarget.sqrMagnitude;

				if (squareDistanceToTarget < closestDistance)
				{
					closestDistance = squareDistanceToTarget;
					target = currentTarget;
				}
			}

			_target = target.transform;
			_completed = true;
			Finish();
		}

		public bool Finish()
		{
			if (_completed)
			{
				_onSuccess?.Invoke(_target, _target.GetComponent<T>());
			}
			return true;
		}

		public bool IsComplete()
		{
			return _completed;
		}
	}
}
