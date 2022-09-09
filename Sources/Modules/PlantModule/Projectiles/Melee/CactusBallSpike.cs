using System;
using Everglow.Sources.Modules.PlantModule.Buffs;
using Everglow.Sources.Modules.PlantModule.Common;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Everglow.Sources.Modules.PlantModule.Projectiles.Melee
{
	public class CactusBallSpike : ModProjectile
	{
		public override string Texture=> "Terraria/Images/Projectile_763";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cactus spike");
			DisplayName.AddTranslation(PlantUtils.LocaizationChinese, "仙人掌刺");
		}
		public override void SetDefaults()
		{
			Projectile.CloneDefaults(763);
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 10;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(ModContent.BuffType<CactusBallBuff>(), 60, false);
		}
		public override void OnHitPvp(Player target, int damage, bool crit)
		{
			target.AddBuff(ModContent.BuffType<CactusBallBuff>(), 60, true, false);
		}
	}
}
