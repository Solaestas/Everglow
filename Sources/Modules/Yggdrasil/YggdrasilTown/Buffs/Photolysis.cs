namespace Everglow.Yggdrasil.YggdrasilTown.Buffs;

public class Photolysis : ModBuff
{
	public override void SetStaticDefaults()
	{
		Main.debuff[Type] = true;
		Main.buffNoSave[Type] = true;
	}

	public override void Update(NPC npc, ref int buffIndex)
	{
		Dust dust = Dust.NewDustDirect(npc.TopLeft - new Vector2(4), npc.width, npc.height, ModContent.DustType<Dusts.LampWood_Dust_fluorescent_appear>());
		dust.alpha = 0;
		dust.rotation = Main.rand.NextFloat(0.3f, 0.7f);
		dust.velocity = npc.velocity * 0.1f;
		Vector3 lightValue = Lighting.GetSubLight(npc.Center);
		npc.lifeRegen -= (int)lightValue.Length();
	}
}