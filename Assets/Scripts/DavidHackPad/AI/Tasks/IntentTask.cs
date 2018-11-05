using System;

namespace Assets.Scripts.DavidHackPad.AI.Tasks
{
	public class IntentTask : ITask
	{
		private readonly IInteractable _interactor;
		private readonly IInteractable _interactee;
		private readonly EIntent _intention;
		private readonly Action _onSuccess;
		private readonly Action _onFail;
		private bool _completed;

		public IntentTask(IInteractable interactor, IInteractable interactee, EIntent intention, Action onSuccess = null, Action onFail = null)
		{
			_interactor = interactor;
			_interactee = interactee;
			_intention = intention;
			_onSuccess = onSuccess;
			_onFail = onFail;
			_completed = false;
		}

		public bool Start()
		{
			return true;
		}

		public void Execute()
		{
			_interactee.HandleCommunication(_interactor, _intention, _onSuccess, _onFail);
			Finish();
		}

		public bool Finish()
		{
			_completed = true;
			return true;
		}

		public bool IsComplete()
		{
			return _completed;
		}
	}
}