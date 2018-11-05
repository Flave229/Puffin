namespace Assets.Scripts.DavidHackPad.AI.Tasks
{
	public interface ITask
	{
		bool Start();
		void Execute();
		bool Finish();
		bool IsComplete();
	}
}