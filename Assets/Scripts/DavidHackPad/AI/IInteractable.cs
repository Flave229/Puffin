using System;
using Assets.Scripts.DavidHackPad.AI.Tasks;

namespace Assets.Scripts.DavidHackPad.AI
{
	public interface IInteractable
	{
		void HandleCommunication(IInteractable interactor, EIntent intent, Action onSuccessfulInteraction = null, Action onFailedInteraction = null);
	}
}