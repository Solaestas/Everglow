using Everglow.Commons.Physics;
using Microsoft.Xna.Framework.Input;
using Terraria.GameContent.Drawing;
using Terraria.GameInput;
using Terraria.ModLoader.IO;
using static Everglow.Commons.Physics.Rope;

namespace Everglow.Commons.TileHelper;

/// <summary>
/// 缆绳物块,可用于制作彩灯,彩带等
/// </summary>
public abstract class CableTile : ModTile, ITileFluentlyDrawn
{
	public override void SetStaticDefaults()
	{
		Main.tileFrameImportant[Type] = true;
		TileID.Sets.BlocksWaterDrawingBehindSelf[Type] = true;
		AddMapEntry(new Color(151, 31, 32));
	}

	/// <summary>
	/// 绳头位置 绳子
	/// </summary>
	public Dictionary<Point, Rope> RopesOfAllThisTileInTheWorld = new Dictionary<Point, Rope>();

	public override void HitWire(int i, int j)
	{
	}

	/// <summary>
	/// 玩家拉绳点位
	/// </summary>
	public Dictionary<Player, Point> HasHoldRope = new Dictionary<Player, Point>();

	/// <summary>
	/// 右键效果。不拿绳拆绳，拿绳拉绳，拉了绳挂绳
	/// </summary>
	/// <param name="i"></param>
	/// <param name="j"></param>
	/// <returns></returns>
	public override bool RightClick(int i, int j)
	{
		if (HasHoldRope.ContainsKey(Main.LocalPlayer))
		{
			Point point = HasHoldRope[Main.LocalPlayer];
			if (i != point.X || j != point.Y)
			{
				if (new Vector2(point.X - i, point.Y - j).Length() * 16 < 600)
				{
					AddRope(i, j, ConnectRope(i, j, point.X, point.Y));
				}
			}
			HasHoldRope.Remove(Main.LocalPlayer);
			return base.RightClick(i, j);
		}
		if (Main.LocalPlayer.HeldItem.ModItem is CableTileItem)
		{
			HasHoldRope.Add(Main.LocalPlayer, new Point(i, j));
			return base.RightClick(i, j);
		}
		RemoveAllRope(i, j);
		return base.RightClick(i, j);
	}

	public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
	{
		RemoveAllRope(i, j);
		base.KillTile(i, j, ref fail, ref effectOnly, ref noItem);
	}

	/// <summary>
	/// 移除挂在这个点上所有的绳子无论头尾
	/// </summary>
	/// <param name="i"></param>
	/// <param name="j"></param>
	public virtual void RemoveAllRope(int i, int j)
	{
		RemoveRopeHeadAt(i, j);
		foreach (Rope rope in RopesOfAllThisTileInTheWorld.Values)
		{
			if ((rope.GetMassList.Last().Position - new Vector2(i + 0.5f, j + 0.5f) * 16f).Length() < 4)
			{
				foreach (Point point in RopesOfAllThisTileInTheWorld.Keys)
				{
					Rope tryRope;
					RopesOfAllThisTileInTheWorld.TryGetValue(point, out tryRope);
					if (tryRope == rope)
					{
						RemoveRopeEffect(rope);
						RopesOfAllThisTileInTheWorld.Remove(point);
						break;
					}
				}
			}
		}
	}

	/// <summary>
	/// 移除此坐标为绳头的绳子
	/// </summary>
	/// <param name="i"></param>
	/// <param name="j"></param>
	public virtual void RemoveRopeHeadAt(int i, int j)
	{
		if (RopesOfAllThisTileInTheWorld.ContainsKey(new Point(i, j)))
		{
			Rope rope;
			RopesOfAllThisTileInTheWorld.TryGetValue(new Point(i, j), out rope);
			RemoveRopeEffect(rope);
			RopesOfAllThisTileInTheWorld.Remove(new Point(i, j));
		}
	}

	public override void NearbyEffects(int i, int j, bool closer)
	{
		base.NearbyEffects(i, j, closer);
	}

