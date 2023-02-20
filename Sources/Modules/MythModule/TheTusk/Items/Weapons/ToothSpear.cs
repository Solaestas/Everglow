using Everglow.Sources.Modules.MythModule.Common;
using ReLogic.Graphics;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;

namespace Everglow.Sources.Modules.MythModule.TheTusk.Items.Weapons
{
    public class ToothSpear : ModItem
    {
        public override void SetStaticDefaults()
        {
            /*DisplayName.SetDefault("Tooth Spear");
			DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "血龙脊");
			Tooltip.SetDefault("Devine Weapon\nUse it to kill a dragon\nRight click allow you dash,no Sync,cooling for 8s");
			Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "神话武器\n去吧,效仿上古的勇士,拿它屠龙\n右键冲刺,无共鸣时,冷却8秒");*/
        }

        public override void SetDefaults()
        {
            // Common Properties
            Item.rare = 3; // Assign this item a rarity level of Pink
            Item.value = Item.sellPrice(silver: 50); // The number and type of coins item can be sold for to an NPC

            // Use Properties
            Item.useStyle = ItemUseStyleID.Shoot; // How you use the item (swinging, holding out, etc.)
            Item.useAnimation = 24; // The length of the item's use animation in ticks (60 ticks == 1 second.)
            Item.useTime = 24; // The length of the item's use time in ticks (60 ticks == 1 second.)
            Item.UseSound = SoundID.Item71; // The sound that this item plays when used.
            Item.autoReuse = true; // Allows the player to hold click to automatically use the item again. Most spears don't autoReuse, but it's possible when used in conjunction with CanUseItem()		

            // Weapon Properties
            Item.damage = 38;
            Item.crit = 12;
            Item.knockBack = 6.5f;
            Item.noUseGraphic = true; // When true, the item's sprite will not be visible while the item is in use. This is true because the spear projectile is what's shown so we do not want to show the spear sprite as well.
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true; // Allows the item's animation to do damage. This is important because the spear is actually a projectile instead of an item. This prevents the melee hitbox of this item.

            // Projectile Properties
            Item.shootSpeed = 3.7f; // The speed of the projectile measured in pixels per frame.
            Item.shoot = ModContent.ProjectileType<Projectiles.Weapon.ToothSpear>(); //The projectile that is fired from this weapon
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.useTime = 60;
                Item.useAnimation = 18;
            }
            else
            {
                Item.useTime = 18;
                Item.useAnimation = 18;
            }
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            MythContentPlayer myplayer = player.GetModPlayer<MythContentPlayer>();
            if (player.altFunctionUse == 2 && myplayer.Dashcool < 1)
            {
                CoolRarr = 480 - (int)(myplayer.StackDamageAdd / 0.05f * 90);
                myplayer.Dashcool = 480 - (int)(myplayer.StackDamageAdd / 0.05f * 90);
                Projectile.NewProjectile(source, position, velocity * 6, ModContent.ProjectileType<Projectiles.Weapon.ToothSpear2>(), damage * 2, knockback, player.whoAmI, 0f, 0f);
                player.velocity += velocity * 6;
                return false;
            }
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
        int CoolRarr = 0;
        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Vector2 slotSize = new Vector2(52f, 52f);
            position -= slotSize * Main.inventoryScale / 2f - frame.Size() * scale / 2f;
            Vector2 drawPos = position + slotSize * Main.inventoryScale / 2f/* - texture.Size() * Main.inventoryScale / 2f*/;
            Texture2D RArr = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/Items/Weapons/RightBloodSpice").Value;
            if (!Main.gamePaused)
            {
                if (CoolRarr > 0)
                {
                    CoolRarr--;
                    spriteBatch.Draw(RArr, drawPos + new Vector2(21) * scale, null, new Color(0, 0, 0, 255), 0f, new Vector2(8), scale * 1.5f, SpriteEffects.None, 0f);
                    Main.spriteBatch.DrawString(FontAssets.MouseText.Value, ((int)(CoolRarr / 60f)).ToString(), drawPos + new Vector2(18) * scale, Color.Red, 0f, Vector2.Zero, scale * 1.5f, SpriteEffects.None, 0);
                }
                else
                {
                    CoolRarr = 0;
                    spriteBatch.Draw(RArr, drawPos + new Vector2(21) * scale, null, new Color(255, 255, 255, 150), 0f, new Vector2(8), scale * 1.5f, SpriteEffects.None, 0f);
                }
            }
        }
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
        public override bool? UseItem(Player player)
        {
            if (!Main.dedServ)
            {
                SoundEngine.PlaySound(Item.UseSound, player.Center);
            }

            return null;
        }
    }
}
