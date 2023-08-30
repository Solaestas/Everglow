using Everglow.Yggdrasil.WorldGeneration;
using Everglow.Yggdrasil.YggdrasilTown.NPCs.SquamousShell;
using Everglow.Yggdrasil.YggdrasilTown.VFXs;
using SubworldLibrary;
using Terraria.ObjectData;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles;

public class SquamousShellSeal : ModTile
{
	public int ReSpawnTimer = 0;
	public int DissolveTimer = 0;
	public override void SetStaticDefaults()
	{
		Main.tileFrameImportant[Type] = true;
		Main.tileLavaDeath[Type] = false;

		Main.tileWaterDeath[Type] = false;
		TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
		TileObjectData.newTile.Height = 10;
		TileObjectData.newTile.Width = 20;
		TileObjectData.newTile.CoordinateHeights = new int[]
		{
			16,
			16,
			16,
			16,
			16,
			16,
			16,
			16,
			16,
			16
		};
		TileObjectData.newTile.StyleHorizontal = true;
		TileObjectData.newTile.LavaDeath = false;
		TileObjectData.addTile(Type);
		AddMapEntry(new Color(79, 76, 75));
	}
	public override bool CanExplode(int i, int j)
	{
		return false;
	}
	public override void NumDust(int i, int j, bool fail, ref int num)
	{
		num = 0;
		base.NumDust(i, j, fail, ref num);
	}
	public override bool CanKillTile(int i, int j, ref bool blockDamaged)
	{
		return false;
	}
	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
	{
		Tile tile = Main.tile[i, j];
		if (tile.TileFrameX == 0 && tile.TileFrameY == 0)
		{
			Color lightColor = Lighting.GetColor(i + 10, j + 5);
			var zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
				zero = Vector2.Zero;

			Texture2D deadS = ModAsset.DeadSquamousShell.Value;
			Texture2D frontSeal = ModAsset.SquamousShellSeal_front.Value;
			Texture2D backSeal = ModAsset.SquamousShellSeal_back.Value;
			Texture2D lightEffect = ModAsset.SquamousShellSeal.Value;
			if (ReSpawnTimer <= 0)
			{
				spriteBatch.Draw(backSeal, new Vector2(i, j) * 16 - Main.screenPosition + zero + new Vector2(8), null, lightColor, 0, Vector2.zeroVector, 1f, SpriteEffects.None, 0);
				spriteBatch.Draw(lightEffect, new Vector2(i, j) * 16 - Main.screenPosition + zero + new Vector2(8), null, new Color(1f, 1f, 1f, 0), 0, Vector2.zeroVector, 1f, SpriteEffects.None, 0);
				spriteBatch.Draw(deadS, new Vector2(i, j) * 16 - Main.screenPosition + zero + new Vector2(8), null, lightColor, 0, Vector2.zeroVector, 1f, SpriteEffects.None, 0);
				spriteBatch.Draw(frontSeal, new Vector2(i, j) * 16 - Main.screenPosition + zero + new Vector2(8), null, lightColor, 0, Vector2.zeroVector, 1f, SpriteEffects.None, 0);
			}
			else if (ReSpawnTimer < 300)
			{
				Vector2 dive = new Vector2(0, ReSpawnTimer);
				spriteBatch.Draw(backSeal, new Vector2(i, j) * 16 - Main.screenPosition + zero + dive + new Vector2(8), null, lightColor, 0, Vector2.zeroVector, 1f, SpriteEffects.None, 0);
				float colorValue = (150 - ReSpawnTimer) / 150f;
				colorValue = Math.Max(0, colorValue);
				spriteBatch.Draw(lightEffect, new Vector2(i, j) * 16 - Main.screenPosition + zero + new Vector2(8), null, new Color(colorValue, colorValue, colorValue, 0), 0, Vector2.zeroVector, 1f, SpriteEffects.None, 0);
				spriteBatch.Draw(deadS, new Vector2(i, j) * 16 - Main.screenPosition + zero + dive + new Vector2(8), null, lightColor, 0, Vector2.zeroVector, 1f, SpriteEffects.None, 0);
				spriteBatch.Draw(frontSeal, new Vector2(i, j) * 16 - Main.screenPosition + zero + dive + new Vector2(8), null, lightColor, 0, Vector2.zeroVector, 1f, SpriteEffects.None, 0);
			}
			if (NPC.CountNPCS(ModContent.NPCType<SquamousShell>()) == 0)
			{
				if (ReSpawnTimer > 300)
				{
					ReSpawnTimer = 300;
				}
				ReSpawnTimer--;
			}
			if (DissolveTimer > 0)
			{
				float colorValue = DissolveTimer / 60f;
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
				Effect dissolve = Commons.ModAsset.DissolveWithLight.Value;
				var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
				var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;
				float dissolveDuration = colorValue * 1.2f - 0.2f;
				dissolve.Parameters["uTransform"].SetValue(model * projection);
				dissolve.Parameters["uNoise"].SetValue(Commons.ModAsset.Noise_cell.Value);
				dissolve.Parameters["duration"].SetValue(dissolveDuration);
				dissolve.Parameters["uLightColor"].SetValue(lightColor.ToVector4());
				dissolve.Parameters["uDissolveColor"].SetValue(new Vector4(0.2f, 0.6f, 0.7f, 1f));
				dissolve.Parameters["uNoiseSize"].SetValue(2f);
				dissolve.Parameters["uNoiseXY"].SetValue(new Vector2(0.5f, 0.3f));
				dissolve.CurrentTechnique.Passes[0].Apply();
				spriteBatch.Draw(backSeal, new Vector2(i, j) * 16 + zero + new Vector2(8), null, lightColor, 0, Vector2.zeroVector, 1f, SpriteEffects.None, 0);
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.EffectMatrix);
				spriteBatch.Draw(lightEffect, new Vector2(i, j) * 16 - Main.screenPosition + zero + new Vector2(8), null, new Color(colorValue, colorValue, colorValue, 0), 0, Vector2.zeroVector, 1f, SpriteEffects.None, 0);
				if(DissolveTimer > 30)
				{
					spriteBatch.Draw(deadS, new Vector2(i, j) * 16 - Main.screenPosition + zero + new Vector2(8), null, lightColor, 0, Vector2.zeroVector, 1f, SpriteEffects.None, 0);
				}
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

				dissolve.Parameters["uTransform"].SetValue(model * projection);
				dissolve.Parameters["uNoise"].SetValue(Commons.ModAsset.Noise_cell.Value);
				dissolve.Parameters["duration"].SetValue(dissolveDuration);
				dissolve.Parameters["uLightColor"].SetValue(lightColor.ToVector4());
				dissolve.Parameters["uDissolveColor"].SetValue(new Vector4(0.2f, 0.6f, 0.7f, 1f));
				dissolve.Parameters["uNoiseSize"].SetValue(2f);
				dissolve.Parameters["uNoiseXY"].SetValue(new Vector2(0.5f, 0.3f));
				dissolve.CurrentTechnique.Passes[0].Apply();
				spriteBatch.Draw(frontSeal, new Vector2(i, j) * 16 + zero + new Vector2(8), null, lightColor, 0, Vector2.zeroVector, 1f, SpriteEffects.None, 0);
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.EffectMatrix);
				DissolveTimer--;
			}
			else
			{
				DissolveTimer = 0;
			}
		}
		return false;
	}
	public override void NearbyEffects(int i, int j, bool closer)
	{
		base.NearbyEffects(i, j, closer);
	}
	public override void MouseOver(int i, int j)
	{
		base.MouseOver(i, j);
	}
	public override bool RightClick(int i, int j)
	{
		if (ReSpawnTimer <= 0)
		{
			if (NPC.CountNPCS(ModContent.NPCType<SquamousShell>()) == 0)
			{
				for (int x = -20; x <= 20; x++)
				{
					for (int y = -10; y <= 10; y++)
					{
						Tile tile = Main.tile[i + x, j + y];
						if (tile.TileType == Type)
						{
							if (tile.TileFrameX == 180 && tile.TileFrameY == 162)
							{
								NPC.NewNPC(null, (i + x) * 16 + 18, (j + y) * 16 + 14, ModContent.NPCType<SquamousShell>());
								ReSpawnTimer = 360000;
								DissolveTimer = 60;
							}
						}
					}
				}
			}
		}
		return base.RightClick(i, j);
	}
}