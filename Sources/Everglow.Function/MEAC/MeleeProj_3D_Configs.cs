using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace Everglow.Commons.MEAC;

public class MeleeProj_3D_Configs : ModConfig
{
	/// <summary>
	/// 用于整个Mod，包括各个客户端的设置
	/// </summary>
	public override ConfigScope Mode => ConfigScope.ServerSide;

	[DefaultValue(false)]
	public bool MeleeWeaponScreenShake;

	[DefaultValue(false)]
	public bool MeleeWeaponProjectBindWithScreen_Draw;

	[DefaultValue(false)]
	public bool MeleeWeaponProjectBindWithScreen_Behavior;

	[DefaultValue(MathHelper.PiOver4)]
	[Range(MathHelper.Pi/6f, MathHelper.Pi/3f)]
	public float FOV_Angle;

	public static bool ShouldMeleeWeaponScreenShake
	{
		get
		{
			return ModContent.GetInstance<MeleeProj_3D_Configs>().MeleeWeaponScreenShake;
		}
	}

	public static bool IsMeleeWeaponProjectBindWithScreen_Draw
	{
		get
		{
			return ModContent.GetInstance<MeleeProj_3D_Configs>().MeleeWeaponProjectBindWithScreen_Draw;
		}
	}

	public static bool IsMeleeWeaponProjectBindWithScreen_Behavior
	{
		get
		{
			return ModContent.GetInstance<MeleeProj_3D_Configs>().MeleeWeaponProjectBindWithScreen_Behavior;
		}
	}

	public static float AngleofFOV
	{
		get
		{
			return ModContent.GetInstance<MeleeProj_3D_Configs>().FOV_Angle;
		}
	}
}