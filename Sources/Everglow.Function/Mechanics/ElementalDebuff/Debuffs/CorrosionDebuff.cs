using Everglow.Commons.Mechanics.ElementalDebuff.Projectiles;
using ReLogic.Content;

namespace Everglow.Commons.Mechanics.ElementalDebuff.Debuffs;

public class CorrosionDebuff : ElementalDebuff
{
	public const int CorrosionProjectileDamage = 20;

	public CorrosionDebuff()
		: base(ElementalDebuffType.Corrosion)
	{
		BuildUpMax = 1000;
		DurationMax = 720;
		DotDamage = 2;
		ProcDamage = 200;
	}

	public override Asset<Texture2D> Texture => ModAsset.Corrosion;

	public override Color Color => Color.Purple;

	public override void PostProc(NPC npc)
	{
		var ownerIsServer = ProccedBy == 255;
		var owner = ownerIsServer ? -1 : Main.myPlayer;
		if (ProccedBy == Main.myPlayer)
		{
			Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, npc.velocity, ModContent.ProjectileType<Corrosion_Projectile>(), CorrosionProjectileDamage, 0.5f, owner, ai0: npc.whoAmI);
		}

		// Limit the number of Corrosion_Projectile exist of an owner
		var ownCorrosionProjs = Main.projectile
			.Where(p => p.active
			&& p.owner == owner
			&& p.type == ModContent.ProjectileType<Corrosion_Projectile>());
		if (ownCorrosionProjs.Count() > Corrosion_Projectile.ExistMax_Player)
		{
			var limit = ownerIsServer
				? Corrosion_Projectile.ExistMax_Server
				: Corrosion_Projectile.ExistMax_Player;
			foreach (var proj in ownCorrosionProjs
				.OrderBy(p => p.timeLeft)
				.Take(ownCorrosionProjs.Count() - limit))
			{
				proj.Kill();
			}
		}
	}
}