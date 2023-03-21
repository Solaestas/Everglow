using Everglow.Sources.Commons.Core.Utils;
using Everglow.Sources.Modules.IIIDModule.Projectiles.NonIIIDProj.GoldenCrack;
using Everglow.Sources.Modules.IIIDModule.Projectiles.NonIIIDProj.PlanetBefallArray;
using Everglow.Sources.Modules.IIIDModule.Projectiles.PlanetBefall;
using Everglow.Sources.Modules.MEACModule.Projectiles;
using Everglow.Sources.Modules.MythModule.TheFirefly.Pylon;
using ReLogic.Graphics;
using System.Security.AccessControl;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using static Everglow.Sources.Modules.IIIDModule.Projectiles.NonIIIDProj.GoldenCrack.Tree;

namespace Everglow.Sources.Modules.MEACModule.Items
{
    public class VortexVanquisherItem : ModItem
    {
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.width = 1;
            Item.height = 1;
            Item.useAnimation = 5;
            Item.useTime = 5;
            Item.shootSpeed = 5f;
            Item.knockBack = 6.5f;
            Item.damage = 90;
            Item.rare = ItemRarityID.Green;

            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.noUseGraphic = true;

            Item.value = Item.sellPrice(gold: 1);
        }

        private int CoolTimeForE = 0;
        private int CoolTimeForQ = 0;
        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Vector2 slotSize = new Vector2(52f, 52f);
            position -= slotSize * Main.inventoryScale / 2f - frame.Size() * scale / 2f;
            Vector2 drawPos = position + slotSize * Main.inventoryScale / 2f;
            Texture2D RArr1 = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MEACModule/NonTrueMeleeProj/Post").Value;
            Texture2D RArr2 = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MEACModule/NonTrueMeleeProj/PlanetBeFall").Value;
            if (!Main.gamePaused)
            {
                if (CoolTimeForE > 0)
                {
                    CoolTimeForE--;
                    spriteBatch.Draw(RArr1, drawPos + new Vector2(26.73f) * scale + new Vector2(4, 4), null, new Color(0, 0, 0, 255), 0f, RArr1.Size() / 2f, scale * 1.91f, SpriteEffects.None, 0f);
                    Main.spriteBatch.DrawString(FontAssets.MouseText.Value, (CoolTimeForE / 60f).ToString("#.#"), drawPos + new Vector2(22.91f) * scale, Color.White, 0f, Vector2.Zero, scale * 1.91f, SpriteEffects.None, 0);
                }
                else
                {
                    CoolTimeForE = 0;
                    spriteBatch.Draw(RArr1, drawPos + new Vector2(26.73f) * scale + new Vector2(4, 4), null, new Color(155, 155, 155, 50), 0f, RArr1.Size() / 2f, scale * 1.91f, SpriteEffects.None, 0f);
                }
                if (CoolTimeForQ > 0)
                {
                    CoolTimeForQ--;
                    spriteBatch.Draw(RArr2, drawPos + new Vector2(26.73f) * scale + new Vector2(-20, 4), null, new Color(0, 0, 0, 255), 0f, RArr2.Size() / 2f, scale * 1.91f, SpriteEffects.None, 0f);
                    Main.spriteBatch.DrawString(FontAssets.MouseText.Value, (CoolTimeForQ / 60f).ToString("#.#"), drawPos + new Vector2(22.91f) * scale + new Vector2(-30, 0), Color.White, 0f, Vector2.Zero, scale * 1.91f, SpriteEffects.None, 0);
                }
                else
                {
                    CoolTimeForQ = 0;
                    spriteBatch.Draw(RArr2, drawPos + new Vector2(26.73f) * scale + new Vector2(-20, 4), null, new Color(155, 155, 155, 50), 0f, RArr2.Size() / 2f, scale * 1.91f, SpriteEffects.None, 0f);
                }
            }
        }

        public override bool CanUseItem(Player player)
        {

            if (base.CanUseItem(player))
            {

                return false;
            }
            return base.CanUseItem(player);
        }
        internal bool LeftClick = false;
        public override void HoldItem(Player player)
        {
            Main.screenPosition += new Vector2(0, 100);
            if (player.ownedProjectileCounts[ModContent.ProjectileType<VortexVanquisher>()] + player.ownedProjectileCounts[ModContent.ProjectileType<VortexVanquisherThump>()] < 1)
            {

                if (Main.myPlayer == player.whoAmI)
                {
                    if (Main.mouseMiddle && Main.mouseMiddleRelease)
                    {
                        if (CoolTimeForQ > 0)
                        {
                            return;
                        }
                        CoolTimeForQ = 100;
                        Projectile PlanetBeFall = Projectile.NewProjectileDirect(null, new Vector2(player.Center.X, Main.MouseWorld.Y - 1500), Vector2.Zero, ModContent.ProjectileType<PlanetBeFall>(), Item.damage*10, Item.knockBack * 10, player.whoAmI);
                        Projectile Proj = Projectile.NewProjectileDirect(player.GetSource_FromAI(), Main.MouseWorld, Vector2.Zero, ModContent.ProjectileType<PlanetBefallArray>(), 0, 0, player.whoAmI);
                        Proj.Center = Main.MouseWorld;
                        PlanetBeFall.ai[0] = Proj.Center.X;
                        PlanetBeFall.ai[1] = Proj.Center.Y;
                        PlanetBeFall.velocity = Vector2.Normalize(Proj.Center - new Vector2(player.Center.X, Main.MouseWorld.Y - 1500))/4;
                    }
                    if (player.altFunctionUse != 2)
                    {
                        if (LeftClick && !Main.mouseLeft)
                        {
                            Projectile proj = Projectile.NewProjectileDirect(player.GetSource_ItemUse(Item), player.Center, Vector2.Zero, ModContent.ProjectileType<VortexVanquisher>(), player.GetWeaponDamage(Item), Item.knockBack, player.whoAmI);
                            proj.scale *= Item.scale;
                        }
                    }
                    else
                    {
                        if (CoolTimeForE > 0)
                        {
                            return;
                        }
                        CoolTimeForE = 60;
                        bool HasProj = false;
                        foreach (Projectile proj in Main.projectile)
                        {
                            if (proj.owner == player.whoAmI && proj.type == ModContent.ProjectileType<NonTrueMeleeProj.GoldShield>() && proj.active)
                            {
                                proj.timeLeft = 1200;
                                proj.ai[1] = 150;//盾量
                                HasProj = true;
                            }
                        }
                        if (!HasProj)
                        {
                            Projectile proj2 = Projectile.NewProjectileDirect(player.GetSource_ItemUse(Item), player.Center, Vector2.Zero, ModContent.ProjectileType<NonTrueMeleeProj.GoldShield>(), player.GetWeaponDamage(Item), Item.knockBack, player.whoAmI);
                            proj2.ai[1] = 150;//盾量
                        }
                        Vector2 CheckPoint = Main.MouseWorld;
                        for (int y = 0; y < 60; y++)
                        {
                            if (Collision.SolidCollision(CheckPoint, 1, 1))
                            {
                                break;
                            }
                            else
                            {
                                CheckPoint += new Vector2(0, 10) * player.gravDir;
                            }
                        }
                        if (!Collision.SolidCollision(CheckPoint, 1, 1))
                        {
                            return;
                        }


                        Vector2 TotalVector = Vector2.Zero;//合向量
                        int TCount = 0;
                        for (int a = 0; a < 12; a++)
                        {
                            Vector2 v0 = new Vector2(10, 0).RotatedBy(a / 6d * Math.PI);
                            if (Collision.SolidCollision(CheckPoint + v0, 1, 1))
                            {
                                TotalVector -= v0;
                                TCount++;
                            }
                            else
                            {
                                TotalVector += v0;
                            }
                        }
                        for (int a = 0; a < 24; a++)
                        {
                            Vector2 v0 = new Vector2(20, 0).RotatedBy(a / 12d * Math.PI);
                            if (Collision.SolidCollision(CheckPoint + v0, 1, 1))
                            {
                                TotalVector -= v0 * 0.5f;
                                TCount++;
                            }
                            else
                            {
                                TotalVector += v0 * 0.5f;
                            }
                        }
                        if (TotalVector == Vector2.Zero || TCount > 30)
                        {
                            return;
                        }

                        int f = Projectile.NewProjectile(player.GetSource_ItemUse(Item), CheckPoint, Vector2.Zero, ModContent.ProjectileType<NonTrueMeleeProj.StonePost>(), Item.damage, 0, player.whoAmI, 1);
                        float Angle = (float)Math.Atan2(TotalVector.Y, TotalVector.X);
                        Main.projectile[f].rotation = (float)(Angle - Math.PI * 1.5);
                    }
                }
                if (LeftClick)
                {
                    ClickTime++;
                    if (ClickTime > 30)
                    {
                        int playerdir = Main.MouseWorld.X > player.Center.X ? 1 : -1;
                        player.direction = playerdir;
                        Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.Center, new Vector2(Math.Sign(Main.MouseWorld.X - player.Center.X), 0), ModContent.ProjectileType<VortexVanquisherThump>(), Item.damage * 2, 0, player.whoAmI); //Original: Item.damage * 6
                        ClickTime = 0;
                    }
                }
            }
            else
            {
                ClickTime = 0;
            }
            LeftClick = Main.mouseLeft;
            base.HoldItem(player);
        }
        int ClickTime = 0;
        public override bool AltFunctionUse(Player player)
        {

            return true;
        }
        public override void AddRecipes()
        {

        }
    }
    public class PlanetBeFallScreenMovePlayer : ModPlayer
    {
        public int AnimationTimer = 0;
        public bool PlanetBeFallAnimation = false;
        public Projectile proj;
        const float MaxTime = 180;
        public override void ModifyScreenPosition()
        {
            Vector2 target;
            if (proj != null)
            {
                if (proj.owner == Player.whoAmI)
                {
                    target = proj.Center - Main.ScreenSize.ToVector2() / 2;
                    if (PlanetBeFallAnimation)
                    {

                        AnimationTimer+=10;
                        float Value = (1 - MathF.Cos(AnimationTimer / 60f * MathF.PI)) / 2f;
                        if (AnimationTimer >= 60 && AnimationTimer < 120)
                        {
                            AnimationTimer -= 6;
                            Value = 1;
                        }
                        if (AnimationTimer >= 120)
                        {
                            AnimationTimer -= 7;
                            Value = (1 + MathF.Cos((AnimationTimer - 120) / 60f * MathF.PI)) / 2f;
                        }

                        if (AnimationTimer >= MaxTime)
                        {
                            AnimationTimer = (int)MaxTime;
                            PlanetBeFallAnimation = false;
                        }
                        Player.immune = true;
                        Player.immuneTime = 1;
                        Main.screenPosition = (Value).Lerp(Main.screenPosition, target);
                    }
                }
            }
        }
    }
}
