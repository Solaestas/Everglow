using Terraria.Enums;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Accessories.Furnace;

public class MoltenCore : ModItem
{
	private const float OnePercent = 1f;
	private const int DamageReduction = 5;
	private const int DefenseBonus = 5;
	private const int CritBonus = 5;
	private const int DamageBonus = 5;
	private const int MoveSpeedBonus = 5;
	private const int JumpSpeedBonus = 5;

	private const int ItemFrames = 4;

	private int FrameCounter { get; set; } = 0;

	public override void SetDefaults()
	{
		Item.width = 44;
		Item.height = 46;
		Item.accessory = true;

		Item.SetShopValues(ItemRarityColor.Lime7, Item.buyPrice(gold: 10));
	}

	public override void UpdateAccessory(Player player, bool hideVisual)
	{
		// 1. Increase abilities when player has some debuffs
		// ==================================================
		// Including: OnFire, OnFire3, CursedInferno
		// Increasing 5% damage receive, 5 def, 5% crit, 5% damage, 5% speed, 5% jump speed
		if (player.HasBuff(BuffID.OnFire) || player.HasBuff(BuffID.OnFire3) || player.HasBuff(BuffID.CursedInferno))
		{
			player.endurance = OnePercent - ((OnePercent - player.endurance) * DamageReduction / 100f);
			player.statDefense += DefenseBonus;
			player.GetCritChance(DamageClass.Generic) += CritBonus;
			player.GetDamage(DamageClass.Generic) += DamageBonus / 100f;
			player.moveSpeed += MoveSpeedBonus / 100f;
			player.jumpSpeedBoost += JumpSpeedBonus / 100f;
		}
	}

	public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
	{
		Texture2D texture = ModAsset.MoltenCore.Value;
		int frameWidth = frame.Width;
		int frameHeight = frame.Height / ItemFrames;
		int frameOffsetY = frameHeight * (((int)Main.time / 5) % 4);
		spriteBatch.Draw(
			texture,
			position,
			new Rectangle(0, frameOffsetY, frameWidth, frameHeight),
			drawColor,
			0,
			new Vector2(frameWidth, frameHeight) / 2,
			1,
			SpriteEffects.None,
			0);

		FrameCounter = (FrameCounter + 1) % ItemFrames;
		return false;
	}

	public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
	{
		Texture2D texture = ModAsset.MoltenCore.Value;
		int frameWidth = texture.Width;
		int frameHeight = texture.Height / ItemFrames;
		int frameOffsetY = frameHeight * (((int)Main.time / 5) % 4);

		spriteBatch.Draw(
			texture,
			Item.Center - Main.screenPosition,
			new Rectangle(0, frameOffsetY, frameWidth, frameHeight),
			lightColor,
			0,
			new Vector2(frameWidth, frameHeight / 2) / 2,
			1,
			SpriteEffects.None,
			0);

		FrameCounter = (FrameCounter + 1) % ItemFrames;
		return false;
	}
}