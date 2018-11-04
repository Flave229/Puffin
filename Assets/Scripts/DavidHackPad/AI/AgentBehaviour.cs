using Assets.Scripts.DavidHackPad.AI.ColliderHelpers;
using Assets.Scripts.DavidHackPad.AI.Desires;
using Assets.Scripts.DavidHackPad.AI.Movement;
using Assets.Scripts.DavidHackPad.Energy;
using UnityEngine;
using Random = System.Random;

namespace Assets.Scripts.DavidHackPad.AI
{
	[RequireComponent(typeof(Rigidbody))]
	public class AgentBehaviour : MonoBehaviour, IEnergyReciever, IAiCollider
	{
		private Rigidbody _rigidbody;
		private float _kiloJouleEnergy;
		private bool _alive;

		// Scan for food every 3 seconds
		private float _timeSinceLastFoodScan;

		private Food _targetFood;

		void Start()
		{
			_rigidbody = GetComponent<Rigidbody>();
			_kiloJouleEnergy = 200;
			_alive = true;
			_timeSinceLastFoodScan = Mathf.Infinity;
		}

		void Update()
		{
			if (_alive == false)
			{
				return;
			}

			// Making energy decay at about 5 kilojoules per second
			_kiloJouleEnergy -= Time.deltaTime * 10;
			_timeSinceLastFoodScan += Time.deltaTime;

			// Agent wants food
			if (_timeSinceLastFoodScan > 3 || _targetFood == null)
			{
				_timeSinceLastFoodScan = 0;
				FindFood();
			}

			if (_targetFood == null)
			{
				return;
			}

			ISteeringBehaviour seekForce = new Seek();
			Vector3 forceTowardsFood = seekForce.ApplyForce(transform.position, _targetFood.transform.position);
			forceTowardsFood = new Vector3(forceTowardsFood.x, 0, forceTowardsFood.z) * 3;

			// Temp hack that makes them fall over if they run out of energy
			if (_kiloJouleEnergy > 0)
			{
				_rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
				_rigidbody.AddForce(forceTowardsFood, ForceMode.Force);
			}
			else
			{
				KillAgent();
				_rigidbody.AddForce(forceTowardsFood * 30, ForceMode.Force);
			}
		}

		private void FindFood()
		{
			Food[] foodObjects = FindObjectsOfType<Food>();

			if (foodObjects.Length == 0)
			{
				return;
			}

			_targetFood = foodObjects[0];
			float closestDistance = Mathf.Infinity;
			for (int foodIndex = 1; foodIndex < foodObjects.Length; ++foodIndex)
			{
				Food food = foodObjects[foodIndex];
				Vector3 directionTowardsFood = food.transform.position - this.transform.position;
				float squareDistanceToTarget = directionTowardsFood.sqrMagnitude;

				if (squareDistanceToTarget < closestDistance)
				{
					closestDistance = squareDistanceToTarget;
					_targetFood = food;
				}
			}
		}

		private void KillAgent()
		{
			_rigidbody.constraints = RigidbodyConstraints.None;
			Random randomGenerator = new Random();
			_rigidbody.AddForce(randomGenerator.Next(5, 10), 0, randomGenerator.Next(5, 10));
			_alive = false;

			foreach (Transform child in transform)
			{
				Destroy(child.gameObject);
			}
		}

		public void ConsumeEnergy(EnergyCompontent energy)
		{
			if (energy.EnergyType == EEnergyType.NOURISHMENT)
			{
				_kiloJouleEnergy += energy.KiloJoules;
				FindFood();
			}
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
			_rigidbody.AddForce(new Vector3(force.x, 0, force.z) / 3, ForceMode.Force);
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
			_rigidbody.AddForce(new Vector3(force.x, 0, force.z) / 3, ForceMode.Force);
		}

		private bool IsAlive()
		{
			return _alive;
		}
	}
}