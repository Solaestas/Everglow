﻿using Everglow.Sources.Modules.MythModule.Common;
using Terraria.Audio;
using Terraria.DataStructures;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Slingshots
{
    /// <summary>
    /// Default damage = 6 width*height = 36*36 useT = useA = 21 crit = 8  useStyle = ItemUseStyleID.Shoot rare = ItemRarityID.White value = Item.sellPrice(0, 0, 0, 50) 
    /// noMelee = true noUseGraphic = true autoReuse = false
    /// DamageType = DamageClass.Ranged UseSound = SoundID.Item5
    /// knockBack = 2f shootSpeed = 1f
    /// 
    /// </summary>
    public abstract class SlingshotItem : ModItem
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Item.damage = 6;
            Item.width = 36;
            Item.height = 36;
            Item.useTime = 21;
            Item.useAnimation = 21;
            Item.crit = 8;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.rare = ItemRarityID.White;
            Item.value = Item.sellPrice(0, 0, 0, 50);


            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = false;

            Item.DamageType = DamageClass.Ranged;
            Item.UseSound = new SoundStyle("Everglow/Sources/Modules/MythModule/MiscItems/Weapons/Slingshots/Sounds/SlingshotUse"); //SoundID.Item5?

            Item.knockBack = 2f;
            Item.shootSpeed = 1f;

            SetDef();
            Item.shoot = ProjType;
        }
        public virtual void SetDef()
        {

        }
        internal int ProjType;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.ownedProjectileCounts[ProjType] < 1)
            {
                Projectile.NewProjectile(source, position, Vector2.Zero, ProjType, damage, knockback, player.whoAmI, Item.shootSpeed, Item.useAnimation);
            }
            return false;
        }
    }
}
