using Terraria.DataStructures;
using Terraria.Localization;
namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Fragrans
{
    public class FragransBoomerang : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Swirl Cloud At Night");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "旋云舞月");
            Tooltip.SetDefault("Don't be lonely when moon and fragrans bright, breeze and moon still.");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "花好月圆时,愿清风与你相伴");
			ItemGlowManager.AutoLoadItemGlow(this);
		}
        public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
			Item.glowMask = ItemGlowManager.GetItemGlow(this);
			Item.useStyle = 1;
            Item.shootSpeed = 17f;
            Item.shoot = ModContent.ProjectileType<MiscItems.Projectiles.Weapon.Fragrans.FragransBoomerang>();
            Item.width = 68;
            Item.height = 68;
            Item.UseSound = SoundID.Item1;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.rare = 10;
            Item.damage = 270;
            Item.autoReuse = true;
            Item.value = Item.sellPrice(0, 20, 0, 0);
            Item.DamageType = DamageClass.Melee;
        }
        private int l = 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float Dam = damage;
            if (MiscItems.Projectiles.Weapon.Fragrans.Fragrans.FragransIndex == 1)
            {
                Dam *= player.GetDamage(DamageClass.Melee).Additive;
            }
            if (MiscItems.Projectiles.Weapon.Fragrans.Fragrans.FragransIndex == 2)
            {
                Dam *= player.GetDamage(DamageClass.Melee).Additive;
                Dam *= player.GetDamage(DamageClass.Melee).Additive;
            }
            if (MiscItems.Projectiles.Weapon.Fragrans.Fragrans.FragransIndex == 3)
            {
                Dam *= player.GetDamage(DamageClass.Melee).Additive;
                Dam *= player.GetDamage(DamageClass.Melee).Additive;
                Dam *= player.GetDamage(DamageClass.Melee).Additive;
            }
            Projectile.NewProjectile(source, position + new Vector2(0, -24), velocity, type, damage, knockback, Main.LocalPlayer.whoAmI, 0f, 0f);
            l++;
            return false;
        }
        /*public override bool CanUseItem(Player player)
		{
			return File.Exists(Main.WorldPath + Main.LocalPlayer.name + "MoonFraBoomerang.json");
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
							File.Create(Main.WorldPath + Main.LocalPlayer.name + "MoonFraBoomerang.json").Close();
						}
						x = 15;
						break;
					}
				}
			}
			return base.OnPickup(player);
		}*/
        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            /*Texture2D texture = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/Ban").Value;
			Vector2 slotSize = new Vector2(52f, 52f);
			position -= slotSize * Main.inventoryScale / 2f - frame.Size() * scale / 2f;
			Vector2 drawPos = position + slotSize * Main.inventoryScale / 2f;
			float alpha = 0.8f;
			Vector2 textureOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
			if (!File.Exists(Main.WorldPath + Main.LocalPlayer.name + "MoonFraBoomerang.json"))
			{
				spriteBatch.Draw(texture, drawPos, null, drawColor * alpha, 0f, textureOrigin, Main.inventoryScale, SpriteEffects.None, 0f);
			}*/
        }
    }
}
