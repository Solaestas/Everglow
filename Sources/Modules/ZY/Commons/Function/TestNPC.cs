namespace Everglow.ZYModule.Commons.Function;

internal class TestNPC : ModNPC
{
	public override string Texture => $"Terraria/Images/NPC_{NPCID.Zombie}";
	public override void SetDefaults()
	{
		NPC.CloneDefaults(NPCID.Zombie);
	}
}
