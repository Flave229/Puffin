using Assets.Scripts.DavidHackPad.AI.Tasks;

namespace Assets.Scripts.DavidHackPad.AI.Needs
{
	public interface INeed
	{
		// Priority should only exceed 1 when the need is CRITICAL to the entities survival
		float GetPriority();

		ITask GenerateTask();
	}
}