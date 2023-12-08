using Terraria.DataStructures;

namespace Everglow.Myth.Misc.Projectiles.Weapon.Melee;

class ComingGhost : MeleeProj
{
	public override void SetDef()
	{
		Projectile.aiStyle = -1;
		Projectile.timeLeft = 30;
		Projectile.extraUpdates = 1;
		Projectile.scale = 1f;
		Projectile.hostile = false;
		Projectile.friendly = true;
		Projectile.tileCollide = false;
		Projectile.ignoreWater = true;
		Projectile.penetrate = -1;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 15;
		Projectile.DamageType = DamageClass.Melee;

		Projectile.width = 80;
		Projectile.height = 80;
		Projectile.tileCollide = false;
		Projectile.friendly = true;
		longHandle = false;
		maxAttackType = 0;
		trailLength = 20;
		shadertype = "Trail";
		AutoEnd = false;
	}
	private int HasHit = 0;
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		HasHit++;
		if (HasHit > 2)
			return;
		Player player = Main.player[Projectile.owner];
		Vector2 v = new Vector2(0, 10).RotatedByRandom(Math.PI * 2) * 5f;
		Projectile.NewProjectile(null, target.Center - v * 3, v, ModContent.ProjectileType<GhostHit>(), Projectile.damage, Projectile.knockBack, player.whoAmI, Main.rand.NextFloat(-0.05f, 0.05f));
	}
	public override string TrailShapeTex()
	{
		return "Everglow/Commons/Textures/Melee";
	}
	public override string TrailColorTex()
	{
		return "Everglow/Myth/Misc/Projectiles/Weapon/Melee/ComingGhost_meleeColor";
	}
	public override float TrailAlpha(float factor)
	{
		return base.TrailAlpha(factor) * 1.15f;
	}
	public override BlendState TrailBlendState()
	{
		return BlendState.NonPremultiplied;
	}
	public override void End()
	{
		Player player = Main.player[Projectile.owner];
		player.legFrame = new Rectangle(0, 0, player.legFrame.Width, player.legFrame.Height);
		player.fullRotation = 0;
		player.legRotation = 0;
		player.legPosition = Vector2.Zero;
		Projectile.Kill();
		player.GetModPlayer<MEACPlayer>().isUsingMeleeProj = false;
	}
	public override void OnSpawn(IEntitySource source)
	{

	}
	public override void AI()
	{
		Player player = Main.player[Projectile.owner];
		base.AI();
		TestPlayerDrawer Tplayer = player.GetModPlayer<TestPlayerDrawer>();
		Tplayer.HideLeg = true;
		useTrail = true;
		float timeMul = 1 / player.meleeSpeed;
		if (attackType == 0)
		{
			if (timer < 24 * timeMul)//前摇
			{
				useTrail = false;
				LockPlayerDir(player);
				float targetRot = -MathHelper.PiOver2 - player.direction * 0.5f;
				mainVec = Vector2.Lerp(mainVec, Vector2Elipse(100, targetRot, +1.2f), 0.4f / timeMul);
				mainVec += Projectile.DirectionFrom(player.Center) * 3;
				Projectile.rotation = mainVec.ToRotation();
			}
			if (timer == (int)(20 * timeMul))
				AttSound(SoundID.Item1);
			if (timer > 16 * timeMul && timer < 55 * timeMul)
			{
				isAttacking = true;
				Projectile.rotation += Projectile.spriteDirection * 0.17f / timeMul;
				mainVec = Vector2.Lerp(mainVec, Vector2Elipse(110, Projectile.rotation, -1.2f, -0.3f * Projectile.spriteDirection), 0.4f / timeMul);
				player.fullRotationOrigin = new Vector2(10, 42);
				player.fullRotation = MathF.Sin((timer - 16 * timeMul) / (38f * timeMul) * MathHelper.Pi) * 0.6f * player.direction;
				player.legRotation = -player.fullRotation;
			}
			if (timer > 94 * timeMul)
				NextAttackType();

		}
	}
	public override void OnKill(int timeLeft)
	{
		Player player = Main.player[Projectile.owner];
		player.fullRotation = 0;
	}
	public override void DrawSelf(SpriteBatch spriteBatch, Color lightColor, Vector4 diagonal = default, Vector2 drawScale = default, Texture2D glowTexture = null)
	{
		//drawScale = new Vector2(-0.6f, 1.14f);
		base.DrawSelf(spriteBatch, lightColor, diagonal, drawScale, glowTexture);
	}
	public override void DrawTrail(Color color)
	{
		List<Vector2> SmoothTrailX = GraphicsUtils.CatmullRom(trailVecs.ToList());//平滑
		var SmoothTrail = new List<Vector2>();
		for (int x = 0; x <= SmoothTrailX.Count - 1; x++)
		{
			SmoothTrail.Add(SmoothTrailX[x]);
		}
		if (trailVecs.Count != 0)
			SmoothTrail.Add(trailVecs.ToArray()[trailVecs.Count - 1]);

		int length = SmoothTrail.Count;
		if (length <= 3)
			return;
		Vector2[] trail = SmoothTrail.ToArray();
		var bars = new List<Vertex2D>();

		for (int i = 0; i < length; i++)
		{
			float factor = i / (length - 1f);
			float w = TrailAlpha(factor);
			Color c0 = Color.White;
			if (i == 0)
			{
				c0 = Color.Transparent;
			}
			bars.Add(new Vertex2D(Projectile.Center + trail[i] * 0.3f * Projectile.scale, c0, new Vector3(factor, 1, 0f)));
			bars.Add(new Vertex2D(Projectile.Center + trail[i] * Projectile.scale, c0, new Vector3(factor, 0, w)));
		}
		bars.Add(new Vertex2D(Projectile.Center + mainVec * 0.3f * Projectile.scale, Color.White, new Vector3(0, 1, 0f)));
		bars.Add(new Vertex2D(Projectile.Center + mainVec * Projectile.scale, Color.White, new Vector3(0, 0, 1)));
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, TrailBlendState(), SamplerState.AnisotropicWrap, DepthStencilState.None, RasterizerState.CullNone);
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.ZoomMatrix;

		Effect MeleeTrail = ModContent.Request<Effect>("Everglow/MEAC/Effects/MeleeTrail", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
		MeleeTrail.Parameters["uTransform"].SetValue(model * projection);
		Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>(TrailShapeTex(), ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
		//Main.graphics.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>(TrailColorTex(), ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

		MeleeTrail.Parameters["tex1"].SetValue(ModContent.Request<Texture2D>(TrailColorTex(), ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
		MeleeTrail.CurrentTechnique.Passes[shadertype].Apply();

		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
	}
}