	/// <summary>
	/// 掐掉绳子的视觉效果
	/// </summary>
	/// <param name="rope"></param>
	public virtual void RemoveRopeEffect(Rope rope)
	{
		for (int i = 0; i <= rope.GetMassList.Length - 1; i++)
		{
			_Mass thisMass = rope.GetMassList[i];
			Dust d0 = Dust.NewDustDirect(thisMass.Position - new Vector2(4), 0, 0, DustID.Asphalt);
			d0.noGravity = true;
			d0.velocity *= 0f;
			d0.scale = 0.8f;
			if (thisMass.Mass == 2)
			{
				for (int t = 0; t < 3; t++)
				{
					Dust d = Dust.NewDustDirect(thisMass.Position + new Vector2(0, 5) - new Vector2(4), 0, 0, DustID.Electric);
					d.velocity = new Vector2(0, Main.rand.NextFloat(0f, 3.5f)).RotatedByRandom(MathHelper.TwoPi);
					d.scale = Main.rand.NextFloat(0.1f, 0.3f);
				}
			}
			if (i == 0 || i == rope.GetMassList.Length - 1)
			{
				for (int t = 0; t < 5; t++)
				{
					Dust d = Dust.NewDustDirect(thisMass.Position + new Vector2(0, 5) - new Vector2(4), 0, 0, DustID.Electric);
					d.velocity = new Vector2(0, Main.rand.NextFloat(0.5f, 6.5f)).RotatedByRandom(MathHelper.TwoPi);
					d.scale = Main.rand.NextFloat(0.3f, 0.7f);
				}
			}
		}
	}

	/// <summary>
	/// 挂上绳子
	/// </summary>
	/// <param name="i"></param>
	/// <param name="j"></param>
	/// <param name="rope"></param>
	public virtual void AddRope(int i, int j, Rope rope)
	{
		if (RopesOfAllThisTileInTheWorld.ContainsKey(new Point(i, j)))
		{
			return;
		}
		RopesOfAllThisTileInTheWorld.Add(new Point(i, j), rope);
	}

	/// <summary>
	/// 在附近遍历物块链接绳子
	/// </summary>
	/// <param name="i"></param>
	/// <param name="j"></param>
	/// <returns></returns>
	public virtual Rope FindSameAndCreateRope(int i, int j)
	{
		for (int x = -30; x < 31; x++)
		{
			for (int y = -30; y < 31; y++)
			{
				Tile tile = Main.tile[i + x, j + y];
				if (tile.TileType == Type && !(x == 0 && y == 0))
				{
					int counts = (int)new Vector2(x, y).Length() * 4;

					Rope rope = new Rope(new Vector2(i, j) * 16 + new Vector2(8), new Vector2(i + x, j + y) * 16 + new Vector2(8), counts, 7, 0.05f, (Vector2) => Vector2.Zero, true, 16, 2);

					return rope;
				}
			}
		}
		return null;
	}

