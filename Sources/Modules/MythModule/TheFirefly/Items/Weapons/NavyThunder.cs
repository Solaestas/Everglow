﻿using Terraria.DataStructures;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Items.Weapons
{
    public class NavyThunder : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemGlowManager.AutoLoadItemGlow(this);
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 5));
        }

        public override void SetDefaults()
        {
            Item.glowMask = ItemGlowManager.GetItemGlow(this);
            Item.damage = 78;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 10;
            Item.width = 104;
            Item.height = 38;
            Item.useTime = 35;
            Item.useAnimation = 35;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.knockBack = 5f;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.DD2_BetsyFlameBreath with { MaxInstances = 3 };
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.NavyThunder>();
            Item.shootSpeed = 0;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.ownedProjectileCounts[type] > 0)
            {
                return false;
            }
            return true;
        }
    }
}