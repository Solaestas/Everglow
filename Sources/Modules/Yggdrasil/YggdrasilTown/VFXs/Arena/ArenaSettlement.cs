using Everglow.SubSpace;
using Everglow.Yggdrasil.WorldGeneration;
using Everglow.Yggdrasil.YggdrasilTown.NPCs.TownNPCs;
using Everglow.Yggdrasil.YggdrasilTown.Tiles.LampWood.Furniture;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.VFXs.Arena;

[Pipeline(typeof(ArenaSettlementPipeline))]
public class ArenaSettlement : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PreDrawFilter;

	public int Timer;

	public Texture2D Texture = ModAsset.SettlementIconsAtlas.Value;

	public NPC BossNPC;

	public int NPCType = -1;

	public bool ShouldKill = false;

	/// <summary>
	/// 0: Success;1: Fail;2: Tie
	/// </summary>
	public int State;

	public override void OnSpawn()
	{
		base.OnSpawn();
	}

	public override void Update()
	{
		if (NPCType == -1)
		{
			if (BossNPC != null)
			{
				NPCType = BossNPC.type;
			}
			else
			{
				NPCType = -1;
			}
		}
		if (!YggdrasilTownCentralSystem.InArena_YggdrasilTown() || NPCType == -1)
		{
			Active = false;
			return;
		}

		if(State == 1 && !ShouldKill)
		{
			foreach (Player player in Main.player)
			{
				if (player != null && player.active)
				{
					player.KillMe(PlayerDeathReason.ByNPC(BossNPC.whoAmI), 99999999, -1);
					player.dead = true;
					player.respawnTimer = 9 * 60;
				}
			}
		}

		if (ShouldKill)
		{
			if (Timer > 0)
			{
				Timer -= 10;
			}
			else
			{
				Timer = 0;
				Active = false;
			}
		}
		else
		{
			if (Timer <= 120)
			{
				Timer++;
			}
			else
			{
				Timer = 120;
			}
		}
		base.Update();
	}

	public override void Draw()
	{
		float pocession = Timer / 120f;
		var screenCenter = new Vector2(Main.screenWidth, Main.screenHeight) * 0.5f;
		var drawColor = new Color(1f, 1f, 1f, 1f);
		Vector2 successSize = new Vector2(612, 322);
		Vector2 successCoord = new Vector2(0, 28);
		Vector2 failSize = new Vector2(602, 338);
		Vector2 failCoord = new Vector2(0, 378);
		var chooseSize = successSize;
		var chooseCoord = successCoord;
		if (State == 1)
		{
			chooseSize = failSize;
			chooseCoord = failCoord;
		}

		// Main Icon
		List<Vertex2D> bars = new List<Vertex2D>();
		bars.Add(screenCenter - chooseSize * 0.5f, drawColor, new Vector3(chooseCoord.X / Texture.Width, chooseCoord.Y / Texture.Height, pocession));
		bars.Add(screenCenter + new Vector2(chooseSize.X, -chooseSize.Y) * 0.5f, drawColor, new Vector3((chooseCoord.X + chooseSize.X) / Texture.Width, chooseCoord.Y / Texture.Height, pocession));
		bars.Add(screenCenter + new Vector2(-chooseSize.X, chooseSize.Y) * 0.5f, drawColor, new Vector3(chooseCoord.X / Texture.Width, (chooseSize.Y + chooseCoord.Y) / Texture.Height, pocession));
		bars.Add(screenCenter + chooseSize * 0.5f, drawColor, new Vector3((chooseSize + chooseCoord) / Texture.Size(), pocession));
		Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);

		// Return Button
		Vector2 returnSize = new Vector2(212, 48);
		Vector2 returnCoord = new Vector2(0, 734);
		Vector2 returnPos = screenCenter + new Vector2(-150, 240);
		Color returnColor = drawColor;
		if (MouseInArea(returnPos - returnSize * 0.5f, returnSize))
		{
			returnColor = new Color(0.2f, 0.2f, 0.2f, 1f);
			if (Main.mouseLeft && Main.mouseLeftRelease)
			{
				SendBack();
			}
		}
		bars = new List<Vertex2D>();
		bars.Add(returnPos - returnSize * 0.5f, returnColor, new Vector3(returnCoord.X / Texture.Width, returnCoord.Y / Texture.Height, pocession));
		bars.Add(returnPos + new Vector2(returnSize.X, -returnSize.Y) * 0.5f, returnColor, new Vector3((returnCoord.X + returnSize.X) / Texture.Width, returnCoord.Y / Texture.Height, pocession));
		bars.Add(returnPos + new Vector2(-returnSize.X, returnSize.Y) * 0.5f, returnColor, new Vector3(returnCoord.X / Texture.Width, (returnSize.Y + returnCoord.Y) / Texture.Height, pocession));
		bars.Add(returnPos + returnSize * 0.5f, returnColor, new Vector3((returnSize + returnCoord) / Texture.Size(), pocession));
		Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);

		// Retry Button
		Vector2 retrySize = new Vector2(182, 48);
		Vector2 retryCoord = new Vector2(216, 734);
		Vector2 retryPos = screenCenter + new Vector2(120, 240);
		Color retryColor = drawColor;
		if (MouseInArea(retryPos - retrySize * 0.5f, retrySize))
		{
			retryColor = new Color(0.2f, 0.2f, 0.2f, 1f);
			if (Main.mouseLeft && Main.mouseLeftRelease)
			{
				Retry();
			}
		}
		bars = new List<Vertex2D>();
		bars.Add(retryPos - retrySize * 0.5f, retryColor, new Vector3(retryCoord.X / Texture.Width, retryCoord.Y / Texture.Height, pocession));
		bars.Add(retryPos + new Vector2(retrySize.X, -retrySize.Y) * 0.5f, retryColor, new Vector3((retryCoord.X + retrySize.X) / Texture.Width, retryCoord.Y / Texture.Height, pocession));
		bars.Add(retryPos + new Vector2(-retrySize.X, retrySize.Y) * 0.5f, retryColor, new Vector3(retryCoord.X / Texture.Width, (retrySize.Y + retryCoord.Y) / Texture.Height, pocession));
		bars.Add(retryPos + retrySize * 0.5f, retryColor, new Vector3((retrySize + retryCoord) / Texture.Size(), pocession));
		Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);

		// Draw Score
		if (State == 0)
		{
			int score = YggdrasilTownCentralSystem.ArenaScore;
			Vector2 numberSize = new Vector2(52, 52);
			int offSetX = -6;
			if (score.ToString().Length == 1)
			{
				Vector2 numberTopLeft = new Vector2(2 + 54 * (score % 10), 792);
				int numberOffsetY = -40;
				Vector2 drawPos = screenCenter + new Vector2(0 + offSetX, numberOffsetY);
				bars = new List<Vertex2D>();
				bars.Add(drawPos - numberSize * 0.5f, drawColor, new Vector3(numberTopLeft / Texture.Size(), pocession));
				bars.Add(drawPos + new Vector2(numberSize.X, -numberSize.Y) * 0.5f, drawColor, new Vector3((numberTopLeft + new Vector2(numberSize.X, 0)) / Texture.Size(), pocession));
				bars.Add(drawPos + new Vector2(-numberSize.X, numberSize.Y) * 0.5f, drawColor, new Vector3((numberTopLeft + new Vector2(0, numberSize.Y)) / Texture.Size(), pocession));
				bars.Add(drawPos + numberSize * 0.5f, drawColor, new Vector3((numberTopLeft + numberSize) / Texture.Size(), pocession));
				Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);
			}
			if (score.ToString().Length == 2)
			{
				int firstNumber = score % 10;
				int secondNumber = ((score - firstNumber) % 100) / 10;
				Vector2 numberTopLeft = new Vector2(2 + 54 * firstNumber, 792);
				int numberOffsetY = -40;
				Vector2 drawPos = screenCenter + new Vector2(27 + offSetX, numberOffsetY);
				bars = new List<Vertex2D>();
				bars.Add(drawPos - numberSize * 0.5f, drawColor, new Vector3(numberTopLeft / Texture.Size(), pocession));
				bars.Add(drawPos + new Vector2(numberSize.X, -numberSize.Y) * 0.5f, drawColor, new Vector3((numberTopLeft + new Vector2(numberSize.X, 0)) / Texture.Size(), pocession));
				bars.Add(drawPos + new Vector2(-numberSize.X, numberSize.Y) * 0.5f, drawColor, new Vector3((numberTopLeft + new Vector2(0, numberSize.Y)) / Texture.Size(), pocession));
				bars.Add(drawPos + numberSize * 0.5f, drawColor, new Vector3((numberTopLeft + numberSize) / Texture.Size(), pocession));
				Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);

				numberTopLeft = new Vector2(2 + 54 * secondNumber, 792);
				drawPos = screenCenter + new Vector2(-27 + offSetX, numberOffsetY);
				bars = new List<Vertex2D>();
				bars.Add(drawPos - numberSize * 0.5f, drawColor, new Vector3(numberTopLeft / Texture.Size(), pocession));
				bars.Add(drawPos + new Vector2(numberSize.X, -numberSize.Y) * 0.5f, drawColor, new Vector3((numberTopLeft + new Vector2(numberSize.X, 0)) / Texture.Size(), pocession));
				bars.Add(drawPos + new Vector2(-numberSize.X, numberSize.Y) * 0.5f, drawColor, new Vector3((numberTopLeft + new Vector2(0, numberSize.Y)) / Texture.Size(), pocession));
				bars.Add(drawPos + numberSize * 0.5f, drawColor, new Vector3((numberTopLeft + numberSize) / Texture.Size(), pocession));
				Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);
			}
			if (score.ToString().Length == 3)
			{
				int firstNumber = score % 10;
				int secondNumber = ((score - firstNumber) % 100) / 10;
				int thirdNumber = (score - secondNumber * 10 - firstNumber) / 100;
				Vector2 numberTopLeft = new Vector2(2 + 54 * firstNumber, 792);
				int numberOffsetY = -40;
				Vector2 drawPos = screenCenter + new Vector2(54 + offSetX, numberOffsetY);
				bars = new List<Vertex2D>();
				bars.Add(drawPos - numberSize * 0.5f, drawColor, new Vector3(numberTopLeft / Texture.Size(), pocession));
				bars.Add(drawPos + new Vector2(numberSize.X, -numberSize.Y) * 0.5f, drawColor, new Vector3((numberTopLeft + new Vector2(numberSize.X, 0)) / Texture.Size(), pocession));
				bars.Add(drawPos + new Vector2(-numberSize.X, numberSize.Y) * 0.5f, drawColor, new Vector3((numberTopLeft + new Vector2(0, numberSize.Y)) / Texture.Size(), pocession));
				bars.Add(drawPos + numberSize * 0.5f, drawColor, new Vector3((numberTopLeft + numberSize) / Texture.Size(), pocession));
				Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);

				numberTopLeft = new Vector2(2 + 54 * secondNumber, 792);
				drawPos = screenCenter + new Vector2(0 + offSetX, numberOffsetY);
				bars = new List<Vertex2D>();
				bars.Add(drawPos - numberSize * 0.5f, drawColor, new Vector3(numberTopLeft / Texture.Size(), pocession));
				bars.Add(drawPos + new Vector2(numberSize.X, -numberSize.Y) * 0.5f, drawColor, new Vector3((numberTopLeft + new Vector2(numberSize.X, 0)) / Texture.Size(), pocession));
				bars.Add(drawPos + new Vector2(-numberSize.X, numberSize.Y) * 0.5f, drawColor, new Vector3((numberTopLeft + new Vector2(0, numberSize.Y)) / Texture.Size(), pocession));
				bars.Add(drawPos + numberSize * 0.5f, drawColor, new Vector3((numberTopLeft + numberSize) / Texture.Size(), pocession));
				Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);

				numberTopLeft = new Vector2(2 + 54 * thirdNumber, 792);
				drawPos = screenCenter + new Vector2(-54 + offSetX, numberOffsetY);
				bars = new List<Vertex2D>();
				bars.Add(drawPos - numberSize * 0.5f, drawColor, new Vector3(numberTopLeft / Texture.Size(), pocession));
				bars.Add(drawPos + new Vector2(numberSize.X, -numberSize.Y) * 0.5f, drawColor, new Vector3((numberTopLeft + new Vector2(numberSize.X, 0)) / Texture.Size(), pocession));
				bars.Add(drawPos + new Vector2(-numberSize.X, numberSize.Y) * 0.5f, drawColor, new Vector3((numberTopLeft + new Vector2(0, numberSize.Y)) / Texture.Size(), pocession));
				bars.Add(drawPos + numberSize * 0.5f, drawColor, new Vector3((numberTopLeft + numberSize) / Texture.Size(), pocession));
				Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);
			}
		}
	}

	public void Retry()
	{
		if(BossNPC == null || !BossNPC.active)
		{
			if(NPCType != -1)
			{
				NPC npc = NPC.NewNPCDirect(WorldGen.GetItemSource_FromTileBreak(180, 194), 180 * 16, 194 * 16, NPCType);
				BossNPC = npc;
			}
			else
			{
				CombatText.NewText(Main.LocalPlayer.Hitbox, Color.White, "Null NPCType Exception!");
				return;
			}
		}
		if (BossNPC != null && BossNPC.active)
		{
			BossNPC.position = new Point(180, 194).ToWorldCoordinates();
			TownNPC_LiveInYggdrasil tNLIY = BossNPC.ModNPC as TownNPC_LiveInYggdrasil;
			if (tNLIY != null)
			{
				tNLIY.SetDefaultsToArena();
			}
			foreach(Player player in Main.player)
			{
				if(player != null)
				{
					player.respawnTimer = 0;
					player.position = new Point(60, 144).ToWorldCoordinates();
				}
			}
			for (int x = 20; x < Main.maxTilesX - 20; x++)
			{
				int y1 = 170;
				int y2 = 185;

				Tile tile1 = TileUtils.SafeGetTile(x, y1);
				tile1.TileType = (ushort)ModContent.TileType<LampWoodPlatform>();
				tile1.HasTile = true;

				Tile tile2 = TileUtils.SafeGetTile(x, y2);
				tile2.TileType = (ushort)ModContent.TileType<LampWoodPlatform>();
				tile2.HasTile = true;
			}
		}
		ShouldKill = true;
	}

	public void SendBack()
	{
		RoomManager.ExitALevelOfRoom();
		ShouldKill = true;
	}

	public bool MouseInArea(Vector2 drawPos, Vector2 size)
	{
		Vector2 mousePos = Main.MouseScreen;
		if (mousePos.X >= drawPos.X && mousePos.X <= drawPos.X + size.X)
		{
			if (mousePos.Y >= drawPos.Y && mousePos.Y <= drawPos.Y + size.Y)
			{
				return true;
			}
		}
		return false;
	}
}