using System;
using Everglow.Commons.Weapons;
using Terraria.DataStructures;

namespace Everglow.Myth.TheFirefly.NPCs.Bosses;

public class MothSummonEffect : TrailingProjectile
{
	public override string Texture => "Everglow/Commons/Textures/Empty";
	public NPC Moth;
	public override void SetDef()
	{
		base.SetDef();
	}
	public override void OnSpawn(IEntitySource source)
	{
		bool hasMoth = false;
		foreach (var npc in Main.npc)
		{
			if (npc.active && npc.type == ModContent.NPCType<CorruptMoth>())
			{
				if((npc.Center - Projectile.Center).Length() < 2500)
				{
					hasMoth = true;
					Moth = npc;
					break;
				}
			}
		}
		if (!hasMoth)
		{
			Projectile.active = false;
		}
		base.OnSpawn(source);
	}
	public override void AI()
	{
		base.AI();
	}
	public override bool PreDraw(ref Color lightColor)
	{
		return base.PreDraw(ref lightColor);
	}
}