	/// <summary>
	/// 两点式挂绳
	/// </summary>
	/// <param name="i"></param>
	/// <param name="j"></param>
	/// <param name="i2"></param>
	/// <param name="j2"></param>
	/// <returns></returns>
	public virtual Rope ConnectRope(int i, int j, int i2, int j2)
	{
		Tile tile = Main.tile[i2, j2];
		if (tile.TileType == Type)
		{
			int counts = (int)new Vector2(i2 - i, j2 - j).Length() * 4;
			Rope rope = new Rope(new Vector2(i, j) * 16 + new Vector2(8), new Vector2(i2, j2) * 16 + new Vector2(8), counts, 7, 0.05f, (Vector2) => Vector2.Zero, true, 16, 2);
			return rope;
		}
		return null;
	}

	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
	{
		TileFluentDrawManager.AddFluentPoint(this, i, j);
		if (HasHoldRope.ContainsKey(Main.LocalPlayer))
		{
			if (Main.LocalPlayer.HeldItem.ModItem is not CableTileItem)
			{
				HasHoldRope.Remove(Main.LocalPlayer);
				return base.PreDraw(i, j, spriteBatch);
			}
			if (Main.mouseRight && Main.mouseRightRelease)
			{
				HasHoldRope.Remove(Main.LocalPlayer);
				return base.PreDraw(i, j, spriteBatch);
			}
			if (HasHoldRope[Main.LocalPlayer] == new Point(i, j))
			{
				var zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
				if (Main.drawToScreen)
				{
					zero = Vector2.Zero;
				}
				Vector2 vZoom = Main.GameViewMatrix.Zoom;
				vZoom.Y *= Main.LocalPlayer.gravDir;

				Vector2 ScreenCenter = Main.screenTarget.Size() / 2f + Main.screenPosition; // 世界位置屏幕中心
				Vector2 CorrectedMouseScreenCenter = (Main.MouseScreen - Main.screenTarget.Size() / 2f) / vZoom; // 鼠标的中心指向位
				Vector2 CorrectedMouseWorld = CorrectedMouseScreenCenter + ScreenCenter; // 鼠标世界坐标校正
				Vector2 toTarget = new Vector2(i, j) * 16 + new Vector2(8) - CorrectedMouseWorld;
				Color drawColor = Color.Lerp(new Color(0.15f, 0.15f, 0.4f, 1f), new Color(0.45f, 0.15f, 0.8f, 1f), MathF.Sin((float)Main.timeForVisualEffects * 0.08f) * 0.5f + 0.5f) * 0.5f;
				Color lightColor = Lighting.GetColor(i, j);
				Texture2D pixel = ModAsset.Trail.Value;
				if (toTarget.Length() > 900)
				{
					HasHoldRope.Remove(Main.LocalPlayer);
				}
				if (toTarget.Length() > 600)
				{
					toTarget = Vector2.Normalize(toTarget) * 600.01f;
					drawColor = new Color(1f * lightColor.R / 255f, 0, 0, 0.3f);
					float step0 = toTarget.Length();

					for (int t = 0; t <= step0; t += 8)
					{
						float value = t / step0;
						Vector2 drawPos = Vector2.Lerp(new Vector2(i, j) * 16 + new Vector2(8), new Vector2(i, j) * 16 + new Vector2(8) - toTarget, value) - Main.screenPosition + zero;
						spriteBatch.Draw(pixel, drawPos, new Rectangle(0, 127, 2, 2), drawColor, toTarget.ToRotation(), new Vector2(1), new Vector2(4f, 1f), SpriteEffects.None, 0);
						if (t == 600)
						{
							spriteBatch.Draw(pixel, drawPos, new Rectangle(0, 127, 2, 2), drawColor, 0, new Vector2(1), new Vector2(5f, 5f), SpriteEffects.None, 0);
						}
					}
					return base.PreDraw(i, j, spriteBatch);
				}
				int x = (int)(CorrectedMouseWorld.X / 16f);
				int y = (int)(CorrectedMouseWorld.Y / 16f);
				if (x > 20 && x < Main.maxTilesX - 20)
				{
					if (y > 20 && y < Main.maxTilesY - 20)
					{
						int type = Main.tile[x, y].TileType;
						ModTile modTile0 = TileLoader.GetTile(type);
						if (modTile0 is CableTile)
						{
							Texture2D block = ModAsset.TileBlock.Value;
							drawColor = new Color(1f * lightColor.R / 255f + 0.4f, 0.4f, 0, 0.3f);
							if(RopesOfAllThisTileInTheWorld.ContainsKey(new Point(x, y)))
							{
								drawColor = new Color(1f * lightColor.R / 255f + 0.4f, 0, 0, 0.5f);
							}
							if(i == x && j == y)
							{
								drawColor = new Color(1f * lightColor.R / 255f + 0.4f, 0, 0, 0.5f);
							}
							spriteBatch.Draw(block, new Vector2(x, y) * 16 + new Vector2(8) - Main.screenPosition + zero, null, drawColor, 0, block.Size() * 0.5f, 1f, SpriteEffects.None, 0);
						}
					}
				}
				float step = toTarget.Length();
				for (int t = 0; t <= step; t += 8)
				{
					float value = t / step;
					Vector2 drawPos = Vector2.Lerp(new Vector2(i, j) * 16 + new Vector2(8), new Vector2(i, j) * 16 + new Vector2(8) - toTarget, value) - Main.screenPosition + zero;
					spriteBatch.Draw(pixel, drawPos, new Rectangle(0, 127, 2, 2), drawColor, toTarget.ToRotation(), new Vector2(1), new Vector2(4f, 1f), SpriteEffects.None, 0);
					if (t == 600)
					{
						spriteBatch.Draw(pixel, drawPos, new Rectangle(0, 127, 2, 2), drawColor, 0, new Vector2(1), new Vector2(5f, 5f), SpriteEffects.None, 0);
					}
				}
			}
		}
		return base.PreDraw(i, j, spriteBatch);
	}

