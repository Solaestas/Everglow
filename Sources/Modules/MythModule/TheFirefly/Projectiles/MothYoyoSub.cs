using Terraria.Localization;
using static Everglow.Sources.Commons.Function.Vertex.VertexBase;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Projectiles
{
    public class MothYoyoSub : ModProjectile
    {
        public override string Texture => "Everglow/Sources/Modules/MythModule/Bosses/CorruptMoth/Projectiles/ButterflyDream";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dream Butterfly");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "蓝蝶幻梦");
            Main.projFrames[Projectile.type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 114514;
            Projectile.alpha = 0;
            Projectile.penetrate = -1;
            Projectile.scale = 1f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
        }
        public Vector3 targetPos;
        private Vector3 v3Position;
        
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(targetPos.X);
            writer.Write(targetPos.Y);
            writer.Write(targetPos.Z);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            targetPos.X = reader.ReadSingle();
            targetPos.Y = reader.ReadSingle();
            targetPos.Z = reader.ReadSingle();
        }
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center,new Vector3(0,0.5f,1f));
            Projectile owner = Main.projectile[(int)Projectile.ai[0]];
            if (!owner.active||owner.type !=ModContent.ProjectileType<MothYoyoProjectile>())
                Projectile.Kill();
            int t = 114514 - Projectile.timeLeft;
            if (Projectile.timeLeft % 5 == 0)
            {
                Projectile.frame++;
            }
            if (t < 20)//开始
                v3Position = Vector3.Lerp(v3Position, targetPos, 0.1f);
            else if (owner.ai[0]==-1)//收回
            {
                v3Position = Vector3.Lerp(v3Position, Vector3.Zero, 0.1f);
                Projectile.scale -= 0.05f;
            }
            else
            {
                v3Position = Vector3.Transform(v3Position, Matrix.CreateRotationY(owner.velocity.X * 0.01f+0.02f));
                v3Position = Vector3.Transform(v3Position, Matrix.CreateRotationX(owner.velocity.Y * 0.01f));
            }
            Projectile.Center = owner.Center +new Vector2(v3Position.X,v3Position.Y);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D Light = Common.MythContent.QuickTexture("Bosses/CorruptMoth/Projectiles/FixCoinLight3");
            Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 origin = new Vector2(tex.Width / 2, tex.Height / 6);
            Rectangle sourceRec = tex.Frame(1, 3, 0, Projectile.frame % 3);

            Projectile owner = Main.projectile[(int)Projectile.ai[0]];
            Vector2 pos = Projection(v3Position + new Vector3(owner.Center.X,owner.Center.Y,0), Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight) / 2, out float scale, 1000); ;
            Color c = new Color(255, 255, 255, 0);
            Main.spriteBatch.Draw(Light, pos - Main.screenPosition, null, c * 0.2f, Projectile.rotation, Light.Size() / 2, Projectile.scale * scale, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(tex, pos - Main.screenPosition, sourceRec, c, Projectile.rotation, origin, Projectile.scale * scale, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            return false;
        }
        private static Vector2 Projection(Vector3 v3, Vector2 center, out float scale, float viewZ)
        {
            float k2 = -viewZ / (v3.Z - viewZ);
            scale = k2;
            Vector2 v = new Vector2(v3.X, v3.Y);
            return v + (k2 - 1) * (v - center);
        }
    }
}