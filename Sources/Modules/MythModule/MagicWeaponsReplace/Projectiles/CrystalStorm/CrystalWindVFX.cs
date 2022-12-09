﻿using Everglow.Sources.Commons.Core;
using Everglow.Sources.Commons.Core.VFX;
using Everglow.Sources.Commons.Core.VFX.Pipelines;
using Everglow.Sources.Commons.Core.VFX.Visuals;
using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MythModule.Common;
using ReLogic.Content;

namespace Everglow.Sources.Modules.MythModule.MagicWeaponsReplace.Projectiles.CrystalStorm;

internal abstract class ShaderDraw : Visual
{
    public override CallOpportunity DrawLayer => CallOpportunity.PostDrawDusts;
    public Vector2 position;
    public Vector2 velocity;
    public float[] ai;
    public ShaderDraw() { }
    public ShaderDraw(Vector2 position, Vector2 velocity, params float[] ai)
    {
        this.position = position;
        this.velocity = velocity;
        this.ai = ai;//可以认为params传入的都是右值，可以直接引用
    }
}

internal class CrystalWindPipeline : Pipeline
{
    public override void Load()
    {
        effect = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/MagicWeaponsReplace/Projectiles/CursedFlames/CursedFlame", AssetRequestMode.ImmediateLoad);
        effect.Value.Parameters["uNoise"].SetValue(ModContent.Request<Texture2D>("Everglow/Sources/Modules/ExampleModule/VFX/Perlin", AssetRequestMode.ImmediateLoad).Value);
    }
    public override void BeginRender()
    {
        var effect = this.effect.Value;
        var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
        var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.ZoomMatrix;
        effect.Parameters["uTransform"].SetValue(model * projection);
        Texture2D FlameColor = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/CursedFlames/Cursed_Color");
        VFXManager.spriteBatch.BindTexture<Vertex2D>(FlameColor);
        Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.AnisotropicClamp;
        VFXManager.spriteBatch.Begin(BlendState.AlphaBlend, DepthStencilState.None, SamplerState.LinearWrap, RasterizerState.CullNone);
        effect.CurrentTechnique.Passes[0].Apply();
    }

    public override void EndRender()
    {
        VFXManager.spriteBatch.End();
    }
}
[Pipeline(typeof(CrystalWindPipeline), typeof(BloomPipeline))]
internal class CrystalWindVFX : ShaderDraw
{
    public List<Vector2> oldPos = new List<Vector2>();
    public float timer;
    public float maxTime;
    public float rotation;
    private Vector2 AimCenter = Vector2.Zero;
    private Vector2 OldAimCenter = Vector2.Zero;
    /// <summary>
    /// ai[0]纹理相位,ai[1]上升力系数,ai[2]归属于哪个弹幕,ai[3]x轴迁移系数
    /// </summary>
    public CrystalWindVFX() { }
    public CrystalWindVFX(int maxTime, Vector2 position, Vector2 velocity, params float[] ai) : base(position, velocity, ai)
    {
        this.maxTime = maxTime;
    }

    public override void Update()
    {
        //Base Datas
       for(int x = 0;x<3;x++)
        {
            position += velocity;
            oldPos.Add(position);
            if (oldPos.Count > 30)
            {
                oldPos.RemoveAt(0);
            }
            timer++;
            if (timer > maxTime)
            {
                Active = false;
            }

            float delC = 0.05f * (float)Math.Sin((maxTime - timer) / 40d * Math.PI);
            Lighting.AddLight((int)(position.X / 16), (int)(position.Y / 16), 0.15f * delC, 0, 0.85f * delC);
            if (Collision.SolidCollision(position, 0, 0))
            {
                Active = false;
            }




            if ((OldAimCenter - Main.projectile[(int)ai[2]].Center).Length() > 200 && OldAimCenter != Vector2.Zero)
            {
                if (timer < maxTime - 20)
                {
                    timer += 5;
                }
            }
            if (Main.projectile[(int)ai[2]].active && Main.projectile[(int)ai[2]].timeLeft > 200 && Main.projectile[(int)ai[2]].type == ModContent.ProjectileType<Storm>())
            {
                AimCenter = Main.projectile[(int)ai[2]].Center;
                OldAimCenter = Main.projectile[(int)ai[2]].Center;
            }

            float Dy = AimCenter.Y - position.Y;
            float xCoefficient = Dy * Dy / 600f - 0.4f * Dy + 50;
            Vector2 TrueAim = AimCenter + new Vector2(xCoefficient * (float)(Math.Sin(Main.timeForVisualEffects * 0.3f + rotation)), 0) - position;

            ai[3] = (byte)(ai[3] * 0.95 + xCoefficient * 0.05);

            velocity = velocity * 0.75f + new Vector2(Utils.SafeNormalize(TrueAim, new Vector2(0, 0.05f)).X, -ai[1] * 0.3f) * 0.25f / ai[3] * 500f;
            velocity *= Main.rand.NextFloat(0.85f, 1.15f);
        }

    }

    public override void Draw()
    {
        Vector2[] pos = oldPos.Reverse<Vector2>().ToArray();
        float fx = timer / maxTime;
        int len = pos.Length;
        if (len <= 2)
        {
            return;
        }
        Vertex2D[] bars = new Vertex2D[len * 2 - 1];
        for (int i = 1; i < len; i++)
        {
            Vector2 normal = oldPos[i] - oldPos[i - 1];
            normal = Vector2.Normalize(normal).RotatedBy(Math.PI * 0.5);
            Color drawcRope = new Color(fx * fx * fx * 2, 0.5f, 1, 150 / 255f);
            float width = Math.Min((float)(Math.Sin(i / (double)(len) * Math.PI) * 30), 10) * 0.2f;
            bars[2 * i - 1] = (new Vertex2D(oldPos[i] + normal * width, drawcRope, new Vector3(0 + ai[0], i / 80f, 0)));
            bars[2 * i] = (new Vertex2D(oldPos[i] - normal * width, drawcRope, new Vector3(0.05f + ai[0], i / 80f, 0)));
        }
        bars[0] = new Vertex2D((bars[1].position + bars[2].position) * 0.5f, Color.White, new Vector3(0.5f, 0, 0));
        VFXManager.spriteBatch.Draw(bars, PrimitiveType.TriangleStrip);
    }
}