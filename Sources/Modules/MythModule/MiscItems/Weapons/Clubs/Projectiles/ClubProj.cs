using Everglow.Sources.Modules.MEACModule;
using Everglow.Sources.Commons.Core.VFX;
namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Clubs.Projectiles
{
    public abstract class ClubProj : ModProjectile, IWarpProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 80;
            Projectile.height = 80;
            Projectile.penetrate = -1;
            Projectile.localNPCHitCooldown = 1;

            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.tileCollide = false;

            Projectile.DamageType = DamageClass.Melee;
            SetDef();
        }
        public virtual void SetDef()
        {

        }
        internal float Omega = 0;
        internal int DamageStartValue = 0;
        public override void AI()
        {
            if (DamageStartValue == 0)
            {
                DamageStartValue = Projectile.damage;
                Projectile.damage = 0;
            }
            Projectile.damage = (int)(DamageStartValue * Omega * 3.334);

            Player p = Main.player[Projectile.owner];
            Vector2 v = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY) - p.Center;
            v = v / v.Length();
            Projectile.velocity = v * 15f;
            Vector2 vT0 = Main.MouseWorld - p.Center;
            p.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, (float)(Math.Atan2(vT0.Y, vT0.X) - Math.PI / 2d));
            Projectile.position = p.position + v + new Vector2(-(Projectile.width / 2 - p.width / 2), -24);
            Projectile.spriteDirection = p.direction;
            Projectile.rotation += Omega;
            if (Projectile.timeLeft > 20)
            {
                if (Omega < 0.3f)
                {
                    Omega += 0.003f;
                }
            }
            else
            {
                Omega *= 0.9f;
            }
            if (Projectile.timeLeft < 22 && Main.mouseLeft && !p.dead && p.HeldItem.type == ModContent.ItemType<Clubs.WoodenClub>())
            {
                Projectile.timeLeft = 22;
            }
            if (p.dead)
            {
                Projectile.Kill();
            }
            p.ChangeDir(Projectile.direction);
            p.heldProj = Projectile.whoAmI;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects effects = SpriteEffects.None;
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
            int Heig = texture.Height;
            int y = Heig * Projectile.frame;
            if (Projectile.spriteDirection == 1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Player player = Main.player[Projectile.owner];
            Color color = Lighting.GetColor((int)Projectile.Center.X / 16, (int)((double)Projectile.Center.Y / 16.0));

            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Rectangle?(new Rectangle(0, y, Heig, 64)), color, Projectile.rotation, new Vector2(32, 32), Projectile.scale, effects, 0f);
            for (int i = 0; i < 5; i++)
            {
                float alp = Omega / 0.4f;
                Color color2 = new Color((int)(color.R * (5 - i) / 5f * alp), (int)(color.G * (5 - i) / 5f * alp), (int)(color.B * (5 - i) / 5f * alp), (int)(color.A * (5 - i) / 5f * alp));
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, color2, Projectile.rotation - i * 0.75f * Omega, new Vector2(32, 32), Projectile.scale, effects, 0f);
            }
            return false;
        }
        public void DrawWarp(VFXBatch spriteBatch)
        {

        }
    }
}
