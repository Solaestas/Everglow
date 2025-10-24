using Everglow.Commons.DataStructures;
using Everglow.Commons.Graphics;

namespace Everglow.Commons.Utilities;

[Obsolete]
public class ValueBuffSystem : ModSystem
{
	public struct NPCValueBuff
	{
		public int Target { get; set; }

		public int Type { get; set; }

		public int Value { get; set; }

		public int ValueMax { get; set; }

		public bool BreakOut { get; set; }

		public bool Active { get; set; }
	}

	public static List<NPCValueBuff> AllBuffs { get; set; } = [];

	public override void PostDrawInterface(SpriteBatch spriteBatch)
	{
		base.PostDrawInterface(spriteBatch);
		SpriteBatchState sBS = GraphicsUtils.GetState(spriteBatch).Value;
		spriteBatch.End();
		spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, default, default, null, Main.GameViewMatrix.TransformationMatrix);
		for (int i = AllBuffs.Count - 1; i >= 0; i--)
		{
			var buff = AllBuffs[i];
			if (buff.Target >= 0)
			{
				NPC npc = Main.npc[buff.Target];
				if (npc != null)
				{
					ValueBarHelper.DrawValueBar(spriteBatch, npc.Bottom - Main.screenPosition, buff.Value / (float)buff.ValueMax, Color.White, Color.White);
					if (!Main.gamePaused)
					{
						if (buff.Value >= buff.ValueMax)
						{
							SetValue(i, buff.ValueMax);
							EnableOutBrick(i);
						}
						if (buff.BreakOut)
						{
							AddValue(i, -4);
							if (npc.life <= 15)
							{
								npc.StrikeNPC(30, 0, 1);
							}
							else
							{
								npc.life -= 15;
							}
							if (buff.Value <= 0)
							{
								AllBuffs.RemoveAt(i);
								continue;
							}
						}
					}
				}
				if (npc == null || !npc.active || npc.life <= 0)
				{
					AllBuffs.RemoveAt(i);
					continue;
				}
			}
			else
			{
				AllBuffs.RemoveAt(i);
				continue;
			}
		}
		spriteBatch.End();
		spriteBatch.Begin(sBS);
	}

	public static void AddValue(int index, int value)
	{
		if (AllBuffs.Count > index && index >= 0)
		{
			var newBuff = AllBuffs[index];
			newBuff.Value += value;
			AllBuffs[index] = newBuff;
		}
	}

	public static void SetValue(int index, int value)
	{
		if (AllBuffs.Count > index && index >= 0)
		{
			var newBuff = AllBuffs[index];
			newBuff.Value = value;
			AllBuffs[index] = newBuff;
		}
	}

	public static void EnableOutBrick(int index)
	{
		if (AllBuffs.Count > index && index >= 0)
		{
			var newBuff = AllBuffs[index];
			newBuff.BreakOut = true;
			AllBuffs[index] = newBuff;
		}
	}

	public static void DisableOutBrick(int index)
	{
		if (AllBuffs.Count > index && index >= 0)
		{
			var newBuff = AllBuffs[index];
			newBuff.BreakOut = false;
			AllBuffs[index] = newBuff;
		}
	}
}