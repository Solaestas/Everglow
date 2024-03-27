using System.Reflection;

/// <summary>
/// 此特征可以免去由于模式改变而引起的基础数值被tml篡改
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class NoGameModeScaleAttribute : Attribute
{
}
public class NoGameModeScale : GlobalNPC
{
	/// <summary>
	/// 拒绝由于模式改变而引起的基础数值被tml篡改
	/// </summary>
	/// <param name="numPlayers"></param>
	/// <param name="balance"></param>
	/// <param name="bossAdjustment"></param>
	public override void ApplyDifficultyAndPlayerScaling(NPC npc, int numPlayers, float balance, float bossAdjustment)
	{
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
		base.ApplyDifficultyAndPlayerScaling(npc, numPlayers, balance, bossAdjustment);
	}
}