	public void FluentDraw(Vector2 screenPosition, Point pos, SpriteBatch spriteBatch, TileDrawing tileDrawing)
	{
		Rope rope;
		RopesOfAllThisTileInTheWorld.TryGetValue(pos, out rope);
		if (rope != null)
		{
			if (!Main.gamePaused)
			{
				rope.Update(1);
			}
			DrawCable(rope, pos, spriteBatch, tileDrawing);
		}
	}

	public string BulbTexturePath;

	/// <summary>
	/// 绘制挂绳
	/// </summary>
	/// <param name="rope"></param>
	/// <param name="pos"></param>
	/// <param name="spriteBatch"></param>
	/// <param name="tileDrawing"></param>
	/// <param name="color"></param>
	public virtual void DrawCable(Rope rope, Point pos, SpriteBatch spriteBatch, TileDrawing tileDrawing, Color color = default(Color))
	{
		// 回声涂料
		if (!TileDrawing.IsVisible(Main.tile[pos]))
		{
			return;
		}

		var tile = Main.tile[pos];
		ushort type = tile.TileType;
		int paint = Main.tile[pos].TileColor;
		Texture2D tex = PaintedTextureSystem.TryGetPaintedTexture(BulbTexturePath, type, 1, paint, tileDrawing);
		var tileSpriteEffect = SpriteEffects.None;
		for (int i = 0; i < rope.GetMassList.Length - 1; i++)
		{
			_Mass thisMass = rope.GetMassList[i];
			_Mass nextMass = rope.GetMassList[i + 1];

			int totalPushTime = 80;
			float pushForcePerFrame = 1.26f;
			float windCycle = 0;
			if (tileDrawing.InAPlaceWithWind((int)((thisMass.Position.X - 8) / 16f), (int)((thisMass.Position.Y - 8) / 16f), 1, 1))
			{
				windCycle = tileDrawing.GetWindCycle((int)((thisMass.Position.X - 8) / 16f), (int)((thisMass.Position.Y - 8) / 16f), tileDrawing._sunflowerWindCounter);
			}

			float highestWindGridPushComplex = tileDrawing.GetHighestWindGridPushComplex((int)((thisMass.Position.X - 8) / 16f), (int)((thisMass.Position.Y - 8) / 16f), 1, 1, totalPushTime, pushForcePerFrame, 3, swapLoopDir: true);
			windCycle += highestWindGridPushComplex;
			float rotation = -windCycle * 0.4f;
			if (!Main.gamePaused)
			{
				rope.ApplyForceSpecial(i, new Vector2(windCycle * 1, 4 * thisMass.Mass));
			}

			// 支持发光涂料
			Color tileLight;
			if (color != default(Color))
			{
				tileLight = color;
			}
			else
			{
				tileLight = Lighting.GetColor((int)((thisMass.Position.X - 8) / 16f), (int)((thisMass.Position.Y - 8) / 16f)) * 10;
			}
			tileDrawing.DrawAnimatedTile_AdjustForVisionChangers(pos.X, pos.Y, tile, type, 0, 0, ref tileLight, tileDrawing._rand.NextBool(4));
			tileLight = tileDrawing.DrawTiles_GetLightOverride(pos.X, pos.Y, tile, type, 0, 0, tileLight);
			Vector2 toNextMass = nextMass.Position - thisMass.Position;
			Vector2 drawPos = thisMass.Position - Main.screenPosition;
			spriteBatch.Draw(tex, drawPos, new Rectangle(0, 2, 2, 2), tileLight, toNextMass.ToRotation(), new Vector2(1f), new Vector2(toNextMass.Length() / 2f, 1), tileSpriteEffect, 0);
			if (thisMass.Mass == 2)
			{
				spriteBatch.Draw(tex, drawPos, new Rectangle(0, 6, 10, 18), tileLight, rotation, new Vector2(5f, 0), 1f, tileSpriteEffect, 0);
				Lighting.AddLight(thisMass.Position, new Vector3(0.7f, 0.4f, 0.3f));
			}
		}
	}
}

