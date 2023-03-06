using Everglow.Sources.Modules.MythModule.MiscItems.Projectiles.Accessory;
using Terraria.Audio;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Accessories
{
    [AutoloadEquip(EquipType.Neck)]
    public class GoldLiquidPupil : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemGlowManager.AutoLoadItemGlow(this);
        }
        public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
            Item.glowMask = ItemGlowManager.GetItemGlow(this);
            Item.width = 26;
            Item.height = 22;
            Item.value = 5500;
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            GoldLiquidPupilEquiper gLPE = player.GetModPlayer<GoldLiquidPupilEquiper>();
            gLPE.GoldLiquidPupilEnable = true;
        }
    }
    class GoldLiquidPupilEquiper : ModPlayer
    {
        public bool GoldLiquidPupilEnable = false;
        public override void ResetEffects()
        {
            GoldLiquidPupilEnable = false;
        }
        public override void PostHurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit, int cooldownCounter)
        {
            if (GoldLiquidPupilEnable)
            {
                if (Player.ownedProjectileCounts[ModContent.ProjectileType<IchorRing>()] <= 0)
                {
                    Projectile.NewProjectileDirect(Player.GetSource_FromThis(), Player.Center, Vector2.Zero, ModContent.ProjectileType<IchorRing>(), 6, 1.5f, Player.whoAmI);
                    for (int i = 0; i < 12; i++)
                    {
                        GenerateDust();
                    }
                    SoundEngine.PlaySound(SoundID.Splash.WithPitchOffset(-0.2f), Player.Center);
                }
            }
        }
        public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            if (GoldLiquidPupilEnable)
            {
                damage = (int)(damage + target.defense * 0.175f);
            }
        }
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (GoldLiquidPupilEnable)
            {
                damage = (int)(damage + target.defense * 0.175f);
            }
        }
        public override void ModifyHitPvp(Item item, Player target, ref int damage, ref bool crit)
        {
            if (GoldLiquidPupilEnable)
            {
                damage = (int)(damage + target.statDefense * 0.175f);
            }
        }
        public override void ModifyHitPvpWithProj(Projectile proj, Player target, ref int damage, ref bool crit)
        {
            if (GoldLiquidPupilEnable)
            {
                damage = (int)(damage + target.statDefense * 0.175f);
            }
        }
        private void GenerateDust()
        {
            Vector2 velocity = new Vector2(0, Main.rand.NextFloat(4.3f, 6f)).RotatedByRandom(6.283);
            Dust D = Dust.NewDustDirect(Player.Center - new Vector2(4)/*Dust的Size=8x8*/, 0, 0, DustID.Ichor, 0, 0, 150, default, Main.rand.NextFloat(0.4f, 1.1f));
            D.noGravity = true;
            D.velocity = velocity;
        }
    }
}
