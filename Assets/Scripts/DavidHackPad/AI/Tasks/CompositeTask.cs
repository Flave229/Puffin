using System;
using System.Collections.Generic;

namespace Assets.Scripts.DavidHackPad.AI.Tasks
{
	public class CompositeTask : ITask
	{
		private Queue<ITask> _internalTasks;
		private ITask _currentTask;
		private bool _complete;

		public CompositeTask(Queue<ITask> tasks)
		{
			_internalTasks = tasks;
			_complete = false;
		}

		public bool Start()
		{
			if (_internalTasks.Count <= 0)
			{
				return false;
			}
			_currentTask = _internalTasks.Dequeue();
			return true;
		}

		public void Execute()
		{
			if (_currentTask.IsComplete())
			{
				if (_internalTasks.Count <= 0)
				{
					Finish();
					return;
				}
				_currentTask = _internalTasks.Dequeue();
				_currentTask.Start();
			}

			_currentTask.Execute();
		}

		public bool Finish()
		{
			_complete = true;
			return true;
		}

		public bool IsComplete()
		{
			return _complete;
		}
	}
}
