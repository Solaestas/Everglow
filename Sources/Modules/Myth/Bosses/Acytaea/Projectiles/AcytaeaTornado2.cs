using Everglow.Sources.Commons.Core.VFX;
using Everglow.Sources.Commons.Core.VFX.Visuals;
using Everglow.Sources.Commons.Function.Vertex;

namespace Everglow.Sources.Modules.MythModule.Bosses.Acytaea.Projectiles;

[Pipeline(typeof(NPPipeline), typeof(AcytaeaPipeline))]
internal class AcytaeaTornado2 : VisualProjectile
{
    public override void SetDefaults()
    {
        Projectile.width = 70;
        Projectile.height = 70;
        Projectile.friendly = false;
        Projectile.hostile = true;
        Projectile.penetrate = -1;
        Projectile.timeLeft = 1200;
        Projectile.tileCollide = false;
        Projectile.DamageType = DamageClass.Melee;
        Projectile.extraUpdates = 3;
        ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        ProjectileID.Sets.TrailCacheLength[Projectile.type] = 70;
    }

    public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
    {
    }

    private int AIMNpc = -1;

    public override void AI()
    {
        if (AIMNpc < 0)
        {
            for (int i = 0; i < 200; i++)
            {
                if (Main.npc[i].type == ModContent.NPCType<NPCs.Acytaea>())
                {
                    AIMNpc = i;
                    break;
                }
            }
        }
        if (AIMNpc >= 0)
        {
            Projectile.Center = Main.npc[AIMNpc].Center;
        }
        Timer = Projectile.timeLeft / 15f + 6;
        WHOAMI = Projectile.whoAmI;
        Typ2 = Projectile.type;
    }

    public override bool PreDraw(ref Color lightColor)
    {
        return false;
    }

    public static float Timer = 0;
    public static int WHOAMI = -1;
    public static int Typ2 = -1;

    public override void Draw()
    {
        for (int z = -3; z < 4; z++)
        {
            List<Vertex2D> Vx = new List<Vertex2D>();

            for (int h = 0; h < 60; h++)
            {
                float MinCosZ = (float)(2.4 - Math.Cos(z / 30d * Math.PI)) / 2.4f;
                Vector2 vBla = new Vector2(60 * MinCosZ, 0).RotatedBy(Timer - h * 0.1f + z * z);
                vBla.Y *= 0.3f;
                Vector2 vb = Main.projectile[WHOAMI].Center + vBla + new Vector2(0, 0);
                Vector2 vCla = new Vector2(60 * MinCosZ, 0).RotatedBy(Timer - 0.1f - h * 0.1f + z * z);
                vCla.Y *= 0.3f;
                Vector2 vc = Main.projectile[WHOAMI].Center + vCla + new Vector2(0, 0);
                Color color3 = new Color(255, 255, 255, 0);
                if (Main.projectile[WHOAMI].timeLeft < 255)
                {
                    color3 = new Color(Main.projectile[WHOAMI].timeLeft, Main.projectile[WHOAMI].timeLeft, Main.projectile[WHOAMI].timeLeft, 0);
                }
                if (Main.projectile[WHOAMI].timeLeft > 945)
                {
                    color3 = new Color(1200 - Main.projectile[WHOAMI].timeLeft, 1200 - Main.projectile[WHOAMI].timeLeft, 1200 - Main.projectile[WHOAMI].timeLeft, 0);
                }
                float a0 = (float)Math.Sqrt((3 - Math.Abs(z)) / 3f);
                color3.R = (byte)(color3.R * a0);
                color3.G = (byte)(color3.G * a0);
                color3.B = (byte)(color3.B * a0);
                color3.A = (byte)(color3.A * a0);
                Vx.Add(new Vertex2D(vc - Main.screenPosition + new Vector2(0, z * 200), color3, new Vector3((h + 1) / 60f, 0, 0)));
                Vx.Add(new Vertex2D(vb - Main.screenPosition + new Vector2(0, z * 200), color3, new Vector3(h / 60f, 0, 0)));
                Vx.Add(new Vertex2D(Main.projectile[WHOAMI].Center - Main.screenPosition + new Vector2(0, 400) + new Vector2(0, z * 200), color3, new Vector3(0.5f, 1, 0)));
            }
            Texture2D t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/Bosses/Acytaea/Projectiles/AcytaeaTornado2").Value;
            //Main.graphics.GraphicsDevice.Textures[0] = t;//GoldenBloodScaleMirror
            VFXManager.spriteBatch.Draw(t, Vx, PrimitiveType.TriangleList);
        }
    }
}