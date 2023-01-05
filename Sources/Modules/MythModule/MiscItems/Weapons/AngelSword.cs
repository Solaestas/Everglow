using ReLogic.Graphics;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons
{
    public class AngelSword : ModItem
    {
        public override void SetStaticDefaults()
        {// TODO: Localization Needed
            //DisplayName.SetDefault("Angel Sword");
            //DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "天使契约");
            //Tooltip.SetDefault("Rightclick to release a light wave");
            //Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "右键释放强力金色光刃");
        }

        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 64;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 50;
            Item.useAnimation = 20;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Melee;
            Item.damage = 150;
            Item.knockBack = 6;
            Item.crit = 16;

            Item.value = Item.buyPrice(gold: 5);
            Item.rare = 8;
            Item.UseSound = SoundID.Item1;

            Item.shoot = ModContent.ProjectileType<MiscProjectiles.Weapon.Melee.AngelSword>(); // ID of the projectiles the sword will shoot
            Item.shootSpeed = 24f; // Speed of the projectiles the sword will shoot
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                Projectile.NewProjectile(source, position + new Vector2(0, -24), velocity, ModContent.ProjectileType<MiscProjectiles.Weapon.Melee.AngelSword2>(), damage, knockback, player.whoAmI, 0f, 0f);
                CoolRarr = 300;
                Item.noUseGraphic = true;
                Item.noMelee = true;
                return false;
            }
            Item.noUseGraphic = false;
            Item.noMelee = false;
            return true;
        }
        public override bool AltFunctionUse(Player player)
        {
            return CoolRarr <= 0;
        }
        int CoolRarr = 0;
        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Vector2 slotSize = new Vector2(52f, 52f);
            position -= slotSize * Main.inventoryScale / 2f - frame.Size() * scale / 2f;
            Vector2 drawPos = position + slotSize * Main.inventoryScale / 2f/* - texture.Size() * Main.inventoryScale / 2f*/;

            Texture2D RArr = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/MiscProjectiles/Weapon/Melee/RightAngSword").Value;
            if (!Main.gamePaused)
            {
                if (CoolRarr > 0)
                {
                    CoolRarr--;
                    spriteBatch.Draw(RArr, drawPos + new Vector2(14), null, new Color(0, 0, 0, 255), 0f, new Vector2(8), 1f, SpriteEffects.None, 0f);
                    Main.spriteBatch.DrawString(FontAssets.MouseText.Value, ((int)(CoolRarr / 60f)).ToString(), drawPos + new Vector2(12), Color.Red, 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0);
                }
                else
                {
                    CoolRarr = 0;
                    spriteBatch.Draw(RArr, drawPos + new Vector2(14), null, new Color(255, 255, 255, 0), 0f, new Vector2(8), 1f, SpriteEffects.None, 0f);
                }
            }
        }
    }
}
