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
using Everglow.Commons.Vertex;
using Terraria.ID;
using Everglow.Commons.MEAC;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using ReLogic.Content;
using Everglow.IIID.Projectiles;
using Everglow.IIID.Projectiles.PlanetBefall;
using Everglow.IIID.Projectiles.NonIIIDProj.PlanetBefallArray;
using Everglow.Commons.IIID;
using Terraria.GameContent.ItemDropRules;

namespace Everglow.IIID.Projectiles.NonIIIDProj.GoldenCrack
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
			return Math.Clamp((float)Math.Max(0, Math.Sqrt(u) * Math.Cos(v) * 0.3 + 0.5) + 0.4f, 0.7f, 1f);
		}
		public void Generate()
		{
			root = new Node(0, 0, Rand(), 300 * Rand(), false, true, false, false, Vector2.Zero);
			root = Buildmaster(root, 5);
			Nodes = new Node[cnt + 1];
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
				var rad = Rand(MathHelper.Pi / 4f);
				Node brunch = new Node(cnt, (node.rad > 0) ? Math.Abs(rad) : -Math.Abs(rad), node.size * Rand(), node.length * Rand(), false, false, true, false, Position(node.position, vel, node));
				node.isfork = true;
				node.children.Add(Buildbrunch(brunch, 0));

			}
			// 参数修改了
			Node master = new Node(cnt, Rand(MathHelper.Pi / 6f), node.size * Rand(), node.length * Rand() * 0.9f, false, true, false, false, Position(node.position, vel, node));
			node.children.Add(Buildmaster(master, dep + 1));
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
			Node child = new Node(cnt, Rand(MathHelper.Pi / 6f), node.size * Rand(), node.length * Rand() * 0.9f, false, false, true, false, Position(node.position, vel, node));
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
	public class GoldenCrack : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 6;
			Projectile.height = 20;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 180;
			Projectile.tileCollide = false;
			Projectile.scale = 0.8f;
			Projectile.alpha = 60;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = true;
			Projectile.hide = true;
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
			Color color = Color.Gold;
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
						float width = Math.Clamp(Projectile.ai[0] * 25 - NodePosition[i].Length(), 0, 10) * Projectile.ai[1];
						vertices.Add(new Vertex2D(NodePosition[i] + Projectile.Center + (i == 0 ? 6 : 1) * normalDir * width * ((float)Math.Pow(0.9f, i)) - Main.screenPosition, color, new Vector3(1f, 0f, 1f)));
						vertices.Add(new Vertex2D(NodePosition[i] + Projectile.Center - (i == 0 ? 6 : 1) * normalDir * width * ((float)Math.Pow(0.9f, i)) - Main.screenPosition, color, new Vector3(1f, 0f, 1f)));
					}
				}
			}
			Main.graphics.GraphicsDevice.Textures[0] = TextureAssets.MagicPixel.Value;
			if (vertices.Count > 2)
			{
				Main.graphics.GraphicsDevice.DrawUserPrimitives<Vertex2D>(PrimitiveType.TriangleStrip, vertices.ToArray(), 0, vertices.Count - 2);
			}
			Main.spriteBatch.End();
			Main.spriteBatch.Begin((SpriteSortMode)0, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, (Effect)null, Main.GameViewMatrix.TransformationMatrix);
			return false;
		}
		public void DrawBloom()
		{
			Color color = Color.Gold;
			this.PreDraw(ref color);
		}
	}
	internal class IIIDModule : ModSystem
	{
		public float BloomIntensity = 1;
		RenderTarget2D render;
		RenderTarget2D screen;
		RenderTarget2D bloom1;
		RenderTarget2D bloom2;
		RenderTarget2D GoldenCrack;

		Effect Bloom1, GoldenCrackVFX, Radial;

		public override string Name => "IIID";
		public override void Load()
		{
			Bloom1 = ModAsset.Bloom1.Value;
			GoldenCrackVFX = ModAsset.Effects_GoldenCrack.Value;
			Radial = ModAsset.Radial.Value;
			On_FilterManager.EndCapture += FilterManager_EndCapture;//原版绘制场景的最后部分——滤镜。在这里运用render保证不会与原版冲突
			Main.OnResolutionChanged += Main_OnResolutionChanged;
		}
		public override void Unload()
		{
			On_FilterManager.EndCapture -= FilterManager_EndCapture;
			Main.OnResolutionChanged -= Main_OnResolutionChanged;
		}

		private void Main_OnResolutionChanged(Vector2 obj)//在分辨率更改时，重建render防止某些bug
		{
			if (render != null)
			{
				CreateRender();
			}
		}

		ViewProjectionParams viewProjectionParams = new ViewProjectionParams
		{
			ViewTransform = Matrix.Identity,
			FieldOfView = MathF.PI / 3f,
			AspectRatio = 1.0f,
			ZNear = 1f,
			ZFar = 2000
		};
		private void FilterManager_EndCapture(On_FilterManager.orig_EndCapture orig, FilterManager self, RenderTarget2D finalTexture, RenderTarget2D screenTarget1, RenderTarget2D screenTarget2, Color clearColor)
		{
			GraphicsDevice gd = Main.instance.GraphicsDevice;
			SpriteBatch sb = Main.spriteBatch;
			if (render == null)
			{
				CreateRender();
			}
			if (gd == null)
			{
				return;
			}

			Bloom1 = ModContent.Request<Effect>("Everglow/IIID/Effects/Bloom1").Value;
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
				if (proj.active && proj.type == ModContent.ProjectileType<GoldenCrack>())
				{
					Color c3 = Color.Gold;
					(proj.ModProjectile as GoldenCrack).PreDraw(ref c3);
				}
				if (proj.active && proj.type == ModContent.ProjectileType<PlanetBefallArray.PlanetBefallArray>())
				{
					(proj.ModProjectile as PlanetBefallArray.PlanetBefallArray).DrawBloom();
					if (BloomIntensity <= (proj.ModProjectile as PlanetBefallArray.PlanetBefallArray).BloomIntensity)
					{
						BloomIntensity = (proj.ModProjectile as PlanetBefallArray.PlanetBefallArray).BloomIntensity;
					}
					else
					{
						BloomIntensity = 1;
					}
				}
			}
			Main.spriteBatch.End();

			//取样

			gd.SetRenderTarget(bloom2);
			gd.Clear(Color.Transparent);
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
			Bloom1.CurrentTechnique.Passes[0].Apply();//取亮度超过m值的部分
			Bloom1.Parameters["m"].SetValue(0.5f);
			Main.spriteBatch.Draw(screen, new Rectangle(0, 0, Main.screenWidth / 3, Main.screenHeight / 3), Color.White);
			Main.spriteBatch.End();

			//处理

			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
			Bloom1.Parameters["uScreenResolution"].SetValue(new Vector2(Main.screenWidth, Main.screenHeight) / 3f);
			Bloom1.Parameters["uRange"].SetValue(1.5f);//范围
			Bloom1.Parameters["uIntensity"].SetValue(BloomIntensity);//发光强度
			for (int i = 0; i < 2; i++)//交替使用两个RenderTarget2D，进行多次模糊
			{
				Bloom1.CurrentTechnique.Passes["GlurV"].Apply();//横向
				gd.SetRenderTarget(bloom1);
				gd.Clear(Color.Transparent);
				Main.spriteBatch.Draw(bloom2, Vector2.Zero, Color.White);

				Bloom1.CurrentTechnique.Passes["GlurH"].Apply();//纵向
				gd.SetRenderTarget(bloom2);
				gd.Clear(Color.Transparent);
				Main.spriteBatch.Draw(bloom1, Vector2.Zero, Color.White);
			}
			Main.spriteBatch.End();

			gd.SetRenderTarget(Main.screenTarget);
			gd.Clear(Color.Transparent);

			//叠加
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
			Main.spriteBatch.Draw(Main.screenTargetSwap, Vector2.Zero, Color.White);
			Main.spriteBatch.Draw(bloom2, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);
			Main.spriteBatch.End();

			GoldenCrackVFX = ModContent.Request<Effect>("Everglow/IIID/Effects/GoldenCrack").Value;
			gd.SetRenderTarget(Main.screenTargetSwap);
			gd.Clear(Color.Transparent);
			sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
			sb.Draw(Main.screenTarget, Vector2.Zero, Color.White);
			sb.End();

			gd.SetRenderTarget(render);
			gd.Clear(Color.Transparent);
			sb.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
			foreach (Projectile proj in Main.projectile)
			{
				if (proj.active && proj.type == ModContent.ProjectileType<GoldenCrack>())
				{
					Color c3 = Color.Gold;
					(proj.ModProjectile as GoldenCrack).PreDraw(ref c3);
				}
			}

			sb.End();

			gd.SetRenderTarget(Main.screenTarget);
			gd.Clear(Color.Transparent);
			sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
			sb.Draw(Main.screenTargetSwap, Vector2.Zero, Color.White);
			sb.End();

			sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
			gd.Textures[1] = ModContent.Request<Texture2D>("Everglow/IIID/Projectiles/NonIIIDProj/GoldenCrack/GoldenCrack").Value;
			GoldenCrackVFX.CurrentTechnique.Passes["Tentacle"].Apply();
			GoldenCrackVFX.Parameters["m"].SetValue(0.01f);
			GoldenCrackVFX.Parameters["n"].SetValue(0.01f);
			sb.Draw(render, Vector2.Zero, Color.White);
			sb.End();

			gd.SetRenderTarget(Main.screenTargetSwap);
			gd.Clear(Color.Transparent);
			sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
			sb.Draw(Main.screenTarget, Vector2.Zero, Color.White);
			sb.End();

			gd.SetRenderTarget(Main.screenTarget);
			gd.Clear(Color.Transparent);
			Main.spriteBatch.Begin((SpriteSortMode)0, BlendState.Additive);
			Main.spriteBatch.Draw(Main.screenTargetSwap, Vector2.Zero, Color.White);
			foreach (Projectile proj in Main.projectile)
			{
				if (proj.active && proj.ModProjectile is IIIDProj)
				{
					(proj.ModProjectile as IIIDProj).DrawIIIDProj(viewProjectionParams);
				}
			}
			Main.spriteBatch.End();

			foreach (Projectile proj in Main.projectile)
			{
				if (proj.active && proj.type == ModContent.ProjectileType<PlanetBefallExplosion.PlanetBefallExplosion>())
				{
					float BlurOffset = (proj.ModProjectile as PlanetBefallExplosion.PlanetBefallExplosion).BlurOffset;
					gd.SetRenderTarget(Main.screenTargetSwap);
					gd.Clear(Color.Transparent);
					sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
					sb.Draw(Main.screenTarget, Vector2.Zero, Color.White);
					sb.End();

					gd.SetRenderTarget(Main.screenTarget);
					Radial = ModAsset.Radial.Value;
					sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
					Radial.CurrentTechnique.Passes["Blend"].Apply();
					Radial.Parameters["_BlurOffset"].SetValue(BlurOffset / Math.Max(10, MathF.Pow((proj.Center - Main.LocalPlayer.Center).Length() / 100, 2)));
					Radial.Parameters["_Center"].SetValue((proj.Center - Main.screenPosition)/new Vector2(Main.screenWidth, Main.screenHeight));
					sb.Draw(Main.screenTargetSwap, Vector2.Zero, Color.White);
					sb.End();
				}
			}


			orig(self, finalTexture, screenTarget1, screenTarget2, clearColor);
		}
		public override void OnModLoad()
		{
			if (render != null)
			{
				CreateRender();
			}
			base.OnModLoad();
		}
		public void CreateRender()
		{
			render = new RenderTarget2D(Main.graphics.GraphicsDevice, Main.screenWidth, Main.screenHeight);
			screen = new RenderTarget2D(Main.graphics.GraphicsDevice, Main.screenWidth, Main.screenHeight);
			GoldenCrack = new RenderTarget2D(Main.graphics.GraphicsDevice, Main.screenWidth, Main.screenHeight);
			bloom1 = new RenderTarget2D(Main.graphics.GraphicsDevice, Main.screenWidth / 3, Main.screenHeight / 3);
			bloom2 = new RenderTarget2D(Main.graphics.GraphicsDevice, Main.screenWidth / 3, Main.screenHeight / 3);
		}

	}
}

