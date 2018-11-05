using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.DavidHackPad.AI.ColliderHelpers;
using Assets.Scripts.DavidHackPad.AI.Desires;
using Assets.Scripts.DavidHackPad.AI.Needs;
using Assets.Scripts.DavidHackPad.AI.Tasks;
using Assets.Scripts.DavidHackPad.AI.Tasks.Movement;
using Assets.Scripts.DavidHackPad.Energy;
using UnityEngine;
using Random = System.Random;

namespace Assets.Scripts.DavidHackPad.AI
{
	[RequireComponent(typeof(Rigidbody))]
	public class AgentBehaviour : MonoBehaviour, IEnergyTransferer, IAiCollider, IInteractable
	{
		private Rigidbody _rigidbody;
		private float _kiloJouleEnergy;
		private bool _alive;

		private List<INeed> _needs;
		private ITask _currentTask;
		// Scan for food every 3 seconds
		private float _timeSinceLastFoodScan;

		private Food _targetFood;

		void Start()
		{
			_rigidbody = GetComponent<Rigidbody>();
			_kiloJouleEnergy = 200;
			_alive = true;
			_timeSinceLastFoodScan = Mathf.Infinity;

			_needs = GetComponents<INeed>().ToList();

			_rigidbody.freezeRotation = true;
		}

		void Update()
		{
			if (_alive == false)
			{
				return;
			}

			if (_kiloJouleEnergy <= 0)
			{
				KillAgent();
			}

			// Normal Human need minimum 1200 calories a day to prevent illness
			// This maps roughly to 5020 kilojoules of energy.
			// Assuming a "day" takes 10 mins in the game. 10 mins = 600 seconds.
			// 600 seconds to naturally consume 5020 kJ.
			// 1 second consumes 8.36 calories
			_kiloJouleEnergy -= Time.deltaTime * 8.36f;

			if (_currentTask == null || _currentTask.IsComplete())
			{
				INeed highestNeed = GetHighestPriorityNeed();
				_currentTask = highestNeed.GenerateTask();
			}

			_currentTask?.Execute();
		}

		private INeed GetHighestPriorityNeed()
		{
			if (_needs.Count <= 0)
			{
				return null;
			}

			INeed highestPriorityNeed = _needs[0];
			float priorityValue = highestPriorityNeed.GetPriority();
			foreach (INeed need in _needs)
			{
				if (need.GetPriority() < priorityValue)
				{
					continue;
				}

				highestPriorityNeed = need;
				priorityValue = need.GetPriority();
			}

			return highestPriorityNeed;
		}

		private void KillAgent()
		{
			_rigidbody.constraints = RigidbodyConstraints.None;
			Random randomGenerator = new Random();

			double sign = randomGenerator.NextDouble() * 2 - 1;
			int multiply = sign < 0 ? -1 : 1;
			float randomX = randomGenerator.Next(5, 10) * multiply;

			sign = randomGenerator.NextDouble() * 2 - 1;
			multiply = sign < 0 ? -1 : 1;
			float randomZ = randomGenerator.Next(5, 10) * multiply;
			_rigidbody.AddForce(randomX, 0, randomZ, ForceMode.Impulse);

			_alive = false;
			foreach (Transform child in transform)
			{
				Destroy(child.gameObject);
			}
		}

		public void GiveEnergy(EnergyCompontent energy)
		{
			if (energy.EnergyType == EEnergyType.NOURISHMENT)
			{
				_kiloJouleEnergy += energy.KiloJoules;
			}
		}

		public EnergyCompontent TakeEnergy(EnergyCompontent energy)
		{
			if (energy.EnergyType == EEnergyType.NOURISHMENT)
			{
				// Can not currently eat people
			}
			return new EnergyCompontent();
		}

		public void AiOnTriggerEnter(Collider collidingEntity)
		{
			AgentBehaviour agentBehaviour = collidingEntity.GetComponent<AgentBehaviour>();
			if (agentBehaviour == null || agentBehaviour.IsAlive() == false)
			{
				return;
			}

			ISteeringBehaviour repelBehaviour = new Repel();
			Vector3 force = repelBehaviour.ApplyForce(transform.position, collidingEntity.transform.position);
			_rigidbody.AddForce(new Vector3(force.x, 0, force.z), ForceMode.Force);
		}

		public void AiOnTriggerStay(Collider collidingEntity)
		{
			AgentBehaviour agentBehaviour = collidingEntity.GetComponent<AgentBehaviour>();
			if (agentBehaviour == null || agentBehaviour.IsAlive() == false)
			{
				return;
			}

			ISteeringBehaviour repelBehaviour = new Repel();
			Vector3 force = repelBehaviour.ApplyForce(transform.position, collidingEntity.transform.position);
			_rigidbody.AddForce(new Vector3(force.x, 0, force.z), ForceMode.Force);
		}

		private bool IsAlive()
		{
			return _alive;
		}

		public float GetEnergyLevel()
		{
			return _kiloJouleEnergy;
		}

		public void HandleCommunication(IInteractable interactor, EIntent intent, Action onSuccessfulInteraction = null,
			Action onFailedInteraction = null)
		{
			throw new NotImplementedException();
		}
	}
}