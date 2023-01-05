using Terraria.DataStructures;
using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Fragrans
{
    public class FragransGun : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fragrans Flame");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "星火流芳");
            Tooltip.SetDefault("Don't be lonely when moon and fragrans bright, breeze and moon still.");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "花好月圆时,愿清风与你相伴");
        }
        public static short GetGlowMask = 0;
        private int o = 0;
        public override void SetDefaults()
        {
            Item.glowMask = ItemGlowManager.GetItemGlow(this);
            // Common Properties
            Item.width = 52;
            Item.height = 34;
            Item.rare = 10;
            Item.value = 200000;

            // Use Properties
            Item.useTime = 9;
            Item.useAnimation = 9;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item36;

            // Weapon Properties
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 200;
            Item.knockBack = 6f;
            Item.noMelee = true;

            // Gun Properties
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 20f;
            Item.useAmmo = AmmoID.Bullet;
        }
        int Coun = 0;
        /*public override bool CanUseItem(Player player)
		{
			return File.Exists(Main.WorldPath + Main.LocalPlayer.name + "MoonFraGun.json");
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
							File.Create(Main.WorldPath + Main.LocalPlayer.name + "MoonFraGun.json").Close();
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
			if (!File.Exists(Main.WorldPath + Main.LocalPlayer.name + "MoonFraGun.json"))
			{
				spriteBatch.Draw(texture, drawPos, null, drawColor * alpha, 0f, textureOrigin, Main.inventoryScale, SpriteEffects.None, 0f);
			}*/
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Coun++;
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
            if (Coun == 5)
            {
                Coun = 0;
                const int NumProjectiles = 10;

                for (int i = 0; i < NumProjectiles; i++)
                {
                    Vector2 Vc = velocity.RotatedByRandom(MathHelper.ToRadians(15)) * 1.1f;
                    Vc *= 1f - Main.rand.NextFloat(0.3f);
                    Projectile.NewProjectileDirect(source, position, Vc, ModContent.ProjectileType<MiscProjectiles.Weapon.Fragrans.FragransBullet>(), (int)(Dam * 1.1f), knockback, player.whoAmI);
                }
            }
            for (int i = 0; i < 3; i++)
            {
                Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(1));
                newVelocity *= 2f - Main.rand.NextFloat(0.15f);
                newVelocity = newVelocity.RotatedByRandom(0.15f);
                Projectile.NewProjectileDirect(source, position - velocity * 0.7f, newVelocity, ModContent.ProjectileType<MiscProjectiles.Weapon.Fragrans.FragransGun>(), (int)(Dam * 0.8f), knockback, player.whoAmI, player.GetCritChance(DamageClass.Ranged));
            }
            return false;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-20f, -2f);
        }
    }
}
