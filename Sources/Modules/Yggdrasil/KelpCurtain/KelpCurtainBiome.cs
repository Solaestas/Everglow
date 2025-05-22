using Everglow.Yggdrasil.Common;
using SubworldLibrary;

namespace Everglow.Yggdrasil.KelpCurtain;

public class KelpCurtainBiome : ModBiome
{
	/// <summary>
	/// The stratumbound of 2nd and 3rd stratum.
	/// </summary>
	public static List<Point> StratumBoundCurve = new List<Point>();

	public override int Music => YggdrasilContent.QuickMusic(ModAsset.KelpCurtainBGM_Path);

	public override SceneEffectPriority Priority => SceneEffectPriority.Environment;

	public override string BestiaryIcon => "Everglow/Yggdrasil/KelpCurtain/KelpCurtainIcon";

	public override string BackgroundPath => base.BackgroundPath;

	public override string MapBackground => "Everglow/Yggdrasil/KelpCurtain/Backgrounds/KelpCurtain_MapBackground";

	public override ModWaterStyle WaterStyle => ModContent.GetInstance<Water.KelpCurtainWaterStyle>();

	public override Color? BackgroundColor => base.BackgroundColor;

	public override void Load()
	{
		base.Load();
	}

	public override bool IsBiomeActive(Player player)
	{
		if (SubworldSystem.IsActive<YggdrasilWorld>())
		{
			if (Main.screenPosition.Y > Main.maxTilesY * 0.72f * 16 && Main.screenPosition.Y < Main.maxTilesY * 0.9f * 16)
			{
				if(player.Center.X >= FindClosestStratumBoundPointX(player) * 16)
				{
					return true;
				}
			}
		}
		return false;
	}

	public float FindClosestStratumBoundPointX(Player player)
	{
		if(StratumBoundCurve.Count > 0)
		{
			float minY = int.MaxValue;
			float currentX = -1;
			foreach (var point in StratumBoundCurve)
			{
				float deltaY = MathF.Abs((float)point.Y * 16 - player.Center.Y);
				if (deltaY < minY)
				{
					minY = deltaY;
					currentX = point.X;
				}
			}
			return currentX;
		}
		else
		{
			return -1;
		}
	}

	public static float FindClosestStratumBoundPointX(float checkTileY)
	{
		if (StratumBoundCurve.Count > 0)
		{
			float minY = int.MaxValue;
			float currentX = -1;
			foreach (var point in StratumBoundCurve)
			{
				float deltaY = MathF.Abs((float)point.Y - checkTileY);
				if (deltaY < minY)
				{
					minY = deltaY;
					currentX = point.X;
				}
			}
			return currentX;
		}
		else
		{
			return -1;
		}
	}

	public override void OnInBiome(Player player)
	{
		YggdrasilEnvironmentLightManager.LightingScene = YggdrasilScene.KelpCurtain;
		base.OnInBiome(player);
	}
}