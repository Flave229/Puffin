namespace Assets.Scripts.DavidHackPad.Math
{
	public static class MeasurementConverter
	{
		public static float ConvertCaloriesToKiloJoules(float calories)
		{
			return calories * 4.184f;
		}

		public static float ConvertKiloJoulesToCalories(float energy)
		{
			return energy / 4.184f;
		}
	}
}
