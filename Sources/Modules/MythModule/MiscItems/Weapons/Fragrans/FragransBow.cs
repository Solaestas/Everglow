using ReLogic.Graphics;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Fragrans
{
    public class FragransBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fragrans Branch");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "惊风射月");
            Tooltip.SetDefault("Don't be lonely when moon and fragrans bright, breeze and moon still.");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "花好月圆时,愿清风与你相伴");
            ItemGlowManager.AutoLoadItemGlow(this);
        }
        public static short GetGlowMask = 0;
        private int o = 0;
        public override void SetDefaults()
        {
            Item.glowMask = ItemGlowManager.GetItemGlow(this);
            Item.width = 42;
            Item.height = 70;
            Item.rare = 10;

            Item.useTime = 6;
            Item.useAnimation = 6;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item5;

            Item.DamageType = DamageClass.Ranged;
            Item.damage = 245;
            Item.knockBack = 15f;
            Item.noMelee = true;

            Item.shoot = ModContent.ProjectileType<MiscProjectiles.Weapon.Fragrans.FragransArrow>();
            Item.shootSpeed = 37f;
            Item.useAmmo = AmmoID.Arrow;
        }
        private int l = 0;
        /*public override bool CanUseItem(Player player)
        {
            return File.Exists(Main.WorldPath + Main.LocalPlayer.name + "MoonFraBow.json");
        }
        public override bool OnPickup(Player player)
        {
            for (int x = -10; x < 11; x++)
            {
                for (int y = -10; y < 11; y++)
                {
                    if (Main.tile[x + (int)(player.Center.X / 16f), y + (int)(player.Center.Y / 16f)].TileType == ModContent.TileType<Tiles.MoonFragrans>())
                    {
                        if (File.Exists(Main.WorldPath + Main.LocalPlayer.name + "MoonFra.json"))
                        {
                            File.Create(Main.WorldPath + Main.LocalPlayer.name + "MoonFraBow.json");
                        }
                        x = 15;
                        break;
                    }
                }
            }
            return base.OnPickup(player);
        }*/
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float Dam = damage;
            if (MiscProjectiles.Weapon.Fragrans.Fragrans.FragransIndex == 1)
            {
                Dam *= player.GetDamage(DamageClass.Ranged).Additive;
            }
            if (MiscProjectiles.Weapon.Fragrans.Fragrans.FragransIndex == 2)
            {
                Dam *= player.GetDamage(DamageClass.Ranged).Additive;
                Dam *= player.GetDamage(DamageClass.Ranged).Additive;
            }
            if (MiscProjectiles.Weapon.Fragrans.Fragrans.FragransIndex == 3)
            {
                Dam *= player.GetDamage(DamageClass.Ranged).Additive;
                Dam *= player.GetDamage(DamageClass.Ranged).Additive;
                Dam *= player.GetDamage(DamageClass.Ranged).Additive;
            }
            if (player.altFunctionUse == 2)
            {
                for (int h = 0; h < 200; h++)
                {
                    if ((Main.npc[h].Center - player.Center).Length() < 2048 && !Main.npc[h].dontTakeDamage && !Main.npc[h].friendly && Main.npc[h].active && Main.npc[h].life >= 0)
                    {
                        Projectile.NewProjectile(source, position, Main.npc[h].velocity, ModContent.ProjectileType<MiscProjectiles.Weapon.Fragrans.FragransAim>(), 0, knockback, player.whoAmI, h, Dam * 10);
                    }
                }
                CoolRarr = 900;
                return false;
            }
            type = ModContent.ProjectileType<MiscProjectiles.Weapon.Fragrans.FragransArrow>();

            Projectile.NewProjectile(source, position, velocity, type, (int)Dam, knockback, player.whoAmI, 0f);
            return false;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-16f, 0f);
        }
        public override bool AltFunctionUse(Player player)
        {
            return CoolRarr <= 0;
        }
        int CoolRarr = 0;
        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            /*Texture2D texture = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/Ban").Value;
			Vector2 slotSize = new Vector2(52f, 52f);
			position -= slotSize * Main.inventoryScale / 2f - frame.Size() * scale / 2f;
			Vector2 drawPos = position + slotSize * Main.inventoryScale / 2f;
			float alpha = 0.8f;
			Vector2 textureOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
			if (!File.Exists(Main.WorldPath + Main.LocalPlayer.name + "MoonFraGun.json"))
			{
				spriteBatch.Draw(texture, drawPos, null, drawColor * alpha, 0f, textureOrigin, Main.inventoryScale, SpriteEffects.None, 0f);
			}*/
            Vector2 slotSize = new Vector2(52f, 52f);
            position -= slotSize * Main.inventoryScale / 2f - frame.Size() * scale / 2f;
            Vector2 drawPos = position + slotSize * Main.inventoryScale / 2f;
            Texture2D RArr = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/MiscProjectiles/Weapon/Fragrans/RightFraArrow").Value;
            if (!Main.gamePaused)
            {
                if (CoolRarr > 0)
                {
                    CoolRarr--;
                    spriteBatch.Draw(RArr, drawPos + new Vector2(26.73f) * scale, null, new Color(0, 0, 0, 255), 0f, new Vector2(8), scale * 1.91f, SpriteEffects.None, 0f);
                    Main.spriteBatch.DrawString(FontAssets.MouseText.Value, ((int)(CoolRarr / 60f)).ToString(), drawPos + new Vector2(22.91f) * scale, Color.Red, 0f, Vector2.Zero, scale * 1.91f, SpriteEffects.None, 0);
                }
                else
                {
                    CoolRarr = 0;
                    spriteBatch.Draw(RArr, drawPos + new Vector2(26.73f) * scale, null, new Color(255, 255, 255, 150), 0f, new Vector2(8), scale * 1.91f, SpriteEffects.None, 0f);
                }
            }
        }
    }
}
