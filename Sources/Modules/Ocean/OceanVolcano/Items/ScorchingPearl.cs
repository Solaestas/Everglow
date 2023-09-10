using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Localization;
namespace Everglow.Ocean.OceanVolcano.Items
{
    public class ScorchingPearl : ModItem
	{
		public override void SetStaticDefaults()
		{
            // base.DisplayName.SetDefault("熔珠");
			Item.staff[base.Item.type] = true;
            // base.// DisplayName.AddTranslation(GameCulture.Chinese, "熔珠");
            GetGlowMask = Everglow.Ocean.SetStaticDefaultsGlowMask(this);
        }
        public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
            Item.glowMask = GetGlowMask;
            base.Item.damage = 462;
			base.Item.DamageType = DamageClass.Magic;
			base.Item.mana = 5;
			base.Item.width = 54;
			base.Item.height = 54;
			base.Item.useTime = 8;
			base.Item.useAnimation = 8;
            Item.crit = 22;
            base.Item.useStyle = 5;
			base.Item.noMelee = true;
			base.Item.knockBack = 0.5f;
			base.Item.value = 12000;
			base.Item.rare = 11;
			base.Item.UseSound = SoundID.Item60;
			base.Item.autoReuse = true;
            base.Item.shoot =ModContent.ProjectileType<Everglow.Ocean.Projectiles.MeltingPearl>();
			base.Item.shootSpeed = 2f;
		}
	}
}
