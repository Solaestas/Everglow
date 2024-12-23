using Everglow.Commons.DataStructures;
using Everglow.Commons.Interfaces;
using Terraria.Audio;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class KissOfCthulhu_Projectile : ModProjectile
{
	public const int ActiveTimerMax = 60;

	public RenderTarget2D ShellTarget;
	public RenderTarget2D OriginTarget;

	private IHookHandler _resizeHook;
	private IHookHandler _exitHook;

	private bool HasNotHitTargetOrTile { get; set; } = true;

	private int ActiveTimer
	{
		get => (int)Projectile.ai[0];
		set => Projectile.ai[0] = value;
	}

	public override string Texture => ModAsset.KissOfCthulhu_Mod;

	public override void SetDefaults()
	{
		Projectile.width = 24;
		Projectile.height = 24;
		Projectile.timeLeft = 300;

		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.tileCollide = true;
		Projectile.ignoreWater = true;
		Projectile.penetrate = -1;

		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 10;
	}

	public override bool OnTileCollide(Vector2 oldVelocity)
	{
		ActiveProjectile();
		return false;
	}

	public override void OnSpawn(IEntitySource source)
	{
		Projectile.rotation = Projectile.velocity.ToRotation();
		ActiveTimer = ActiveTimerMax;
	}

	public override void AI()
	{
		if (HasNotHitTargetOrTile)
		{
			// Draw shadow flame here
		}
		else
		{
			Projectile.rotation += 0.02f;
			if (ActiveTimer >= ActiveTimerMax / 5)
			{
				Projectile.scale += 0.1f;
			}
			else
			{
				Projectile.scale -= 0.5f;
			}

			if (--ActiveTimer <= 0)
			{
				Projectile.Kill();
			}
		}
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		ActiveProjectile();

		target.GetGlobalNPC<EverglowGlobalNPC>().ElementDebuffs[ElementDebuffType.Necrosis].AddBuildUp(100);
	}

	private void ActiveProjectile()
	{
		SoundEngine.PlaySound(SoundID.Item103);
		HasNotHitTargetOrTile = false;
		Projectile.velocity = Vector2.Zero;
	}

	public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
	{
		behindNPCs.Add(index);
	}

	public override bool PreDraw(ref Color lightColor)
	{
		var drawColor = new Color(0.4f, 0f, 1f, 0);
		var scaleX = Projectile.scale * (0.5f + 0.5f * ActiveTimer / ActiveTimerMax * 5);
		var scaleY = Projectile.scale * (0.8f + 0.2f * ActiveTimer / ActiveTimerMax * 5);

		SpriteBatchState sBS = GraphicsUtils.GetState(Main.spriteBatch).Value;
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

		if (!HasNotHitTargetOrTile)
		{
			scaleX = scaleY = 1;
			Effect shineEffect = ModAsset.KissOfCthulhuParticle.Value;
			shineEffect.Parameters["u_time"].SetValue((float)Main.timeForVisualEffects * 0.2f);
			shineEffect.Parameters["u_resolution"].SetValue(new Vector2(1 * scaleX, y: 1 * scaleY));
			shineEffect.CurrentTechnique.Passes["Test"].Apply();
		}

		var vertices = new List<Vertex2D>();

		float size = 50f * Projectile.scale;
		for (int i = 0; i < size; i++)
		{
			var offset1 = new Vector2((i - size / 2) * scaleX, size / 2 * scaleY).RotatedBy(Projectile.rotation);
			vertices.Add(Projectile.Center + offset1 - Main.screenPosition, drawColor, new Vector3(i / size, 1f, 0f));
			var offset2 = new Vector2((i - size / 2) * scaleX, -size / 2 * scaleY).RotatedBy(Projectile.rotation);
			vertices.Add(Projectile.Center + offset2 - Main.screenPosition, drawColor, new Vector3(i / size, 0f, 0f));
		}

		Main.graphics.GraphicsDevice.Textures[0] = Commons.ModAsset.Point.Value;
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices.ToArray(), 0, vertices.Count - 2);

		Main.spriteBatch.End();
		Main.spriteBatch.Begin(sBS);

		return false;
	}
}