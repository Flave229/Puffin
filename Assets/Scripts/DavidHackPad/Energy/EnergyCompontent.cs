namespace Assets.Scripts.DavidHackPad.Energy
{
	public enum EEnergyType
	{
		UNKNOWN,
		NOURISHMENT
	}

	public struct EnergyCompontent
	{
		public EEnergyType EnergyType;
		public float KiloJoules;
	}
}