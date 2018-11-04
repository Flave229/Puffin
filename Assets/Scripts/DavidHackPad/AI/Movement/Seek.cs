using UnityEngine;

namespace Assets.Scripts.DavidHackPad.AI.Movement
{
	public class Seek : ISteeringBehaviour
	{
		public Vector3 ApplyForce(Vector3 positionOfActor, Vector3 positionOfTarget)
		{
			return Vector3.Normalize(positionOfTarget - positionOfActor);
		}
	}
}