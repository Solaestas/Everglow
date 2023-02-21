namespace Everglow.Common.PlayerUtils;

public class WeaponPrefix
{
	public static float getPrefixDamage(int PrefixType)
	{
		float damageMult = 1f;
		if (PrefixType < 85)
		{
			switch (PrefixType)
			{
				case 3:
					damageMult = 1.05f;
					break;
				case 4:
					damageMult = 1.1f;
					break;
				case 5:
					damageMult = 1.15f;
					break;
				case 6:
					damageMult = 1.1f;
					break;
				case 81:
					damageMult = 1.15f;
					break;
				case 8:
					damageMult = 0.85f;
					break;
				case 10:
					damageMult = 0.85f;
					break;
				case 12:
					damageMult = 1.05f;
					break;
				case 13:
					damageMult = 0.9f;
					break;
				case 16:
					damageMult = 1.1f;
					break;
				case 20:
					damageMult = 1.1f;
					break;
				case 21:
					damageMult = 1.1f;
					break;
				case 82:
					damageMult = 1.15f;
					break;
				case 22:
					damageMult = 0.85f;
					break;
				case 25:
					damageMult = 1.15f;
					break;
				case 58:
					damageMult = 0.85f;
					break;
				case 26:
					damageMult = 1.1f;
					break;
				case 28:
					damageMult = 1.15f;
					break;
				case 83:
					damageMult = 1.15f;
					break;
				case 30:
					damageMult = 0.9f;
					break;
				case 31:
					damageMult = 0.9f;
					break;
				case 32:
					damageMult = 1.1f;
					break;
				case 34:
					damageMult = 1.1f;
					break;
				case 35:
					damageMult = 1.15f;
					break;
				case 52:
					damageMult = 0.9f;
					break;
				case 84:
					damageMult = 1.17f;
					break;
				case 37:
					damageMult = 1.1f;
					break;
				case 53:
					damageMult = 1.1f;
					break;
				case 55:
					damageMult = 1.05f;
					break;
				case 59:
					damageMult = 1.15f;
					break;
				case 60:
					damageMult = 1.15f;
					break;
				case 39:
					damageMult = 0.7f;
					break;
				case 40:
					damageMult = 0.85f;
					break;
				case 41:
					damageMult = 0.9f;
					break;
				case 57:
					damageMult = 1.18f;
					break;
				case 43:
					damageMult = 1.1f;
					break;
				case 46:
					damageMult = 1.07f;
					break;
				case 50:
					damageMult = 0.8f;
					break;
				case 51:
					damageMult = 1.05f;
					break;
			}
		}
		return damageMult;
	}
}
