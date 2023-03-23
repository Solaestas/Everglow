using System.ComponentModel;

using Terraria.ModLoader.Config;

namespace Everglow.Commons.FeatureFlags;

public class EverglowConfig : ModConfig
{
	/// <summary>
	/// 用于整个Mod，包括各个客户端的设置
	/// </summary>
	public override ConfigScope Mode => ConfigScope.ServerSide;

	/// <summary>
	/// 是否是Debug模式，如果是那么我们可以打印很多debug信息，或者运行一些debug下才有的逻辑
	/// </summary>
	[DefaultValue(false)]
	[Label("Enable Debug Mode")]
	[Tooltip("[For developers] Enable debug mode to allow debug functions to run")]
	public bool debugMode;

	public static bool DebugMode
	{
		get
		{
			return ModContent.GetInstance<EverglowConfig>().debugMode;
		}
	}

	public static ulong GetDevSteamID64()
	{
		ulong[] devSteamID64 = { 76561198188141971 /*Mr. Skirt | DXTST*/, 76561199027957722 /*Dirty Octopus | 脏渔歌*/, 76561198426724713  /*u_silver | 新萌の绿草*/, 76561198890883939 /*ju_zhang | 太阳照常升起*/, 76561198841173839  /*ye_you | 夜谷紫幽*/, 76561198384962275 /*Slime1024 | 凝胶 | TheGelatum*/, 76561199058565968 /*Omnielement | 万象元素*/, 76561198990010097 /*yiyang223*/, 76561199096708618  /*Lacewing*/, 76561198868731640  /*JSDA Ling*/, 76561199072827330 /*Jack Lyh*/, 76561198805079871  /*Cyrillya | crapsky223*/, 76561199070402024 /*FelixYang777*/, 76561198826286747 /*Colin Weiss*/, 76561198827572696  /*SilverMoon | 942293328 */, 76561198300589095 /*Setnour6*/};
		ulong[] allDevSteamID64 = new ulong[devSteamID64.Length];
		//for (var i = 0; i < devSteamID64.Length; i++)
		//{
		//	allDevSteamID64[i] = devSteamID64[i];
		//}
		return allDevSteamID64[devSteamID64.Length];
	}
}

// 以后解决这个Config
public class EverglowClientConfig : ModConfig
{
	/// <summary>
	/// 用于各个客户端的个性化设置
	/// </summary>
	public override ConfigScope Mode => ConfigScope.ClientSide;

	[Header("$Mods.Everglow.Config.Header.TextureReplace")]
	[DefaultValue(TextureReplaceMode.Terraria)]
	[Label("$Mods.Everglow.Config.TextureReplace.Label")]
	[Tooltip("$Mods.Everglow.Config.TextureReplace.Tooltip")]
	[DrawTicks]
	public TextureReplaceMode TextureReplace;

	[Header("$Mods.Everglow.Config.Header.AudioReplace")]

	[DefaultValue(true)]
	[Label("$Mods.Everglow.Config.ItemPickSoundReplace.Label")]
	public bool ItemPickSoundReplace;

	[DefaultValue(MothAudioReplaceMode.MothFighting)]
	[Label("$Mods.Everglow.Config.MothAudioReplace.Label")]
	[Tooltip("$Mods.Everglow.Config.MothAudioReplace.Tooltip")]
	[DrawTicks]
	public MothAudioReplaceMode MothAudioReplace;

	[DefaultValue(TuskAudioReplaceMode.TuskFighting)]
	[Label("$Mods.Everglow.Config.TuskAudioReplace.Label")]
	[Tooltip("$Mods.Everglow.Config.TuskAudioReplace.Tooltip")]
	[DrawTicks]
	public TuskAudioReplaceMode TuskAudioReplace;

	public override void OnChanged()
	{
		if ((int)TextureReplace >= 3)
			TextureReplace = TextureReplaceMode.Terraria;
		if ((int)MothAudioReplace >= 3)
			MothAudioReplace = MothAudioReplaceMode.MothFighting;
		if ((int)TuskAudioReplace >= 2)
			TuskAudioReplace = TuskAudioReplaceMode.TuskFighting;
		//if (AssetReplaceModule.IsLoaded)
		//{
		//	AssetReplaceModule.ReplaceTextures(TextureReplace);
		//}

		base.OnChanged();
	}

	public static int ReplaceMothAudio
	{
		get
		{
			return (int)ModContent.GetInstance<EverglowClientConfig>().MothAudioReplace;
		}
	}
}

public enum TextureReplaceMode
{
	Terraria,
	[Label("Eternal Resolve")]
	EternalResolve,
	Myth,
}
public enum MothAudioReplaceMode
{
	[Label("Original")]
	MothFighting,
	[Label("Alternate")]
	AltMothFighting,
	[Label("Old")]
	OldMothFighting,
}
public enum TuskAudioReplaceMode
{
	[Label("Original")]
	TuskFighting,
	[Label("Old")]
	OldTuskFighting,
}
