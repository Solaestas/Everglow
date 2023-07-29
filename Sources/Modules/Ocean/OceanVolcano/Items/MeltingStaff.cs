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
    public class MeltingStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
            // // base.DisplayName.SetDefault("熔烬法杖");
			Item.staff[base.Item.type] = true;
            // base.// DisplayName.AddTranslation(GameCulture.Chinese, "熔烬法杖");
        }
        public override void SetDefaults()
        {
            base.Item.damage = 462;
			base.Item.DamageType = DamageClass.Magic;
			base.Item.mana = 5;
			base.Item.width = 54;
			base.Item.height = 54;
			base.Item.useTime = 15;
			base.Item.useAnimation = 15;
            Item.crit = 22;
            base.Item.useStyle = 5;
			base.Item.noMelee = true;
			base.Item.knockBack = 0.5f;
			base.Item.value = 12000;
			base.Item.rare = 11;
			base.Item.UseSound = SoundID.Item60;
			base.Item.autoReuse = true;
            base.Item.shoot =ModContent.ProjectileType<Everglow.Ocean.Projectiles.MeltingStaff>();
			base.Item.shootSpeed = 2.7f;
		}
	}
}
