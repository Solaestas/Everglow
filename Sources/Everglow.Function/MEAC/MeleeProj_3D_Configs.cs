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
}