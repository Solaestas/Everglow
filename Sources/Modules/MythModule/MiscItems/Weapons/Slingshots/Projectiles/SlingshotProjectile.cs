using Terraria.Audio;
using Everglow.Sources.Modules.MythModule.Common;
namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Slingshots.Projectiles
{
    public abstract class SlingshotProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 36;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Ranged;
            SetDef();
        }
        public virtual void SetDef()
        {

        }
        internal bool Release = true;
        internal int Power = 0;
        internal int ShootProjType = ModContent.ProjectileType<NormalAmmo>();
        internal int SlingshotLength = 14;
        public override void AI()
        {
            Power++;
            Player player = Main.player[Projectile.owner];
            player.heldProj = Projectile.whoAmI;
            if (Power == 24 && Main.mouseLeft)
            {
                SoundEngine.PlaySound(new SoundStyle("Everglow/Sources/Modules/MythModule/MiscItems/Weapons/Slingshots/Sounds/Slingshot"), Projectile.Center);
            }
            Vector2 MouseToPlayer = Main.MouseWorld - player.MountedCenter;
            if (Main.mouseLeft && Release)
            {
                Projectile.rotation = (float)(Math.Atan2(MouseToPlayer.Y, MouseToPlayer.X) + Math.PI * 0.25);
                Projectile.Center = Main.player[Projectile.owner].MountedCenter + Vector2.Normalize(MouseToPlayer) * 15f + new Vector2(0, -4);
                Projectile.timeLeft = 5 + Power;
            }
            float DrawRot;
            if (Projectile.Center.X < player.MountedCenter.X)
            {
                player.direction = -1;
                DrawRot = Projectile.rotation - MathF.PI / 4f;
            }
            else
            {
                player.direction = 1;
                DrawRot = Projectile.rotation - MathF.PI * 0.25f;
            }
            Vector2 MinusShootDir = new Vector2(SlingshotLength, -SlingshotLength).RotatedBy(DrawRot) + Projectile.Center - Main.MouseWorld;
            Vector2 SlingshotStringHead = new Vector2(SlingshotLength, -SlingshotLength).RotatedBy(DrawRot) + Vector2.Normalize(MinusShootDir) * Power / 3f;
            if (player.direction == -1)
            {
                MinusShootDir = new Vector2(SlingshotLength, -SlingshotLength).RotatedBy(DrawRot + Math.PI / 2d) + Projectile.Center - Main.MouseWorld;
                SlingshotStringHead = new Vector2(SlingshotLength, -SlingshotLength).RotatedBy(DrawRot + Math.PI / 2d) + Vector2.Normalize(MinusShootDir) * Power / 3f;
            }
            if (!Main.mouseLeft && Release)
            {
                Projectile.Center = Main.player[Projectile.owner].MountedCenter + Vector2.Normalize(MouseToPlayer) * 15f + new Vector2(0, -4);
                SoundEngine.PlaySound(new SoundStyle("Everglow/Sources/Modules/MythModule/MiscItems/Weapons/Slingshots/Sounds/SlingshotShoot"), Projectile.Center);

                Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center + SlingshotStringHead, -Vector2.Normalize(MinusShootDir) * (float)(Power / 5f + 8f), ShootProjType, (int)(Projectile.damage * (1 + Power / 40f)), Projectile.knockBack, player.whoAmI);
                Main.NewText(p.damage);

                Projectile.timeLeft = 5;
                Release = false;
            }
            if (!Main.mouseLeft && !Release)
            {
                Projectile.Center = Main.player[Projectile.owner].MountedCenter + Vector2.Normalize(MouseToPlayer) * 15f + new Vector2(0, -4);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }

        public override void PostDraw(Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Texture2D TexMain = (Texture2D)ModContent.Request<Texture2D>(Texture);
            Color drawColor = Lighting.GetColor((int)(Projectile.Center.X / 16.0), (int)(Projectile.Center.Y / 16.0));
            SpriteEffects spriteEffect = SpriteEffects.None;
            float DrawRot;
            if (Projectile.Center.X < player.MountedCenter.X)
            {
                player.direction = -1;
                DrawRot = Projectile.rotation - MathF.PI / 4f;
            }
            else
            {
                player.direction = 1;
                DrawRot = Projectile.rotation - MathF.PI * 0.25f;
            }
            Vector2 MouseToPlayer = Main.MouseWorld - player.MountedCenter;
            Vector2 SlingshotStringHead = new Vector2(SlingshotLength, -SlingshotLength).RotatedBy(DrawRot) + Projectile.Center - Main.MouseWorld;
            Vector2 SlingshotStringTail = new Vector2(SlingshotLength, -SlingshotLength).RotatedBy(DrawRot) + Vector2.Normalize(SlingshotStringHead) * Power * 0.3125f;
            if (player.direction == -1)
            {
                SlingshotStringHead = new Vector2(SlingshotLength, -SlingshotLength).RotatedBy(DrawRot + Math.PI / 2d) + Projectile.Center - Main.MouseWorld;
                SlingshotStringTail = new Vector2(SlingshotLength, -SlingshotLength).RotatedBy(DrawRot + Math.PI / 2d) + Vector2.Normalize(SlingshotStringHead) * Power * 0.3125f;
            }

            Vector2 SlingshotStringTailToPlayer = player.MountedCenter - (Projectile.Center + SlingshotStringTail);
            if (Projectile.Center.X < player.MountedCenter.X)
            {
                spriteEffect = SpriteEffects.FlipVertically;
                if (Main.mouseLeft)
                {
                    player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, (float)(Math.Atan2(MouseToPlayer.Y, MouseToPlayer.X) - MathF.PI * 0.75f));
                    player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, (float)(Math.Atan2(SlingshotStringTailToPlayer.Y, SlingshotStringTailToPlayer.X) - MathF.PI * 1.5f));
                }
                DrawRot = Projectile.rotation - MathF.PI / 4f;
            }
            else
            {
                player.direction = 1;

                if (Main.mouseLeft)
                {
                    player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, (float)(Math.Atan2(MouseToPlayer.Y, MouseToPlayer.X) - MathF.PI * 0.25f));
                    player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, (float)(Math.Atan2(SlingshotStringTailToPlayer.Y, SlingshotStringTailToPlayer.X) + MathF.PI * 0.5f));
                }
                DrawRot = Projectile.rotation - MathF.PI * 0.25f;
            }
            Main.spriteBatch.Draw(TexMain, Projectile.Center - Main.screenPosition, null, drawColor, DrawRot, TexMain.Size() / 2f, 1f, spriteEffect, 0);
        }
    }
}