public class CableEneity : ModTileEntity
{
	public override bool IsTileValidForEntity(int x, int y)
	{
		if (TileLoader.GetTile(Main.tile[x, y].TileType) is CableTile)
		{
			return true;
		}
		throw new NotImplementedException();
	}

	/// <summary>
	/// 种类 头尾
	/// </summary>
	public Dictionary<int, List<(int, int, int, int)>> RopesOfAllThisTileInTheWorld = new Dictionary<int, List<(int, int, int, int)>>();

	public override void SaveData(TagCompound tag)
	{
		// using MemoryStream stream = new MemoryStream();
		// using BinaryWriter writer = new BinaryWriter(stream);
		// var components = ((ModTileEntity)this).GetComponents();
		// writer.Write(components.Count);
		// foreach (var component in components)
		// {
		// writer.Write(component.GetType().FullName);
		// if (component is IBinarySupport binary)
		// {
		// binary.WriteToBinary(writer);
		// }
		// }
		// tag.Add("Loita:Components", stream.GetBuffer());
		// base.SaveData(item, tag);
	}

	public override void LoadData(TagCompound tag) => base.LoadData(tag);
}


// public class CableEneity : ModTileEntity
// {
// public override bool IsTileValidForEntity(int x, int y)
// {
// if (TileLoader.GetTile(Main.tile[x, y].TileType) is CableTile)
// {
// return true;
// }
// throw new NotImplementedException();
// }
// public override void LoadData(TagCompound tag)
// {
// foreach (ModTile modTile in TileLoader.tiles)
// {
// if (modTile is CableTile)
// {
// CableTile cableTile = modTile as CableTile;
// if (cableTile != null)
// {
// Dictionary<Point, Point> ropesOfTile;
// string path = Path.Combine(Main.SavePath, "Mods", "ModDatas", Mod.Name, "RopeDatas");
// if (!Directory.Exists(path))
// {
// return;
// }
// string WorldIDName = Main.worldID.ToString() + Main.worldName;

// string readPath = path + "\\Rope" + WorldIDName + cableTile.Name + ".ropeio";
// Dictionary<Point, Point> ropesCoord = new Dictionary<Point, Point>();
// tag.GetEnumerator(modTile.Name, List<(int, int)>);
// if (ropesCoord != new Dictionary<Point, Point>())
// {
// foreach (Point point in ropesOfTile.Keys)
// {
// Point end = ropesOfTile[point];
// Rope rope = cableTile.ConnectRope(point.X, point.Y, end.X, end.Y);
// if (rope != null)
// {
// cableTile.AddRope(point.X, point.Y, rope);
// }
// }
// }
// }
// }
// }
// base.LoadData(tag);
// }
// public override void SaveData(TagCompound tag)
// {
// foreach (ModTile modTile in TileLoader.tiles)
// {
// if (modTile is CableTile)
// {
// CableTile cableTile = modTile as CableTile;
// if (cableTile != null)
// {
// string path = Path.Combine(Main.SavePath, "Mods", "ModDatas", Mod.Name, "RopeDatas");
// if (!Directory.Exists(path))
// {
// Directory.CreateDirectory(path);
// }
// string WorldIDName = Main.worldID.ToString() + Main.worldName;

// string writePath = path + "\\Rope" + WorldIDName + cableTile.Name + ".ropeio";
// // 将 Dictionary 写入Tag
// Dictionary<Point, Point> ropesCoord = new Dictionary<Point, Point>();
// foreach (Point point in cableTile.RopesOfAllThisTileInTheWorld.Keys)
// {
// Rope rope = cableTile.RopesOfAllThisTileInTheWorld[point];
// Point end = (rope.GetMassList.Last().Position / 16f).ToPoint();
// ropesCoord.Add(point, end);
// }
// foreach(Point point in ropesCoord.Keys)
// {
// tag.Add(modTile.Name + point.ToString(), ropesCoord[point].ToString());
// }
// }
// }
// }
// base.SaveData(tag);
// }
// public static (int, int) OpenTag(string inputValue)
// {
// string cleanedInput = CleanInput(inputValue);
// if (cleanedInput.Contains(","))
// {
// // 用逗号分割字符串
// string[] parts = cleanedInput.Split(',');
// // 尝试将分割后的字符串转换成整数
// if (int.TryParse(parts[0], out int num1) && int.TryParse(parts[1], out int num2))
// {
// return (num1, num2);
// }
// else
// {
// Console.WriteLine("Wrong format string error.");
// return (-1, -1);
// }
// }
// else
// {
// Console.WriteLine("Wrong format string error.");
// return (-1, -1);
// }
// }
// static string CleanInput(string input)
// {
// // 剔除非数字和逗号
// string cleanedInput = "";
// foreach (char c in input)
// {
// if (char.IsDigit(c) || c == ',')
// {
// cleanedInput += c;
// }
// }
// return cleanedInput;
// }
// }

