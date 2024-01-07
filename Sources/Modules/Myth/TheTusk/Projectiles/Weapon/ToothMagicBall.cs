using Everglow.Myth.Common;
using Terraria.Localization;
namespace Everglow.Myth.TheTusk.Projectiles.Weapon;

public class ToothMagicBall : ModProjectile
{
	public override void SetDefaults()
	{
		Projectile.width = 18;
		Projectile.height = 18;
		Projectile.aiStyle = -1;
		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.ignoreWater = true;
		Projectile.DamageType = DamageClass.Magic;
		Projectile.tileCollide = true;
		Projectile.timeLeft = 1080;
		Projectile.alpha = 0;
		Projectile.penetrate = -1;
		Projectile.scale = 1;
	}
	public override void AI()
	{
		Player player = Main.player[Projectile.owner];
		player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, (float)(Math.PI / 2d) * -player.direction);
		if (Projectile.timeLeft >= 1000)
			Projectile.timeLeft = 10;
		if (player.controlUseItem && player.statMana > player.HeldItem.mana)
			Projectile.timeLeft = 10;
		if (player.itemTime == player.itemTimeMax)
		{
			Vector2 velocity = Vector2.Normalize(Main.MouseWorld - player.Center) * player.HeldItem.shootSpeed;
			Projectile.NewProjectile(Terraria.Entity.InheritSource(Projectile), player.Center + new Vector2(28 * player.direction, -5), velocity, ModContent.ProjectileType<ToothMagic>(), Projectile.damage, Projectile.knockBack, player.whoAmI);
		}
		Projectile.Center = player.Center;
		player.heldProj = Projectile.whoAmI;
		if (player.controlUseItem)
		{
			if (Energy < 120)
			{
				Energy++;
			}
		}
		else
		{
			Energy *= 0.7f;
		}
	}
	Vector2[] VB = new Vector2[4];
	Vector2[] VT = new Vector2[10];
	Vector2[] VTMax = new Vector2[10];

	public override void PostDraw(Color lightColor)
	{
		Player player = Main.player[Projectile.owner];
		DrawPowerEffect();
		if (!player.controlUseItem)
		{
			return;
		}
		Texture2D TC = MythContent.QuickTexture("TheTusk/Items/Weapons/ToothMagicBallBloodCore");
		Texture2D TB0 = MythContent.QuickTexture("TheTusk/Items/Weapons/ToothMagicBallBloodDrop0");
		Texture2D TB1 = MythContent.QuickTexture("TheTusk/Items/Weapons/ToothMagicBallBloodDrop1");
		Texture2D TB2 = MythContent.QuickTexture("TheTusk/Items/Weapons/ToothMagicBallBloodDrop2");
		Texture2D TB3 = MythContent.QuickTexture("TheTusk/Items/Weapons/ToothMagicBallBloodDrop3");
		Texture2D TT0 = MythContent.QuickTexture("TheTusk/Items/Weapons/ToothMagicBallBloodDropTusk0");
		Texture2D TT1 = MythContent.QuickTexture("TheTusk/Items/Weapons/ToothMagicBallBloodDropTusk1");
		Texture2D TT2 = MythContent.QuickTexture("TheTusk/Items/Weapons/ToothMagicBallBloodDropTusk2");
		Texture2D TT3 = MythContent.QuickTexture("TheTusk/Items/Weapons/ToothMagicBallBloodDropTusk3");
		Texture2D TT4 = MythContent.QuickTexture("TheTusk/Items/Weapons/ToothMagicBallBloodDropTusk4");
		Texture2D TT5 = MythContent.QuickTexture("TheTusk/Items/Weapons/ToothMagicBallBloodDropTusk5");
		Texture2D TT6 = MythContent.QuickTexture("TheTusk/Items/Weapons/ToothMagicBallBloodDropTusk6");
		Texture2D TT7 = MythContent.QuickTexture("TheTusk/Items/Weapons/ToothMagicBallBloodDropTusk7");
		Texture2D TT8 = MythContent.QuickTexture("TheTusk/Items/Weapons/ToothMagicBallBloodDropTusk8");
		Texture2D TT9 = MythContent.QuickTexture("TheTusk/Items/Weapons/ToothMagicBallBloodDropTusk9");
		var drawOrigin = new Vector2(TC.Width * 0.5f, TC.Height * 0.5f);
		Color c0 = Lighting.GetColor((int)(player.Center.X / 16f), (int)(player.Center.Y / 16f));
		SpriteEffects sp = SpriteEffects.None;
		
		if (player.direction == -1)
			sp = SpriteEffects.FlipHorizontally;
		if (VTMax[0] == Vector2.Zero)
		{
			for (int s = 0; s < 10; s++)
			{
				VTMax[s] = new Vector2(0, Main.rand.NextFloat(2.5f, 4f)).RotatedBy(s / 7.5 * Math.PI);
			}
		}
		for (int s = 0; s < 10; s++)
		{
			VT[s] = VTMax[s] * (float)(Math.Sin(s + Main.time * 0.03f) + 0.4);
		}
		Main.spriteBatch.Draw(TC, player.Center + new Vector2(20 * player.direction, -7) - Main.screenPosition, null, c0, 0, drawOrigin, player.itemTime / (float)player.itemTimeMax, sp, 0);
		Main.spriteBatch.Draw(TB0, player.Center + VB[0] + new Vector2(28 * player.direction, -5) - Main.screenPosition, null, c0, 0, drawOrigin, 1, sp, 0);
		Main.spriteBatch.Draw(TB1, player.Center + VB[1] + new Vector2(28 * player.direction, -5) - Main.screenPosition, null, c0, 0, drawOrigin, 1, sp, 0);
		Main.spriteBatch.Draw(TB2, player.Center + VB[2] + new Vector2(28 * player.direction, -5) - Main.screenPosition, null, c0, 0, drawOrigin, 1, sp, 0);
		Main.spriteBatch.Draw(TB3, player.Center + VB[3] + new Vector2(28 * player.direction, -5) - Main.screenPosition, null, c0, 0, drawOrigin, 1, sp, 0);
		Main.spriteBatch.Draw(TT0, player.Center + VT[0] + new Vector2(28 * player.direction, -5) - Main.screenPosition, null, c0, 0, drawOrigin, 1, sp, 0);
		Main.spriteBatch.Draw(TT1, player.Center + VT[1] + new Vector2(28 * player.direction, -5) - Main.screenPosition, null, c0, 0, drawOrigin, 1, sp, 0);
		Main.spriteBatch.Draw(TT2, player.Center + VT[2] + new Vector2(28 * player.direction, -5) - Main.screenPosition, null, c0, 0, drawOrigin, 1, sp, 0);
		Main.spriteBatch.Draw(TT3, player.Center + VT[3] + new Vector2(28 * player.direction, -5) - Main.screenPosition, null, c0, 0, drawOrigin, 1, sp, 0);
		Main.spriteBatch.Draw(TT4, player.Center + VT[4] + new Vector2(28 * player.direction, -5) - Main.screenPosition, null, c0, 0, drawOrigin, 1, sp, 0);
		Main.spriteBatch.Draw(TT5, player.Center + VT[5] + new Vector2(28 * player.direction, -5) - Main.screenPosition, null, c0, 0, drawOrigin, 1, sp, 0);
		Main.spriteBatch.Draw(TT6, player.Center + VT[6] + new Vector2(28 * player.direction, -5) - Main.screenPosition, null, c0, 0, drawOrigin, 1, sp, 0);
		Main.spriteBatch.Draw(TT7, player.Center + VT[7] + new Vector2(28 * player.direction, -5) - Main.screenPosition, null, c0, 0, drawOrigin, 1, sp, 0);
		Main.spriteBatch.Draw(TT8, player.Center + VT[8] + new Vector2(28 * player.direction, -5) - Main.screenPosition, null, c0, 0, drawOrigin, 1, sp, 0);
		Main.spriteBatch.Draw(TT9, player.Center + VT[9] + new Vector2(28 * player.direction, -5) - Main.screenPosition, null, c0, 0, drawOrigin, 1, sp, 0);
	}
	public float Energy = 0;
	public void DrawPowerEffect()
	{
		Player player = Main.player[Projectile.owner];
		Vector2 bulbPos = player.Center + new Vector2(20 * player.direction, -7);
		float energyValue = Energy / 250f;
		Color c0 = new Color(1, energyValue * energyValue * 0.2f, energyValue * 0.1f, 0);
		float timeValue = (float)Main.time * 0.009f;
		List<Vertex2D> bars = new List<Vertex2D>();
		float accuracy = 16;
		List<Vertex2D> bars2 = new List<Vertex2D>();
		for(int x = 0;x < 9;x++)
		{
			Vector2 addPos = Vector2.zeroVector;
			Vector2 addVel = new Vector2(0, 4 * energyValue).RotatedBy(x / 9f * MathHelper.TwoPi - timeValue * 0.4f);
			for (int t = 0; t <= accuracy; t++)
			{
				float factor = t / accuracy;
				Vector2 velLeft = Vector2.Normalize(addVel.RotatedBy(MathHelper.PiOver2)) * 134 * energyValue;
				if (t == 0)
				{
					bars2.Add(new Vertex2D(bulbPos + addPos, Color.White * 0, new Vector3(timeValue + factor * factor + MathF.Sin(x), 0.5f, factor)));
					bars2.Add(new Vertex2D(bulbPos + addPos - velLeft, Color.White * 0, new Vector3(timeValue + factor * factor + MathF.Sin(x), 1, factor)));
				}
				bars2.Add(new Vertex2D(bulbPos + addPos, Color.White * (1 - factor), new Vector3(timeValue + factor * factor + MathF.Sin(x), 0.5f, factor)));
				bars2.Add(new Vertex2D(bulbPos + addPos - velLeft, Color.White * (1 - factor), new Vector3(timeValue + factor * factor + MathF.Sin(x), 1, factor)));

				if (t == 0)
				{
					bars.Add(new Vertex2D(bulbPos + addPos, c0 * 0, new Vector3(timeValue + factor * factor + MathF.Sin(x), 0.5f, factor)));
					bars.Add(new Vertex2D(bulbPos + addPos - velLeft, c0 * 0, new Vector3(timeValue + factor * factor + MathF.Sin(x), 1, factor)));
				}
				bars.Add(new Vertex2D(bulbPos + addPos, c0 * (1 - factor) * (1 - factor), new Vector3(timeValue + factor * factor + MathF.Sin(x), 0.5f, factor)));
				bars.Add(new Vertex2D(bulbPos + addPos - velLeft, c0 * (1 - factor) * (1 - factor), new Vector3(timeValue + factor * factor + MathF.Sin(x), 1, factor)));
				addPos += addVel;
				addVel = addVel.RotatedBy((1 - factor * factor * 1.2f) * 0.36f) * 1.08f;
			}
		}

		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		Effect effect = Commons.ModAsset.Trailing.Value;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;
		effect.Parameters["uTransform"].SetValue(model * projection);
		effect.CurrentTechnique.Passes[0].Apply();
		Main.graphics.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
		Main.graphics.GraphicsDevice.Textures[0] = Commons.ModAsset.Trail_2_black.Value;
		Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
		if (bars2.Count > 3)
		{
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars2.ToArray(), 0, bars2.Count - 2);
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars2.ToArray(), 0, bars2.Count - 2);
		}

		Main.graphics.GraphicsDevice.Textures[0] = Commons.ModAsset.Trail_2.Value;
		if (bars.Count > 3)
		{
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		}
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);


		if(!player.controlUseItem)
		{
			if(energyValue < 0.3f)
			{
				Texture2D light = ModAsset.CursedHitStar.Value;
				Main.spriteBatch.Draw(light, bulbPos - Main.screenPosition, null, c0, 0 + Projectile.ai[1], light.Size() / 2f, new Vector2(1f, energyValue * energyValue * 8), SpriteEffects.None, 0);
				Main.spriteBatch.Draw(light, bulbPos - Main.screenPosition, null, c0, 1.57f + Projectile.ai[1], light.Size() / 2f, new Vector2(0.5f, energyValue * energyValue * 8), SpriteEffects.None, 0);
			}
		}
	}
}