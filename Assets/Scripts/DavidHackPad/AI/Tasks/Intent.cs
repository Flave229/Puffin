using System;

namespace Assets.Scripts.DavidHackPad.AI.Tasks
{
	[Flags]
	public enum EIntent
	{
		UNKNOWN = 0,
		FIND = 1,
		EAT = 2
	}
}