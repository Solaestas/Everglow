using Terraria.Enums;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Accessories;

public class TelescopicPlatformBeta : TelescopicPlatform
{
	public override void SetDefaults()
	{
		Item.DefaultToAccessory(80, 98);
		PillarCount = 21;
		MaxHeight = 720;
		MoveSpeed = 6;
		Texture2 = ModAsset.TelescopicPlatformBeta.Value;
		BodyDrawOffsetY = 8;
		BodyRect = new Rectangle(0, 0, 80, 38);
		PillarFrontRect = new Rectangle(0, 56, 40, 42);
		PillarBackRect = new Rectangle(40, 56, 40, 42);
		Item.SetShopValues(ItemRarityColor.LightRed4, Item.buyPrice(silver: 280000));
		base.SetDefaults();
	}
}