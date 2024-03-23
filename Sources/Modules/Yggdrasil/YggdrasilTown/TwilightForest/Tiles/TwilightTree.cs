using Everglow.Commons.Physics;
using Terraria.Localization;

namespace Everglow.Yggdrasil.YggdrasilTown.TwilightForest.Tiles;

public class TwilightTree : ModTile
{
	public override void PostSetDefaults()
	{
		Main.tileSolid[Type] = false;
		Main.tileLavaDeath[Type] = false;
		Main.tileFrameImportant[Type] = true;
		Main.tileBlockLight[Type] = false;
		Main.tileLighted[Type] = true;
		Main.tileAxe[Type] = true;
		Main.tileNoAttach[Type] = false;

		TileID.Sets.IsATreeTrunk[Type] = true;
		var modTranslation = Language.GetText("Mods.Everglow.MapEntry.TwilightTree");
		AddMapEntry(new Color(58, 53, 50), modTranslation);
		DustType = ModContent.DustType<Dusts.TwilightTreeDust>();
		AdjTiles = new int[] { Type };

		Ins.HookManager.AddHook(CodeLayer.PostDrawTiles, DrawRopes);
	}
	public List<Rope> LoadRope(Vector2 basePosition, int style, RenderingTransformFunction renderingTransform)
	{
		var result = new List<Rope>();
		switch (style)
		{
			case 3:
				var rope = new Rope(new Vector2(74, 196) + basePosition, 1f, (WorldGen.genRand.Next(0, 60) + 140) / 500f, WorldGen.genRand.Next(7, 12), renderingTransform, true);
				result.Add(rope);
				rope = new Rope(new Vector2(148, 196) + basePosition, 1f, (WorldGen.genRand.Next(0, 60) + 140) / 500f, WorldGen.genRand.Next(7, 12), renderingTransform, true);
				result.Add(rope);
				rope = new Rope(new Vector2(261, 218) + basePosition, 1f, (WorldGen.genRand.Next(0, 60) + 140) / 500f, WorldGen.genRand.Next(7, 12), renderingTransform, true);
				result.Add(rope);
				rope = new Rope(new Vector2(270, 192) + basePosition, 1f, (WorldGen.genRand.Next(0, 60) + 140) / 500f, WorldGen.genRand.Next(7, 12), renderingTransform, true);
				result.Add(rope);
				break;
		}
		return result;
	}

