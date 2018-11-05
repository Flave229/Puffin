using System.Collections.Generic;
using Assets.Scripts.DavidHackPad.AI.Desires;
using Assets.Scripts.DavidHackPad.AI.Tasks;
using Assets.Scripts.DavidHackPad.AI.Tasks.Movement;
using Assets.Scripts.DavidHackPad.Energy;
using Assets.Scripts.DavidHackPad.Math;
using UnityEngine;

namespace Assets.Scripts.DavidHackPad.AI.Needs
{
	[RequireComponent(typeof(AgentBehaviour))]
	public class Hunger : MonoBehaviour, INeed
	{
		[SerializeField]
		public uint CriticalCaloryLevel;
		[SerializeField]
		public uint HungryCaloryLevel;
		[SerializeField]
		public uint SatisfiedCaloryLevel;
		[SerializeField]
		public uint FullCaloryLevel;
		
		private uint _criticalEnergyLevel;
		private uint _hungryEnergyLevel;
		private uint _satisfiedEnergyLevel;
		private uint _fullEnergyLevel;

		private AgentBehaviour _owningAgent;

		void Awake()
		{
#if UNITY_EDITOR
			if (CriticalCaloryLevel > HungryCaloryLevel)
			{
				Debug.LogError("The Critical Calory Level must be lower than the Hungry Calory Level");
			}
			if (HungryCaloryLevel > SatisfiedCaloryLevel)
			{
				Debug.LogError("The Hungry Calory Level must be lower than the Satisfied Calory Level");
			}
			if (SatisfiedCaloryLevel > FullCaloryLevel)
			{
				Debug.LogError("The Satisfied Calory Level must be lower than the Full Calory Level");
			}
#endif
			_criticalEnergyLevel = (uint)MeasurementConverter.ConvertCaloriesToKiloJoules(CriticalCaloryLevel);
			_hungryEnergyLevel = (uint)MeasurementConverter.ConvertCaloriesToKiloJoules(HungryCaloryLevel);
			_satisfiedEnergyLevel = (uint)MeasurementConverter.ConvertCaloriesToKiloJoules(SatisfiedCaloryLevel);
			_fullEnergyLevel = (uint)MeasurementConverter.ConvertCaloriesToKiloJoules(FullCaloryLevel);
		}

		void Start()
		{
			_owningAgent = GetComponent<AgentBehaviour>();
		}

		public float GetPriority()
		{
			float currentEnergy = _owningAgent.GetEnergyLevel();

			// Between 0 and critical, the priority exceeds 1 up to 10 (Life dependant)
			if (currentEnergy < _criticalEnergyLevel)
			{
				return 1 + ((1 - (currentEnergy / _criticalEnergyLevel)) * 9);
			}
			// Between critical and hungry, so still need a vaguely high priority. 0.7 - 1
			if (currentEnergy < _hungryEnergyLevel)
			{
				return 1 - ((Mathf.InverseLerp(_criticalEnergyLevel, _hungryEnergyLevel, currentEnergy) * 0.3f) + 0.7f);
			}
			// Currently satisfied but would still benefit from more food. priority 0.2 - 0.7
			if (currentEnergy < _satisfiedEnergyLevel)
			{
				return 0.7f - ((Mathf.InverseLerp(_hungryEnergyLevel, _satisfiedEnergyLevel, currentEnergy) * 0.5f) + 0.2f);
			}

			return 0.2f - ((Mathf.InverseLerp(_satisfiedEnergyLevel, _fullEnergyLevel, currentEnergy) * 0.2f));
		}

		public ITask GenerateTask()
		{
			Queue<ITask> queuedTasks = new Queue<ITask>();
			ITask compositeTask = new CompositeTask(queuedTasks);

			ITask findFood = new FindTask<Food>(_owningAgent, (target, food) =>
			{
				// Called when food found. Create seek task.
				ITask seekFood = new SeekTask(_owningAgent, target, 3, () =>
				{
					ITask intentTask = new IntentTask(_owningAgent, food, EIntent.EAT, () =>
					{
						EnergyCompontent foodEnergy = food.TakeEnergy(new EnergyCompontent
						{
							EnergyType = EEnergyType.NOURISHMENT
						});
						_owningAgent.GiveEnergy(new EnergyCompontent
						{
							EnergyType = EEnergyType.NOURISHMENT,
							KiloJoules = foodEnergy.KiloJoules
						});
					});
					queuedTasks.Enqueue(intentTask);
				});
				queuedTasks.Enqueue(seekFood);
			});
			queuedTasks.Enqueue(findFood);
			compositeTask.Start();

			return compositeTask;
		}
	}
}