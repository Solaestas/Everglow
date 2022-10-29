using Terraria.DataStructures;
using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.MagicWeaponsReplace.GlobalItems
{
    public class MagicBooksReplace : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            base.SetDefaults(item);
        }

        public override bool PreDrawTooltipLine(Item item, DrawableTooltipLine line, ref int yOffset)
        {
            if (Main.LocalPlayer.GetModPlayer<MagicBookPlayer>().MagicBookLevel == 1)
            {
                if (item.type == ItemID.WaterBolt)
                {
                }
            }
            return base.PreDrawTooltipLine(item, line, ref yOffset);
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (Main.LocalPlayer.GetModPlayer<MagicBookPlayer>().MagicBookLevel == 1)
            {
                if (item.type == ItemID.WaterBolt)
                {
                    tooltips.Add(new TooltipLine(ModLoader.GetMod("Everglow"), "Text1", "After hitting the enemy 5 times, you will get a Water Orb. right click to consume it and teleport to the cursor. The maximum number of Water Orbs you can have is 6. \nWhen the number of Water Orbs reaches 6, you can consume all of the Water Orbs with the middle mouse button to get 5 seconds of high intensity attacks. \nSwitch weapons to clear all the water orbs."));
                    if (Language.ActiveCulture.Name == "zh-Hans")
                    {
                        tooltips.Add(new TooltipLine(ModLoader.GetMod("Everglow"), "Text1", "累计命中敌人5次后获得一个水之球,右键消耗并传送至老鼠\n水之球最多叠加6个,水之球达到6个时,中键消耗全部水之球获得5秒高强度攻击\n切换武器清除全部水之球"));
                    }
                }
                if (item.type == ItemID.DemonScythe)
                {
                    //TODO 英语翻译
                    if (Language.ActiveCulture.Name == "zh-Hans")
                    {
                        tooltips.Add(new TooltipLine(ModLoader.GetMod("Everglow"), "Text1", "法术替换为旋转的恶魔月刃,速度会逐渐加快,伤害和击退力度也随着速度增长而加大"));
                    }
                }
                if (item.type == ItemID.BookofSkulls)
                {
                    //TODO 英语翻译
                    if (Language.ActiveCulture.Name == "zh-Hans")
                    {
                        tooltips.Add(new TooltipLine(ModLoader.GetMod("Everglow"), "Text1", "命中敌人后释放若干骨刺,骨刺会与下一次攻击一起发射,最多拥有12个骨刺\n右键在地面召唤烈焰白骨之爪"));
                    }
                }
            }
            base.ModifyTooltips(item, tooltips);
        }

        public override bool? UseItem(Item item, Player player)
        {
            if (player.GetModPlayer<MagicBookPlayer>().MagicBookLevel == 0)
            {
                if (item.type == ItemID.WaterBolt)
                {
                    item.noUseGraphic = false;
                }
                if (item.type == ItemID.BookofSkulls)
                {
                    item.noUseGraphic = false;
                }
                if (item.type == ItemID.DemonScythe)
                {
                    item.autoReuse = false;
                    item.noUseGraphic = false;
                }
                if (item.type == ItemID.CursedFlames)
                {
                    item.noUseGraphic = false;
                }
                if (item.type == ItemID.GoldenShower)
                {
                    item.noUseGraphic = false;
                }
                if (item.type == ItemID.CrystalStorm)
                {
                    item.noUseGraphic = false;
                }
                if (item.type == ItemID.MagnetSphere)
                {
                    item.noUseGraphic = false;
                }
                if (item.type == ItemID.RazorbladeTyphoon)
                {
                    item.noUseGraphic = false;
                }
                return base.UseItem(item, player);
            }
            if (item.type == ItemID.WaterBolt)
            {
                item.noUseGraphic = true;
            }
            if (item.type == ItemID.BookofSkulls)
            {
                item.noUseGraphic = true;
            }
            if (item.type == ItemID.DemonScythe)
            {
                item.autoReuse = true;
                item.noUseGraphic = true;
            }
            if (item.type == ItemID.CursedFlames)
            {
                item.noUseGraphic = true;
            }
            if (item.type == ItemID.GoldenShower)
            {
                item.noUseGraphic = true;
            }
            if (item.type == ItemID.CrystalStorm)
            {
                item.noUseGraphic = true;
            }
            if (item.type == ItemID.MagnetSphere)
            {
                item.noUseGraphic = true;
            }
            if (item.type == ItemID.RazorbladeTyphoon)
            {
                item.noUseGraphic = true;
            }
            // Aim Types
            if (item.type == ItemID.WaterBolt)
            {
                int aimType = ModContent.ProjectileType<Projectiles.WaterBolt.WaterBoltBook>();
                if (player.ownedProjectileCounts[aimType] < 1)
                {
                    Projectile.NewProjectile(player.GetSource_FromAI(), player.Center, Vector2.Zero, aimType, 0, 0, player.whoAmI);
                }
                aimType = ModContent.ProjectileType<Projectiles.WaterBolt.WaterBoltArray>();
                if (player.ownedProjectileCounts[aimType] < 1)
                {
                    Projectile.NewProjectile(player.GetSource_FromAI(), player.Center, Vector2.Zero, aimType, 0, 0, player.whoAmI);
                }
            }
            if (item.type == ItemID.DemonScythe)
            {
                int aimType = ModContent.ProjectileType<Projectiles.DemonScythe.DemonScytheBook>();
                if (player.ownedProjectileCounts[aimType] < 1)
                {
                    Projectile.NewProjectile(player.GetSource_FromAI(), player.Center, Vector2.Zero, aimType, 0, 0, player.whoAmI);
                }
                aimType = ModContent.ProjectileType<Projectiles.DemonScythe.DemonScytheArray>();
                if (player.ownedProjectileCounts[aimType] < 1)
                {
                    Projectile.NewProjectile(player.GetSource_FromAI(), player.Center, Vector2.Zero, aimType, 0, 0, player.whoAmI);
                }
            }
            if (item.type == ItemID.MagnetSphere)
            {
                int aimType = ModContent.ProjectileType<Projectiles.MagnetSphere.MagnetSphereBook>();
                if (player.ownedProjectileCounts[aimType] < 1)
                {
                    Projectile.NewProjectile(player.GetSource_FromAI(), player.Center, Vector2.Zero, aimType, 0, 0, player.whoAmI);
                }
                aimType = ModContent.ProjectileType<Projectiles.MagnetSphere.MagnetSphereArray>();
                if (player.ownedProjectileCounts[aimType] < 1)
                {
                    Projectile.NewProjectile(player.GetSource_FromAI(), player.Center, Vector2.Zero, aimType, 0, 0, player.whoAmI);
                }
            }
            if (item.type == ItemID.RazorbladeTyphoon)
            {
                int aimType = ModContent.ProjectileType<Projectiles.RazorbladeTyphoon.RazorbladeTyphoonBook>();
                if (player.ownedProjectileCounts[aimType] < 1)
                {
                    Projectile.NewProjectile(player.GetSource_FromAI(), player.Center, Vector2.Zero, aimType, 0, 0, player.whoAmI);
                }
                aimType = ModContent.ProjectileType<Projectiles.RazorbladeTyphoon.RazorbladeTyphoonArray>();
                if (player.ownedProjectileCounts[aimType] < 1)
                {
                    Projectile.NewProjectile(player.GetSource_FromAI(), player.Center, Vector2.Zero, aimType, 0, 0, player.whoAmI);
                }
            }
            if (item.type == ItemID.CursedFlames)
            {
                int aimType = ModContent.ProjectileType<Projectiles.CursedFlames.CursedFlamesBook>();
                if (player.ownedProjectileCounts[aimType] < 1)
                {
                    Projectile.NewProjectile(player.GetSource_FromAI(), player.Center, Vector2.Zero, aimType, 0, 0, player.whoAmI);
                }
                aimType = ModContent.ProjectileType<Projectiles.CursedFlames.CursedFlamesArray>();
                if (player.ownedProjectileCounts[aimType] < 1)
                {
                    Projectile.NewProjectile(player.GetSource_FromAI(), player.Center, Vector2.Zero, aimType, 0, 0, player.whoAmI);
                }
            }
            if (item.type == ItemID.CrystalStorm)
            {
                int aimType = ModContent.ProjectileType<Projectiles.CrystalStorm.CrystalStormBook>();
                if (player.ownedProjectileCounts[aimType] < 1)
                {
                    Projectile.NewProjectile(player.GetSource_FromAI(), player.Center, Vector2.Zero, aimType, 0, 0, player.whoAmI);
                }
                aimType = ModContent.ProjectileType<Projectiles.CrystalStorm.CrystalStormArray>();
                if (player.ownedProjectileCounts[aimType] < 1)
                {
                    Projectile.NewProjectile(player.GetSource_FromAI(), player.Center, Vector2.Zero, aimType, 0, 0, player.whoAmI);
                }
            }
            if (item.type == ItemID.BookofSkulls)
            {
                int aimType = ModContent.ProjectileType<Projectiles.BookofSkulls.BookofSkullsBook>();
                if (player.ownedProjectileCounts[aimType] < 1)
                {
                    Projectile.NewProjectile(player.GetSource_FromAI(), player.Center, Vector2.Zero, aimType, 0, 0, player.whoAmI);
                }
                aimType = ModContent.ProjectileType<Projectiles.BookofSkulls.BookofSkullsArray>();
                if (player.ownedProjectileCounts[aimType] < 1)
                {
                    Projectile.NewProjectile(player.GetSource_FromAI(), player.Center, Vector2.Zero, aimType, 0, 0, player.whoAmI);
                }
            }
            if (item.type == ItemID.GoldenShower)
            {
                int aimType = ModContent.ProjectileType<Projectiles.GoldenShower.GoldenShowerBook>();
                if (player.ownedProjectileCounts[aimType] < 1)
                {
                    Projectile.NewProjectile(player.GetSource_FromAI(), player.Center, Vector2.Zero, aimType, 0, 0, player.whoAmI);
                }
                aimType = ModContent.ProjectileType<Projectiles.GoldenShower.GoldenShowerArray>();
                if (player.ownedProjectileCounts[aimType] < 1)
                {
                    Projectile.NewProjectile(player.GetSource_FromAI(), player.Center, Vector2.Zero, aimType, 0, 0, player.whoAmI);
                }
            }
            return base.UseItem(item, player);
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.GetModPlayer<MagicBookPlayer>().MagicBookLevel == 0)
            {
                return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
            }
            if (item.type == ItemID.WaterBolt)
            {
                return false;
            }
            if (item.type == ItemID.DemonScythe)
            {
                return false;
            }
            if (item.type == ItemID.BookofSkulls)
            {
                return false;
            }
            if (item.type == ItemID.GoldenShower)
            {
                return false;
            }
            if (item.type == ItemID.CursedFlames)
            {
                return false;
            }
            if (item.type == ItemID.CrystalStorm)
            {
                return false;
            }
            if (item.type == ItemID.MagnetSphere)
            {
                return false;
            }
            if (item.type == ItemID.RazorbladeTyphoon)
            {
                return false;
            }
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }
    }

    internal class MagicBookPlayer : ModPlayer
    {
        public int MagicBookLevel = 0;
        public int WaterBoltHasHit = 0;

        public override void PreUpdate()
        {
            MagicBookLevel = 0;
            base.PreUpdate();
        }

        public override bool PreItemCheck()
        {
            //MagicBookLevel = 0;
            if (WaterBoltHasHit > 0)
            {
                if (Player.HeldItem.type != ItemID.WaterBolt || MagicBookLevel == 0)
                {
                    WaterBoltHasHit = 0;
                    /*foreach(Projectile p in Main.projectile)
                    {
                        if(p.owner == Player.whoAmI && p.type == ModContent.ProjectileType<Projectiles.WaterBolt.WaterTeleport>())
                        {
                            p.Kill();
                        }
                    }*/
                }
            }
            return base.PreItemCheck();
        }
    }
}