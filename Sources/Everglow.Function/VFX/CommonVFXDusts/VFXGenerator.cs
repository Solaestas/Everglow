namespace Everglow.Commons.VFX.CommonVFXDusts;
class VFXGenerator : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 30;
		Item.height = 20;
		Item.useTurn = true;
		Item.useAnimation = 4;
		Item.useTime = 4;
		Item.autoReuse = false;
		Item.useStyle = ItemUseStyleID.Swing;
	}
	public override bool CanUseItem(Player player)
	{
		float mulVelocity = 1f;
		int type = Main.rand.Next(9, 10);

		switch (type)
		{
			case 0:
				for (int g = 0; g < 20; g++)
				{
					Vector2 newVelocity = new Vector2(0, mulVelocity * Main.rand.NextFloat(0f, 4f)).RotatedByRandom(MathHelper.TwoPi);
					var somg = new IceSmogDust
					{
						velocity = newVelocity,
						Active = true,
						Visible = true,
						position = player.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283) + newVelocity * 16,
						maxTime = Main.rand.Next(237, 345),
						scale = Main.rand.NextFloat(320f, 435f),
						rotation = Main.rand.NextFloat(6.283f),
						ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), 0 }
					};
					Ins.VFXManager.Add(somg);
				}
				break;
			case 1:
				for (int g = 0; g < 80; g++)
				{
					Vector2 newVelocity = new Vector2(0, mulVelocity * Main.rand.NextFloat(0f, 12f)).RotatedByRandom(MathHelper.TwoPi);
					var somg = new IceParticleDust
					{
						velocity = newVelocity,
						Active = true,
						Visible = true,
						position = player.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283),
						maxTime = Main.rand.Next(237, 345),
						scale = Main.rand.NextFloat(12.20f, 32.35f),
						rotation = Main.rand.NextFloat(6.283f),
						ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), 0 }
					};
					Ins.VFXManager.Add(somg);
				}
				break;
			case 2:
				for (int g = 0; g < 380; g++)
				{
					Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(2f, 9.6f)).RotatedByRandom(MathHelper.TwoPi);
					var smog = new SnowPieceDust
					{
						velocity = newVelocity + player.velocity * Main.rand.NextFloat(0f, 0.03f),
						Active = true,
						Visible = true,
						coord0 = new Vector2(Main.rand.NextFloat(0.1f, 0.2f), 0).RotatedByRandom(6.283),
						coord1 = new Vector2(Main.rand.NextFloat(0.1f, 0.2f), 0).RotatedByRandom(6.283),
						position = player.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283) + player.velocity * Main.rand.NextFloat(-3f, 2f),
						maxTime = Main.rand.Next(147, 285),
						scale = Main.rand.NextFloat(2f, 12f),
						rotation = Main.rand.NextFloat(6.283f),
						rotation2 = Main.rand.NextFloat(6.283f),
						omega = Main.rand.NextFloat(-10f, 10f),
						phi = Main.rand.NextFloat(6.283f),
						ai = new float[] { Main.rand.NextFloat(0f, 1f), Main.rand.NextFloat(0f, 1f), Main.rand.NextFloat(-0.005f, 0.005f) }
					};
					Ins.VFXManager.Add(smog);
				}
				break;
			case 3:
				for (int g = 0; g < 40; g++)
				{
					Vector2 newVelocity = new Vector2(0, mulVelocity * Main.rand.NextFloat(2f, 4f)).RotatedByRandom(MathHelper.TwoPi);
					var somg = new FireSmogDust
					{
						velocity = newVelocity,
						Active = true,
						Visible = true,
						position = player.Center + new Vector2(Main.rand.NextFloat(0, 12), 0).RotatedByRandom(6.283) + newVelocity * 3,
						maxTime = Main.rand.Next(37, 45),
						scale = Main.rand.NextFloat(20f, 35f),
						rotation = Main.rand.NextFloat(6.283f),
						ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), 0 }
					};
					Ins.VFXManager.Add(somg);
				}
				break;
			case 4:
				for (int g = 0; g < 40; g++)
				{
					Vector2 newVelocity = new Vector2(0, mulVelocity * Main.rand.NextFloat(0f, 7f)).RotatedByRandom(MathHelper.TwoPi);
					var fire = new FireDust
					{
						velocity = newVelocity,
						Active = true,
						Visible = true,
						position = player.Center + new Vector2(Main.rand.NextFloat(0, 6f), 0).RotatedByRandom(6.283) + newVelocity * 3,
						maxTime = Main.rand.Next(16, 45),
						scale = Main.rand.NextFloat(20f, 60f),
						rotation = Main.rand.NextFloat(6.283f),
						ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), 0 }
					};
					Ins.VFXManager.Add(fire);
				}
				break;
			case 5:
				for (int g = 0; g < 720; g++)
				{
					Vector2 newVelocity = new Vector2(0, mulVelocity * Main.rand.NextFloat(2f, 6f)).RotatedByRandom(MathHelper.TwoPi);
					var spark = new FireSparkDust
					{
						velocity = newVelocity,
						Active = true,
						Visible = true,
						position = player.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283) - player.velocity + newVelocity * 3,
						maxTime = Main.rand.Next(137, 245),
						scale = Main.rand.NextFloat(0.1f, Main.rand.NextFloat(0.1f, 17.0f)),
						rotation = Main.rand.NextFloat(6.283f),
						ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), Main.rand.NextFloat(-0.13f, 0.13f) }
					};
					Ins.VFXManager.Add(spark);
				}
				break;
			case 6:
				for (int g = 0; g < 40; g++)
				{
					Vector2 newVelocity = new Vector2(0, mulVelocity * Main.rand.NextFloat(0f, 7f)).RotatedByRandom(MathHelper.TwoPi);
					var fire = new CurseFlameDust
					{
						velocity = newVelocity,
						Active = true,
						Visible = true,
						position = player.Center + new Vector2(Main.rand.NextFloat(0, 6f), 0).RotatedByRandom(6.283) + newVelocity * 3,
						maxTime = Main.rand.Next(16, 45),
						scale = Main.rand.NextFloat(20f, 60f),
						rotation = Main.rand.NextFloat(6.283f),
						ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), 0 }
					};
					Ins.VFXManager.Add(fire);
				}
				break;
			case 7:
				for (int g = 0; g < 500; g++)
				{
					Vector2 newVelocity = new Vector2(0, mulVelocity * Main.rand.NextFloat(2f, 6f)).RotatedByRandom(MathHelper.TwoPi);
					var spark = new CurseFlameSparkDust
					{
						velocity = newVelocity,
						Active = true,
						Visible = true,
						position = player.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283) - player.velocity + newVelocity * 3,
						maxTime = Main.rand.Next(137, 245),
						scale = Main.rand.NextFloat(0.1f, Main.rand.NextFloat(4f, 47.0f)),
						rotation = Main.rand.NextFloat(6.283f),
						ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), Main.rand.NextFloat(-0.13f, 0.13f) }
					};
					Ins.VFXManager.Add(spark);
				}
				break;
			case 8:
				for (int g = 0; g < 20; g++)
				{
					Vector2 newVelocity = new Vector2(0, mulVelocity * Main.rand.NextFloat(0f, 4f)).RotatedByRandom(MathHelper.TwoPi);
					var somg = new VaporDust
					{
						velocity = newVelocity,
						Active = true,
						Visible = true,
						position = player.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283) + newVelocity * 16,
						maxTime = Main.rand.Next(104, 220),
						scale = Main.rand.NextFloat(320f, 435f),
						rotation = Main.rand.NextFloat(6.283f),
						ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), 0 }
					};
					Ins.VFXManager.Add(somg);
				}
				break;
			case 9:
				for (int g = 0; g < 200; g++)
				{
					Vector2 newVelocity = new Vector2(0, mulVelocity * Main.rand.NextFloat(2f, 6f)).RotatedByRandom(MathHelper.TwoPi);
					var spark = new JungleSporeDust
					{
						velocity = newVelocity,
						Active = true,
						Visible = true,
						position = player.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283) - player.velocity + newVelocity * 3,
						maxTime = Main.rand.Next(137, 245),
						scale = Main.rand.NextFloat(0.1f, Main.rand.NextFloat(4f, 47.0f)),
						rotation = Main.rand.NextFloat(6.283f),
						ai = new float[] { Main.rand.NextFloat(0.0f, Main.rand.NextFloat(0.0f, 1.0f)), Main.rand.NextFloat(-0.03f, 0.03f)}
					};
					Ins.VFXManager.Add(spark);
				}
				break;
		}

		return true;
	}
	public override bool? UseItem(Player player)
	{
		return base.UseItem(player);
	}
}
