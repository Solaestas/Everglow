using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Localization;
namespace MythMod.Items.Volcano
{
    public class ScorchingPearl : ModItem
	{
		public override void SetStaticDefaults()
		{
            base.DisplayName.SetDefault("熔珠");
			Item.staff[base.item.type] = true;
            base.DisplayName.AddTranslation(GameCulture.Chinese, "熔珠");
            GetGlowMask = MythMod.SetStaticDefaultsGlowMask(this);
        }
        public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
            item.glowMask = GetGlowMask;
            base.item.damage = 462;
			base.item.magic = true;
			base.item.mana = 5;
			base.item.width = 54;
			base.item.height = 54;
			base.item.useTime = 8;
			base.item.useAnimation = 8;
            item.crit = 22;
            base.item.useStyle = 5;
			base.item.noMelee = true;
			base.item.knockBack = 0.5f;
			base.item.value = 12000;
			base.item.rare = 11;
			base.item.UseSound = SoundID.Item60;
			base.item.autoReuse = true;
            base.item.shoot = base.mod.ProjectileType("MeltingPearl");
			base.item.shootSpeed = 2f;
		}
	}
}
