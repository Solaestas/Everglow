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

	public static bool ShouldMeleeWeaponScreenShake
	{
		get
		{
			return ModContent.GetInstance<MeleeProj_3D_Configs>().MeleeWeaponScreenShake;
		}
	}
}