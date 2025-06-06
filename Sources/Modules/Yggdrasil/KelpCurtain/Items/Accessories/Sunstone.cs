using Everglow.Yggdrasil.KelpCurtain.Buffs;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.KelpCurtain.Items.Accessories;

public class Sunstone : ModItem
{
	public const int LifeMaxBonus = 20;
	public const int CooldownDuration = 600 * 60; // 600 s
	public const int BuffHealPerSecond = 5;
	public const int EffectRange = 5 * 16; // 5 blocks
	public const int OnFireDuration = 10 * 60;
	public const int DispelBuffLife = 200;

	public override string Texture => Commons.ModAsset.White_Mod;

	public override void SetDefaults()
	{
		Item.width = 20;
		Item.height = 20;

		Item.accessory = true;

		Item.rare = ItemRarityID.Orange;
		Item.value = Item.buyPrice(gold: 20);
	}

	public override void UpdateAccessory(Player player, bool hideVisual)
	{
		player.statLifeMax2 += LifeMaxBonus; // Increase max life by 20.
		player.GetModPlayer<SunstonePlayer>().SunstoneEnable = true;
	}

	public class SunstonePlayer : ModPlayer
	{
		public const int FrameCount = 4; // Number of frames for the Sunstone effect animation.

		public bool SunstoneEnable { get; set; } = false;

		public int Frame { get; set; } = 0;

		public bool SunstoneBuffActive => SunstoneEnable && Player.HasBuff<SunstoneBuff>();

		public override void ResetEffects()
		{
			SunstoneEnable = false;
		}

		public override void UpdateEquips()
		{
			if (SunstoneBuffActive)
			{
				if (Main.timeForVisualEffects % 15 == 0)
				{
					Frame = ++Frame % FrameCount;
				}

				Player.lifeRegenTime = 0; // Disable natural life regeneration

				// Amend Player life to 0, before heal Player.
				Player.statLife = Math.Max(Player.statLife, 0);

				// Heal Player every second.
				if (Main.timeForVisualEffects % 60 == 0)
				{
					Player.Heal(BuffHealPerSecond);
				}

				// Disable controls naturally
				Player.webbed = true;
				Player.noFallDmg = true;
			}
		}

		public override void PostUpdate()
		{
			if (SunstoneBuffActive)
			{
				foreach (var npc in Main.ActiveNPCs)
				{
					if (Vector2.Distance(npc.Center, Player.Center) < EffectRange)
					{
						npc.AddBuff(BuffID.OnFire, OnFireDuration);
					}
				}
			}
		}

		public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genDust, ref PlayerDeathReason damageSource)
		{
			if (SunstoneEnable)
			{
				if (!Player.HasBuff<SunstoneCooldown>())
				{
					Player.AddBuff(ModContent.BuffType<SunstoneBuff>(), 2);
					Player.AddBuff(ModContent.BuffType<SunstoneCooldown>(), CooldownDuration);
					return false;
				}
			}

			return true;
		}

		public override bool CanBeHitByNPC(NPC npc, ref int cooldownSlot) => !SunstoneBuffActive;

		public override bool CanBeHitByProjectile(Projectile projectile) => !SunstoneBuffActive;

		public override bool CanBeTeleportedTo(Vector2 teleportPosition, string context) => !SunstoneBuffActive;

		public override void HideDrawLayers(PlayerDrawSet drawInfo)
		{
			if (SunstoneBuffActive && !Main.gameMenu)
			{
				foreach (var layer in PlayerDrawLayerLoader.Layers)
				{
					layer.Hide();
				}
			}
		}

		public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
		{
			if (SunstoneBuffActive && !Main.gameMenu)
			{
				var stoneTex = ModAsset.SunstoneTest.Value;
				var frame = stoneTex.Frame(1, FrameCount, 0, Frame);
				Main.spriteBatch.Draw(stoneTex, drawInfo.drawPlayer.Bottom - Main.screenPosition, frame, new Color(1f, 0.5f, 0f), 0f, new Vector2(frame.Width / 2, frame.Height), 1f, SpriteEffects.None, 0);

				var glowTex = Commons.ModAsset.Point.Value;
				Main.spriteBatch.Draw(glowTex, drawInfo.drawPlayer.Center - Main.screenPosition, null, new Color(0.3f, 0.2f, 0f, 0f), 0f, glowTex.Size() / 2, 1f, SpriteEffects.None, 0);
			}
		}
	}
}