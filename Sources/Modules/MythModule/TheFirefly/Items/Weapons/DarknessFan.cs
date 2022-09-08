using Everglow.Sources.Modules.MythModule.TheFirefly.Projectiles;
using Everglow.Sources.Modules.MythModule.TheFirefly.WorldGeneration;
using Everglow.Sources.Modules.MythModule.Common;
using Terraria.DataStructures;
using ReLogic.Graphics;
using Terraria.GameContent;
using Terraria.ID;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Items.Weapons
{
    public class DarknessFan : ModItem
    {
        public override void SetStaticDefaults()
        {
            GetGlowMask = MythContent.SetStaticDefaultsGlowMask(this);
        }
        public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
            Item.glowMask = GetGlowMask;
            Item.damage = 17;
            Item.DamageType = DamageClass.Summon;
            Item.mana = 7;
            Item.width = 74;
            Item.height = 90;
            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.knockBack = 5f;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.DD2_GhastlyGlaivePierce;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.GlowingButterfly>();
            Item.shootSpeed = 8;
        }
        private int l = 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2 && CoolRarr == 0)
            {
                CoolRarr = 120;
                Projectile.NewProjectile(source, position + new Vector2(0, -24), velocity * 3.4f, ModContent.ProjectileType<Projectiles.DarkFanFly>(), damage * 2, knockback, player.whoAmI, 6 + player.maxMinions * 1.5f, 0f);
                Item.useTime = 2;
                Item.useAnimation = 2;
                //Item.UseSound = SoundID.DD2_JavelinThrowersAttack;
                return false;
            }
            if (l % 4 == 0)
            {
                type = ModContent.ProjectileType<Projectiles.DarkFan>();
            }
            else if (l % 4 == 1)
            {
                type = ModContent.ProjectileType<Projectiles.DarkFan>();
            }
            else if (l % 4 == 2)
            {
                type = ModContent.ProjectileType<Projectiles.DarkFan>();
            }
            else
            {
                type = ModContent.ProjectileType<Projectiles.DarkFan>();
            }
            Projectile.NewProjectile(source, position + new Vector2(0, -24), velocity, type, damage, knockback, player.whoAmI, 0f, 0f);
            Item.useTime = 36;
            Item.useAnimation = 36;
            l++;
            return false;
        }

        private int CoolRarr = 0;
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2 && CoolRarr == 0)
            {
                Item.useTime = 2;
                Item.useAnimation = 2;
            }
            else
            {
                Item.useTime = 36;
                Item.useAnimation = 36;
            }
            return true;
        }
        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Vector2 slotSize = new Vector2(52f, 52f);
            position -= slotSize * Main.inventoryScale / 2f - frame.Size() * scale / 2f;
            Vector2 drawPos = position + slotSize * Main.inventoryScale / 2f/* - texture.Size() * Main.inventoryScale / 2f*/;
            Texture2D RArr = MythContent.QuickTexture("TheFirefly/Projectiles/GlowFanTex/RightDFan");
                //ModContent.Request<Texture2D>("MythMod/UIImages/RightDFan").Value;
            if (!Main.gamePaused)
            {
                if (CoolRarr > 0)
                {
                    CoolRarr--;
                    spriteBatch.Draw(RArr, drawPos + new Vector2(33.6f) * scale, null, new Color(0, 0, 0, 255), 0f, new Vector2(8), scale * 2.4f, SpriteEffects.None, 0f);
                    Main.spriteBatch.DrawString(FontAssets.MouseText.Value, ((int)(CoolRarr / 60f)).ToString(), drawPos + new Vector2(28.8f) * scale, Color.Red, 0f, Vector2.Zero, scale * 2.4f, SpriteEffects.None, 0);
                }
                else
                {
                    CoolRarr = 0;
                    spriteBatch.Draw(RArr, drawPos + new Vector2(33.6f) * scale, null, new Color(255, 255, 255, 150), 0f, new Vector2(8), scale * 2.4f, SpriteEffects.None, 0f);
                }
            }
        }
        public override bool? UseItem(Player player)
        {
            if (player.itemAnimation == player.itemAnimationMax)
            {
                if (SubWorldModule.SubworldSystem.IsActive<MothWorld>())
                {
                    Main.NewText("gen!");
                    float num = 0f;
                    for (int i = 0; i < SubWorldModule.SubworldSystem.current.Tasks.Count; i++)
                    {
                        num += SubWorldModule.SubworldSystem.current.Tasks[i].Weight;
                    }
                    Terraria.WorldBuilding.WorldGenerator.CurrentGenerationProgress = new Terraria.WorldBuilding.GenerationProgress
                    {
                        TotalWeight = num
                    };
                    Terraria.WorldBuilding.WorldGenConfiguration config = SubWorldModule.SubworldSystem.current.Config;
                    for (int j = 0; j < SubWorldModule.SubworldSystem.current.Tasks.Count; j++)
                    {
                        WorldGen._genRand = new Terraria.Utilities.UnifiedRandom(Main.ActiveWorldFileData.Seed);
                        Main.rand = new Terraria.Utilities.UnifiedRandom(Main.ActiveWorldFileData.Seed);
                        Terraria.WorldBuilding.GenPass genPass = SubWorldModule.SubworldSystem.current.Tasks[j];
                        Terraria.WorldBuilding.WorldGenerator.CurrentGenerationProgress.Start(genPass.Weight);
                        genPass.Apply(Terraria.WorldBuilding.WorldGenerator.CurrentGenerationProgress, config?.GetPassConfiguration(genPass.Name));
                        Terraria.WorldBuilding.WorldGenerator.CurrentGenerationProgress.End();
                    }
                    Terraria.WorldBuilding.WorldGenerator.CurrentGenerationProgress = null;
                }
                else
                {
                    if (!SubWorldModule.SubworldSystem.Enter<MothWorld>())
                    {
                        Main.NewText("Fail!");
                    }
                }
            }
            return base.UseItem(player);
        }
    }
}
