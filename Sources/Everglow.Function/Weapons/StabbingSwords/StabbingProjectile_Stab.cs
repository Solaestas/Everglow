using Everglow.Commons.MEAC;
using Everglow.Commons.Skeleton2D;
using Everglow.Commons.Vertex;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Shaders;

namespace Everglow.Commons.Weapons.StabbingSwords
{
	public abstract class StabbingProjectile_Stab : ModProjectile
	{
		/// <summary>
		/// 常规颜色
		/// </summary>
		public Color Color = Color.White;
		/// <summary>
		/// 阴影强度
		/// </summary>
		public float Shade = 0f;
		/// <summary>
		/// 重影深度
		/// </summary>
		public float TradeShade = 0f;
		/// <summary>
		/// 重影彩色部分亮度
		/// </summary>
		public float TradeLightColorValue = 0f;
		/// <summary>
		/// 重影数量
		/// </summary>
		public int TradeLength = 0;//小于200
		/// <summary>
		/// 重影大小缩变,小于1
		/// </summary>
		public float FadeScale = 0f;
		/// <summary>
		/// 刀光宽度1
		/// </summary>
		public float DrawWidth = 1f;
		/// <summary>
		/// 重影深度缩变,小于1
		/// </summary>
		public float FadeTradeShade = 0f;
		/// <summary>
		/// 重影彩色部分亮度缩变,小于1
		/// </summary>
		public float FadeLightColorValue = 0f;
		/// <summary>
		/// 表示刺剑攻击长度,标准长度1
		/// </summary>
		public float MaxLength = 1f;
		/// <summary>
		/// 荧光颜色,默认不会发光
		/// </summary>
		public Color GlowColor = Color.Transparent;
		/// <summary>
		/// 荧光颜色缩变,小于1
		/// </summary>
		public float FadeGlowColorValue = 0f;
		public override string Texture => "Everglow/Commons/Weapons/StabbingSwords/StabbingProjectile";
		public override void SetDefaults()
		{
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.netImportant = true;
			Projectile.friendly = false;
			Projectile.hostile = false;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 15;
			Projectile.extraUpdates = 5;
			Projectile.tileCollide = false;
		}
		Vector2 mainVec = Vector2.One;
		Vector2 startCenter = Vector2.zeroVector;
		int toKill = 120;
		public override void OnSpawn(IEntitySource source)
		{
			Player player = Main.player[Projectile.owner];
			Vector2 toMouse = Main.MouseWorld - player.RotatedRelativePoint(player.MountedCenter);
			toMouse.Normalize();
			if (toMouse.HasNaNs())
			{
				toMouse = Vector2.UnitX * player.direction;
			}
			if (toMouse.X != Projectile.velocity.X || toMouse.Y != Projectile.velocity.Y)
			{
				Projectile.netUpdate = true;
			}
			Projectile.velocity = toMouse;
			SoundStyle ss = new SoundStyle("Everglow/Commons/Weapons/StabbingSwords/swordswing");
			SoundEngine.PlaySound(ss, Projectile.Center);
			startCenter = Projectile.Center;
		}
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			mainVec = Projectile.velocity;
			ProduceWaterRipples(new Vector2(mainVec.Length(), 30));
			if (Projectile.timeLeft <= 1)
			{
				toKill--;
				if(toKill > 0)
				{
					Projectile.timeLeft++;
					float value = (Projectile.timeLeft + toKill) / 135f;
					float BodyRotation = MathF.Sin(value * MathF.PI) * player.direction * 0.5f;
					TestPlayerDrawer Tplayer = player.GetModPlayer<TestPlayerDrawer>();
					Tplayer.HeadRotation = 0;
					Tplayer.HideLeg = true;
					player.headRotation = -BodyRotation;
					Tplayer.HeadRotation = player.headRotation;
					player.fullRotation = BodyRotation;
					player.fullRotationOrigin = new Vector2(player.Hitbox.Width / 2f, player.gravDir == -1 ? 0 : player.Hitbox.Height);
				}
				else
				{
					TestPlayerDrawer Tplayer = player.GetModPlayer<TestPlayerDrawer>();
					player.legFrame = new Rectangle(0, 0, player.legFrame.Width, player.legFrame.Height);
					player.fullRotation = 0;
					player.legRotation = 0;
					Tplayer.HeadRotation = 0;
					Tplayer.HideLeg = false;

					player.legPosition = Vector2.Zero;
				}
			}
			if (toKill >= 120)
			{
				Projectile.position = player.RotatedRelativePoint(player.MountedCenter, reverseRotation: false, addGfxOffY: false) - Projectile.Size / 2f + Projectile.velocity * (15 - Projectile.timeLeft) * 2;
				Projectile.rotation = Projectile.velocity.ToRotation();
				Projectile.spriteDirection = Projectile.direction;

			}
			else
			{
				Projectile.extraUpdates = 24;
			}
			
		}
		private void ProduceWaterRipples(Vector2 beamDims)
		{
			mainVec = Projectile.velocity;
			var shaderData = (WaterShaderData)Terraria.Graphics.Effects.Filters.Scene["WaterDistortion"].GetShader();
			float waveSine = 1f * (float)Math.Sin(Main.GlobalTimeWrappedHourly * 20f);
			Vector2 ripplePos = Projectile.Center + new Vector2(beamDims.X * 0.5f, 0f).RotatedBy(mainVec.ToRotation());
			Color waveData = new Color(0.5f, 0.1f * Math.Sign(waveSine) + 0.5f, 0f, 1f) * Math.Abs(waveSine);
			shaderData.QueueRipple(ripplePos, waveData, beamDims, RippleShape.Square, mainVec.ToRotation());
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Player player = Main.player[Projectile.owner];
			if(toKill > 60)
			{
				Texture2D itemTexture = TextureAssets.Item[player.HeldItem.type].Value;
				Main.spriteBatch.Draw(itemTexture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation + MathF.PI * 0.25f, itemTexture.Size() / 2f, 1, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override void PostDraw(Color lightColor)
		{
			Vector2 normalized = Vector2.Normalize(Projectile.velocity.RotatedBy(Math.PI * 0.5)) * 50 * toKill /120f;
			Vector2 start = startCenter;
			Vector2 end = Projectile.Center + Projectile.velocity * 100;
			float value = (Projectile.timeLeft + toKill) / 135f;
			Vector2 middle = Vector2.Lerp(end, start, MathF.Sqrt(value) * 0.5f);
			float time = (float)(Main.time * 0.03);
			float dark = MathF.Sin(value * MathF.PI) * 4;
			List<Vertex2D> bars = new List<Vertex2D>
			{
				new Vertex2D(start + normalized,new Color(0, 0, 0, 120) * 0.1f,new Vector3(1 + time, 0, 0)),
				new Vertex2D(start - normalized,new Color(0, 0, 0, 120)* 0.1f,new Vector3(1 + time, 1, 0)),
				new Vertex2D(middle + normalized,Color.White* 0.1f * dark,new Vector3(0.5f + time, 0, 0.5f)),
				new Vertex2D(middle - normalized,Color.White* 0.1f * dark,new Vector3(0.5f + time, 1, 0.5f)),
				new Vertex2D(end + normalized,Color.White* 0.9f * dark,new Vector3(0f + time, 0, 1)),
				new Vertex2D(end - normalized,Color.White* 0.9f * dark,new Vector3(0f + time, 1, 1))
			};
			if (bars.Count >= 3)
			{
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
				Effect effect = ModAsset.StabSwordEffect.Value;
				var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
				var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition, 0)) * Main.GameViewMatrix.TransformationMatrix;
				effect.Parameters["uTransform"].SetValue(model * projection);
				effect.Parameters["uProcession"].SetValue(0.21f);
				effect.CurrentTechnique.Passes[0].Apply();
				Main.graphics.graphicsDevice.Textures[0] = ModAsset.Trail_black.Value;
				Main.graphics.graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
			}
			Color alphaColor = Color;
			alphaColor.A = 0;
			alphaColor.R = (byte)(alphaColor.R * lightColor.R / 255f);
			alphaColor.G = (byte)(alphaColor.G * lightColor.G / 255f);
			alphaColor.B = (byte)(alphaColor.B * lightColor.B / 255f);

			normalized = Vector2.Normalize(Projectile.velocity.RotatedBy(Math.PI * 0.5)) * 36 * toKill / 120f;
			bars = new List<Vertex2D>
			{
				new Vertex2D(start + normalized,new Color(0, 0, 0, 0),new Vector3(1 + time, 0, 0)),
				new Vertex2D(start - normalized,new Color(0, 0, 0, 0),new Vector3(1 + time, 1, 0)),
				new Vertex2D(middle + normalized,alphaColor,new Vector3(0.5f + time, 0, 0.5f)),
				new Vertex2D(middle - normalized,alphaColor,new Vector3(0.5f + time, 1, 0.5f)),
				new Vertex2D(end + normalized,alphaColor * 1.2f,new Vector3(0f + time, 0, 1)),
				new Vertex2D(end - normalized,alphaColor * 1.2f,new Vector3(0f + time, 1, 1))
			};
			if (bars.Count >= 3)
			{
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
				Effect effect = ModAsset.StabSwordEffect.Value;
				var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
				var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition, 0)) * Main.GameViewMatrix.TransformationMatrix;
				effect.Parameters["uTransform"].SetValue(model * projection);
				effect.Parameters["uProcession"].SetValue(value * 1.1f);
				effect.CurrentTechnique.Passes[0].Apply();
				Main.graphics.graphicsDevice.Textures[0] = ModAsset.Trail_1.Value;
				Main.graphics.graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
			}

