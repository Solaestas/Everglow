using System.ComponentModel;
using Everglow.Commons.AssetReplace.UIReplace.Core;
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
	// TODO 转移到hjson中
	//[Label("Enable Debug Mode")]
	//[Tooltip("[For developers] Enable debug mode to allow debug functions to run")]
	public bool debugMode;

	public static bool DebugMode
	{
		get
		{
			return ModContent.GetInstance<EverglowConfig>().debugMode;
		}
	}
}

// 以后解决这个Config
public class EverglowClientConfig : ModConfig
{
	/// <summary>
	/// 用于各个客户端的个性化设置
	/// </summary>
	public override ConfigScope Mode => ConfigScope.ClientSide;

	[Header("TextureReplace")]
	[DefaultValue(TextureReplaceMode.Terraria)]
	[DrawTicks]
	public TextureReplaceMode TextureReplace;

	[Header("AudioReplace")]

	[DefaultValue(true)]
	public bool ItemPickSoundReplace;

	[DefaultValue(MothAudioReplaceMode.MothFighting)]
	[DrawTicks]
	public MothAudioReplaceMode MothAudioReplace;

	[DefaultValue(TuskAudioReplaceMode.TuskFighting)]
	[DrawTicks]
	public TuskAudioReplaceMode TuskAudioReplace;

	public override void OnChanged()
	{
		if ((int)TextureReplace >= 5)
			TextureReplace = TextureReplaceMode.Terraria;
		if ((int)MothAudioReplace >= 3)
			MothAudioReplace = MothAudioReplaceMode.MothFighting;
		if ((int)TuskAudioReplace >= 2)
			TuskAudioReplace = TuskAudioReplaceMode.TuskFighting;
		if (UIReplaceModule.IsLoaded)
		{
			UIReplaceModule.ReplaceTextures(TextureReplace);
		}

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
	Default,
	EternalResolve,
	Everglow,
	Myth
}
public enum MothAudioReplaceMode
{
	MothFighting,
	AltMothFighting,
	OldMothFighting,
}
public enum TuskAudioReplaceMode
{
	TuskFighting,
	OldTuskFighting,
}
