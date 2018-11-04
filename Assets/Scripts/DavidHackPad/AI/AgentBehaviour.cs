using Assets.Scripts.DavidHackPad.AI.Desires;
using Assets.Scripts.DavidHackPad.AI.Movement;
using Assets.Scripts.DavidHackPad.Energy;
using UnityEngine;

namespace Assets.Scripts.DavidHackPad.AI
{
	[RequireComponent(typeof(Rigidbody))]
	public class AgentBehaviour : MonoBehaviour, IEnergyReciever
	{
		private Rigidbody _rigidbody;
		private float _kiloJouleEnergy;
		private bool _alive;

		void Start()
		{
			_rigidbody = GetComponent<Rigidbody>();
			_kiloJouleEnergy = 200;
			_alive = true;
		}

		void Update()
		{
			if (_alive == false)
			{
				return;
			}

			// Making energy decay at about 5 kilojoules per second
			_kiloJouleEnergy -= Time.deltaTime * 5;

			// Agent wants food
			Food[] foodObjects = FindObjectsOfType<Food>();

			if (foodObjects.Length == 0)
			{
				return;
			}

			Food closestFood = foodObjects[0];
			float closestDistance = Mathf.Infinity;
			for(int foodIndex = 1; foodIndex < foodObjects.Length; ++foodIndex)
			{
				Food food = foodObjects[foodIndex];
				Vector3 directionTowardsFood = food.transform.position - this.transform.position;
				float squareDistanceToTarget = directionTowardsFood.sqrMagnitude;

				if (squareDistanceToTarget < closestDistance)
				{
					closestDistance = squareDistanceToTarget;
					closestFood = food;
				}
			}

			Seek seekForce = new Seek();
			Vector3 forceTowardsFood = seekForce.ApplyForce(transform.position, closestFood.transform.position);
			forceTowardsFood = new Vector3(forceTowardsFood.x, 0, forceTowardsFood.z) * 3;

			// Temp hack that makes them fall over if they run out of energy
			if (_kiloJouleEnergy > 0)
			{
				_rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
				_rigidbody.AddForce(forceTowardsFood, ForceMode.Force);
			}
			else
			{
				_alive = false;
				_rigidbody.constraints = RigidbodyConstraints.None;
				_rigidbody.AddForce(forceTowardsFood * 30, ForceMode.Force);
			}
		}

		public void ConsumeEnergy(EnergyCompontent energy)
		{
			if (energy.EnergyType == EEnergyType.NOURISHMENT)
			{
				_kiloJouleEnergy += energy.KiloJoules;
			}
		}
	}
}