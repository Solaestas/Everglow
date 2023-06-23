namespace Everglow.Myth.Misc.Projectiles.Weapon.Magic.FreezeFeatherMagic;
internal class FrozenRingPipeline : Pipeline
{
	public override void Load()
	{
		effect = ModAsset.FrozenRing;
		effect.Value.Parameters["uNoise"].SetValue(Commons.ModAsset.Noise_spiderNet.Value);
		effect.Value.Parameters["uHeatMap"].SetValue(ModAsset.HeatMap_frozenRing.Value);
	}
	public override void BeginRender()
	{
		var effect = this.effect.Value;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;
		effect.Parameters["uTransform"].SetValue(model * projection);
		Texture2D halo = Commons.ModAsset.Trail.Value;
		Ins.Batch.BindTexture<Vertex2D>(halo);
		Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.AnisotropicWrap;
		Ins.Batch.Begin(BlendState.AlphaBlend, DepthStencilState.None, SamplerState.AnisotropicWrap, RasterizerState.CullNone);
		effect.CurrentTechnique.Passes[0].Apply();
	}

	public override void EndRender()
	{
		Ins.Batch.End();
	}
}
[Pipeline(typeof(FrozenRingPipeline))]
internal class FreezeFeatherMagicArray : VisualProjectile
{
	public float WingPower = 0;
	public bool OldControlUp = false;
	public int Timer = 0;
	public Vector2 RingPos = Vector2.Zero;

	public override string Texture => "Everglow/" + ModAsset.FreezeFeatherMagicPath;
	public override void SetDefaults()
	{
		Projectile.width = 28;
		Projectile.height = 28;
		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.penetrate = -1;
		Projectile.timeLeft = 100000;
		Projectile.tileCollide = false;
		base.SetDefaults();
	}
	public override void AI()
	{
		Player player = Main.player[Projectile.owner];
		Projectile.Center = Projectile.Center * 0.7f + (player.Center + new Vector2(-player.direction * 22, -12 * player.gravDir * (float)(0.2 + Math.Sin(Main.timeForVisualEffects / 18d) / 2d))) * 0.3f;
		Projectile.spriteDirection = player.direction;
		Projectile.velocity *= 0;
		if (player.HeldItem.type == ModContent.ItemType<Misc.Items.Weapons.FreezeFeatherMagic>() && player.active && !player.dead)
		{
			Projectile.timeLeft = player.itemTime + 60;
			if (player.itemTime > 0)
			{
				if (Timer < 30)
				{
					Timer++;
				}
				Player.CompositeArmStretchAmount playerCASA = Player.CompositeArmStretchAmount.Full;
				player.SetCompositeArmFront(true, playerCASA, (float)(-Math.Sin(Main.timeForVisualEffects / 18d) * 0.6 + 1.2) * -player.direction);
				Vector2 vTOMouse = Main.MouseWorld - player.Center;
				player.SetCompositeArmBack(true, playerCASA, (float)(Math.Atan2(vTOMouse.Y, vTOMouse.X) - Math.PI / 2d));
			}
			else
			{
				if (Timer > 15)
				{
					Timer--;
				}
			}
		}
		else
		{
			Timer--;
			if (Timer < 0)
				Projectile.Kill();
		}

		Projectile.rotation = player.fullRotation;
		RingPos = RingPos * 0.9f + new Vector2(-12 * player.direction, -24 * player.gravDir) * 0.1f;

		IceFeatherOwner mplayer = player.GetModPlayer<IceFeatherOwner>();
		mplayer.HasFreezeWing = false;
		if ((player.wingTime <= 0 || player.wings == 0) && !player.mount._active)
		{
			if (WingPower > 0)
			{
				if (player.controlUp && player.velocity.Y != 0)
				{
					mplayer.HasFreezeWing = true;
					WingPower -= 0.8f;
					Item item = new Item();
					item.SetDefaults(ItemID.FrozenWings);
					player.equippedWings = item;
					player.wings = item.wingSlot;
					player.wingsLogic = item.wingSlot;
				}
			}
		}
		if (player.controlUp && player.velocity.Y != 0)
		{
			if ((!OldControlUp && player.wingTime <= 0) || player.wingTime == 2)
			{
				for (int j = 0; j < 16; j++)
				{
					Vector2 v = new Vector2(0, Main.rand.NextFloat(7, 20)).RotatedByRandom(MathHelper.TwoPi);
					Dust.NewDust(player.position, player.width, player.height, ModContent.DustType<Dusts.FreezeFeather>(), v.X, v.Y, 150, default, Main.rand.NextFloat(1.8f, 3.7f));
				}
			}
		}
		if (WingPower >= 210)
		{
			WingPower = 210;
		}
		OldControlUp = player.controlUp && player.velocity.Y != 0;
		if (Main.mouseRight && Main.mouseRightRelease)
		{
			if (WingPower < 21)
			{
				return;
			}
			WingPower -= 21;
			Vector2 pos = Projectile.Center + new Vector2(Main.rand.NextFloat(-600, 600), -1600);
			Vector2 vel = Vector2.Normalize(Main.MouseWorld - pos) * 60;
			Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), pos, vel, ModContent.ProjectileType<GiantFreezeFeather>(), player.HeldItem.damage * 5, 10, Projectile.owner);
			Timer = 30;
		}
	}
	public override bool PreDraw(ref Color lightColor)
	{
		Projectile.hide = false;
		return false;
	}
	public override CodeLayer DrawLayer => CodeLayer.PostDrawTiles;
	public override void Draw()
	{
		Vector2 toBottom = new Vector2(0, 40);
		List<Vertex2D> bars = new List<Vertex2D>();
		for (int x = 0; x < 40; x++)
		{
			float pocession = 1 - Timer / 30f;
			Vector2 radious = toBottom.RotatedBy(x / 20d * Math.PI);
			float width = 75f;
			if (x / 40f > WingPower / 210f)
			{
				pocession += 0.7f;
			}
			Vector2 normalizedRadious = radious / 40f * MathF.Sin(x / 40f * MathF.PI) * width;
			bars.Add(new Vertex2D(Projectile.Center + radious + normalizedRadious, new Color(x / 40f, 0.1f, pocession, 0.0f), new Vector3(0 + (float)Main.time * 0.014f, x / 15f, 0)));
			bars.Add(new Vertex2D(Projectile.Center + radious, new Color(x / 40f, 0.9f, pocession, 0.0f), new Vector3(0.8f + (float)Main.time * 0.014f, x / 15f, 0)));
		}
		Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);
	}
}
class IceFeatherOwner : ModPlayer
{
	public bool HasFreezeWing = false;
	public override void PostUpdateMiscEffects()
	{
		if (HasFreezeWing)
		{
			if (Player.ownedProjectileCounts[ModContent.ProjectileType<FreezeFeatherMagicArray>()] < 1)
			{
				HasFreezeWing = false;
				return;
			}
			Item item = new Item();
			item.SetDefaults(ItemID.FrozenWings);
			Player.equippedWings = item;
			Player.wings = item.wingSlot;
			Player.wingsLogic = item.wingSlot;
			Player.wingTime += 1;
		}
	}
}
