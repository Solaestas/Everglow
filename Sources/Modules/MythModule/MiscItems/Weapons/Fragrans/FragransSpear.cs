using ReLogic.Graphics;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Fragrans
{
    public class FragransSpear : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Moon Dosn't Shine");
            //DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "月色无芒");
            //Tooltip.SetDefault("Don't be lonely when moon and fragrans bright, breeze and moon still.");
            //Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "花好月圆时,愿清风与你相伴");
            ItemGlowManager.AutoLoadItemGlow(this);
        }
        public static short GetGlowMask = 0;
        private int o = 0;
        public override void SetDefaults()
        {
            Item.glowMask = ItemGlowManager.GetItemGlow(this);
            Item.damage = 250;
            Item.DamageType = DamageClass.Melee;
            Item.width = 74;
            Item.height = 80;
            Item.useTime = 17;
            Item.useAnimation = 17;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.knockBack = 6;
            Item.value = 200000;
            Item.rare = 10;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<MiscItems.Projectiles.Weapon.Fragrans.FragransBlade>();
            Item.shootSpeed = 8;
            Item.crit = 8;
        }
        private int l = 0;
        /*public override bool CanUseItem(Player player)
        {
            return File.Exists(Main.WorldPath + Main.LocalPlayer.name + "MoonFraSpear.json");
        }*/
        public override bool AltFunctionUse(Player player)
        {
            return CoolRarr <= 0;
        }
        /*public override bool OnPickup(Player player)
        {
            for(int x = -10;x < 11;x++)
            {
                for (int y = -10; y < 11; y++)
                {
                    if(Main.tile[x + (int)(player.Center.X / 16f), y + (int)(player.Center.Y / 16f)].TileType == ModContent.TileType<Tiles.MoonFragrans>())
                    {
                        if (File.Exists(Main.WorldPath + Main.LocalPlayer.name + "MoonFra.json"))
                        {
                            File.Create(Main.WorldPath + Main.LocalPlayer.name + "MoonFraSpear.json").Close();
                        }
                        x = 15;
                        break;
                    }
                }
            }
            return base.OnPickup(player);
        }*/
        public static int coll = 0;
        public override void HoldItem(Player player)
        {
            if (Main.mouseRight)
            {
                coll = 18;
            }
            if (coll > 0)
            {
                player.maxFallSpeed = 200f;
                coll--;
            }
            else
            {
                coll = 0;
            }
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                CoolRarr = 60;
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<MiscItems.Projectiles.Weapon.Fragrans.FragransSpear4>(), damage * 5, knockback, player.whoAmI, 0f, 0f);
                return false;
            }
            int ai0 = 0;
            float Damk = 1;
            if (l % 5 == 0)
            {
                type = ModContent.ProjectileType<MiscItems.Projectiles.Weapon.Fragrans.FragransSpear>();
            }
            else if (l % 5 == 1)
            {
                type = ModContent.ProjectileType<MiscItems.Projectiles.Weapon.Fragrans.FragransSpear2>();
            }
            else if (l % 5 == 2)
            {
                type = ModContent.ProjectileType<MiscItems.Projectiles.Weapon.Fragrans.FragransSpear>();
            }
            else if (l % 5 == 3)
            {
                type = ModContent.ProjectileType<MiscItems.Projectiles.Weapon.Fragrans.FragransSpear3>();
                Damk = 1.33f;
            }
            else
            {
                type = ModContent.ProjectileType<MiscItems.Projectiles.Weapon.Fragrans.FragransSpear3>();
                Damk = 1.33f;
                ai0 = 1;
            }
            Projectile.NewProjectile(source, position + new Vector2(0, -24), velocity, type, (int)(damage * Damk), knockback, player.whoAmI, ai0, 0f);
            l++;
            return false;
        }
        public static int CoolRarr = 0;
        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            /*Texture2D texture = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/Ban").Value;
            Vector2 slotSize = new Vector2(52f, 52f);
            position -= slotSize * Main.inventoryScale / 2f - frame.Size() * scale / 2f;
            Vector2 drawPos = position + slotSize * Main.inventoryScale / 2f/* - texture.Size() * Main.inventoryScale;
            float alpha = 0.8f;
            Vector2 textureOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
            if(!File.Exists(Main.WorldPath + Main.LocalPlayer.name + "MoonFraSpear.json"))
            {
                spriteBatch.Draw(texture, drawPos, null, drawColor * alpha, 0f, textureOrigin, Main.inventoryScale, SpriteEffects.None, 0f);
            }*/
            Vector2 slotSize = new Vector2(52f, 52f);
            Vector2 drawPos = position + slotSize * Main.inventoryScale / 2f;
            Texture2D RArr = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/MiscItems/Projectiles/Weapon/Fragrans/RightFraSpice").Value;
            if (!Main.gamePaused)
            {
                if (CoolRarr > 0)
                {
                    CoolRarr--;
                    spriteBatch.Draw(RArr, drawPos + new Vector2(14f) * scale, null, new Color(0, 0, 0, 255), 0f, new Vector2(8), scale * 1.64f, SpriteEffects.None, 0f);
                    Main.spriteBatch.DrawString(FontAssets.MouseText.Value, ((int)(CoolRarr / 60f)).ToString(), drawPos + new Vector2(11f) * scale, Color.Red, 0f, Vector2.Zero, scale * 1.64f, SpriteEffects.None, 0);
                }
                else
                {
                    CoolRarr = 0;
                    spriteBatch.Draw(RArr, drawPos + new Vector2(14f) * scale, null, new Color(255, 255, 255, 150), 0f, new Vector2(8), scale * 1.64f, SpriteEffects.None, 0f);
                }
            }
        }
    }
}
