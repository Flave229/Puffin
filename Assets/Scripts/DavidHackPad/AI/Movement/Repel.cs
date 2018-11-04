using UnityEngine;

namespace Assets.Scripts.DavidHackPad.AI.Movement
{
	class Repel : ISteeringBehaviour
	{
		public Vector3 ApplyForce(Vector3 positionOfActor, Vector3 positionOfTarget)
		{
			return Vector3.Normalize(positionOfActor - positionOfTarget);
		}
	}
}