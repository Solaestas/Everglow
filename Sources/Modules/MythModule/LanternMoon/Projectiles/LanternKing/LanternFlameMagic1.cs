using Everglow.Sources.Modules.MythModule.LanternMoon.NPCs.LanternGhostKing;

namespace Everglow.Sources.Modules.MythModule.LanternMoon.Projectiles.LanternKing
{
    public class LanternFlameMagic1 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("灯火环");
        }
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.friendly = false;
            Projectile.extraUpdates = 10;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 80000000;
            Projectile.tileCollide = false;
        }
        float r = 0;
        private Vector2 v0;
        private float b = 0;
        public override void AI()
        {
            r = LanternGhostKing.CirR;
            Projectile.position = LanternGhostKing.Cirposi;
            /* if(Projectile.timeLeft >= 200 && Projectile.timeLeft <= 1000 && Projectile.timeLeft % 100 == 0)
             {
                 double io = Main.rand.NextFloat(0, 10f);
                 for (int i = 0; i < 16; i++)
                 {
                     Vector2 v = new Vector2(0,8).RotatedBy(i * Math.PI / 8d + io);
                     Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, v.X, v.Y, ModContent.ProjectileType<Projectiles.LanternKing.DarkFlameball2"), 40, 0f, Main.myPlayer, 0f, 0f);
                 }
             }
             for (int k = 0;k < r;k++)
             {
                 Vector2 v = Projectile.Center + new Vector2(0, r).RotatedByRandom(Math.PI * 2);
                 int l = Dust.NewDust(v, 0, 0, mod.DustType("DarkF"), 0, 0, 0, default(Color), 2f);
                 Main.dust[l].velocity.X = 0;
                 Main.dust[l].velocity.Y = 0;
                 Main.dust[l].noGravity = true;
             }*/
            if (NPC.CountNPCS(ModContent.NPCType<LanternGhostKing>()) < 1)
            {
                Projectile.Kill();
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return false;
        }
        /*public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 16; i++)
            {
                Vector2 v = new Vector2(0, Main.rand.Next(0,14)).RotatedByRandom(Math.PI);
                Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, v.X, v.Y, ModContent.ProjectileType<Projectiles.LanternKing.DarkFlameball"), 40, 0f, Main.myPlayer, 0f, 0f);
            }
            for (int a = 0; a < 180; a++)
            {
                Vector2 vector = Projectile.Center;
                Vector2 v = new Vector2(0, Main.rand.NextFloat(6f, 7.5f)).RotatedByRandom(Math.PI * 2);
                int num = Dust.NewDust(vector - new Vector2(4, 4), 2, 2, mod.DustType("DarkF"), v.X, v.Y, 0, default(Color), Main.rand.NextFloat(1.1f, 2.2f));
                Main.dust[num].velocity = v;
                Main.dust[num].noGravity = false;
                Main.dust[num].fadeIn = 1f + (float)Main.rand.NextFloat(-0.5f, 0.5f) * 0.1f;
            }
        }*/
        private Effect eff2;
        public override void PostDraw(Color lightColor)
        {

        }


        // 自定义顶点数据结构，注意这个结构体里面的顺序需要和shader里面的数据相同
        private struct CustomVertexInfo : IVertexType
        {
            private static VertexDeclaration _vertexDeclaration = new VertexDeclaration(new VertexElement[3]
            {
                new VertexElement(0, VertexElementFormat.Vector2, VertexElementUsage.Position, 0),
                new VertexElement(8, VertexElementFormat.Color, VertexElementUsage.Color, 0),
                new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.TextureCoordinate, 0)
            });
            public Vector2 Position;
            public Color Color;
            public Vector3 TexCoord;

            public CustomVertexInfo(Vector2 position, Color color, Vector3 texCoord)
            {
                this.Position = position;
                this.Color = color;
                this.TexCoord = texCoord;
            }

            public VertexDeclaration VertexDeclaration
            {
                get
                {
                    return _vertexDeclaration;
                }
            }
        }
    }
}
