using Everglow.Yggdrasil.KelpCurtain.Tiles.GeyserAirBuds;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;

namespace Everglow.Yggdrasil.KelpCurtain.Tiles.DeathJadeLake.LightningMechanism;

public class UnderwaterLightningMechanismEntity : ModTileEntity
{
	public enum MechanismState
	{
		Resting, // 冷却
		Normal, // 常规
	}

	private MechanismState state = MechanismState.Resting; // 当前状态
	public int RestTimer = 0; // 冷却计时器
	public int StartFrame = 0; // 开始帧
	public int CurrentFrame = -1; // 当前帧
	private int frameCounter = 0; // 帧计时器
	private const int FrameTimer = 4; // 帧间隔
	private const int AllFrames = 10; // 总帧数

	public override void Update()
	{
		if (CurrentFrame == -1)
		{
			CurrentFrame = StartFrame;
		}
		if(state == MechanismState.Resting)
		{
			frameCounter++;
			if (frameCounter >= FrameTimer)
			{
				frameCounter = 0;
				CurrentFrame++;
				if (CurrentFrame >= AllFrames)
				{
					CurrentFrame = 0;
					state = MechanismState.Normal;
				}
			}
		}
	}

	public void SetState(MechanismState state)
	{
		this.state = state;
	}

	public int GetFrame()
	{
		return CurrentFrame;
	}

	public MechanismState GetState()
	{
		return state;
	}

	public override bool IsTileValidForEntity(int x, int y)
	{
		Tile tile = Main.tile[x, y];
		return tile.HasTile && (tile.TileType == ModContent.TileType<UnderwaterLightningMechanism>() || tile.TileType == ModContent.TileType<UnderwaterLightningMechanism_H>());
	}

	public override void SaveData(TagCompound tag)
	{
		tag["State"] = (int)state;
		tag["RestTimer"] = RestTimer;
		tag["CurrentFrame"] = CurrentFrame;
		tag["FrameCounter"] = frameCounter;
	}

	public override void LoadData(TagCompound tag)
	{
		state = (MechanismState)tag.GetInt("State");
		RestTimer = tag.GetInt("RestTimer");
		CurrentFrame = tag.GetInt("CurrentFrame");
		frameCounter = tag.GetInt("FrameCounter");
	}

	public override void NetSend(BinaryWriter writer)
	{
		writer.Write((int)state);
		writer.Write(RestTimer);
		writer.Write(CurrentFrame);
		writer.Write(frameCounter);
	}

	public override void NetReceive(BinaryReader reader)
	{
		state = (MechanismState)reader.ReadInt32();
		RestTimer = reader.ReadInt32();
		CurrentFrame = reader.ReadInt32();
		frameCounter = reader.ReadInt32();
	}
}