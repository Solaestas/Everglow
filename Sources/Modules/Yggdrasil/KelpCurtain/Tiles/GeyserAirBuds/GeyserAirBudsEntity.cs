using Everglow.Yggdrasil.KelpCurtain.Dusts;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;

namespace Everglow.Yggdrasil.KelpCurtain.Tiles.GeyserAirBuds;

public class GeyserAirBudsEntity : ModTileEntity
{
	public enum GeyserState
	{
		Blooming, // 绽放
		Resting, // 冷却
		Recovering, // 恢复
	}

	private GeyserState state = GeyserState.Blooming; // 当前状态
	public int RestTimer = 0; // 冷却计时器
	public int StartFrame = 0; // 开始帧
	private int currentFrame = -1; // 当前帧
	private int frameCounter = 0; // 帧计时器
	private const int CoolTimer = 900; // 冷却时间
	private const int FrameTimer = 6; // 帧间隔
	private const int AllFrames = 10; // 总帧数
	private const int BloomMaxFrame = 6; // 收花收到尽头

	public override void Update()
	{
		Dust dust = Dust.NewDustDirect(Position.ToWorldCoordinates() - new Vector2(4), 0, 0, ModContent.DustType<GeyserBudDust_Red>());
		dust.velocity *= 0;
		dust.noGravity = true;
		if (currentFrame == -1)
		{
			currentFrame = StartFrame;

			// 我不知道为什么NetPlace之后会莫名其妙在左上一格的位置多Place一个，很诡异
			if (Main.tile[Position].TileFrameX != 18 || Main.tile[Position].TileFrameY != 18)
			{
				Kill(Position.X, Position.Y);
				return;
			}
		}
		switch (state)
		{
			case GeyserState.Blooming:
				frameCounter++;
				if (frameCounter == FrameTimer)
				{
					frameCounter = 0;
					currentFrame++;
					if (currentFrame >= BloomMaxFrame)
					{
						currentFrame = BloomMaxFrame;
						state = GeyserState.Resting;
					}
				}
				break;
			case GeyserState.Resting:
				RestTimer++;

				if (RestTimer == CoolTimer)
				{
					state = GeyserState.Recovering;
					RestTimer = 0;
				}
				break;
			case GeyserState.Recovering:
				frameCounter++;
				if (frameCounter == FrameTimer)
				{
					frameCounter = 0;
					currentFrame++;
					if (currentFrame == AllFrames)
					{
						Point16 origin = new(1, 1);
						int baseX = Position.X - origin.X;
						int baseY = Position.Y - origin.Y;

						// 替换为平台 Tile（GeyserAirBudsPlatform）
						int newTileType = ModContent.TileType<GeyserAirBudsPlatform>();
						for (int x = 0; x < 2; x++)
						{
							for (int y = 0; y < 2; y++)
							{
								Tile t = Main.tile[baseX + x, baseY + y];
								t.TileType = (ushort)newTileType;
								t.TileFrameX = (short)(x * 18);
								t.TileFrameY = (short)(y * 18);
							}
						}

						// 移除 TileEntity
						Kill(Position.X, Position.Y);

						// 同步
						if (Main.netMode != NetmodeID.SinglePlayer)
						{
							NetMessage.SendTileSquare(-1, baseX + 1, baseY + 1, 3, 2);
						}
					}
				}
				break;
		}
	}

	public void SetState(GeyserState state)
	{
		this.state = state;
	}

	public int GetFrame()
	{
		return currentFrame;
	}

	public GeyserState GetState()
	{
		return state;
	}

	public override bool IsTileValidForEntity(int x, int y)
	{
		Tile tile = Main.tile[x, y];
		return tile.HasTile && tile.TileType == ModContent.TileType<GeyserAirBuds>();
	}

	public override void SaveData(TagCompound tag)
	{
		tag["State"] = (int)state;
		tag["RestTimer"] = RestTimer;
		tag["CurrentFrame"] = currentFrame;
		tag["FrameCounter"] = frameCounter;
	}

	public override void LoadData(TagCompound tag)
	{
		state = (GeyserState)tag.GetInt("State");
		RestTimer = tag.GetInt("RestTimer");
		currentFrame = tag.GetInt("CurrentFrame");
		frameCounter = tag.GetInt("FrameCounter");
	}

	public override void NetSend(BinaryWriter writer)
	{
		writer.Write((int)state);
		writer.Write(RestTimer);
		writer.Write(currentFrame);
		writer.Write(frameCounter);
	}

	public override void NetReceive(BinaryReader reader)
	{
		state = (GeyserState)reader.ReadInt32();
		RestTimer = reader.ReadInt32();
		currentFrame = reader.ReadInt32();
		frameCounter = reader.ReadInt32();
	}
}