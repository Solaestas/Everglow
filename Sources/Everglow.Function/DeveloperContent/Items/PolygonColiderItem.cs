using Everglow.Commons.Enums;
using Everglow.Commons.Utilities;
using Everglow.Commons.Vertex;
using Everglow.Commons.VFX;
using Everglow.Commons.VFX.Pipelines;

namespace Everglow.Commons.DeveloperContent.Items;

/// <summary>
/// Visulalize the data of mouse-covered-tile.
/// </summary>
public class PolygonColiderItem : ModItem
{
	public int ColiderVFXTimer = 0;

	public override void SetDefaults()
	{
		Item.width = 16;
		Item.height = 16;
		Item.value = 0;
		Item.rare = ItemRarityID.White;
	}

	public override void HoldItem(Player player)
	{
		if(ColiderVFXTimer == 0)
		{
			var system = new PolygonCollisionHelper()
			{
				Visible = true,
				Active = true,
				Owner = player,
			};
			Ins.VFXManager.Add(system);
			ColiderVFXTimer = 60;
		}
	}
}