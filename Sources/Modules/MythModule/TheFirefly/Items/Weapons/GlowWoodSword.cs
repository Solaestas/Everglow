using Everglow.Sources.Modules.MythModule.TheFirefly.Projectiles;
using Everglow.Sources.Modules.MythModule.TheFirefly.WorldGeneration;
using Everglow.Sources.Modules.MythModule.Common;
using Terraria.DataStructures;
using ReLogic.Graphics;
using Terraria.GameContent;
using Terraria.ID;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Items.Weapons
{
    public class GlowWoodSword : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemGlowManager.AutoLoadItemGlow(this);
        }
        public override void SetDefaults()
        {
            Item.glowMask = ItemGlowManager.GetItemGlow(this);
            Item.damage = 13;
            Item.DamageType = DamageClass.Melee;
            Item.width = 56;
            Item.height = 56;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5f;
            Item.value = Item.sellPrice(0, 0, 0, 70);
            Item.rare = ItemRarityID.White;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = false;
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (player.itemAnimation % 4 == 2)
            {
                Vector2 v0 = new Vector2(-3 * player.direction, -7 * player.gravDir).RotatedBy((player.itemAnimationMax - player.itemAnimation) * 0.13 * player.direction * player.gravDir + Math.Sin(Main.timeForVisualEffects / 16) * 0.3);
                Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.Center + v0 * 3,v0, ModContent.ProjectileType<Projectiles.GlowWoodSword>(), (int)(Item.damage * 0.3f), Item.knockBack, player.whoAmI);
            }
            base.MeleeEffects(player, hitbox);
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<GlowWood>(), 12);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
