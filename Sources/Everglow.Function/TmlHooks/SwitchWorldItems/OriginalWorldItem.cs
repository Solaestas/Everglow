using MathNet.Numerics.LinearAlgebra.Factorization;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.IO;
using Terraria.ModLoader.Config;
using Terraria.Social;
using Terraria.ID;

namespace Everglow.Commons.TmlHooks.SwitchWorldItems;

internal class OriginalWorldItem : SwitchWorldItemBase
{
	public override Asset<Texture2D> Icon => null;
	public override int OrderIndex => 0;

	public OriginalWorldItem()
	{
	}

	public override void Enter(WorldFileData data)
	{
		WorldGen.playWorld();
	}
}