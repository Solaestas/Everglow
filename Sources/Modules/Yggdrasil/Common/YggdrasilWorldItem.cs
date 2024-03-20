using Everglow.Commons.TmlHooks.SwitchWorldItems;
using ReLogic.Content;
using SubworldLibrary;
using Terraria.IO;

namespace Everglow.Yggdrasil.Common;

internal class YggdrasilWorldItem : SwitchWorldItemBase
{
	public override Asset<Texture2D> Icon => ModAsset.YggdrasilImpression;

	public override int OrderIndex => 1;

	public override void Enter(WorldFileData data)
	{
		//SubworldSystem.Enter<YggdrasilWorld>();
	}
}