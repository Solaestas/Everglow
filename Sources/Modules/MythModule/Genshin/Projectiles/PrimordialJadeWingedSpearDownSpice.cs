using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MythModule.Common;
namespace Everglow.Sources.Modules.MythModule.Genshin.Projectiles
{
    public class PrimordialJadeWingedSpearDownSpice : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 180;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 80;
            Projectile.alpha = 0;
            Projectile.penetrate = -1;
            Projectile.scale = 1f;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
            Projectile.extraUpdates = 1;
        }
        public override void AI()
        {
            if (Projectile.timeLeft <= 78)
            {
                Projectile.friendly = false;
            }
        }
        public static int CyanStrike = 0;
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            CyanStrike = 1;
            Projectile.NewProjectile(Projectile.InheritSource(Projectile), target.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.XiaoHit>(), 0, 0, Projectile.owner, 0.45f);
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override void Load()
        {
            On.Terraria.CombatText.NewText_Rectangle_Color_string_bool_bool += CombatText_NewText_Rectangle_Color_string_bool_bool;
            On.Terraria.Main.DoDraw_Tiles_NonSolid += DrawXiao;
        }
        private void DrawXiao(On.Terraria.Main.orig_DoDraw_Tiles_NonSolid orig, Terraria.Main self)
        {
            for (int p = 0; p < Main.projectile.Length; p++)
            {
                if (Main.projectile[p].type == ModContent.ProjectileType<PrimordialJadeWingedSpearDownShake>())
                {
                    int MaxStr = Math.Clamp((80 - Main.projectile[p].timeLeft) / 2, 1, 8);
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                    List<Vertex2D> Vx = new List<Vertex2D>();
                    Vector2 Vbase = Main.projectile[p].Center - Main.screenPosition + new Vector2(0, 24);
                    Vector2 v0 = new Vector2(0, -1);
                    Vector2 v0T = new Vector2(1, 0);
                    float length = Main.projectile[p].ai[0];
                    v0 = v0 * length * Math.Clamp((80 - Main.projectile[p].timeLeft) / 24f, 0, 1f);
                    v0T = v0T * 77.77f * Main.projectile[p].timeLeft / 120f;
                    v0 = v0.RotatedBy(Main.projectile[p].rotation);
                    v0T = v0T.RotatedBy(Main.projectile[p].rotation);

                    Color cr = new Color(0.4f, 0.7f, 0.7f, 0);
                    float fadeK = Math.Clamp((Main.projectile[p].timeLeft - 10) / 24f, 0, 1f);


                    Vx.Add(new Vertex2D(Vbase + v0 * 2, cr, new Vector3(1, 0, 0)));
                    Vx.Add(new Vertex2D(Vbase + (v0 + v0T) * fadeK + (v0 * 2) * (1 - fadeK), cr, new Vector3(1, fadeK, 0)));
                    Vx.Add(new Vertex2D(Vbase + (v0 * 2) * (1 - fadeK), cr, new Vector3(1 - fadeK, fadeK, 0)));

                    Vx.Add(new Vertex2D(Vbase + v0 * 2, cr, new Vector3(1, 0, 0)));
                    Vx.Add(new Vertex2D(Vbase + (v0 - v0T) * fadeK + (v0 * 2) * (1 - fadeK), cr, new Vector3(1 - fadeK, 0, 0)));
                    Vx.Add(new Vertex2D(Vbase + (v0 * 2) * (1 - fadeK), cr, new Vector3(1 - fadeK, fadeK, 0)));

                    Texture2D t = MythContent.QuickTexture("Genshin/Projectiles/PrimordialJadeWingedSpearDownSpice");

                    Main.graphics.GraphicsDevice.Textures[0] = t;
                    Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                }
                if (Main.projectile[p].type == ModContent.ProjectileType<PrimordialJadeWingedSpearDownSpice>())
                {
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                    List<Vertex2D> Vx = new List<Vertex2D>();
                    Vector2 Vbase = Main.projectile[p].Center - Main.screenPosition + new Vector2(0, 24);
                    Vector2 v0 = new Vector2(0, -1);
                    Vector2 v0T = new Vector2(1, 0);
                    float length = Main.projectile[p].ai[0];
                    v0 = v0 * length * Math.Clamp((80 - Main.projectile[p].timeLeft) / 12f, 0, 1f);
                    v0T = v0T * 77.77f;
                    v0 = v0.RotatedBy(Main.projectile[p].rotation);
                    v0T = v0T.RotatedBy(Main.projectile[p].rotation);

                    float Stre = Math.Clamp((Main.projectile[p].timeLeft - 40) / 40f, 0f, 1f);
                    Color cr = new Color(Stre, Stre, Stre, 0);
                    Color ct = Lighting.GetColor((int)(Main.projectile[p].Center.X) / 16, (int)(Main.projectile[p].Center.Y) / 16);
                    Color cp = new Color(200, 200, 200, 0);
                    float fadeK = Math.Clamp((Main.projectile[p].timeLeft - 10) / 24f, 0, 1f);
                    float fadeG = Math.Clamp((Main.projectile[p].timeLeft - 10) / 24f + 0.12f, 0, 1f);

                    Vx.Add(new Vertex2D(Vbase + v0 * 2, cp, new Vector3(1, 0, 0)));
                    Vx.Add(new Vertex2D(Vbase + (v0 + v0T) * fadeG + (v0 * 2) * (1 - fadeG), cp, new Vector3(1, fadeG, 0)));
                    Vx.Add(new Vertex2D(Vbase + (v0 * 2) * (1 - fadeG), cp, new Vector3(1 - fadeG, fadeG, 0)));

                    Vx.Add(new Vertex2D(Vbase + v0 * 2, cp, new Vector3(1, 0, 0)));
                    Vx.Add(new Vertex2D(Vbase + (v0 - v0T) * fadeG + (v0 * 2) * (1 - fadeG), cp, new Vector3(1 - fadeG, 0, 0)));
                    Vx.Add(new Vertex2D(Vbase + (v0 * 2) * (1 - fadeG), cp, new Vector3(1 - fadeG, fadeG, 0)));

                    if (fadeG < 0.75 && fadeG > 0)
                    {
                        int f = Dust.NewDust(Vbase + (v0 * 2) * (1 - fadeG) + Main.screenPosition - new Vector2(4), 1, 1, 229, 0, 0, 0, default, Main.rand.NextFloat(0.35f, 0.6f));
                        Main.dust[f].noGravity = true;
                        Main.dust[f].noLight = true;
                        f = Dust.NewDust(Vbase + (v0 * 2) * (1 - fadeG) + Main.screenPosition - new Vector2(4), 1, 1, 229, 0, 0, 0, default, Main.rand.NextFloat(0.35f, 0.6f));
                        Main.dust[f].noGravity = true;
                        Main.dust[f].noLight = true;
                    }

                    Vx.Add(new Vertex2D(Vbase + v0 * 2, ct, new Vector3(1, 0, 0)));
                    Vx.Add(new Vertex2D(Vbase + (v0 + v0T) * fadeK + (v0 * 2) * (1 - fadeK), ct, new Vector3(1, fadeK, 0)));
                    Vx.Add(new Vertex2D(Vbase + (v0 * 2) * (1 - fadeK), ct, new Vector3(1 - fadeK, fadeK, 0)));

                    Vx.Add(new Vertex2D(Vbase + v0 * 2, ct, new Vector3(1, 0, 0)));
                    Vx.Add(new Vertex2D(Vbase + (v0 - v0T) * fadeK + (v0 * 2) * (1 - fadeK), ct, new Vector3(1 - fadeK, 0, 0)));
                    Vx.Add(new Vertex2D(Vbase + (v0 * 2) * (1 - fadeK), ct, new Vector3(1 - fadeK, fadeK, 0)));

                    Vx.Add(new Vertex2D(Vbase + v0 * 2, cr, new Vector3(1, 0, 0)));
                    Vx.Add(new Vertex2D(Vbase + (v0 + v0T) * fadeK + (v0 * 2) * (1 - fadeK), cr, new Vector3(1, fadeK, 0)));
                    Vx.Add(new Vertex2D(Vbase + (v0 * 2) * (1 - fadeK), cr, new Vector3(1 - fadeK, fadeK, 0)));

                    Vx.Add(new Vertex2D(Vbase + v0 * 2, cr, new Vector3(1, 0, 0)));
                    Vx.Add(new Vertex2D(Vbase + (v0 - v0T) * fadeK + (v0 * 2) * (1 - fadeK), cr, new Vector3(1 - fadeK, 0, 0)));
                    Vx.Add(new Vertex2D(Vbase + (v0 * 2) * (1 - fadeK), cr, new Vector3(1 - fadeK, fadeK, 0)));


                    Texture2D t = MythContent.QuickTexture("Genshin/Projectiles/PrimordialJadeWingedSpearDownSpice");
                    Main.graphics.GraphicsDevice.Textures[0] = t;
                    Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                }
            }
            orig(self);
        }
        private int CombatText_NewText_Rectangle_Color_string_bool_bool(On.Terraria.CombatText.orig_NewText_Rectangle_Color_string_bool_bool orig, Rectangle location, Color color, string text, bool dramatic, bool dot)
        {
            if (CyanStrike > 0)
            {
                color = new Color(0, 255, 174);
                CyanStrike--;
            }
            return orig(location, color, text, dramatic, dot);
        }
    }
}