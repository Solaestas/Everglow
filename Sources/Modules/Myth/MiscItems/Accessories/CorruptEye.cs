using Everglow.Sources.Modules.MythModule.MagicWeaponsReplace.Projectiles.CursedFlames;
using Terraria.Audio;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Accessories
{
	[AutoloadEquip(EquipType.Neck)]
	public class CorruptEye : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemGlowManager.AutoLoadItemGlow(this);
		}
		public static short GetGlowMask = 0;
		public override void SetDefaults()
		{
			Item.glowMask = ItemGlowManager.GetItemGlow(this);
			Item.width = 32;
			Item.height = 32;
			Item.value = 5000;
			Item.accessory = true;
			Item.rare = ItemRarityID.Pink;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.statDefense += 5;
			player.GetCritChance(DamageClass.Generic) += 4;
			player.buffImmune[39] = true;
			CorruptEyeEquiper cEE = player.GetModPlayer<CorruptEyeEquiper>();
			cEE.CorruptEyeEnable = true;
		}
	}
	class CorruptEyeEquiper : ModPlayer
	{
		public bool CorruptEyeEnable = false;
		public override void ResetEffects()
		{
			CorruptEyeEnable = false;
		}
		public override void PostHurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit, int cooldownCounter)
		{
			if (CorruptEyeEnable)
			{
				for (int i = 0; i < 5; i++)
				{
					Vector2 velocity = new Vector2(0, Main.rand.NextFloat(4.3f, 6f)).RotatedByRandom(6.283);
					Projectile CursedFlame = Projectile.NewProjectileDirect(Player.GetSource_FromThis(), Player.Center, velocity, ProjectileID.CursedFlameFriendly, 60, 1.5f, Player.whoAmI);
					CursedFlame.timeLeft = Main.rand.Next(25, 45);
				}
				for (int i = 0; i < 18; i++)
				{
					GenerateVFX();
				}
				for (int i = 0; i < 12; i++)
				{
					GenerateDust();
				}
				SoundEngine.PlaySound((SoundID.DD2_FlameburstTowerShot.WithPitchOffset(-0.2f)), Player.Center);
			}
		}
		private void GenerateVFX()
		{
			Vector2 v2 = Player.velocity;
			float mulVelocity = 0.3f + v2.Length() / 10f;
			Vector2 velocity = new Vector2(0, Main.rand.NextFloat(4.3f, 14f)).RotatedByRandom(6.283);
			CursedFlameDust cf = new CursedFlameDust
			{
				velocity = velocity,
				Active = true,
				Visible = true,
				position = Player.Center + new Vector2(0, Main.rand.NextFloat(0f, 35f)),
				maxTime = Main.rand.Next(27, 72),
				ai = new float[] { Main.rand.NextFloat(0.1f, 1f), Main.rand.NextFloat(-0.04f, 0.04f), Main.rand.NextFloat(3.6f, 10f) * mulVelocity }
			};
			VFXManager.Add(cf);
		}
		private void GenerateDust()
		{
			Vector2 velocity = new Vector2(0, Main.rand.NextFloat(4.3f, 6f)).RotatedByRandom(6.283);
			Dust D = Dust.NewDustDirect(Player.Center - new Vector2(4)/*Dust的Size=8x8*/, 0, 0, DustID.CursedTorch, 0, 0, 150, default, Main.rand.NextFloat(0.4f, 1.1f));
			D.noGravity = true;
			D.velocity = velocity;
		}
	}
}
