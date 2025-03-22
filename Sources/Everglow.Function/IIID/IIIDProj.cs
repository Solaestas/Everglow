using Terraria.GameContent;

namespace Everglow.Commons.IIID
{
	public abstract class IIIDProj : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 120;
			Projectile.height = 120;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 1200;
			Projectile.hostile = false;
			Projectile.alpha = 255;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.hide = true;
			SetDef();
		}

		public virtual void SetDef()
		{
		}

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

		public Vector2 lookat = Main.screenPosition + Main.ScreenSize.ToVector2() / 2;
		public int RenderTargetSize = Math.Max(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width) / 2;
		public float rate = 2000F / Math.Max(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width);

		public Matrix DefaultPerspectiveMatrix()
		{
			return
							 Matrix.CreateScale(1000F / RenderTargetSize)
				* Matrix.CreateRotationX((float)Main.timeForVisualEffects * 0.01f)
				* Matrix.CreateRotationZ((float)Main.timeForVisualEffects * 0.01f)
				* Matrix.CreateTranslation(new Vector3(5, -100, 1500))
				* Matrix.CreateLookAt(
					new Vector3((Projectile.Center.X - lookat.X) / -1f, (Projectile.Center.Y - lookat.Y) / -1f, 0) * rate,
					new Vector3((Projectile.Center.X - lookat.X) / -1f, (Projectile.Center.Y - lookat.Y) / -1f, 500) * rate,
					new Vector3(0, -1, 0) * rate)
				* Main.GameViewMatrix.ZoomMatrix
				* Matrix.CreateTranslation(new Vector3(-Main.GameViewMatrix.TransformationMatrix.M41, -Main.GameViewMatrix.TransformationMatrix.M42, 0));
		}

		/// <summary>
		/// 模型 ( 用ObjReader.LoadFile("")导入 )
		/// The Model ( Imported by ObjReader.LoadFile("") )
		/// </summary>
		public ObjReader.Model model = ObjReader.LoadFile(ModAsset.Cube_Mod);

		/// <summary>
		/// 模型主要贴图
		/// The Main Texture of Model
		/// </summary>
		public Texture2D IIIDTexture = TextureAssets.MagicPixel.Value;

		/// <summary>
		/// 模型法线贴图
		/// The Normal Texture of Model
		/// </summary>
		public Texture2D NormalTexture = TextureAssets.MagicPixel.Value;

		/// <summary>
		/// 模型材质贴图
		/// The Material Texture of Model
		/// </summary>
		public Texture2D MaterialTexture = TextureAssets.MagicPixel.Value;

		/// <summary>
		/// 模型自发光贴图
		/// The Emission Texture of Model
		/// </summary>
		public Texture2D EmissionTexture = TextureAssets.MagicPixel.Value;

		/// <summary>
		/// 模型运动矩阵(可以用于处理模型的透视和旋转，以及微调模型平移，但模型的大幅度复杂移动尽量在AI（）中处理)
		/// The Matrix of Model Movement(can be used to handle the perspective and rotation of the model, as well as fine-tune the model translation, but the large and complex movement of the model should be handled in AI())
		/// </summary>
		public virtual Matrix ModelMovementMatrix()
		{
			return Matrix.identity;
		}

		/// <summary>
		/// Bloom参数（暂不可用）
		/// BloomParams(Not available temporarily)
		/// </summary>
		public BloomParams bloom = new BloomParams
		{
			BlurIntensity = 1.0f,
			BlurRadius = 1.0f,
		};

		/// <summary>
		/// 摄像机参数（FieldOfView为视场角也就是透视程度；AspectRatio为摄像头长宽比；只有在ZNear和ZFar之间的模型才可以被渲染）
		/// BloomParams (FieldOfView is the field of view, which is the degree of perspective; AspectRatio is the aspect ratio of the camera; Only models between ZNear and ZFar can be rendered)
		/// </summary>
		public ViewProjectionParams viewProjectionParams = new ViewProjectionParams
		{
			ViewTransform = Matrix.Identity,
			FieldOfView = MathF.PI / 3f,
			AspectRatio = 1.0f,
			ZNear = 1f,
			ZFar = 1200f,
		};

		/// <summary>
		/// 一些艺术效果参数（默认不启用）
		/// Some artistic effect parameters(Not enabled by default)
		/// </summary>
		public ArtParameters artParameters = new ArtParameters
		{
			EnablePixelArt = false,
			EnableOuterEdge = false,
		};

		public void DrawIIIDProj(ViewProjectionParams viewProjectionParams)
		{
			lookat = Main.screenPosition + Main.ScreenSize.ToVector2() / 2;
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
					model.texCoords[model.faces[f].TextureCoords[2]]); // 每个面的切向量

				Vector3 surfaceNormal = Vector3.Normalize(Vector3.Cross(B - A, C - A)); // 每个面的法向量
				float check = Vector3.Dot(tangentVector, surfaceNormal);

				// Debug.Assert(check == 0);
				for (int i = 0; i < 3; i++)
				{
					Vector3 axis = new Vector3(1, -3, 2);
					Vector2 texCoord = hasTexCoord ? model.texCoords[model.faces[f].TextureCoords[i]] : Vector2.Zero;
					Vector3 normal = hasNormal ? model.normals[model.faces[f].Normals[i]] : Vector3.Zero;
					Vector3 v3 = hasPosition ? model.positions[model.faces[f].Positions[i]] * 103 : Vector3.Zero;

					// v3 = SpinWithAxis(v3, axis, (float)(Main.timeForVisualEffects * 0.04f));
					// normal = SpinWithAxis(normal, axis, (float)(Main.timeForVisualEffects * 0.04f));
					vertices.Add(new VertexRP(v3, new Vector3(texCoord, 0), normal, tangentVector));
				}
			}
			var modelPipeline = ModContent.GetInstance<ModelRenderingPipeline>();

			ModelEntity entity = new ModelEntity
			{
				Vertices = vertices,
				Texture = IIIDTexture,
				NormalTexture = NormalTexture,
				MaterialTexture = MaterialTexture,
				EmissionTexture = EmissionTexture,
				ModelTransform = ModelMovementMatrix(),
			};

			modelPipeline.BeginCapture(viewProjectionParams, bloom, artParameters);

			modelPipeline.PushModelEntity(entity);

			modelPipeline.EndCapture();

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
			Main.spriteBatch.Draw(modelPipeline.ModelTarget, Vector2.Lerp(Projectile.Center, lookat, 1f) - Main.screenPosition - new Vector2(RenderTargetSize, RenderTargetSize),
				null, Color.White, 0, Vector2.One * 0.5f, 2f, SpriteEffects.None, 0);

			return;
		}
	}
}