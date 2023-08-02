using Everglow.Myth.TheFirefly.Dusts;
using Everglow.Myth.TheFirefly.Projectiles;
using Terraria.Audio;
using Terraria.DataStructures;

namespace Everglow.Myth.TheFirefly.NPCs;

public class BlackStarFruit : ModNPC
{
	public override void SetDefaults()
	{
		NPC.damage = 0;
		NPC.width = 40;
		NPC.height = 40;
		NPC.defense = 0;
		NPC.lifeMax = 1;
		NPC.knockBackResist = 0f;
		NPC.value = Item.buyPrice(0, 0, 0, 0);
		NPC.color = new Color(0, 0, 0, 0);
		NPC.alpha = 0;
		NPC.lavaImmune = true;
		NPC.noGravity = true;
		NPC.noTileCollide = true;
		NPC.behindTiles = true;
		NPC.HitSound = SoundID.NPCHit1;
		NPC.DeathSound = SoundID.NPCDeath1;
		NPC.aiStyle = -1;
	}
	public override void OnSpawn(IEntitySource source)
	{
		NPC.position.Y -= 35f;
		NPC.position.X += Main.rand.NextFloat(-16, 16);
	}
	public override float SpawnChance(NPCSpawnInfo spawnInfo)
	{
		FireflyBiome fireflyBiome = ModContent.GetInstance<FireflyBiome>();
		if (!fireflyBiome.IsBiomeActive(Main.LocalPlayer))
			return 0f;
		return 0.5f;
	}
	public override void AI()
	{
		NPC.dontTakeDamage = false;
		NPC.velocity = new Vector2(0, (float)Math.Sin(Main.time / 50f + NPC.Center.X + NPC.whoAmI) * 0.25f);
		Lighting.AddLight(NPC.Center, 0, 0.1f, 0.8f);
	}
	public override void HitEffect(NPC.HitInfo hit)
	{
		Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.zeroVector, ModContent.ProjectileType<FruitBomb>(), 50, 3);
	}
	public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		SpriteEffects effects = SpriteEffects.None;
		if (NPC.spriteDirection == 1)
			effects = SpriteEffects.FlipHorizontally;
		Texture2D lightShell = ModAsset.Lightball.Value;
		var color = new Color(10, 83, 110, 0);
		Main.spriteBatch.Draw(lightShell, NPC.Center - Main.screenPosition, null, color, NPC.rotation, lightShell.Size() / 2f, 0.025f * (float)(4 + Math.Sin(Main.time / 15d + NPC.position.X / 36d)), effects, 0f);
	}
	public override bool? CanBeHitByProjectile(Projectile projectile)
	{
		if(projectile.type == ModContent.ProjectileType<FruitBomb>())
		{
			return false;
		}
		return base.CanBeHitByProjectile(projectile);
	}
}
