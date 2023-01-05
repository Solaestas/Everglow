using Terraria.DataStructures;
using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Fragrans
{
    public class FragransStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fragrans Bomb Staff");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "闪耀金花");
            Tooltip.SetDefault("Don't be lonely when moon and fragrans bright, breeze and moon still.");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "花好月圆时,愿清风与你相伴");
            ItemGlowManager.AutoLoadItemGlow(this);
        }
        public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
            Item.glowMask = ItemGlowManager.GetItemGlow(this);
            Item.damage = 187;//26→19
            Item.DamageType = DamageClass.Magic; // Makes the damage register as magic. If your item does not have any damage type, it becomes true damage (which means that damage scalars will not affect it). Be sure to have a damage type.
            Item.width = 60;
            Item.height = 58;
            Item.useTime = 17;
            Item.useAnimation = 17;
            Item.useStyle = ItemUseStyleID.Shoot; // Makes the player use a 'Shoot' use style for the Item.
            Item.noMelee = true; // Makes the item not do damage with it's melee hitbox.
            Item.knockBack = 6;
            Item.value = 200000;
            Item.rare = 10;
            Item.UseSound = SoundID.Item71;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<MiscProjectiles.Weapon.Fragrans.FragransMagicBeta>(); // Shoot a black bolt, also known as the projectile shot from the onyx blaster.
            Item.shootSpeed = 10; // How fast the item shoots the projectile.
            Item.crit = 3; // The percent chance at hitting an enemy with a crit, plus the default amount of 4.
            Item.mana = 33; //6→13
        }
        int Coun = 0;
        /*public override bool CanUseItem(Player player)
		{
			return File.Exists(Main.WorldPath + Main.LocalPlayer.name + "MoonFraStaff.json");
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
							File.Create(Main.WorldPath + Main.LocalPlayer.name + "MoonFraStaff.json");
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
			Vector2 drawPos = position + slotSize * Main.inventoryScale / 2f/* - texture.Size() * Main.inventoryScale;
			float alpha = 0.8f;
			Vector2 textureOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
			if (!File.Exists(Main.WorldPath + Main.LocalPlayer.name + "MoonFraStaff.json"))
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
                Dam *= player.GetDamage(DamageClass.Magic).Additive;
            }
            if (MiscProjectiles.Weapon.Fragrans.Fragrans.FragransIndex == 2)
            {
                Dam *= player.GetDamage(DamageClass.Magic).Additive;
                Dam *= player.GetDamage(DamageClass.Magic).Additive;
            }
            if (MiscProjectiles.Weapon.Fragrans.Fragrans.FragransIndex == 3)
            {
                Dam *= player.GetDamage(DamageClass.Magic).Additive;
                Dam *= player.GetDamage(DamageClass.Magic).Additive;
                Dam *= player.GetDamage(DamageClass.Magic).Additive;
            }
            int NumProjectiles = 8;

            for (int i = 0; i < NumProjectiles; i++)
            {
                float ad = 0.3f;
                Projectile.NewProjectileDirect(source, Main.MouseWorld, velocity.RotatedByRandom(6.28) * Main.rand.NextFloat(0.85f, 1.15f), ModContent.ProjectileType<MiscProjectiles.Weapon.Fragrans.FragransMagicBeta>(), (int)(Dam * 0.4f), knockback, player.whoAmI, ad * Main.rand.NextFloat(0.0f, 0.35f));
            }
            Projectile.NewProjectileDirect(source, Main.MouseWorld, Vector2.Zero, ModContent.ProjectileType<MiscProjectiles.Weapon.Fragrans.FragransBomb>(), (int)Dam * 2, knockback, player.whoAmI, 0);
            return false;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-20f, -2f);
        }
    }
}