// public class CableSaveWorld : ModSystem
// {
// public override void LoadWorldData(TagCompound tag)
// {
// foreach (ModTile modTile in TileLoader.tiles)
// {
// if (modTile is CableTile)
// {
// CableTile cableTile = modTile as CableTile;
// if (cableTile != null)
// {
// Dictionary<Point, Point> ropesOfTile;
// string path = Path.Combine(Main.SavePath, "Mods", "ModDatas", Mod.Name, "RopeDatas");
// if (!Directory.Exists(path))
// {
// return;
// }
// string WorldIDName = Main.worldID.ToString() + Main.worldName;

// string readPath = path + "\\Rope" + WorldIDName + cableTile.Name + ".ropeio";
// ropesOfTile = DeserializeDictionary(readPath);
// if (ropesOfTile != null)
// {
// foreach (Point point in ropesOfTile.Keys)
// {
// Point end = ropesOfTile[point];
// Rope rope = cableTile.ConnectRope(point.X, point.Y, end.X, end.Y);
// if (rope != null)
// {
// cableTile.AddRope(point.X, point.Y, rope);
// }
// }
// }
// }
// }
// }
// base.LoadWorldData(tag);
// }
// public override void SaveWorldData(TagCompound tag)
// {
// foreach (ModTile modTile in TileLoader.tiles)
// {
// if (modTile is CableTile)
// {
// CableTile cableTile = modTile as CableTile;
// if (cableTile != null)
// {
// string path = Path.Combine(Main.SavePath, "Mods", "ModDatas", Mod.Name, "RopeDatas");
// if (!Directory.Exists(path))
// {
// Directory.CreateDirectory(path);
// }
// string WorldIDName = Main.worldID.ToString() + Main.worldName;

// string writePath = path + "\\Rope" + WorldIDName + cableTile.Name + ".ropeio";
// // 将 Dictionary 写入文件
// Dictionary<Point, Point> ropesCoord = new Dictionary<Point, Point>();
// foreach(Point point in cableTile.RopesOfAllThisTileInTheWorld.Keys)
// {
// Rope rope = cableTile.RopesOfAllThisTileInTheWorld[point];
// Point end = (rope.GetMassList.Last().Position / 16f).ToPoint();
// ropesCoord.Add(point, end);
// }
// SerializeDictionary(ropesCoord, writePath);
// }
// }
// }
// //Save的似乎不加一个占位符就不会被加载
// tag.Add("", 0);
// base.SaveWorldData(tag);
// }
// // 将 Dictionary 序列化并写入文件
// public void SerializeDictionary(Dictionary<Point, Point> dictionary, string filename)
// {
// try
// {
// using (FileStream fs = new FileStream(filename, FileMode.Create))
// {
// BinaryFormatter formatter = new BinaryFormatter();
// formatter.Serialize(fs, dictionary);
// }
// Console.WriteLine("Dictionary serialized and saved to file.");
// }
// catch (Exception ex)
// {
// Console.WriteLine($"An error occurred: {ex.Message}");
// }
// }

// // 从文件中反序列化 Dictionary
// public Dictionary<Point, Point> DeserializeDictionary(string filename)
// {
// try
// {
// using (FileStream fs = new FileStream(filename, FileMode.Open))
// {
// BinaryFormatter formatter = new BinaryFormatter();
// return (Dictionary<Point, Point>)formatter.Deserialize(fs);
// }
// }
// catch (Exception ex)
// {
// Console.WriteLine($"An error occurred: {ex.Message}");
// return null;
// }
// }
// }