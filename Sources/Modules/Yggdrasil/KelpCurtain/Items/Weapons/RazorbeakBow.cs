using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.UI.Chat;
using ReLogic.Graphics;
using Everglow.Commons.Graphics;
using Everglow.Yggdrasil.KelpCurtain.Projectiles.Ranged;

namespace Everglow.Yggdrasil.KelpCurtain.Items.Weapons;

public class RazorbeakBow : ModItem
{
	public const int BaseDelay = 30;
	public const int ProjectileCountPerUse = 3;

	public override void SetDefaults()
	{
		Item.width = 38;
		Item.height = 80;

		Item.ranged = true;
		Item.damage = 11;
		Item.crit = 4;
		Item.knockBack = 3f;

		Item.UseSound = SoundID.Item5;
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.autoReuse = true;
		Item.noMelee = true;

		Item.useTime = 12;
		Item.useAnimation = 30;
		Item.useLimitPerAnimation = ProjectileCountPerUse;
		Item.reuseDelay = BaseDelay;

		Item.useAmmo = AmmoID.Arrow;
		Item.consumeAmmoOnFirstShotOnly = true;
		Item.shoot = ModContent.ProjectileType<RazorbeakArrow>();
		Item.shootSpeed = 12f;

		Item.rare = ItemRarityID.Green;
		Item.value = Item.buyPrice(gold: 5);
	}

	public override float UseTimeMultiplier(Player player)
	{
		int stack = player.GetModPlayer<RazorbeakBowPlayer>().RazorbeakBowEffectStack;
		var mul = 2 / (1 + stack * RazorbeakBowPlayer.RazorbeakBowAttackSpeedBonusPerStack);
		return stack > 0 ? mul : 2f;
	}

	public override float UseAnimationMultiplier(Player player) => UseTimeMultiplier(player);

	public override void HoldItem(Player player)
	{
		Item.reuseDelay = (int)(BaseDelay * UseTimeMultiplier(player));
	}

	public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
	{
		type = ModContent.ProjectileType<RazorbeakArrow>();
	}

	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
	{
		var modPlayer = player.GetModPlayer<RazorbeakBowPlayer>();

		if (player.ItemAnimationJustStarted)
		{
			modPlayer.GroupIndex++;
			modPlayer.RazorbeakHitInfo.Add(modPlayer.GroupIndex, (0, false));
		}

		int useTime = CombinedHooks.TotalUseTime(Item.useTime, player, Item);
		if (player.itemAnimation <= useTime // not enough time to shoot again
			|| (Item.useLimitPerAnimation != null && player.ItemUsesThisAnimation == Item.useLimitPerAnimation)) // this shot hits the limit
		{
			if (modPlayer.RazorbeakHitInfo.TryGetValue(modPlayer.GroupIndex, out var value))
			{
				modPlayer.RazorbeakHitInfo[modPlayer.GroupIndex] = (value.Count, true); // Mark the group as done
			}
		}

		Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, modPlayer.GroupIndex);
		return false;
	}

	public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
	{
		if (Main.LocalPlayer.HeldItem.ModItem == this)
		{
			// Draw text
			var modPlayer = Main.LocalPlayer.GetModPlayer<RazorbeakBowPlayer>();
			var text = $"{1 + modPlayer.RazorbeakBowEffectStack * RazorbeakBowPlayer.RazorbeakBowAttackSpeedBonusPerStack}x";
			var progress = modPlayer.RazorbeakBowEffectStack / (float)RazorbeakBowPlayer.RazorbeakBowEffectMaxStack;

			var stringSize = ChatManager.GetStringSize(FontAssets.MouseText.Value, text, Vector2.One);
			var drawPos = Main.LocalPlayer.Bottom - Main.screenPosition;
			var textColor = new Color(
				Color.LimeGreen.G / 255f * (0.9f + progress * 0.2f),
				MathHelper.Lerp(1f, Color.LimeGreen.R / 255f * (0.7f + progress * 0.3f), progress),
				Color.LimeGreen.B / 255f)
				* (0.9f + 0.1f * MathF.Sin((float)Main.timeForVisualEffects * 0.04f));
			var textScale = 1f + 0.05f * modPlayer.RazorbeakBowEffectStack;

			var sBS = GraphicsUtils.GetState(spriteBatch).Value;
			spriteBatch.End();
			spriteBatch.Begin(sBS);

			spriteBatch.transformMatrix = Main.GameViewMatrix.ZoomMatrix;
			spriteBatch.DrawString(FontAssets.MouseText.Value, text, drawPos, textColor, 0, new Vector2(stringSize.X * 0.5f, -stringSize.Y * 0.5f), textScale, SpriteEffects.None, 0);

			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

			// Draw timer
			float timer = modPlayer.RazorbeakBowTimer / (float)RazorbeakBowPlayer.RazorbeakBowEffectDuration;
			if (timer > 0)
			{
				ValueBarHelper.DrawCircleValueBar(spriteBatch, drawPos + new Vector2(0, 50f * textScale), 1 - timer, Color.Black, Color.White, 0.08f);
			}

			spriteBatch.End();
			spriteBatch.Begin(sBS);
		}
	}
}

public class RazorbeakBowPlayer : ModPlayer
{
	public const int RazorbeakBowEffectMaxStack = 8;
	public const int RazorbeakBowEffectDuration = 180;
	public const int RazorbeakBowEffectHitRequirement = 3;
	public const float RazorbeakBowAttackSpeedBonusPerStack = 0.25f;

	public int RazorbeakBowEffectStack { get; set; } = 0;

	public int RazorbeakBowTimer { get; set; } = 0;

	public int GroupIndex { get; set; } = 0;

	public Dictionary<int, (int Count, bool Done)> RazorbeakHitInfo { get; } = [];

	public override void PostUpdate()
	{
		// Reset attack speed bonus if the player stop hitting targets for 3 seconds.
		if (RazorbeakBowTimer <= 0)
		{
			RazorbeakBowEffectStack = 0;
		}
		else
		{
			RazorbeakBowTimer--;
		}
	}
}