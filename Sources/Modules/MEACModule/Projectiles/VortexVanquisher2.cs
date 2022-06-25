using Everglow.Sources.Modules.MythModule.Bosses.CorruptMoth.Dusts;
using Terraria.ID;

namespace Everglow.Sources.Modules.MEACModule.Projectiles
{
   
    public class VortexVanquisher2 : ModProjectile
    {
        public override string Texture => "Everglow/Sources/Modules/MEACModule/Projectiles/VortexVanquisher";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("");
            Main.projFrames[Projectile.type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.netImportant = true;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 15;
            Projectile.tileCollide = false;
        }
        public override void AI()
        {
            Projectile.velocity *= 0.94f;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;
            Main.spriteBatch.Draw(tex,Projectile.Center-Main.screenPosition,null,Color.White,Projectile.velocity.ToRotation() + 0.7854f,tex.Size()/2,Projectile.scale,0,0);
            return false;
        }
    }
}
