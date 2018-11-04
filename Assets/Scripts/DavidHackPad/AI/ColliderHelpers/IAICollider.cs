using UnityEngine;

namespace Assets.Scripts.DavidHackPad.AI.ColliderHelpers
{
	public interface IAiCollider
	{
		void AiOnTriggerEnter(Collider collidingEntity);
		void AiOnTriggerStay(Collider collidingEntity);
	}
}