using Everglow.Common.VFX.CommonVFXDusts;
using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Everglow.Myth.Misc.Items.Weapons;

public class DarkFlower : ModItem
{
    private Item item
    {
        get => Item;
    }

    public override void SetStaticDefaults()
    {

        // DisplayName.SetDefault("°µÖ®»¨");
        //Tooltip.SetDefault("");
    }

    public override void SetDefaults()
    {
        item.CloneDefaults(ItemID.FlowerofFrost);
        item.damage = 150;
        item.shootSpeed = 11;
        item.useTime = 1;
        item.useAnimation = 20;
        item.useLimitPerAnimation = 1;
        item.shoot = ModContent.ProjectileType<DarkFlower_Proj>();
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        Projectile.NewProjectile(source,position,velocity+Main.rand.NextVector2Circular(3,3)+player.velocity*0.5f,type,damage,knockback,player.whoAmI);
        return false;
    }
}
