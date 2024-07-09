using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Everglow.Commons.DataStructures;
using Everglow.Commons.NetUtils;
using Everglow.Yggdrasil.YggdrasilTown.VFXs;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class VitalizedRocksrProj : ModProjectile
{
	public override void SetDefaults()
	{
		Projectile.friendly = false;
		Projectile.hostile = false;
		Projectile.width = 50;
		Projectile.height = 50;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 2;
		Projectile.aiStyle = -1;
		Projectile.penetrate = -1;

	}

	public override void AI()
	{
		Player player = Main.player[Projectile.owner];
		if (Projectile.timeLeft < 60 && player.HeldItem.type == ModContent.ItemType<Items.Weapons.RockElemental.VitalizedRocks>())
		{
			Projectile.timeLeft += 2;
		}
		Projectile.Center = Vector2.Lerp(Projectile.Center, player.Center + new Vector2(player.direction * 2, MathF.Sin((float)Main.timeForVisualEffects / 20)) * 20, 0.2f);
	}

	public override bool PreDraw(ref Color lightColor)
	{
		SpriteBatchState sBS = GraphicsUtils.GetState(Main.spriteBatch).Value;
		Main.spriteBatch.End();
		Texture2D hiteffect = ModAsset.RockElemental_defense.Value;
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

		/*Effect dissolve = Commons.ModAsset.DissolveWithLight.Value;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition, 0)) * Main.GameViewMatrix.TransformationMatrix;
		float dissolveDuration = Projectile.timeLeft/ 60 * 1.2f - 0.2f;

		dissolve.Parameters["uTransform"].SetValue(model * projection);
		dissolve.Parameters["uNoise"].SetValue(Commons.ModAsset.Noise_smoothIce.Value);
		dissolve.Parameters["duration"].SetValue(dissolveDuration);
		dissolve.Parameters["uLightColor"].SetValue(new Vector4(0.3f, 0.1f, 0.4f, 0f));
		dissolve.Parameters["uDissolveColor"].SetValue(Vector4.Lerp(new Vector4(0.4f, 0.8f, 0.9f, 0.5f), new Vector4(0.7f, 0.2f, 0.9f, 0.5f), dissolveDuration));
		dissolve.Parameters["uNoiseSize"].SetValue(2f);
		dissolve.Parameters["uNoiseXY"].SetValue(new Vector2(Projectile.whoAmI * 0.04f, Projectile.position.X * 0.002f));
		dissolve.CurrentTechnique.Passes[0].Apply();*/
		Main.spriteBatch.Draw(hiteffect, Projectile.Center, null, new Color(0.4f, 0.2f, 1f, 0f), Projectile.rotation, new Vector2(45f), Projectile.scale, 0, 0);
		DrawShell(false);
		DrawCore();
		DrawShell(true);

		Main.spriteBatch.End();
		Main.spriteBatch.Begin(sBS);

		return false;
	}

	public void DrawCore()
	{
		Texture2D core = ModAsset.VitalizedRocksrProj_Core.Value;

		Player player = Main.player[Projectile.owner];
		Vector2 pos = Projectile.Center - Main.screenPosition;
		var vertex2Ds = new List<Vertex2D>();
		Main.spriteBatch.Draw(core, pos + new Vector2(-25, -25), Color.White);
	}

	public void DrawShell(bool isfront)
	{
		Texture2D Shell = ModAsset.VitalizedRocksrProj.Value;

		Player player = Main.player[Projectile.owner];
		Vector2 pos = Projectile.Center - Main.screenPosition;
		var vertex2Ds = new List<Vertex2D>();
		if (25 * MathF.Cos((float)Main.timeForVisualEffects / 20) < 0)
		{
			if (isfront)
			{

				Main.spriteBatch.Draw(Shell, new Rectangle((int)pos.X + (25 * MathF.Sin((float)Main.timeForVisualEffects / 20) > 0 ? 1 : 0), (int)(pos.Y - 25),
							   (int)(-25 * MathF.Sin((float)Main.timeForVisualEffects / 20)), 50),
							   new Rectangle(0, 0, 25, 50), Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
			}
			else
			{


				Main.spriteBatch.Draw(Shell, new Rectangle((int)(pos.X + 25 * MathF.Sin((float)Main.timeForVisualEffects / 20) + (25 * MathF.Sin((float)Main.timeForVisualEffects / 20) > 0 ? 0 : 1)), (int)(pos.Y - 25),
																   (int)(-25 * MathF.Sin((float)Main.timeForVisualEffects / 20)), 50),
																   new Rectangle(25, 0, 25, 50), Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
			}
		}
		else
		{
			if (!isfront)
			{

				Main.spriteBatch.Draw(Shell, new Rectangle((int)pos.X + (25 * MathF.Sin((float)Main.timeForVisualEffects / 20) < 0 ? 0 : 1), (int)(pos.Y - 25),
							   (int)(-25 * MathF.Sin((float)Main.timeForVisualEffects / 20)), 50),
							   new Rectangle(0, 0, 25, 50), Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
			}
			else
			{


				Main.spriteBatch.Draw(Shell, new Rectangle((int)(pos.X + 25 * MathF.Sin((float)Main.timeForVisualEffects / 20) + (25 * MathF.Sin((float)Main.timeForVisualEffects / 20) > 0 ? 0 : 1)), (int)(pos.Y - 25),
																   (int)(-25 * MathF.Sin((float)Main.timeForVisualEffects / 20)), 50),
																   new Rectangle(25, 0, 25, 50), Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
			}
		}
	}

}


