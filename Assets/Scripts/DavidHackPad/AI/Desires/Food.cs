﻿using Assets.Scripts.DavidHackPad.Energy;
using UnityEngine;

namespace Assets.Scripts.DavidHackPad.AI.Desires
{
	[RequireComponent(typeof(Collider))]
	public class Food : MonoBehaviour
	{
		[SerializeField]
		private int _calories;
		[SerializeField]
		private int _caloryJiggle;

		private float _caloriesLeft;

		void Awake()
		{
			System.Random randomGenerator = new System.Random();
			int randomJiggle = randomGenerator.Next(-_caloryJiggle, _caloryJiggle);

			_calories += randomJiggle;
			_caloriesLeft = _calories;
		}

		void OnTriggerEnter(Collider collidingEntity)
		{
			EIntent intent = EstablishColliderIntent(collidingEntity);

			if (intent == EIntent.UNKNOWN)
			{
				return;
			}

			HandleBeingEaten(collidingEntity);
		}

		void OnTriggerStay(Collider collidingEntity)
		{
			EIntent intent = EstablishColliderIntent(collidingEntity);

			if (intent == EIntent.UNKNOWN)
			{
				return;
			}

			HandleBeingEaten(collidingEntity);
		}

		private void HandleBeingEaten(Collider collidingEntity)
		{
			// Lets say that 10 calories can be eaten in 1 seconds. 
			float caloriesConsumed = Time.deltaTime * 10;

			if (caloriesConsumed > _caloriesLeft)
			{
				caloriesConsumed = _caloriesLeft;
			}

			_caloriesLeft -= caloriesConsumed;
			IEnergyReciever reciever = collidingEntity.GetComponent<IEnergyReciever>();
			reciever?.ConsumeEnergy(new EnergyCompontent
			{
				EnergyType = EEnergyType.NOURISHMENT,
				KiloJoules = caloriesConsumed * 4.184f
			});

			if (_caloriesLeft <= 0)
			{
				Destroy(this.gameObject);
			}
		}

		public float GetStartingCalories()
		{
			return _calories;
		}

		public float GetRemainingCalories()
		{
			return _caloriesLeft;
		}

		private EIntent EstablishColliderIntent(Collider collidingEntity)
		{
			// TODO: Make a new class that keeps track of desires and intents, using that to query instead
			if (collidingEntity.GetComponent<AgentBehaviour>() == null)
			{
				return EIntent.UNKNOWN;
			}

			return EIntent.EAT;
		}
	}
}