	private List<Rope>[] ropes = new List<Rope>[16];
	private Vector2[] basePositions = new Vector2[16];
	private Dictionary<(int x, int y), (int style, List<Rope> ropes)> hasRope = new();
	/// <summary>
	/// 记录当前每个有树枝的节点的绳子Style（即TileFrameX / 256）
	/// </summary>
	public List<(int x, int y, int style)> GetRopeStyleList()
	{
		var result = new List<(int x, int y, int style)>();
		foreach (var keyvaluepair in hasRope)
		{
			result.Add((keyvaluepair.Key.x, keyvaluepair.Key.y, keyvaluepair.Value.style));
		}
		return result;
	}
	public override IEnumerable<Item> GetItemDrops(int i, int j)
	{
		yield return new Item(ModContent.ItemType<Items.TwilightWood>());
	}
	public override bool CanDrop(int i, int j)
	{
		for (int x = 0; x < 6; x++)
		{
			Dust.NewDust(new Vector2(i * 16, j * 16), 16, 16, DustType, 0, 0, 0, default, Main.rand.NextFloat(0.5f, 1f));
		}
		var tile = Main.tile[i, j];
		if (tile.TileFrameY == 2)
		{
			for (int x = 0; x < 12; x++)
			{
				Gore.NewGore(null, new Vector2(i * 16 - 32, j * 16 - 120) + new Vector2(Main.rand.Next(80), Main.rand.Next(120)), new Vector2(0, Main.rand.NextFloat(0f, 1f)).RotatedByRandom(6.283), ModContent.GoreType<TwilightTree_Leaf>(), Main.rand.NextFloat(0.65f, 1.45f));
			}
			for (int x = 0; x < 64; x++)
			{
				Dust.NewDust(new Vector2(i * 16 - 40, j * 16 - 100), 96, 96, DustType, 0, 0, 0, default, Main.rand.NextFloat(0.5f, 1f));
			}
		}
		if (tile.TileFrameY > 3)
			Gore.NewGore(null, new Vector2(i * 16, j * 16) + new Vector2(Main.rand.Next(16), Main.rand.Next(16)), new Vector2(0, Main.rand.NextFloat(0f, 1f)).RotatedByRandom(6.283), ModContent.GoreType<TwilightTree_Leaf>(), Main.rand.NextFloat(0.65f, 1.45f));

		if (!hasRope.ContainsKey((i, j)))
		{
			Ins.Logger.Warn("Drop: Trying to access an non-existent TwilightTree rope" + (i, j).ToString());
			return false;
		}

		var ropes = hasRope[(i, j)].ropes;
		foreach (var r in ropes)
		{
			var acc = new Vector2(Main.rand.NextFloat(-1, 1), 0);
			for (int a = 0; a < r.GetMassList.Length; a++)
			{
				r.GetMassList[a].Force += acc;
				if (Main.rand.NextBool(100))
					Gore.NewGoreDirect(null, r.GetMassList[a].Position, r.GetMassList[a].Velocity * 0.1f, ModContent.GoreType<TwilightTree_Vine>());
				//被砍时对mass操纵写这里
			}
		}
		hasRope.Remove((i, j));
		return true;
	}
	private void Shake(int i, int j, int frameY)
	{
		if (Main.rand.NextBool(7))
		{
			if (frameY == 2)
			{
				for (int x = 0; x < 12; x++)
				{
					Gore.NewGore(null, new Vector2(i * 16 - 32, j * 16 - 120) + new Vector2(Main.rand.Next(80), Main.rand.Next(120)), new Vector2(0, Main.rand.NextFloat(0f, 1f)).RotatedByRandom(6.283), ModContent.GoreType<TwilightTree_Leaf>(), Main.rand.NextFloat(0.65f, 1.45f));
				}
			}
			if (frameY > 3)
				Gore.NewGore(null, new Vector2(i * 16, j * 16) + new Vector2(Main.rand.Next(16), Main.rand.Next(16)), new Vector2(0, Main.rand.NextFloat(0f, 1f)).RotatedByRandom(6.283), ModContent.GoreType<TwilightTree_Leaf>(), Main.rand.NextFloat(0.65f, 1.45f));
		}

		if (!hasRope.ContainsKey((i, j)))
		{
			Ins.Logger.Warn("Shake: Trying to access an non-existent TwilightTree rope" + (i, j).ToString());
			return;
		}

		var ropes = hasRope[(i, j)].ropes;
		foreach (var r in ropes)
		{
			var acc = new Vector2(Main.rand.NextFloat(-120, 120), 0);
			for (int a = 0; a < r.GetMassList.Length; a++)
			{
				r.GetMassList[a].Force += acc;
				if (Main.rand.NextBool(100))
					Gore.NewGoreDirect(null, r.GetMassList[a].Position, r.GetMassList[a].Velocity * 0.1f, ModContent.GoreType<TwilightTree_Vine>());
				//被砍时对mass操纵写这里
			}
		}
	}
	// TODO 144
	public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
	{
		Tile tile = Main.tile[i, j];
		Main.NewText(tile.TileFrameX);
		int Dy = -1;//向上破坏的自变化Y坐标
		if (!fail)//判定为已破碎
		{
			//以下是破坏的特效,比如落叶
			if (tile.TileFrameY < 4)
			{
				Tile tileLeft;
				Tile tileRight;
				tileLeft = Main.tile[i - 1, j];
				if (tileLeft.TileType == Type)
				{
					Shake(i - 1, j, tileLeft.TileFrameY);
				}
				tileRight = Main.tile[i + 1, j];
				if (tileRight.TileType == Type)
				{
					Shake(i + 1, j, tileRight.TileFrameY);
				}
				while (Main.tile[i, j + Dy].HasTile && Main.tile[i, j + Dy].TileType == Type && Dy > -100)
				{
					Shake(i, j + Dy, Main.tile[i, j + Dy].TileFrameY);

					tileLeft = Main.tile[i - 1, j + Dy];
					tileRight = Main.tile[i + 1, j + Dy];
					if (tileLeft.TileType == Type)
					{
						if (tileLeft.TileFrameY == 2)
							break;
						Shake(i - 1, j + Dy, tileLeft.TileFrameY);
					}
					if (tileRight.TileType == Type)
					{
						if (tileRight.TileFrameY == 2)
							break;
						Shake(i + 1, j + Dy, tileRight.TileFrameY);
					}
					Dy -= 1;
				}
				Dy = -1;//向上破坏的自变化Y坐标
				tileLeft = Main.tile[i - 1, j];
				if (tileLeft.TileType == Type)
					tileLeft.HasTile = false;
				tileRight = Main.tile[i + 1, j];
				if (tileRight.TileType == Type)
					tileRight.HasTile = false;
				while (Main.tile[i, j + Dy].TileType == Type && Dy > -100)
				{
					Tile baseTile = Main.tile[i, j + Dy];
					baseTile.HasTile = false;
					if (hasRope.ContainsKey((i, j + Dy)))
					{
						hasRope.Remove((i, j + Dy));
					}

					tileLeft = Main.tile[i - 1, j + Dy];
					tileRight = Main.tile[i + 1, j + Dy];
					if (tileLeft.TileType == Type)
					{
						//清除吊挂的藤条
						tileLeft.HasTile = false;
						if (hasRope.ContainsKey((i - 1, j + Dy)))
						{
							hasRope.Remove((i - 1, j + Dy));
						}
					}

					if (tileRight.TileType == Type)
					{
						//清除吊挂的藤条
						tileRight.HasTile = false;
						if (hasRope.ContainsKey((i + 1, j + Dy)))
						{
							hasRope.Remove((i + 1, j + Dy));
						}
					}
					Dy -= 1;
				}
			}
		}
		else
		{
			Tile tileLeft;
			Tile tileRight;
			while (Main.tile[i, j + Dy].HasTile && Main.tile[i, j + Dy].TileType == Type && Dy > -100)
			{
				Shake(i, j + Dy, Main.tile[i, j + Dy].TileFrameY);
				tileLeft = Main.tile[i - 1, j + Dy];
				tileRight = Main.tile[i + 1, j + Dy];
				if (tileLeft.TileType == Type)
				{
					if (tileLeft.TileFrameY == 2)
						break;
					Shake(i - 1, j + Dy, tileLeft.TileFrameY);
				}
				if (tileRight.TileType == Type)
				{
					if (tileRight.TileFrameY == 2)
						break;
					Shake(i + 1, j + Dy, tileRight.TileFrameY);
				}
				Dy -= 1;
			}
		}
	}
	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
	{
		Texture2D treeTexture = ModAsset.TwilightTree.Value;
		var zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
		if (Main.drawToScreen)
			zero = Vector2.Zero;
		Tile tile = Main.tile[i, j];
		int Width;
		int Height = 16;
		int TexCoordY;
		int OffsetY = 20;
		int OffsetX = 8;
		float Rot = 0;
		Color color = Lighting.GetColor(i, j);
		switch (tile.TileFrameY)
		{
			default://别的表示空,最好写-1
				return false;
			case 0:  //树桩
				Width = 116;
				Height = 42;
				TexCoordY = 298;
				break;
			case 1:  //左树干
				Width = 16;
				TexCoordY = 264;
				OffsetX += -8;
				break;
			case 2:  //右树干
				Width = 16;
				TexCoordY = 280;
				OffsetX += -8;
				break;
			case 3:  //树冠
				Width = 410;
				Height = 262;
				TexCoordY = 0;
				float Wind = Main.windSpeedCurrent / 15f;
				Rot = Wind + (float)Math.Sin(j + Main.timeForVisualEffects / 30f) * Wind * 0.3f;
				OffsetY = 24;
				//对挂条的生成
				if (!hasRope.ContainsKey((i, j)))
					InsertOneTreeRope(i, j, tile.TileFrameY);
				break;
		}
		var origin = new Vector2(Width / 2f, Height);
		spriteBatch.Draw(treeTexture, new Vector2(i * 16 + OffsetX + 8, j * 16 + OffsetY) - Main.screenPosition + zero, new Rectangle(tile.TileFrameX * Width, TexCoordY, Width, Height), color, Rot, origin, 1, SpriteEffects.None, 0);

		if (tile.TileFrameY >= 3)
		{
			var point = new Point(i, j);
			Vector2 tileCenterWS = point.ToWorldCoordinates(8f, 8f);
			if (tileCenterWS.Distance(Main.LocalPlayer.position) < 200)
			{
				var playerRect = Main.LocalPlayer.Hitbox;
				var (_, ropes) = hasRope[(i, j)];
				foreach (var rope in ropes)
				{
					for (int a = 0; a < rope.GetMassList.Length; a++)
					{
						if (playerRect.Contains(rope.GetMassList[a].Position.ToPoint()))
						{
							rope.GetMassList[a].Force += Main.LocalPlayer.velocity * 10;
							if (Main.rand.NextBool(100))
								Gore.NewGoreDirect(null, rope.GetMassList[a].Position, rope.GetMassList[a].Velocity * 0.1f, ModContent.GoreType<TwilightTree_Vine>());
						}
					}
				}
			}
		}
		return false;
	}
	public void DrawRopes()
	{
		foreach (var keyvaluepair in hasRope)
		{
			Vector2 pos = new Vector2(keyvaluepair.Key.x, keyvaluepair.Key.y) * 16;
			if (!VFXManager.InScreen(pos, 200))
			{
				hasRope.Remove(keyvaluepair.Key);
			}
		}
		Color color = Color.White;

		List<Vertex2D> bars = new List<Vertex2D>();
		foreach (var keyvaluepair in hasRope)
		{
			foreach (Rope rope in hasRope[keyvaluepair.Key].ropes)
			{
				if (rope.GetMassList.Length <= 1)
				{
					continue;
				}
				if (!Main.gamePaused)
				{
					rope.Update(1);
				}
				float wind = Main.windSpeedCurrent / 15f;
				float rot = wind + (float)Math.Sin(keyvaluepair.Key.y + Main.timeForVisualEffects / 30f) * wind * 0.3f;
				List<Vector2> massPositionsSmooth = GraphicsUtils.CatmullRom(rope.GetMassList.Select(m => m.Position), 4);

				for (int i = 1; i < massPositionsSmooth.Count; i++)
				{
					float coordY = (4 + i - massPositionsSmooth.Count) / 3f;
					coordY = Math.Max(coordY, 0);
					Vector2 normalized = massPositionsSmooth[i] - massPositionsSmooth[i - 1];
					normalized.Normalize();
					normalized = normalized.RotatedBy(MathHelper.PiOver2) * 2;
					Vector2 drawPos = massPositionsSmooth[i] - Main.screenPosition;
					Vector2 rootPos = new Vector2(keyvaluepair.Key.x, keyvaluepair.Key.y) * 16f;
					Vector2 deltaPos = rootPos - massPositionsSmooth[0];
					Vector2 rotPos = deltaPos.RotatedBy(rot);
					drawPos -= rotPos - deltaPos;
					if (i == 1)
					{
						bars.Add(drawPos - normalized, Color.Transparent, new Vector3(0, 0, 0));
						bars.Add(drawPos + normalized, Color.Transparent, new Vector3(1, 0, 0));
					}
					bars.Add(drawPos - normalized, Color.White, new Vector3(0, coordY, 0));
					bars.Add(drawPos + normalized, Color.White, new Vector3(1, coordY, 0));
					if (i == massPositionsSmooth.Count - 1)
					{
						bars.Add(drawPos - normalized, Color.Transparent, new Vector3(0, 1, 0));
						bars.Add(drawPos + normalized, Color.Transparent, new Vector3(1, 1, 0));
					}
				}
			}
		}
		if (bars.Count > 0)
		{
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
			Main.graphics.GraphicsDevice.Textures[0] = ModAsset.TwilightTree_Vine.Value;
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
			Main.spriteBatch.End();
		}
	}
	public void InsertOneTreeRope(int xTS, int yTS, int style)
	{
		var point = new Point(xTS, yTS);
		Vector2 tileCenterWS = point.ToWorldCoordinates(0, 0);
		if (ropes[style] is null)
		{
			if (style != 3)
			{
				var HalfSize = new Vector2(-5, 24);
				ropes[style] = LoadRope(tileCenterWS + HalfSize, style, (Vector2) => Vector2.Zero);
				hasRope.Add((xTS, yTS), (style, ropes[style]));
			}
			else
			{
				var HalfSize = new Vector2(-188, -244);
				ropes[style] = LoadRope(tileCenterWS + HalfSize, style, (Vector2) => Vector2.Zero);
				hasRope.Add((xTS, yTS), (style, ropes[style]));
			}
			basePositions[style] = tileCenterWS;
		}
		else if (!hasRope.ContainsKey((xTS, yTS)))
		{
			Vector2 deltaPosition = tileCenterWS - basePositions[style];
			var rs = ropes[style].Select(r => r.Clone(deltaPosition)).ToList();

			hasRope.Add((xTS, yTS), (style, rs));
		}
	}
	public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
	{
		Tile tile = Main.tile[i, j];
		if (tile.TileFrameY == 3)
		{
			r = 0.03f;
			g = 0.124f;
			b = 0.124f;
		}
		base.ModifyLight(i, j, ref r, ref g, ref b);
	}
}