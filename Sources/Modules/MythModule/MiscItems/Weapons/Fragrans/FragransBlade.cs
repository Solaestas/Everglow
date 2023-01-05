using Terraria.DataStructures;
using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Fragrans
{
    public class FragransBlade : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Golden Night");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "金秋爽夜");
            Tooltip.SetDefault("Don't be lonely when moon and fragrans bright, breeze and moon still.");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "花好月圆时,愿清风与你相伴");
            ItemGlowManager.AutoLoadItemGlow(this);
        }
        public static short GetGlowMask = 0;
        private int o = 0;
        public override void SetDefaults()
        {
            Item.glowMask = ItemGlowManager.GetItemGlow(this);
            Item.damage = 240;//伤害 原75→现37
            Item.DamageType = DamageClass.Melee; // Makes the damage register as magic. If your item does not have any damage type, it becomes true damage (which means that damage scalars will not affect it). Be sure to have a damage type.
            Item.width = 74;
            Item.height = 80;
            Item.useTime = 17;
            Item.useAnimation = 17;
            Item.useStyle = ItemUseStyleID.Swing; // Makes the player use a 'Shoot' use style for the Item.
            Item.noMelee = true; // Makes the item not do damage with it's melee hitbox.
            Item.noUseGraphic = true;
            Item.knockBack = 6;
            Item.value = 200000;
            Item.rare = 10;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<MiscProjectiles.Weapon.Fragrans.FragransBlade>(); // Shoot a black bolt, also known as the projectile shot from the onyx blaster.
            Item.shootSpeed = 8; // How fast the item shoots the projectile.
            Item.crit = 8; // The percent chance at hitting an enemy with a crit, plus the default amount of 4.
        }
        private int l = 0;
        /*public override bool CanUseItem(Player player)
        {
            return File.Exists(Main.WorldPath + Main.LocalPlayer.name + "MoonFraBlade.json");
        }
        public override bool OnPickup(Player player)
        {
            for(int x = -10;x < 11;x++)
            {
                for (int y = -10; y < 11; y++)
                {
                    if(Main.tile[x + (int)(player.Center.X / 16f), y + (int)(player.Center.Y / 16f)].TileType == ModContent.TileType<Tiles.MoonFragrans>())
                    {
                        if (File.Exists(Main.WorldPath + Main.LocalPlayer.name + "MoonFra.json"))
                        {
                            File.Create(Main.WorldPath + Main.LocalPlayer.name + "MoonFraBlade.json").Close();
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
            if (l % 4 == 0)
            {
                type = ModContent.ProjectileType<MiscProjectiles.Weapon.Fragrans.FragransBlade>();
            }
            else if (l % 4 == 1)
            {
                type = ModContent.ProjectileType<MiscProjectiles.Weapon.Fragrans.FragransBlade2>();
            }
            else if (l % 4 == 2)
            {
                type = ModContent.ProjectileType<MiscProjectiles.Weapon.Fragrans.FragransBlade>();
            }
            else
            {
                type = ModContent.ProjectileType<MiscProjectiles.Weapon.Fragrans.FragransBlade2>();
            }
            Projectile.NewProjectile(source, position + new Vector2(0, -24), velocity, type, damage, knockback, player.whoAmI, 0f, 0f);
            l++;
            return false;
        }
        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
			/*Texture2D texture = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/Ban").Value;
            Vector2 slotSize = new Vector2(52f, 52f);
            position -= slotSize * Main.inventoryScale / 2f - frame.Size() * scale / 2f;
            Vector2 drawPos = position + slotSize * Main.inventoryScale / 2f;
            float alpha = 0.8f;
            Vector2 textureOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
            if(!File.Exists(Main.WorldPath + Main.LocalPlayer.name + "MoonFraBlade.json"))
            {
                spriteBatch.Draw(texture, drawPos, null, drawColor * alpha, 0f, textureOrigin, Main.inventoryScale, SpriteEffects.None, 0f);
            }*/
		}
	}
}
