namespace Everglow.Yggdrasil.YggdrasilTown.Buffs;

public class SpritialDamage : ModBuff
{
	public override string Texture => Commons.ModAsset.DebuffTemplate_Mod;

	public override void SetStaticDefaults()
	{
		Main.debuff[Type] = true;
		Main.buffNoSave[Type] = true;
	}

	public override void Update(NPC npc, ref int buffIndex)
	{
	}
}