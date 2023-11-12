using Everglow.Yggdrasil.YggdrasilTown.Dusts;
namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class CyanVineSword_Projectile : MeleeProj
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
		selfWarp = true;
		AutoEnd = false;
	}
	public override string TrailShapeTex()
	{
		return "Everglow/Commons/Textures/Melee";
	}
	public override string TrailColorTex()
	{
		return "Everglow/Yggdrasil/YggdrasilTown/Projectiles/CyanVineSword_meleeColor";
	}
	public override float TrailAlpha(float factor)
	{
		return base.TrailAlpha(factor) * (1.25f);
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
			if (timer < 3 * timeMul)//前摇
			{
				useTrail = false;
				LockPlayerDir(player);
				float targetRot = -MathHelper.PiOver2 - player.direction * 0.7f;
				mainVec = Vector2.Lerp(mainVec, Vector2Elipse(80, targetRot, 2f), 0.7f);
				mainVec += Projectile.DirectionFrom(player.Center) * 3;
				Projectile.rotation = mainVec.ToRotation();
			}
			if (timer == (int)(20 * timeMul))
				AttSound(SoundID.Item1);
			if (timer > 3 * timeMul && timer < 24 * timeMul)
			{
				isAttacking = true;
				Projectile.rotation += Projectile.spriteDirection * 0.21f / timeMul;
				mainVec = Vector2Elipse(80, Projectile.rotation, 0.6f);
			}

			if (timer > 40 * timeMul)
			{
				player.fullRotation = 0;
				player.legRotation = 0;
				NextAttackType();
			}
		}
	}
	public override void OnKill(int timeLeft)
	{
		Player player = Main.player[Projectile.owner];
		player.fullRotation = 0;
	}
	public override void DrawTrail(Color color)
	{
		Color lightColor = Lighting.GetColor((int)(Projectile.Center.X / 16), (int)(Projectile.Center.Y / 16));
		List<Vector2> SmoothTrailX = GraphicsUtils.CatmullRom(trailVecs.ToList());//平滑
		var SmoothTrail = new List<Vector2>();
		for (int x = 0; x < SmoothTrailX.Count - 1; x++)
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
			float factor = i / (length + 1f);
			float w = TrailAlpha(factor);
			bars.Add(new Vertex2D(Projectile.Center + trail[i] * 0.15f * Projectile.scale, lightColor, new Vector3(factor, 1, 0f)));
			bars.Add(new Vertex2D(Projectile.Center + trail[i] * Projectile.scale, lightColor, new Vector3(factor, 0, w)));
		}
		bars.Add(new Vertex2D(Projectile.Center + mainVec * 0.15f * Projectile.scale, lightColor, new Vector3(1, 1, 0f)));
		bars.Add(new Vertex2D(Projectile.Center + mainVec * Projectile.scale, lightColor, new Vector3(1, 0, 1)));

		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, TrailBlendState(), SamplerState.AnisotropicWrap, DepthStencilState.None, RasterizerState.CullNone);
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;

		Effect MeleeTrail = ModContent.Request<Effect>("Everglow/MEAC/Effects/MeleeTrail", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
		MeleeTrail.Parameters["uTransform"].SetValue(model * projection);

		MeleeTrail.Parameters["tex0"].SetValue(Commons.ModAsset.Melee.Value);
		MeleeTrail.Parameters["tex1"].SetValue(ModAsset.CyanVineSword_meleeColor.Value);
		MeleeTrail.CurrentTechnique.Passes["TrailWithLight"].Apply();

		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
	}
	public override void DrawSelf(SpriteBatch spriteBatch, Color lightColor, Vector4 diagonal = default, Vector2 drawScale = default, Texture2D glowTexture = null)
	{
		drawScale = new Vector2(-0.1f, 1.02f);
		base.DrawSelf(spriteBatch, lightColor, diagonal, drawScale, glowTexture);
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		base.OnHitNPC(target, hit, damageDone);
	}
}