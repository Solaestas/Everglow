using Terraria.Audio;
using Terraria.DataStructures;


namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Legendary
{
    public class SilenceMirror : ModItem
    {
        public override void SetStaticDefaults()
        {
            /*DisplayName.SetDefault("Silence Mirror");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "寂镜");
            Tooltip.SetDefault("Divine Weapon\nContain 2 knifes, mouse right to change\nCombo to increase damage\nBattle never happen out of the world");//\nDamage:\n       Bloody Mirror  Silence Soul\nFirstSwing     81%        64%\nSecondSwing  100%        128%\nThirdSwing   121%       160%\nForthSwing   144%       176%\nFifthSwing  169%       184%\nSixthSwing  196%       188%\nSeventhSwing 225%       190%\nEighthSwing  256%      191%
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "神话武器\n一共两把刀,鼠标右键切换\n连击以提升伤害\n超脱凡尘之外便再无争战");\n血色镜    寂魂\n一段81%    64%\n二段100%    128%\n三段121%   160%\n四段144%   176%\n五段169%   184%\n六段196%   188%\n七段225%   190%\n八段256%   191%*/
            ItemGlowManager.AutoLoadItemGlow(this);
        }
        public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
            Item.glowMask = ItemGlowManager.GetItemGlow(this);
            Mod mod;
            if (!ModLoader.TryGetMod("MythModBeta", out mod))
            {
                Item.damage = 60;
            }
            else
            {
                Item.damage = 240;
            }
            Item.DamageType = DamageClass.Melee; // Makes the damage register as magic. If your item does not have any damage type, it becomes true damage (which means that damage scalars will not affect it). Be sure to have a damage type.
            Item.width = 96;
            Item.height = 198;
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Swing; // Makes the player use a 'Shoot' use style for the Item.
            Item.noMelee = true; // Makes the item not do damage with it's melee hitbox.
            Item.noUseGraphic = true;
            Item.knockBack = 6;
            Item.value = 200000;
            Item.rare = 11;
            Item.UseSound = new SoundStyle("Everglow/Sources/Modules/MythModule/Sounds/Knife");
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<MiscProjectiles.Weapon.Legendary.SlienceMirror>(); // Shoot a black bolt, also known as the projectile shot from the onyx blaster.
            Item.shootSpeed = 8; // How fast the item shoots the projectile.
            Item.crit = 25; // The percent chance at hitting an enemy with a crit, plus the default amount of 4.
        }
        private int l = 0;
        private int Tim2 = 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float Dam = damage;
            if (KTy == 0)
            {
                Item.UseSound = new SoundStyle("Everglow/Sources/Modules/MythModule/Sounds/Knife");
                if (l % 8 == 0)
                {
                    Dam *= 0.64f;
                    Item.useTime = 18;
                    Item.useAnimation = 18;
                    type = ModContent.ProjectileType<MiscProjectiles.Weapon.Legendary.SlienceMirror>();
                }
                else if (l % 8 == 1)
                {
                    Dam *= 0.81f;
                    Item.useTime = 18;
                    Item.useAnimation = 18;
                    type = ModContent.ProjectileType<MiscProjectiles.Weapon.Legendary.SlienceMirror2>();
                }
                else if (l % 8 == 2)
                {
                    Dam *= 1.00f;
                    Item.useTime = 16;
                    Item.useAnimation = 16;
                    type = ModContent.ProjectileType<MiscProjectiles.Weapon.Legendary.SlienceMirror>();
                }
                else if (l % 8 == 3)
                {
                    Dam *= 1.21f;
                    Item.useTime = 16;
                    Item.useAnimation = 16;
                    type = ModContent.ProjectileType<MiscProjectiles.Weapon.Legendary.SlienceMirror3>();
                }
                else if (l % 8 == 4)
                {
                    Dam *= 1.44f;
                    Item.useTime = 16;
                    Item.useAnimation = 16;
                    type = ModContent.ProjectileType<MiscProjectiles.Weapon.Legendary.SlienceMirror>();
                }
                else if (l % 8 == 5)
                {
                    Dam *= 1.69f;
                    Item.useTime = 16;
                    Item.useAnimation = 16;
                    type = ModContent.ProjectileType<MiscProjectiles.Weapon.Legendary.SlienceMirror2>();
                }
                else if (l % 8 == 6)
                {
                    Dam *= 1.96f;
                    Item.useTime = 16;
                    Item.useAnimation = 16;
                    type = ModContent.ProjectileType<MiscProjectiles.Weapon.Legendary.SlienceMirror>();
                }
                else if (l % 8 == 7)
                {
                    Dam *= 2.25f;
                    Item.useTime = 16;
                    Item.useAnimation = 16;
                    type = ModContent.ProjectileType<MiscProjectiles.Weapon.Legendary.SlienceMirror6>();
                }
                cooling2 = 36;
            }
            if (KTy == 1)
            {
                Item.UseSound = new SoundStyle("Everglow/Sources/Modules/MythModule/Sounds/FlameSword");
                if (l % 8 == 0)
                {
                    Item.useTime = 21;
                    Item.useAnimation = 21;
                    Dam *= 0.64f;
                    type = ModContent.ProjectileType<MiscProjectiles.Weapon.Legendary.SlienceMirrorII>();
                }
                else if (l % 8 == 1)
                {
                    Item.useTime = 21;
                    Item.useAnimation = 21;
                    Dam *= 1.28f;
                    type = ModContent.ProjectileType<MiscProjectiles.Weapon.Legendary.SlienceMirrorII2>();
                }
                else if (l % 8 == 2)
                {
                    Item.useTime = 21;
                    Item.useAnimation = 21;
                    Dam *= 1.60f;
                    type = ModContent.ProjectileType<MiscProjectiles.Weapon.Legendary.SlienceMirrorII3>();
                }
                else if (l % 8 == 3)
                {
                    Item.useTime = 21;
                    Item.useAnimation = 21;
                    Dam *= 1.76f;
                    type = ModContent.ProjectileType<MiscProjectiles.Weapon.Legendary.SlienceMirrorII4>();
                }
                else if (l % 8 == 4)
                {
                    Item.useTime = 21;
                    Item.useAnimation = 21;
                    Dam *= 1.84f;
                    type = ModContent.ProjectileType<MiscProjectiles.Weapon.Legendary.SlienceMirrorII>();
                }
                else if (l % 8 == 5)
                {
                    Item.useTime = 21;
                    Item.useAnimation = 21;
                    Dam *= 1.88f;
                    type = ModContent.ProjectileType<MiscProjectiles.Weapon.Legendary.SlienceMirrorII3>();
                }
                else if (l % 8 == 6)
                {
                    Item.useTime = 21;
                    Item.useAnimation = 21;
                    Dam *= 1.90f;
                    type = ModContent.ProjectileType<MiscProjectiles.Weapon.Legendary.SlienceMirrorII2>();
                }
                else if (l % 8 == 7)
                {
                    Item.useTime = 21;
                    Item.useAnimation = 21;
                    Dam *= 1.91f;
                    type = ModContent.ProjectileType<MiscProjectiles.Weapon.Legendary.SlienceMirrorII4>();
                }
                cooling2 = 64;
            }
            Projectile.NewProjectile(source, position + new Vector2(0, -24), velocity, type, (int)Dam, knockback, player.whoAmI, 0f, 0f);
            l++;
            return false;
        }
        int KTy = 0;
        int cooling = 0;
        int cooling2 = 0;
        Vector2 vz0 = new Vector2(-18, -50);
        Vector2 vz1 = new Vector2(18, 50);
        Vector2 vO = new Vector2(0, 100);
        public override void UpdateInventory(Player player)
        {
            int heldItem = player.HeldItem.type;
            if (heldItem == Item.type)
            {
                if (Main.mouseRight)
                {
                    if (KTy == 0 && cooling <= 0)
                    {
                        Item.UseSound = new SoundStyle("Everglow/Sources/Modules/MythModule/Sounds/FlameSword");
                        cooling = 15;
                        KTy = 1;
                    }
                    if (KTy == 1 && cooling <= 0)
                    {
                        Item.UseSound = new SoundStyle("Everglow/Sources/Modules/MythModule/Sounds/Knife");
                        cooling = 15;
                        KTy = 0;
                        Tim2 = 0;
                    }
                }
            }
            if (KTy == 1)
            {
                Tim2++;
            }
            else
            {
                Tim2 = 0;
            }
            base.UpdateInventory(player);
        }
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            if (cooling > 0)
            {
                cooling--;
            }
            else
            {
                cooling = 0;
            }
            if (cooling2 > 0)
            {
                cooling2--;
            }
            else
            {
                cooling2 = 0;
                if (Type == 0)
                {
                    Item.useTime = 16;
                    Item.useAnimation = 16;
                }
                if (Type == 1)
                {
                    Item.useTime = 21;
                    Item.useAnimation = 21;
                }
                l = 0;
            }
            vz0 = new Vector2(-10, -50).RotatedBy(cooling / 15d * MathHelper.Pi);
            vz1 = new Vector2(10, 50).RotatedBy(cooling / 15d * MathHelper.Pi);
            Vector2 vPos0 = vO + vz0;
            Vector2 vPos1 = vO + vz1;
            Texture2D t1 = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/MiscItems/Weapons/Legendary/SilenceMirror1").Value;
            Texture2D t2 = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/MiscItems/Weapons/Legendary/SilenceMirror2").Value;
            Vector2 slotSize = new Vector2(52f, 52f);
            position -= slotSize * Main.inventoryScale / 2f - frame.Size() * scale / 2f;
            Vector2 drawPos = position + slotSize * Main.inventoryScale / 2f;
            float alpha = 1f;
            Vector2 textureOrigin = new Vector2(t1.Width / 2, t1.Height / 2);
            if (KTy == 0)
            {
                spriteBatch.Draw(t2, drawPos + new Vector2(vPos1.X * 27 / vPos1.Y + 6, vPos1.Y * 0.1f - 4), null, drawColor * alpha, 0f, textureOrigin, Main.inventoryScale * 14 / (vPos1.Y), SpriteEffects.None, 0f);
                spriteBatch.Draw(t1, drawPos + new Vector2(vPos0.X * 27 / vPos0.Y + 6, vPos0.Y * 0.1f - 4), null, drawColor * alpha, 0f, textureOrigin, Main.inventoryScale * 14 / (vPos0.Y), SpriteEffects.None, 0f);

            }
            if (KTy == 1)
            {
                spriteBatch.Draw(t1, drawPos + new Vector2(vPos1.X * 27 / vPos1.Y + 6, vPos1.Y * 0.1f - 4), null, drawColor * alpha, 0f, textureOrigin, Main.inventoryScale * 14 / (vPos1.Y), SpriteEffects.None, 0f);
                spriteBatch.Draw(t2, drawPos + new Vector2(vPos0.X * 27 / vPos0.Y + 6, vPos0.Y * 0.1f - 4), null, drawColor * alpha, 0f, textureOrigin, Main.inventoryScale * 14 / (vPos0.Y), SpriteEffects.None, 0f);

            }
            return false;
        }
    }
}
