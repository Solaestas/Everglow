using Terraria.DataStructures;
using Terraria.GameContent.Creative;

namespace Everglow.Yggdrasil.KelpCurtain.Items.Armors.Witherbark
{
	[AutoloadEquip(EquipType.Body)]
	public class WitherbarkBreastPlate : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 28;

			Item.rare = ItemRarityID.Green;
			Item.value = Item.buyPrice(0, 0, 60, 0);

			Item.defense = 2;
		}

		public override void UpdateEquip(Player player)
		{
			player.whipRangeMultiplier += 0.2f;
		}
	}

	public class WitherbarkBreasCloakLayer_Front : PlayerDrawLayer
	{
		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
		{
			return drawInfo.drawPlayer.armor[1] != null && drawInfo.drawPlayer.armor[1].type == ModContent.ItemType<WitherbarkBreastPlate>();
		}

		public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.ArmOverItem);

		public override void Draw(ref PlayerDrawSet drawInfo)
		{
			Texture2D cloak = ModAsset.WitherbarkBreastPlate_CloakFront.Value;

			var position = drawInfo.Center + new Vector2(0f, -7f) - Main.screenPosition;
			position = new Vector2((int)position.X, (int)position.Y); // You'll sometimes want to do this, to avoid quivering.
			Rectangle frame = drawInfo.drawPlayer.legFrame;
			Color drawColor = drawInfo.colorArmorBody;
			drawInfo.DrawDataCache.Add(new DrawData(
				cloak,
				position,
				frame,
				drawColor,
				0f,
				frame.Size() * 0.5f,
				1f,
				SpriteEffects.None,
				0));
		}
	}

	public class WitherbarkBreasCloakLayer_Back : PlayerDrawLayer
	{
		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
		{
			return drawInfo.drawPlayer.armor[1] != null && drawInfo.drawPlayer.armor[1].type == ModContent.ItemType<WitherbarkBreastPlate>();
		}

		public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.Leggings);

		public override void Draw(ref PlayerDrawSet drawInfo)
		{
			Texture2D cloak = ModAsset.WitherbarkBreastPlate_CloakBack.Value;

			var position = drawInfo.Center + new Vector2(0f, -5f) - Main.screenPosition;
			position = new Vector2((int)position.X, (int)position.Y); // You'll sometimes want to do this, to avoid quivering.
			Rectangle frame = drawInfo.drawPlayer.legFrame;
			Color drawColor = drawInfo.colorArmorBody;
			drawInfo.DrawDataCache.Add(new DrawData(
				cloak,
				position,
				frame,
				drawColor,
				0f,
				frame.Size() * 0.5f,
				1f,
				SpriteEffects.None,
				0));
		}
	}

	public class WitherbarkLeggingsPlayer : ModPlayer
	{
		public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
		{
			base.ModifyDrawInfo(ref drawInfo);
		}
	}
}