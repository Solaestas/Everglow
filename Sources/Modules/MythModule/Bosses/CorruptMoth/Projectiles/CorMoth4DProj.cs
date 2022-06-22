using Terraria.Localization;
namespace Everglow.Sources.Modules.MythModule.Bosses.CorruptMoth.Projectiles
{
    public class CorMoth4DProj : ModProjectile
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
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = maxTimeleft = 900;
            Projectile.alpha = 0;
            Projectile.penetrate = -1;
            Projectile.scale = 1f;
        }

        private NPC Owner => Main.npc[(int)Projectile.ai[0]];
        public Vector4 targetPos;
        private Vector4 v4Position;
        private int maxTimeleft;
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(targetPos.X);
            writer.Write(targetPos.Y);
            writer.Write(targetPos.Z);
            writer.Write(targetPos.W);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            targetPos.X = reader.ReadSingle();
            targetPos.Y = reader.ReadSingle();
            targetPos.Z = reader.ReadSingle();
            targetPos.W = reader.ReadSingle();
        }
        public override void AI()
        {
            if (Projectile.timeLeft % 3 == 0 && Main.rand.NextBool())
            {
                Projectile.frame++;
            }

            int t = (maxTimeleft - Projectile.timeLeft);
            if (t == 0)
            {
                Projectile.spriteDirection = Main.rand.NextBool() ? 1 : -1;
                Projectile.ai[1] = 1;
            }
            //逐维度展开
            if (t < 50)
            {
                v4Position.Y = MathHelper.Lerp(v4Position.Y, targetPos.Y, 0.05f);
            }
            else if (t < 100)
            {
                v4Position.X = MathHelper.Lerp(v4Position.X, targetPos.X, 0.05f);
            }
            else if (t < 150)
            {
                v4Position.Z = MathHelper.Lerp(v4Position.Z, targetPos.Z, 0.05f);
            }
            else if (t < 200)
            {
                v4Position.W = MathHelper.Lerp(v4Position.W, targetPos.W, 0.05f);
            }
            else
            {
                v4Position = VecRotByYoZ(v4Position, 0.01f);
                Projectile.ai[1] += 0.001f;
                //Position = Vector4.Normalize(Position) * (Position.Length() + 1f);
            }

            if (!Owner.active)
            {
                Projectile.Kill();
                return;
            }

            Vector3 v3 = Projection(v4Position * Projectile.ai[1] + new Vector4(Owner.Center, 0, 0), 1000);
            if (v3.Z < 800)
            {
                Projectile.hostile = true;
                //Vector2 pos = Projection2(v3, Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight) / 2, out float scale, 1000);
                Projectile.Center = new(v3.X, v3.Y);
            }
            else
            {
                Projectile.hostile = false;
            }

            if (Projectile.timeLeft < 60)
            {
                Projectile.scale -= 0.015f;
                Projectile.hostile = false;
            }
        }
        private static Vector4 VecRotByYoZ(Vector4 vec, float rot)
        {
            Vector2 v = new Vector2(vec.X, vec.W).RotatedBy(rot);
            return new Vector4(v.X, vec.Y, vec.Z, v.Y);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D Light = Common.MythContent.QuickTexture("Bosses/CorruptMoth/Projectiles/FixCoinLight3");
            Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 origin = new Vector2(tex.Width / 2, tex.Height / 6);
            Rectangle sourceRec = tex.Frame(1, 3, 0, Projectile.frame % 3);
            Vector3 v3 = Projection(v4Position * Projectile.ai[1] + new Vector4(Owner.Center, 0, 0), 1000);
            if (v3.Z < 900)
            {

                Vector2 pos = Projection2(v3, Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight) / 2, out float scale, 1000);

                Color c = new Color(255, 255, 255, 0);
                Main.spriteBatch.Draw(Light, pos - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, c * 0.2f, Projectile.rotation, Light.Size() / 2, Projectile.scale * scale, SpriteEffects.None, 0);
                Main.spriteBatch.Draw(tex, pos - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), sourceRec, c, Projectile.rotation, origin, Projectile.scale * scale, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            }
            return false;
        }

        private Vector3 Projection(Vector4 vec, float viewZ)
        {
            float k1 = -viewZ / (vec.W - viewZ);
            Vector3 v3 = new Vector3(vec.X, vec.Y, vec.Z);
            v3 += (k1 - 1) * (v3 - new Vector3(Owner.Center, 0));
            return v3;
        }

        private static Vector2 Projection2(Vector3 v3, Vector2 center, out float scale, float viewZ)
        {
            float k2 = -viewZ / (v3.Z - viewZ);
            scale = k2;
            Vector2 v = new Vector2(v3.X, v3.Y);
            return v + (k2 - 1) * (v - center);
        }
    }
}