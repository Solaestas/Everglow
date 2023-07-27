using System.Runtime.Intrinsics.X86;
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
		NPC.noGravity = true;
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
		NPC.velocity.Y = 2f;
		_coroutineManager.StartCoroutine(new Coroutine(Landing()));
	}
	public override void AI()
	{
		NPC.localAI[0] += 1;
		NPC.TargetClosest(false);
		Player player = Main.player[NPC.target];
		_coroutineManager.Update();
	}
	private IEnumerator<ICoroutineInstruction> Landing()
	{
		while (NPC.velocity.Y != 0)
		{
			NPC.velocity.Y += 0.2f;
			NPC.spriteDirection= 1;
			yield return new SkipThisFrame();
		}
		//这里需要一个震屏
		SoundEngine.PlaySound(SoundID.DD2_BetsyFireballImpact);
		_coroutineManager.StartCoroutine(new Coroutine(Wake()));
	}
	private IEnumerator<ICoroutineInstruction> Wake()
	{
		for (int x = 0; x <= 180; x++)
		{
			WakingTimer++;
			yield return new SkipThisFrame();
		}
		int direction = 1;
		if (Main.player[NPC.target].Center.X < NPC.Center.X)
		{
			direction = -1;
		}
		_coroutineManager.StartCoroutine(new Coroutine(Rush(direction)));
	}
	private IEnumerator<ICoroutineInstruction> Rush(int direction, float acceleration = 0.2f)
	{
		while (!NPC.collideX || NPC.velocity.Y != 0)
		{
			if(Math.Abs(NPC.velocity.X) < 20)
			{
				if(Collision.SolidCollision(NPC.Center + new Vector2(50, 80), 0, 0))
				{
					NPC.velocity.X += direction * acceleration / 3f;
				}
				if (Collision.SolidCollision(NPC.Center + new Vector2(0, 80), 0, 0))
				{
					NPC.velocity.X += direction * acceleration / 3f;
				}
				if (Collision.SolidCollision(NPC.Center + new Vector2(-50, 80), 0, 0))
				{
					NPC.velocity.X += direction * acceleration / 3f;
				}
				NPC.velocity.X += direction * acceleration / 2f;
			}
			if(NPC.collideX)
			{
				NPC.velocity.Y -= 16;
			}
			if(!NPC.collideY)
			{
				NPC.velocity.Y += 0.6f;
			}
			NPC.spriteDirection = direction;
			if(Math.Abs(Main.player[NPC.target].Center.X - NPC.Center.X) > 1000)
			{
				direction *= -1;
				NPC.velocity *= 0;
				NPC.position.X += direction * 20;
			}
			if (Math.Abs(Main.player[NPC.target].Center.X - NPC.Center.X) > 800)
			{
				NPC.velocity *= 0.95f;
			}
			if (NPC.velocity.Y < -20)
			{
				NPC.velocity.Y = -20;
			}
			NPC.rotation = (float)GetVector2Rot(GetRotationVec());
			yield return new SkipThisFrame();
		}
		//这里需要一个震屏
		SoundEngine.PlaySound(SoundID.DD2_BetsyFireballImpact);
		yield return new WaitForFrames(30);
		int newDirection = 1;
		if (Main.player[NPC.target].Center.X < NPC.Center.X)
		{
			newDirection = -1;
		}
		if(newDirection == direction)
		{
			NPC.velocity.Y -= 16f;
			NPC.position.Y -= 40f;
		}
		NPC.position.X += newDirection * 20;
		_coroutineManager.StartCoroutine(new Coroutine(RollingARock(newDirection)));
	}
	private IEnumerator<ICoroutineInstruction> RollingARock(int direction)
	{
		Projectile rock = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, new Vector2(direction * 2, 0), ModContent.ProjectileType<Projectiles.SquamousRollingStone>(), 60, 6);
		rock.scale = 0;
		for(int x = 0;x < 100;x++)
		{
			NPC.rotation -= 0.05f;
			rock.scale += 0.01f;
			yield return new SkipThisFrame();
		}


	}
	private IEnumerator<ICoroutineInstruction> NextAttack(int direction)
	{
		int newDirection = 1;
		if (Main.player[NPC.target].Center.X < NPC.Center.X)
		{
			newDirection = -1;
		}
		if (newDirection == direction)
		{
			NPC.velocity.Y -= 16f;
			NPC.position.Y -= 40f;
		}
		NPC.position.X += newDirection * 20;
		_coroutineManager.StartCoroutine(new Coroutine(Rush(newDirection)));
		yield break;
	}
	public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		Texture2D mainTex = ModAsset.SquamousShell.Value;
		SpriteEffects spe = SpriteEffects.None;
		float drawRot = NPC.rotation * NPC.spriteDirection;
		if (NPC.spriteDirection == -1)
		{
			spe = SpriteEffects.FlipHorizontally;
			drawRot += MathF.PI;
		}


		spriteBatch.Draw(mainTex, NPC.Center - Main.screenPosition, null, drawColor, drawRot, mainTex.Size() * 0.5f, NPC.scale, spe, 0);
		if (WakingTimer < 180)
		{
			Texture2D deadShell = ModAsset.DeadSquamousShell.Value;
			float fade = 1 - WakingTimer / 180f;
			spriteBatch.Draw(deadShell, NPC.Center - Main.screenPosition, null, drawColor * fade, drawRot, deadShell.Size() * 0.5f, NPC.scale, spe, 0);
		}
		return false;
	}
	public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{

	}

	//--------------------------以下为辅助功能模块-------------------------------------------------以下为辅助功能模块-------------------------------------------------以下为辅助功能模块-------------------------------------------------以下为辅助功能模块-------------------------------------------------以下为辅助功能模块-------------------------------------------------
	public double GetVector2Rot(Vector2 value)
	{
		if (value == Vector2.zeroVector)
		{
			double value1 = 0;	
			if (NPC.spriteDirection == -1)
			{
				value1 = Math.PI;
			}
			return value1;
		}
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
		if(outValue.Length() < 0.005)
		{
			outValue= Vector2.Zero;
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
