using Assets.Scripts.DavidHackPad.Energy;
using UnityEngine;

namespace Assets.Scripts.DavidHackPad.AI.Desires
{
	[RequireComponent(typeof(Collider))]
	class Food : MonoBehaviour
	{
		[SerializeField]
		private int _calories;
		[SerializeField]
		private int _caloryJiggle;

		void Awake()
		{
			System.Random randomGenerator = new System.Random();
			int randomJiggle = randomGenerator.Next(-_caloryJiggle, _caloryJiggle);

			_calories += randomJiggle;
		}

		void OnTriggerEnter(Collider collidingEntity)
		{
			EIntent intent = EstablishColliderIntent(collidingEntity);

			if (intent == EIntent.UNKNOWN)
			{
				return;
			}

			IEnergyReciever reciever = collidingEntity.GetComponent<IEnergyReciever>();
			reciever?.ConsumeEnergy(new EnergyCompontent
			{
				EnergyType = EEnergyType.NOURISHMENT,
				KiloJoules = _calories * 4.184f
			});

			Destroy(this.gameObject);
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