			alphaColor.A = 0;
			alphaColor.R = (byte)(565 * lightColor.R / 255f);
			alphaColor.G = (byte)(565 * lightColor.G / 255f);
			alphaColor.B = (byte)(565 * lightColor.B / 255f);
			normalized = Vector2.Normalize(Projectile.velocity.RotatedBy(Math.PI * 0.5)) * 12 * toKill / 120f;
			bars = new List<Vertex2D>
			{
				new Vertex2D(start + normalized,new Color(0, 0, 0, 0),new Vector3(1 + time, 0, 0)),
				new Vertex2D(start - normalized,new Color(0, 0, 0, 0),new Vector3(1 + time, 1, 0)),
				new Vertex2D(middle + normalized,alphaColor,new Vector3(0.5f + time, 0, 0.5f)),
				new Vertex2D(middle - normalized,alphaColor,new Vector3(0.5f + time, 1, 0.5f)),
				new Vertex2D(end + normalized,alphaColor,new Vector3(0f + time, 0, 1)),
				new Vertex2D(end - normalized,alphaColor,new Vector3(0f + time, 1, 1))
			};
			if (bars.Count >= 3)
			{
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
				Effect effect = ModAsset.StabSwordEffect.Value;
				var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
				var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition, 0)) * Main.GameViewMatrix.TransformationMatrix;
				effect.Parameters["uTransform"].SetValue(model * projection);
				effect.Parameters["uProcession"].SetValue(value * 0.6f + 0.4f);
				effect.CurrentTechnique.Passes[0].Apply();
				Main.graphics.graphicsDevice.Textures[0] = ModAsset.Trail.Value;
				Main.graphics.graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
			}
		}
	}
}
