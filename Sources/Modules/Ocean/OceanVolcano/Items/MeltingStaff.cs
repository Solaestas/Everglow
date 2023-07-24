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
    public class MeltingStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
            base.DisplayName.SetDefault("熔烬法杖");
			Item.staff[base.item.type] = true;
            base.DisplayName.AddTranslation(GameCulture.Chinese, "熔烬法杖");
        }
        public override void SetDefaults()
        {
            base.item.damage = 462;
			base.item.magic = true;
			base.item.mana = 5;
			base.item.width = 54;
			base.item.height = 54;
			base.item.useTime = 15;
			base.item.useAnimation = 15;
            item.crit = 22;
            base.item.useStyle = 5;
			base.item.noMelee = true;
			base.item.knockBack = 0.5f;
			base.item.value = 12000;
			base.item.rare = 11;
			base.item.UseSound = SoundID.Item60;
			base.item.autoReuse = true;
            base.item.shoot = base.mod.ProjectileType("MeltingStaff");
			base.item.shootSpeed = 2.7f;
		}
	}
}
