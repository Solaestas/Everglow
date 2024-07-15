using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Everglow.Commons.DataStructures;
using Everglow.Commons.NetUtils;
using Everglow.Yggdrasil.YggdrasilTown.VFXs;
using Microsoft.Xna.Framework.Graphics;
using SteelSeries.GameSense.DeviceZone;
using Terraria;
using Terraria.Audio;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class VitalizedRocksProj : ModProjectile
{
	public override void SetDefaults()
	{
		Projectile.friendly = false;
		Projectile.hostile = false;
		Projectile.width = 50;
		Projectile.height = 50;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 60;
		Projectile.aiStyle = -1;
		Projectile.penetrate = -1;
	}

	private Vector2 p1 = new Vector2(Main.rand.NextFloat(), Main.rand.NextFloat());
	private Vector2 p2 = new Vector2(Main.rand.NextFloat(), Main.rand.NextFloat());
	private Vector2 p3 = new Vector2(Main.rand.NextFloat(), Main.rand.NextFloat());
	private int timer = 0;

	public override void AI()
	{
		timer--;
		Player player = Main.player[Projectile.owner];
		if (player.HeldItem.type == ModContent.ItemType<Items.Weapons.RockElemental.VitalizedRocks>())
		{
			Projectile.timeLeft = 120;
			if (Projectile.ai[0] < 60)
			{
				Projectile.ai[0] += 0.66f;
			}
			if (Projectile.ai[1] < 60)
			{
				Projectile.ai[1] += 0.75f;
			}
		}
		else
		{
			Projectile.ai[0] -= 0.75f;
			Projectile.ai[1] -= 0.66f;
		}
		Projectile.Center = Vector2.Lerp(Projectile.Center, player.Center + new Vector2(player.direction * 2, MathF.Sin((float)Main.timeForVisualEffects / 20)) * 20, 0.2f);
		if (timer <= 0 && player.controlUseItem && player.HeldItem.type == ModContent.ItemType<Items.Weapons.RockElemental.VitalizedRocks>())
		{
			timer = 30;
			for(int i = 0; i < 3; i++)
			{
				Vector2 Pos = player.Center + Vector2.unitXVector.RotatedByRandom(MathF.PI*2) * player.direction * Main.rand.NextFloat(25, 75);
				for (int j = 0; j < 50; j++)
				{
					Pos = player.Center + Vector2.unitXVector.RotatedByRandom(MathF.PI * 2) * player.direction * Main.rand.NextFloat(25, 75);
					if (Collision.CanHit(Pos, 38, 38, Main.MouseWorld, 0, 0))
					{
						break;
					}
				}
				Vector2 toMouse = (Main.MouseWorld - Pos).NormalizeSafe();
				Projectile.NewProjectileDirect(player.GetSource_FromAI(), Pos, toMouse * 9, ModContent.ProjectileType<VitalizedRocksStone>(), Projectile.damage, Projectile.knockBack, player.whoAmI);
			}
		}
	}

	public override bool PreDraw(ref Color lightColor)
	{
		SpriteBatchState sBS = GraphicsUtils.GetState(Main.spriteBatch).Value;
		Main.spriteBatch.End();
		Texture2D hiteffect = ModAsset.RockElemental_defense.Value;
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

		Effect dissolve = ModAsset.DissolveOnPoint.Value;
		float range1 = Projectile.ai[0] / 60;
		float range2 = Projectile.ai[1] / 60;

		dissolve.Parameters["range1"].SetValue(range1);
		dissolve.Parameters["range2"].SetValue(range2);
		dissolve.Parameters["p1"].SetValue(p1);
		dissolve.Parameters["p2"].SetValue(p2);
		dissolve.Parameters["p3"].SetValue(p3);

		dissolve.CurrentTechnique.Passes[0].Apply();
		DrawShell(false);
		DrawCore();
		DrawShell(true);

		Main.spriteBatch.End();
		Main.spriteBatch.Begin(sBS);

		return false;
	}

	public void DrawCore()
	{
		Texture2D core = ModAsset.VitalizedRocksProj_Core.Value;

		Vector2 pos = Projectile.Center - Main.screenPosition;
		Main.spriteBatch.Draw(core, pos + new Vector2(-25, -25), Color.White);
	}

	public void DrawShell(bool isfront)
	{
		Texture2D Shell = ModAsset.VitalizedRocksProj.Value;

		Vector2 pos = Projectile.Center - Main.screenPosition;
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