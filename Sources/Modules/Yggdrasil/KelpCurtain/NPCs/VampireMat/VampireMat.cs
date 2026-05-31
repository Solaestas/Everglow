using Everglow.Commons.Coroutines;
using Everglow.Commons.Mechanics.Miscs;
using Everglow.Commons.Physics.MassSpringSystem;
using Everglow.Yggdrasil.YggdrasilTown.NPCs;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.KelpCurtain.NPCs.VampireMat;

[AutoloadBossHead]
[NoGameModeScale]
public class VampireMat : ModNPC
{
	public CoroutineManager AICoroutine = new CoroutineManager();

	public Rope BodyRope;

	public override void SetStaticDefaults()
	{
		Main.npcFrameCount[NPC.type] = 11;
		NPCSpawnManager.RegisterNPC(Type);
	}

	public override void SetDefaults()
	{
		NPC.width = 100;
		NPC.height = 100;
		NPC.boss = true;
		NPC.noGravity = true;
		NPC.HitSound = SoundID.NPCHit1;
		NPC.DeathSound = SoundID.NPCDeath1;
		NPC.value = 32000;

		NPC.lifeMax = 26000;
		NPC.damage = 70;
		NPC.defense = 45;
		NPC.knockBackResist = 0f;

		NPC.noTileCollide = true;
		NPC.aiStyle = -1;
	}

	public override void OnSpawn(IEntitySource source)
	{
		NPC.localAI[0] = 0;
		NPC.TargetClosest();
		BodyRope = Rope.Create(NPC.Center + new Vector2(150, 0), NPC.Center - new Vector2(-150, 0), 20, 5, 5, 20, 5);
		AICoroutine.StartCoroutine(new Coroutine(Dash_0()));
	}

	public override bool CheckActive()
	{
		return false;
	}

	public override void AI()
	{
		AICoroutine.Update();
		BodyRope.Masses[0].Position = NPC.Center + new Vector2(150, 0).RotatedBy(NPC.rotation);
		BodyRope.ApplyForce();
	}

	public IEnumerator<ICoroutineInstruction> Dash_0()
	{
		yield return new WaitUntil(() => NPC.target >= 0);
		Player player = Main.player[NPC.target];
		int direction = NPC.Center.X > player.Center.X ? 1 : -1;

		NPC.spriteDirection = direction;
		Vector2 toTarget = (player.Center - NPC.Center).SafeNormalize(Vector2.Zero) * 11f;
		float rot = toTarget.ToRotation();
		NPC.velocity = toTarget;
		for (int k = 0; k < 30; k++)
		{
			NPC.rotation = rot * 0.05f + NPC.rotation * 0.95f;
			yield return new SkipThisFrame();
		}
		for (int k = 0; k < 30; k++)
		{
			NPC.velocity *= 0.96f;
			yield return new SkipThisFrame();
		}
		AICoroutine.StartCoroutine(new Coroutine(NextAttack()));
	}

	public IEnumerator<ICoroutineInstruction> NextAttack()
	{
		AICoroutine.StartCoroutine(new Coroutine(Dash_0()));
		yield return new SkipThisFrame();
	}

	public override void FindFrame(int frameHeight)
	{
		float animationSpeed = 0.4f;
		NPC.frameCounter += animationSpeed;
		NPC.frameCounter %= Main.npcFrameCount[NPC.type];
		NPC.frame.Y = (int)NPC.frameCounter * 106;
	}

	public override void HitEffect(NPC.HitInfo hit)
	{
	}

	public override float SpawnChance(NPCSpawnInfo spawnInfo)
	{
		return base.SpawnChance(spawnInfo);
	}

	public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		var texture = ModContent.Request<Texture2D>(Texture).Value;
		var frame = NPC.frame;
		var rotation = NPC.rotation;
		var spriteEffect = NPC.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipVertically;
		spriteBatch.Draw(texture, NPC.Center - Main.screenPosition, frame, drawColor, rotation, frame.Size() / 2, 0.8f, spriteEffect, 0);

		if(BodyRope is not null)
		{
			Texture2D point = Commons.ModAsset.TileBlock.Value;
			foreach (var mass in BodyRope.Masses)
			{
				spriteBatch.Draw(point, mass.Position - Main.screenPosition, null, Color.White, 0, point.Size() * 0.5f, 0.5f, SpriteEffects.None, 0);
			}
		}
		return false;
	}
}