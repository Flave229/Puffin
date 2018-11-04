using UnityEngine;

namespace Assets.Scripts.DavidHackPad.AI.Movement
{
	public interface ISteeringBehaviour
	{
		Vector3 ApplyForce(Vector3 positionOfActor, Vector3 positionOfTarget);
	}
}