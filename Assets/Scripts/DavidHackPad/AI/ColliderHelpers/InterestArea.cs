using UnityEngine;

namespace Assets.Scripts.DavidHackPad.AI.ColliderHelpers
{
	[RequireComponent(typeof(UnityEngine.Collider))]
	public class InterestArea : MonoBehaviour
	{
		[SerializeField]
		private AgentBehaviour _owningObject;

		void OnTriggerEnter(UnityEngine.Collider collidingEntity)
		{
			_owningObject.AiOnTriggerEnter(collidingEntity);
		}

		void OnTriggerStay(UnityEngine.Collider collidingEntity)
		{
			_owningObject.AiOnTriggerEnter(collidingEntity);
		}
	}
}