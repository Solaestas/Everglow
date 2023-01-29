using Everglow.Sources.Commons.Function.IIIDModelReader;
using Everglow.Sources.Commons.Function.Vertex;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.GameContent;
using Humanizer;
using Terraria.GameContent.Drawing;
using Terraria.UI;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;


namespace Everglow.Sources.Modules.IIIDModule.Projectiles.PlanetBefall
{
    
    public class PlanetBeFall : ModProjectile
    {
        public TileDrawing projectorTileDrawing;
        public override void SetDefaults()
        {
            
            Projectile.width = 114;
            Projectile.height = 114;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.aiStyle = -1;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.hostile = false;
            Projectile.alpha = 255;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
           // Projectile.hide = true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Vector2 ToPlayer = (Projectile.Center - player.Center);
            player.heldProj = Projectile.whoAmI;
        }
        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);
        }

        public static ObjReader.Model model;
        public static Asset<Texture2D> NormalMap;

        public Vector3 SpinWithAxis(Vector3 orig, Vector3 axis, float Rotation)
        {
            axis = Vector3.Normalize(axis);
            float k = (float)Math.Cos(Rotation);
            return orig * k + Vector3.Cross(axis, orig * (float)Math.Sin(Rotation)) + Vector3.Dot(axis, orig) * axis * (1 - k);
        }
        public float GetCos(Vector3 v1, Vector3 v2)
        {
            return Vector3.Dot(v1, v2) / v1.Length() / v2.Length();
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            //overPlayers.Add(index);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Viewport viewport = Main.graphics.GraphicsDevice.Viewport;
            //float Vx = viewport.Width > 0 ? (1f / viewport.Width) : 0;
            //float Vy = viewport.Height > 0 ? (1f / viewport.Height) : 0;
            //Matrix matrix = default(Matrix);
            //matrix.M11 = Vx * 2f;
            //matrix.M22 = Vy * 2f;
            //matrix.M33 = 1f;
            //matrix.M44 = 1f;
            //matrix.M41 = -1f;
            //matrix.M42 = 1f;
            //matrix.M41 -= Vx;
            //matrix.M42 -= Vy;
            List<VertexRP> vertices = new List<VertexRP>();
            Main.instance.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            for (int f = 0; f < model.faces.Count; f++)
            {
                bool hasTexCoord = model.faces[f].TextureCoords.Count == 3;
                bool hasNormal = model.faces[f].Normals.Count == 3;
                bool hasPosition = model.faces[f].Positions.Count == 3;
                Debug.Assert(hasTexCoord && hasNormal && hasPosition);

                Vector3 A = model.positions[model.faces[f].Positions[0]] * 103;
                Vector3 B = model.positions[model.faces[f].Positions[1]] * 103;
                Vector3 C = model.positions[model.faces[f].Positions[2]] * 103;
                Vector3 tangentVector = ModelEntity.CalculateTangentVector(
                    A,
                    B,
                    C,
                    model.texCoords[model.faces[f].TextureCoords[0]],
                    model.texCoords[model.faces[f].TextureCoords[1]],
                    model.texCoords[model.faces[f].TextureCoords[2]]);//每个面的切向量

                Vector3 surfaceNormal = Vector3.Normalize(Vector3.Cross(B - A, C - A));//每个面的法向量
                float check = Vector3.Dot(tangentVector, surfaceNormal);
                // Debug.Assert(check == 0);
                for (int i = 0; i < 3; i++)
                {
                    Vector3 axis = new Vector3(1, -3, 2);
                    Vector2 texCoord = hasTexCoord ? model.texCoords[model.faces[f].TextureCoords[i]] : Vector2.Zero;
                    Vector3 normal = hasNormal ? model.normals[model.faces[f].Normals[i]] : Vector3.Zero;
                    Vector3 v3 = hasPosition ? model.positions[model.faces[f].Positions[i]] * 103 : Vector3.Zero;
                    //v3 = SpinWithAxis(v3, axis, (float)(Main.timeForVisualEffects * 0.04f));
                    //normal = SpinWithAxis(normal, axis, (float)(Main.timeForVisualEffects * 0.04f));
                    vertices.Add(new VertexRP(v3, new Vector3(texCoord, 0), normal, tangentVector));

                }
            }
            var modelPipeline = ModContent.GetInstance<ModelRenderingPipeline>();

            // 用这个函数创建透视投影，需要FOV和屏幕宽高比
            var projection = Matrix.CreatePerspectiveFieldOfView(MathF.PI / 3f, 1.0f, 1f, 1200f);
            var t = new Vector3(0, 0, -600);
            t.Y = -t.Y;
            var modelMatrix =
               Matrix.CreateLookAt(new Vector3((Projectile.Center.X - Main.LocalPlayer.Center.X) / -1, (Projectile.Center.Y - Main.LocalPlayer.Center.Y) / -1, 0),
                                     new Vector3((Projectile.Center.X - Main.LocalPlayer.Center.X) / -1, (Projectile.Center.Y - Main.LocalPlayer.Center.Y) / -1, 600),
                                     //new Vector3(Main.LocalPlayer.Center.X- Projectile.Center.X, Main.LocalPlayer.Center.Y- Projectile.Center.Y, 600),
                                     new Vector3(0, -1, 0))
               //Matrix.CreateRotationY((float)Math.Atan((Projectile.Center.X - Main.LocalPlayer.Center.X) / 1200))    
               //* Matrix.CreateRotationX((float)Math.Atan((Projectile.Center.Y - Main.LocalPlayer.Center.Y) / 1200))
               // *Matrix.CreateRotationY((float)Main.time * 0.01f)
               //* Matrix.CreateRotationZ((float)Main.time * 0.01f)
               * Matrix.CreateScale(0.25f)
               * Matrix.CreateTranslation(t);
                


            ModelEntity entity = new ModelEntity
            {
                Vertices = vertices,
                Texture = ModContent.Request<Texture2D>("Everglow/Sources/Modules/IIIDModule/Projectiles/PlanetBefall/PlanetBeFallTexture").Value,
                NormalTexture = NormalMap.Value,
                MaterialTexture = TextureAssets.MagicPixel.Value,//ModContent.Request<Texture2D>("Everglow/Sources/Modules/MEACModule/NonTrueMeleeProj/PlanetBefall/PlanetBeFallTexture").Value,
                EmissionTexture = ModContent.Request<Texture2D>("Everglow/Sources/Modules/IIIDModule/Projectiles/PlanetBefall/PlanetBeFallEmission").Value,
                ModelTransform = modelMatrix,
            };


            //ef.Parameters["uModel"].SetValue(modelMatrix);
            //ef.Parameters["uViewProjection"].SetValue(projection);
            //// 如果Model有非均匀缩放，就要用法线变换矩阵而不是Model矩阵
            //ef.Parameters["uModelNormal"].SetValue(Matrix.Transpose(Matrix.Invert(modelMatrix)));

            BloomParams bloom = new BloomParams
            {
                BlurIntensity = 1.0f,
                BlurRadius = 1.0f
            };
            ViewProjectionParams viewProjectionParams = new ViewProjectionParams
            {
                ViewTransform =  Matrix.Identity,
                FieldOfView = MathF.PI / 3f,
                AspectRatio = 1.0f,
                ZNear = 1f,
                ZFar = 1200f
            };
            ArtParameters artParameters = new ArtParameters
            {
                EnableOuterEdge = false
            };
            modelPipeline.BeginCapture(viewProjectionParams, bloom, artParameters);
            {
                modelPipeline.PushModelEntity(entity);
            }
            modelPipeline.EndCapture();

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone);
            Main.spriteBatch.Draw(modelPipeline.ModelTarget, Vector2.Lerp(Projectile.Center, Main.LocalPlayer.Center, 0.7f) - Main.screenPosition - new Vector2(1000, 1000),
                null, Color.White, 0, Vector2.One * 0.5f, 2f, SpriteEffects.None, 0);

            //Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList,
            //    new Vertex3D[]
            //    {
            //        new Vertex3D(new Vector3(0,0.5f,0),Vector3.Zero, Vector3.Zero),
            //        new Vertex3D(new Vector3(-0.5f,-0.5f,0),Vector3.Zero, Vector3.Zero),
            //        new Vertex3D(new Vector3(0.5f,-0.5f,0),Vector3.Zero, Vector3.Zero)
            //    }, 0, 1);
            /*List<Vertex2D> verticesII = new List<Vertex2D>();
            for (int f = 0; f < model.faces.Count; f++)
            {
                for (int i = 0; i < 3; i++)
                {
                    Vector3 Aixs = new Vector3(1, 3, 2);
                    Vector3 lightS = new Vector3(-1, -2, 3);
                    Vector3 Spin = SpinWithAxis(model.positions[model.faces[f].V[i]], Aixs, Projectile.timeLeft / 40f);
                    Vector3 SpinNormal = SpinWithAxis(model.normals[model.faces[f].N[i]], Aixs, Projectile.timeLeft / 40f);
                    float s = Math.Max(GetCos(lightS, SpinNormal), 0);

                    Vector3 Prepos = Spin / (Spin.Z + 100) * 600;
                    Vector2 pos = new Vector2(Prepos.X, Prepos.Y) + Projectile.Center - Main.screenPosition;
                    verticesII.Add(new Vertex2D(pos, new Color(s,s,s), model.normals[model.faces[f].N[i]]));
                }
            }
            Main.graphics.GraphicsDevice.Textures[0] = TextureAssets.MagicPixel.Value;
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, verticesII.ToArray(), 0, verticesII.Count - 2);*/
            return true;
        }
    }

    public class TestProjModelSystem : ModSystem
    {

        public override void OnModLoad()
        {
            PlanetBeFall.model = ObjReader.LoadFile("Everglow/Sources/Modules/IIIDModule/Projectiles/PlanetBefall/PlanetBeFall.obj");
            PlanetBeFall.NormalMap = ModContent.Request<Texture2D>("Everglow/Sources/Modules/IIIDModule/Projectiles/PlanetBefall/PlanetBeFallNormal");
            base.OnModLoad();
        }

    }
}
