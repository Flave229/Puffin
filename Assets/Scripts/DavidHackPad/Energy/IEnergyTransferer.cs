namespace Assets.Scripts.DavidHackPad.Energy
{
	public interface IEnergyTransferer
	{
		void GiveEnergy(EnergyCompontent energy);
		// Return type returns the actual energy taken (May not be what was requested)
		EnergyCompontent TakeEnergy(EnergyCompontent energy);
	}
}