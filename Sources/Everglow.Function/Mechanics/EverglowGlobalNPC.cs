using System.Reflection;
using Everglow.Commons.Mechanics.Miscs;
using Mono.Cecil.Cil;
using MonoMod.Cil;

namespace Everglow.Commons.Mechanics;

public class EverglowGlobalNPC : GlobalNPC
{
	public override void ApplyDifficultyAndPlayerScaling(NPC npc, int numPlayers, float balance, float bossAdjustment)
	{
		// Rejection of base value tampering by vanilla due to mode change.
		Type type = npc.ModNPC?.GetType();
		if (type != null && type.GetCustomAttribute<NoGameModeScaleAttribute>() != null)
		{
			NPCID.Sets.DontDoHardmodeScaling[npc.type] = true;
			npc.lifeMax = (int)(npc.lifeMax / Main.GameModeInfo.EnemyMaxLifeMultiplier);
			npc.damage = (int)(npc.damage / Main.GameModeInfo.EnemyDamageMultiplier);
			npc.defense = (int)(npc.defense / Main.GameModeInfo.EnemyDefenseMultiplier);
			npc.value = (int)(npc.value / Main.GameModeInfo.EnemyMoneyDropMultiplier);
			npc.knockBackResist = npc.knockBackResist / Main.GameModeInfo.KnockbackToEnemiesMultiplier;
			return;
		}
	}

	public override void Load()
	{
		IL_NPC.UpdateNPC_BuffApplyDOTs += IL_NPC_UpdateNPC_BuffApplyDOTs_lifeRegenExpectedLossPerSecond_Optimization;
	}

	/// <summary>
	/// Optmize the combat text display based on <see cref="NPC.lifeRegenExpectedLossPerSecond"/> calculation for DOT buffs.
	/// <br/>In vanilla, it will create a combat text for each tick of damage,
	/// which is redundant and causes performance issues when many NPCs are affected by DOTs.
	/// </summary>
	/// <param name="il"></param>
	private void IL_NPC_UpdateNPC_BuffApplyDOTs_lifeRegenExpectedLossPerSecond_Optimization(ILContext il)
	{
		ILCursor c = new(il);

		// Record total damage in the final tick.
		if (c.TryGotoNext(
			MoveType.After,
			x => x.MatchLdarg0(),
			x => x.MatchLdfld(out _),
			x => x.MatchLdcI4(120),
			x => x.MatchBge(out _)))
		{
			c.EmitLdcI4(0);
			c.EmitStloc(15);
		}

		if (c.TryGotoNext(
			MoveType.After,
			x => x.MatchLdcI4(1),
			x => x.MatchSub(),
			x => x.MatchStfld(out _),
			x => x.MatchLdarg(0)))
		{
			c.EmitStarg(0);
			c.EmitLdloc(15);
			c.EmitLdcI4(1);
			c.EmitAdd();
			c.EmitStloc(15);
			c.RemoveRange(18);
		}

		// Then pop the total value.(but it seems fail?)
		if (c.TryGotoNext(
			MoveType.After,
			x => x.MatchLdcI4(-120),
			x => x.MatchBle(out _)))
		{
			var label = c.DefineLabel();
			c.EmitLdloc(15);
			c.EmitLdcI4(0);
			c.EmitBgt(label);
			c.EmitRet();

			c.MarkLabel(label);

			c.EmitLdarg0();
			c.EmitLdflda(typeof(Entity).GetField("position"));
			c.EmitLdfld(typeof(Vector2).GetField("X"));
			c.EmitConvI4();

			c.EmitLdarg0();
			c.EmitLdflda(typeof(Entity).GetField("position"));
			c.EmitLdfld(typeof(Vector2).GetField("Y"));
			c.EmitConvI4();

			c.EmitLdarg0();
			c.EmitLdfld(typeof(Entity).GetField("width"));
			c.EmitLdarg0();
			c.EmitLdfld(typeof(Entity).GetField("height"));

			c.Emit(OpCodes.Newobj, typeof(Rectangle).GetConstructor(new[] { typeof(int), typeof(int), typeof(int), typeof(int) }));
			c.EmitLdsfld(typeof(CombatText).GetField("LifeRegenNegative"));

			c.EmitLdloc(15);
			c.EmitLdcI4(0);
			c.EmitLdcI4(1);

			c.EmitCall(typeof(CombatText).GetMethod("NewText", new[] { typeof(Rectangle), typeof(Color), typeof(int), typeof(bool), typeof(bool) }));
			c.EmitPop();
		}
	}
}