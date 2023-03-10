using Everglow.Sources.Modules.OceanModule.Common;
using static Terraria.ModLoader.PlayerDrawLayer;

namespace Everglow.Sources.Modules.OceanModule.Projectiles.Weapons
{
    public class RampageShark : ModProjectile
    {
        public override string Texture => "Everglow/Sources/Modules/OceanModule/Projectiles/Weapons/RampageShark/RampageShark_gun";
        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 100000;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Ranged;
        }
        internal float Power = 0;
        private void Shoot()
        {
            Player player = Main.player[Projectile.owner];
            float chance = (Power / 100f + (player.GetCritChance(DamageClass.Generic)) / 300f);
            Vector2 toMouse = Projectile.Center - player.MountedCenter;
            Vector2 velocity = Vector2.Normalize(toMouse) * 27;
            Item item = player.HeldItem;
            if (Main.rand.NextFloat(1f) > chance)
            {
                Vector2 newvelocity = velocity.RotatedBy(Main.rand.NextFloat(-Power / 384f, Power / 384f));
                Projectile.NewProjectile(item.GetSource_ItemUse(item), Projectile.Center + velocity * 2.5f, newvelocity, item.shoot, item.damage, item.knockBack, player.whoAmI);
            }
            else
            {
                int Times = Main.rand.Next(4, 7);
                for (int i = 0; i < Times; i++)
                {
                    Vector2 newvelocity = velocity.RotatedBy(Main.rand.NextFloat(-Power / 96f, Power / 96f));
                    Projectile.NewProjectile(item.GetSource_ItemUse(item), Projectile.Center + velocity * 2.5f, newvelocity, item.shoot, item.damage, item.knockBack, player.whoAmI);
                }
            }
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Items.Weapons.RampageShark rampage = player.HeldItem.ModItem as Items.Weapons.RampageShark;
            if(rampage != null)
            {
                Power = rampage.CrazyValue;
            }
            else
            {
                Projectile.Kill();
            }

            Vector2 toMouse = Main.MouseWorld - player.MountedCenter;
            toMouse = Vector2.Normalize(toMouse);
            if (player.controlUseItem)
            {
                Projectile.rotation = (float)(Math.Atan2(toMouse.Y, toMouse.X) + Math.PI * 0.25);
                Projectile.Center = player.MountedCenter + Vector2.Normalize(toMouse);
                Projectile.velocity *= 0;
                if(Projectile.timeLeft % player.HeldItem.useTime == 0)
                {
                    Shoot();
                }
                if(Projectile.timeLeft % player.HeldItem.useTime == player.HeldItem.useTime / 2 && Power == 16)
                {
                    Shoot();
                }
            }
            else
            {
                Projectile.Kill();
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
        public override void PostDraw(Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            player.heldProj = Projectile.whoAmI;
            Vector2 toMouse = Projectile.Center - player.MountedCenter;
            if (player.controlUseItem)
            {
                player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, (float)(Math.Atan2(toMouse.Y, toMouse.X) - Math.PI / 2d));
            }

            Texture2D TexMain = OceanContent.QuickTexture("Projectiles/Weapons/RampageShark/RampageShark_gun");
            Texture2D TexMainG = OceanContent.QuickTexture("Projectiles/Weapons/RampageShark/RampageShark_glow");


            SpriteEffects se = SpriteEffects.None;
            if (Projectile.Center.X < player.Center.X)
            {
                se = SpriteEffects.FlipVertically;
                player.direction = -1;
            }
            else
            {
                player.direction = 1;
            }
            Main.spriteBatch.Draw(TexMain, Projectile.Center - Main.screenPosition - new Vector2(0, 0), null, lightColor, Projectile.rotation - (float)(Math.PI * 0.25), TexMain.Size() / 2f, 1f, se, 0);
            float glow = Power / 16f;
            Main.spriteBatch.Draw(TexMainG, Projectile.Center - Main.screenPosition - new Vector2(0, 0), null, new Color(glow, glow, glow, 0), Projectile.rotation - (float)(Math.PI * 0.25), TexMainG.Size() / 2f, 1f, se, 0);
            //Main.spriteBatch.Draw(TexMainG, Projectile.Center - Main.screenPosition - new Vector2(0, 0), new Rectangle(0, 0, (int)(Energy / 180f * 74f) + 30, TexMainG.Height), new Color(255, 255, 255, 0), Projectile.rotation - (float)(Math.PI * 0.25) + (float)(Projectile.ai[0] / -3d) * player.direction, new Vector2(35, 22), 1f, se, 0);
        }
    }
}