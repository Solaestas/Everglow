using Everglow.Yggdrasil.YggdrasilTown.Dusts;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.NPCs;
[NoGameModeScale]
public class ShadowstrikeCaterpillar : Caterpillar
{
	public override void SetDefaults()
	{
		base.SetDefaults();	
		SegmentBehavioralSize = 18;
		SegmentHitBoxSize = 40;
		SegmentCount = 10;
		AnimationSpeed = 3;
		NPC.knockBackResist = -0.12f;
		NPC.lifeMax = 20;
		NPC.damage = 8;
		NPC.value = 6;
		if(Main.expertMode)
		{
			NPC.knockBackResist = -0.08f;
			NPC.lifeMax = 40;
			NPC.damage = 16;
			NPC.value = 12;
		}
		if(Main.masterMode)
		{
			NPC.knockBackResist = -0.04f;
			NPC.lifeMax = 55;
			NPC.damage = 24;
			NPC.value = 15;
		}

	}
	public override void OnSpawn(IEntitySource source)
	{
		base.OnSpawn(source);
	}
	public override Rectangle GetDrawFrame(int Style)
	{
		Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
		int height = texture.Height;
		if (Style == 0)
		{
			return new Rectangle(0, 0, 30, height);
		}
		if (Style == 1)
		{
			return new Rectangle(32, 0, 32, height);
		}
		if (Style == 2)
		{
			return new Rectangle(66, 0, 26, height);
		}
		return base.GetDrawFrame(Style);
	}
	public override void SetStaticDefaults()
	{
		NPCSpawnManager.RegisterNPC(Type);
	}
	public override float SpawnChance(NPCSpawnInfo spawnInfo)
	{
		YggdrasilTownBiome YggdrasilTownBiome = ModContent.GetInstance<YggdrasilTownBiome>();
		if (!YggdrasilTownBiome.IsBiomeActive(Main.LocalPlayer))
			return 0f;
		return 8f;
	}
	public override bool PreKill()
	{
		for (int j = 0; j < Segments.Count; j++)
		{
			Vector2 v0 = new Vector2(0, Main.rand.NextFloat(0, 6f)).RotatedByRandom(MathHelper.TwoPi);
			int type = ModContent.Find<ModGore>("Everglow/ShadowstrikeCaterpillar_gore1").Type;
			if (j == 0)
			{
				type = ModContent.Find<ModGore>("Everglow/ShadowstrikeCaterpillar_gore1").Type;
			}
			if (j == Segments.Count - 1)
			{
				type = ModContent.Find<ModGore>("Everglow/ShadowstrikeCaterpillar_gore2").Type;
			}
			Gore.NewGore(NPC.GetSource_Death(), NPC.Center + Segments[j].SelfPosition, v0, type, NPC.scale);
		}
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < Segments.Count; j++)
			{
				Vector2 pos = NPC.Center + Segments[j].SelfPosition;
				Dust.NewDustDirect(pos - new Vector2(SegmentHitBoxSize / 2), SegmentHitBoxSize / 2, SegmentHitBoxSize / 2, ModContent.DustType<VerdantBlood>());
			}
		}
		return base.PreKill();
	}
}
