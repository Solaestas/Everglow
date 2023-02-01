using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.Utilities;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Everglow.Sources.Commons.Function.Vertex;
using Terraria.ID;
using Everglow.Sources.Modules.MEACModule;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using ReLogic.Content;


namespace Everglow.Sources.Modules.IIIDModule.Projectiles.NonIIIDProj.GoldenCrack
{
    public class Tree
    {
        public int cnt, brunchnum;
        public Node[] Nodes;
        public Node root;
        public UnifiedRandom random;
        public Vector2 vel;
        public Tree(UnifiedRandom random, Vector2 vel)
        {
            cnt = 0;
            brunchnum = 0;
            root = null;
            this.random = random;
            this.vel = vel;
        }

        public class Node
        {
            public int num;
            public bool ismaster, isbrunch;
            public float rad, size, length;
            public bool isterminal;
            public bool isfork;
            public Vector2 position;
            public List<Node> children;
            public Node(int num, float rad, float size, float length, bool isterminal, bool ismaster, bool isbrunch, bool isfork, Vector2 position)
            {
                this.num = num;
                this.ismaster = ismaster;
                this.isbrunch = isbrunch;
                this.rad = rad;
                this.size = size;
                this.length = length;
                this.isterminal = isterminal;
                this.isfork = isfork;
                this.position = position;
                this.children = new List<Node>();
            }
        }
        public Vector2 Position(Vector2 pos, Vector2 vel, Node node)
        {
            return pos + (vel.ToRotation() + node.rad).ToRotationVector2() * node.length;
        }
        private float Rand(float range)
        {
            return random.NextFloatDirection() * range;
        }
        public static float Rand()
        {
            UnifiedRandom random = Main.rand;
            double u = -2 * Math.Log(random.NextDouble());
            double v = 2 * Math.PI * random.NextDouble();
            return Math.Clamp((float)Math.Max(0, Math.Sqrt(u) * Math.Cos(v) * 0.3 + 0.5) + 0.4f, 0.7f, 1.2f);
        }
        public void Generate()
        {

            // 根节点生成，朝向0，粗细1，长度随机50中选
            root = new Node(0, 0, Rand(), 200f * Rand(), false, true, false, false, Vector2.Zero);
            root = Buildmaster(root, 5);
            Nodes = new Node[cnt + 1];
            //root = _build(root, 5);
        }
        private Node Buildmaster(Node node, int dep)
        {
            cnt++;

            if (dep == 10)
            {
                node.isterminal = true;
                return node;

            }

            if (Main.rand.NextBool(5) || dep == 5)
            {
                int numChild = Main.rand.Next(4);
                for (int i = 0; i < numChild; i++)
                {
                    var rad = Rand(MathHelper.Pi / 4f);
                    Node brunch = new Node(cnt, (node.rad > 0) ? Math.Abs(rad) : -Math.Abs(rad), node.size * Rand(), node.length * Rand(), false, false, true, false, Position(node.position, vel, node));
                    node.isfork = true;
                    node.children.Add(Buildbrunch(brunch, 0));
                }
                
            }
            // 参数修改了
            Node master = new Node(cnt, Rand(MathHelper.Pi / 6f), node.size * Rand(), node.length * Rand(), false, true, false, false, Position(node.position, vel, node));
            node.children.Add(Buildmaster(master, dep + 1));
            if (node.isfork)
                CombatText.NewText(new Rectangle((int)Main.LocalPlayer.position.X, (int)Main.LocalPlayer.position.Y, Main.LocalPlayer.width, Main.LocalPlayer.height),
                   new Color(255, 0, 0),
                   node.num,
                   true, false);
            return node;
        }
        private Node Buildbrunch(Node node, int dep)
        {
            cnt++;
            brunchnum++;
            if (dep == 2)
            {

                node.isterminal = true;
                return node;
            }
            Node child = new Node(cnt, Rand(MathHelper.Pi / 6f), node.size * Rand(), node.length * Rand(), false, false, true, false, Position(node.position, vel, node));
            node.children.Add(Buildbrunch(child, dep + 1));
            return node;
        }
        public void Array(Node node)
        {
            Nodes[node.num] = node;
            foreach (var child in node.children)
            {
                Array(child);
            }
        }
    }
    public class GoldenCrack : ModProjectile//,IBloomProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 20;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 1200;
            Projectile.tileCollide = false;
            Projectile.scale = 0.8f;
            Projectile.alpha = 60;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.hide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 1;
        }

        private Tree tree;
        private Vector2[] NodePosition;
        int j, terminal;
        bool back = false;
        bool[] terminals;
        public override void AI()
        {

            if (Projectile.ai[0] == 0)
            {
                tree = new Tree(Main.rand, Projectile.velocity);
                tree.Generate();
                if (tree.root != null)
                    tree.Array(tree.root);

                NodePosition = new Vector2[tree.Nodes.Length + tree.brunchnum];
                terminals = new bool[tree.Nodes.Length + tree.brunchnum];
                for (int i = 0; i < NodePosition.Length; i++)
                {
                    NodePosition[i] = tree.Nodes[j].position;
                    terminals[i] = false;
                    if (tree.Nodes[j].isterminal)
                    {
                        terminal = j;
                        terminals[i] = true;
                        if (tree.Nodes[j].isbrunch)
                        {
                            back = true;
                        }
                        else
                        {
                            j--;
                        }

                    }
                    if (!back)
                    {
                        j++;
                    }
                    else
                    {
                        j--;
                    }
                    if (back && tree.Nodes[j].isfork)
                    {
                        back = false;
                        j = terminal + 1;
                    }


                }
                Projectile.ai[0]++;
            }
            if (Projectile.timeLeft >= 28)
            {
                Projectile.ai[1] += 1f / 60f;
            }
            else
            {
                Projectile.ai[1] -= 1f / 24f;
            }

            if (Projectile.ai[1] >= 1)
            {
                Projectile.ai[1] = 1;
            }
            Projectile.ai[0]++;
        }

        public override bool PreDraw(ref Color lightColor)
        {

            List<Vertex2D> vertices = new List<Vertex2D>();
            Color color = Color.Yellow;
            Main.spriteBatch.End();
            Main.spriteBatch.Begin((SpriteSortMode)0, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, (Effect)null, Main.GameViewMatrix.TransformationMatrix);
            for (int i = 0; i < NodePosition.Length - 1; i++)
            {

                if (NodePosition[i] + Projectile.Center != Vector2.Zero)
                {
                    if (terminals[i])
                    {
                        vertices.Add(new Vertex2D(NodePosition[i] + Projectile.Center - Main.screenPosition, color, new Vector3(1f, 0f, 1f)));
                        // vertices.Add(new Vertex2D(Main.LocalPlayer.Center - Main.screenPosition, new Vector3(1f, 0f, 1f), new Color(200, 100, 200)));


                    }
                    else
                    {
                        Vector2 normalDir = NodePosition[i] - NodePosition[i + 1];
                        normalDir = Vector2.Normalize(new Vector2(0f - normalDir.Y, normalDir.X));
                        float width = Math.Clamp(Projectile.ai[0] * 15 - NodePosition[i].Length(), 0, 5) * Projectile.ai[1];
                        vertices.Add(new Vertex2D(NodePosition[i] + Projectile.Center + (i == 0 ? 2 : 1) * normalDir * width * ((float)Math.Pow(0.9f,i))- Main.screenPosition, color, new Vector3(1f, 0f, 1f)));
                        vertices.Add(new Vertex2D(NodePosition[i] + Projectile.Center - (i == 0 ? 2 : 1) * normalDir * width * ((float)Math.Pow(0.9f, i)) - Main.screenPosition, color, new Vector3(1f, 0f, 1f)));
                    }
                }
            }
            Main.graphics.GraphicsDevice.Textures[1] = TextureAssets.MagicPixel.Value;
            if (vertices.Count > 2)
            {
                Main.graphics.GraphicsDevice.DrawUserPrimitives<Vertex2D>(PrimitiveType.TriangleStrip, vertices.ToArray(), 0, vertices.Count - 2);
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin((SpriteSortMode)0, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, (Effect)null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
        public static Vector2 Normalize2(Vector2 vector2)
        {
            if (vector2 != Vector2.Zero)
            {
                return Vector2.Normalize(vector2);
            }
            return new Vector2(0f, 0.001f);
        }


        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float point = 0f;
            bool flag = false;
            float width = 20f * Projectile.ai[1];
            for (int i = 0; i < Projectile.oldPos.Length - 1 && !(Projectile.oldPos[i + 1] == Vector2.Zero); i++)
            {
                if (Collision.CheckAABBvLineCollision(Utils.TopLeft(targetHitbox), Utils.Size(targetHitbox), Projectile.oldPos[i], Projectile.oldPos[i + 1], width * 2f, ref point))
                {
                    flag = true;
                }
            }
            return flag;
        }

        public void DrawBloom()
        {
            Color color = Color.Yellow;
            this.PreDraw(ref color);
        }
    }







    public class GoldenCrackEffect : ModSystem
    {
        RenderTarget2D render, screen , bloomTarget1 = null, bloomTarget2 = null;
        Effect GoldenCrack;
        public override void OnModLoad()
        {
            GoldenCrack = ModContent.Request<Effect>("Everglow/Sources/Modules/IIIDModule/Effects/GoldenCrack").Value;
            On.Terraria.Graphics.Effects.FilterManager.EndCapture += FilterManager_EndCapture;//原版绘制场景的最后部分——滤镜。在这里运用render保证不会与原版冲突
            Main.OnResolutionChanged += Main_OnResolutionChanged;
            On.Terraria.Main.LoadWorlds += Main_OnLoadWorlds;
            base.OnModLoad();
        }

        public override void OnModUnload()
        {
            On.Terraria.Graphics.Effects.FilterManager.EndCapture -= FilterManager_EndCapture;
            Main.OnResolutionChanged -= Main_OnResolutionChanged;
            On.Terraria.Main.LoadWorlds -= Main_OnLoadWorlds;
            base.OnModUnload();
        }
        private void GetOrig(GraphicsDevice graphicsDevice)
        {
            graphicsDevice.SetRenderTarget(screen);
            graphicsDevice.Clear(Color.Transparent);
            Main.spriteBatch.Begin((SpriteSortMode)0, BlendState.AlphaBlend);
            Main.spriteBatch.Draw((Texture2D)(object)Main.screenTarget, Vector2.Zero, Color.White);
            Main.spriteBatch.End();
        }
        private void FilterManager_EndCapture(On.Terraria.Graphics.Effects.FilterManager.orig_EndCapture orig, Terraria.Graphics.Effects.FilterManager self, Microsoft.Xna.Framework.Graphics.RenderTarget2D finalTexture, Microsoft.Xna.Framework.Graphics.RenderTarget2D screenTarget1, Microsoft.Xna.Framework.Graphics.RenderTarget2D screenTarget2, Microsoft.Xna.Framework.Color clearColor)
        {
            GraphicsDevice gd = Main.instance.GraphicsDevice;
            SpriteBatch sb = Main.spriteBatch;

            if (render == null)
            {
                CreateRender(new Vector2(Main.screenWidth, Main.screenHeight));
            }
            if (gd == null)
            {
                return;
            }


            Effect Bloom = ModContent.Request<Effect>("Everglow/Sources/Modules/MEACModule/Effects/Bloom1", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            //保存原图
            gd.SetRenderTarget(Main.screenTargetSwap);
            gd.Clear(Color.Transparent);
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            Main.spriteBatch.Draw(Main.screenTarget, Vector2.Zero, Color.White);
            Main.spriteBatch.End();

            //在screen上绘制发光部分
            gd.SetRenderTarget(screen);
            gd.Clear(Color.Transparent);
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            foreach (Projectile proj in Main.projectile)
            {
                if (proj.active)
                {
                    if (proj.ModProjectile is GoldenCrack ModProj)
                    {
                        ModProj.DrawBloom();
                    }
                }
            }
            Main.spriteBatch.End();

            //取样

            gd.SetRenderTarget(bloomTarget2);
            gd.Clear(Color.Transparent);
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            Bloom.CurrentTechnique.Passes[0].Apply();//取亮度超过m值的部分
            Bloom.Parameters["m"].SetValue(0.5f);
            Main.spriteBatch.Draw(screen, new Rectangle(0, 0, Main.screenWidth / 3, Main.screenHeight / 3), Color.White);
            Main.spriteBatch.End();

            //处理

            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            Bloom.Parameters["uScreenResolution"].SetValue(new Vector2(Main.screenWidth, Main.screenHeight) / 3f);
            Bloom.Parameters["uRange"].SetValue(1.5f);//范围
            Bloom.Parameters["uIntensity"].SetValue(0.97f);//发光强度
            for (int i = 0; i < 2; i++)//交替使用两个RenderTarget2D，进行多次模糊
            {
                Bloom.CurrentTechnique.Passes["GlurV"].Apply();//横向
                gd.SetRenderTarget(bloomTarget1);
                gd.Clear(Color.Transparent);
                Main.spriteBatch.Draw(bloomTarget2, Vector2.Zero, Color.White);

                Bloom.CurrentTechnique.Passes["GlurH"].Apply();//纵向
                gd.SetRenderTarget(bloomTarget2);
                gd.Clear(Color.Transparent);
                Main.spriteBatch.Draw(bloomTarget1, Vector2.Zero, Color.White);
            }
            Main.spriteBatch.End();

            gd.SetRenderTarget(Main.screenTarget);
            gd.Clear(Color.Transparent);
            //叠加
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            Main.spriteBatch.Draw(Main.screenTargetSwap, Vector2.Zero, Color.White);
            Main.spriteBatch.Draw(bloomTarget2, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);
            Main.spriteBatch.End();



            GoldenCrack = ModContent.Request<Effect>("Everglow/Sources/Modules/IIIDModule/Effects/GoldenCrack").Value;
            GetOrig(gd);
            gd.SetRenderTarget(Main.screenTargetSwap);
            gd.Clear(Color.Transparent);
            Main.spriteBatch.Begin((SpriteSortMode)0, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, (Effect)null, Main.GameViewMatrix.TransformationMatrix);
            foreach (Projectile proj in Main.projectile)
            {
                if (!(proj).active)
                {
                    continue;
                }
                if (proj.type == ModContent.ProjectileType<GoldenCrack>())
                {
                    Color c3 = Color.White;
                    ((proj.ModProjectile as GoldenCrack)).PreDraw(ref c3);
                }
            }

            Main.spriteBatch.End();
            gd.SetRenderTarget(Main.screenTarget);
            gd.Clear(Color.Transparent);
            Main.spriteBatch.Begin((SpriteSortMode)0, BlendState.AlphaBlend);
            Main.spriteBatch.Draw((Texture2D)(object)screen, Vector2.Zero, Color.White);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend);
            GoldenCrack.CurrentTechnique.Passes[0].Apply();
            GoldenCrack.Parameters["m"].SetValue(0.1f);
            GoldenCrack.Parameters["t"].SetValue(0.1f);
            GoldenCrack.Parameters["tex0"].SetValue(ModContent.Request<Texture2D>("Everglow/Sources/Modules/IIIDModule/Projectiles/NonIIIDProj/GoldenCrack/GoldenCrack").Value);
            Main.spriteBatch.Draw((Texture2D)(object)Main.screenTargetSwap, Vector2.Zero, Color.White);
            Main.spriteBatch.End();




            orig(self, finalTexture, screenTarget1, screenTarget2, clearColor);
        }
        private void Main_OnResolutionChanged(Vector2 obj)//在分辨率更改时，重建render防止某些bug
        {
            if (render != null)
            {
                CreateRender(new Vector2(Main.screenWidth, Main.screenHeight));
            }
        }
        private void Main_OnLoadWorlds(On.Terraria.Main.orig_LoadWorlds orig)
        {
            if (render != null)
            {
                CreateRender(new Vector2(Main.screenWidth, Main.screenHeight));
            }
            orig();
        }
        public void CreateRender(Vector2 obj)
        {
            render = new RenderTarget2D(Main.graphics.GraphicsDevice, Main.screenWidth, Main.screenHeight);
            screen = new RenderTarget2D(Main.graphics.GraphicsDevice, Main.screenWidth, Main.screenHeight);
            bloomTarget1 = new RenderTarget2D(Main.graphics.GraphicsDevice, (int)obj.X / 3, (int)obj.Y / 3);
            bloomTarget2 = new RenderTarget2D(Main.graphics.GraphicsDevice, (int)obj.X / 3, (int)obj.Y / 3);
        }
    }
}

