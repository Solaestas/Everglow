using Everglow.Commons.Coroutines;
using Everglow.Commons.CustomTiles;
using Terraria.Audio;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.NPCs.SquamousShell;

[AutoloadBossHead]
public class SquamousShell : ModNPC
{
	public override void SetStaticDefaults()
	{
		Main.npcFrameCount[NPC.type] = 1;
	}
	public override void SetDefaults()
	{
		NPC.aiStyle = -1;
		NPC.damage = 30;
		NPC.width = 140;
		NPC.height = 150;
		NPC.defense = 15;
		NPC.lifeMax = 6000;
		NPC.knockBackResist = 0;
		NPC.lavaImmune = true;
		NPC.noGravity = false;
		NPC.noTileCollide = false;
		NPC.HitSound = SoundID.NPCHit1;
		NPC.DeathSound = SoundID.NPCDeath1;
		NPC.value = 20000;
		NPC.localAI[0] = 0;
	}
	public override float SpawnChance(NPCSpawnInfo spawnInfo)
	{
		return 0f;
	}
	public override void OnSpawn(IEntitySource source)
	{
		_coroutineManager.StartCoroutine(new Coroutine(Landing()));
	}
	private IEnumerator<ICoroutineInstruction> Landing()
	{
		while (!NPC.collideY)
		{
			NPC.velocity.Y += 0.2f;
			yield return new SkipThisFrame();
		}
		//这里需要一个震屏
		SoundEngine.PlaySound(SoundID.DD2_BetsyFireballImpact);
		_coroutineManager.StartCoroutine(new Coroutine(Wake()));
	}
	private IEnumerator<ICoroutineInstruction> Rush(int direction, float acceleration = 0.2f)
	{
		while (!Collision.SolidCollision(new Vector2(NPC.Center.X + direction * 60, NPC.Center.Y), 0, 0))
		{
			if(Math.Abs(NPC.velocity.X) < 20)
			{
				NPC.velocity.X += direction * acceleration;
			}
			yield return new SkipThisFrame();
		}
		//这里需要一个震屏
		SoundEngine.PlaySound(SoundID.DD2_BetsyFireballImpact);
		yield return new WaitForFrames(30);
		_coroutineManager.StartCoroutine(new Coroutine(Rush(direction)));
	}
	private IEnumerator<ICoroutineInstruction> Wake()
	{
		for (int x = 0; x <= 180; x++)
		{
			WakingTimer++;
			yield return new SkipThisFrame();
		}
		int direction = 1;
		if(Main.player[NPC.target].Center.X < NPC.Center.X)
		{
			direction = -1;
		}
		_coroutineManager.StartCoroutine(new Coroutine(Rush(direction)));
	}
	public override void AI()
	{
		NPC.localAI[0] += 1;
		NPC.TargetClosest(false);
		Player player = Main.player[NPC.target];
	}
	public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		Texture2D mainTex = ModAsset.SquamousShell.Value;
		if (WakingTimer < 180)
		{
			Texture2D deadShell = ModAsset.DeadSquamousShell.Value;
			float fade = 1 - WakingTimer / 180f;
			spriteBatch.Draw(deadShell, NPC.Center, null, drawColor * fade, NPC.rotation, deadShell.Size() * 0.5f, NPC.scale, SpriteEffects.None, 0);
		}
		spriteBatch.Draw(mainTex, NPC.Center, null, drawColor, NPC.rotation, mainTex.Size() * 0.5f, NPC.scale, SpriteEffects.None, 0);
		return false;
	}
	public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{

	}

	//--------------------------以下为辅助功能模块-------------------------------------------------以下为辅助功能模块-------------------------------------------------以下为辅助功能模块-------------------------------------------------以下为辅助功能模块-------------------------------------------------以下为辅助功能模块-------------------------------------------------
	public double GetVector2Rot(Vector2 value)
	{
		return Math.Atan2(value.Y, value.X) * NPC.spriteDirection + Math.PI / 2d;
	}
	public double CheckRotDir(double OldRot)
	{
		return -OldRot;
	}
	public void Exchange(ref float value1, ref float value2)
	{
		(value1, value2) = (value2, value1);
	}
	private Vector2 GetRotationVec()
	{
		Vector2 outValue = Vector2.Zero;
		for (int rad = 16; rad < 256; rad += 32)
		{
			for (int rot = 0; rot < rad / 4; rot++)
			{
				float value = rot / (float)rad * 4f;
				if (!Collision.SolidCollision(NPC.Center + new Vector2(0, rad).RotatedBy(value * MathHelper.TwoPi), 0, 0))
					outValue += Vector2.Normalize(new Vector2(0, rad).RotatedBy(value * MathHelper.TwoPi)) / rad;
				else
				{
					outValue -= Vector2.Normalize(new Vector2(0, rad).RotatedBy(value * MathHelper.TwoPi)) / rad;
				}
			}
		}
		return Utils.SafeNormalize(outValue, Vector2.Zero);
	}
	private void CheckSpriteDir()
	{
		if (NPC.velocity.X > 0)
			NPC.spriteDirection = -1;
		if (NPC.velocity.X < 0)
			NPC.spriteDirection = 1;
	}

	private void MoveTo(Vector2 aimPosition, float Speed = 1)
	{
		Vector2 v0 = Utils.SafeNormalize(aimPosition - NPC.Center, Vector2.Zero);
		NPC.velocity.X = v0.X * Speed;
	}
	private void DashTo(Vector2 aim, float Speed)
	{
		NPC.noTileCollide = true;
		Vector2 v0 = Utils.SafeNormalize(aim - NPC.Center, Vector2.Zero);
		NPC.velocity = v0 * Speed;
		NPC.noTileCollide = false;
	}
	private CoroutineManager _coroutineManager = new CoroutineManager();
	private int WakingTimer = 0;
}
