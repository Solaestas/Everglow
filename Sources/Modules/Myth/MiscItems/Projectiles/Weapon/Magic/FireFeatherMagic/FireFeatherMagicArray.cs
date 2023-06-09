using Everglow.Commons.DataStructures;
using static Terraria.ModLoader.PlayerDrawLayer;
using Terraria.Map;

namespace Everglow.Myth.MiscItems.Projectiles.Weapon.Magic.FireFeatherMagic;
internal class FlameRingPipeline : Pipeline
{
	public override void Load()
	{
		effect = ModAsset.FlameRing;
		effect.Value.Parameters["uNoise"].SetValue(Commons.ModAsset.Noise_melting.Value);
		effect.Value.Parameters["uHeatMap"].SetValue(ModAsset.HeatMap_flameRing.Value);
	}
	public override void BeginRender()
	{
		var effect = this.effect.Value;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;
		effect.Parameters["uTransform"].SetValue(model * projection);
		Texture2D halo = Commons.ModAsset.Trail.Value;
		Ins.Batch.BindTexture<Vertex2D>(halo);
		Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.AnisotropicClamp;
		Ins.Batch.Begin(BlendState.AlphaBlend, DepthStencilState.None, SamplerState.LinearWrap, RasterizerState.CullNone);
		effect.CurrentTechnique.Passes[0].Apply();
	}

	public override void EndRender()
	{
		Ins.Batch.End();
	}
}
[Pipeline(typeof(FlameRingPipeline), typeof(BloomPipeline))]
internal class FireFeatherMagicArray : VisualProjectile
{
	public override string Texture => "Everglow/" + ModAsset.FireFeatherMagicPath;
	public override void SetDefaults()
	{
		Projectile.width = 28;
		Projectile.height = 28;
		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.penetrate = -1;
		Projectile.timeLeft = 10000;
		Projectile.tileCollide = false;
		base.SetDefaults();
	}

	public override void AI()
	{
		Player player = Main.player[Projectile.owner];
		Projectile.Center = Projectile.Center * 0.7f + (player.Center + new Vector2(-player.direction * 22, -12 * player.gravDir * (float)(0.2 + Math.Sin(Main.timeForVisualEffects / 18d) / 2d))) * 0.3f;
		Projectile.spriteDirection = player.direction;
		Projectile.velocity *= 0;
		if (player.itemTime > 0 && player.HeldItem.type == ModContent.ItemType<MiscItems.Weapons.FireFeatherMagic>())
		{
			Projectile.timeLeft = player.itemTime + 60;
			if (Timer < 30)
				Timer++;
		}
		else
		{
			Timer--;
			if (Timer < 0)
				Projectile.Kill();
		}
		Player.CompositeArmStretchAmount PCAS = Player.CompositeArmStretchAmount.Full;
		player.SetCompositeArmFront(true, PCAS, (float)(-Math.Sin(Main.timeForVisualEffects / 18d) * 0.6 + 1.2) * -player.direction);
		Vector2 vTOMouse = Main.MouseWorld - player.Center;
		player.SetCompositeArmBack(true, PCAS, (float)(Math.Atan2(vTOMouse.Y, vTOMouse.X) - Math.PI / 2d));
		Projectile.rotation = player.fullRotation;
		RingPos = RingPos * 0.9f + new Vector2(-12 * player.direction, -24 * player.gravDir) * 0.1f;
	}
	internal int Timer = 0;
	internal Vector2 RingPos = Vector2.Zero;
	public override CodeLayer DrawLayer => CodeLayer.PostDrawTiles;
	public override bool PreDraw(ref Color lightColor)
	{
		Projectile.hide = false;
		return false;
	}
	public override void Draw()
	{
		float pocession = 1 - Timer / 30f;
		Vector2 toBottom = new Vector2(0, 40);
		List<Vertex2D> bars = new List<Vertex2D>();
		for(int x = 0;x < 40; x++)
		{
			Vector2 radious = toBottom.RotatedBy(x / 20d * Math.PI);
			Vector2 normalizedRadious = radious / 40f * MathF.Sin(x / 40f * MathF.PI) * 75f;
			bars.Add(new Vertex2D(Projectile.Center + radious + normalizedRadious, new Color(x / 40f, 0.0f, pocession, 0.0f), new Vector3(x / 25f, 0 + (float)Main.time * 0.009f, 0)));
			bars.Add(new Vertex2D(Projectile.Center + radious, new Color(x / 40f, 0.8f, pocession, 0.0f), new Vector3(x / 25f, 0.5f + (float)Main.time * 0.009f, 0)));
		}
		Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);
	}
}