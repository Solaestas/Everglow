namespace Everglow.Food.Utils
{
	internal static class FoodUtils
	{
		public static int GetFrames(int hours, int minutes, int seconds, int frames)
		{
			return ((hours * 60 + minutes) * 60 + seconds) * 60 + frames;
		}
	}
}
