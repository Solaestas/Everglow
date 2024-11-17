using Everglow.Commons.DataStructures;
using Everglow.Yggdrasil.YggdrasilTown.Items.Weapons.SquamousShell;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class EyeOfAnabiosis_MagicCircleFront : ModProjectile
{
	private const int MagicCircleHeight = 80;

	public override string Texture => ModAsset.EyeOfAnabiosis_Mod;

	private Player Owner => Main.player[Projectile.owner];

	public override void SetDefaults()
	{
		Projectile.width = 8;
		Projectile.height = 8;
		Projectile.aiStyle = -1;
		Projectile.timeLeft = 2;
		Projectile.tileCollide = false;
		Projectile.DamageType = DamageClass.Magic;
		Projectile.penetrate = -1;
		Projectile.ignoreWater = true;
	}

	public override void AI()
	{
		Projectile.Center = Owner.MountedCenter;
		Projectile.velocity *= 0;

		UpdateLifetime();
		UpdateTextList();
	}

	public void UpdateLifetime()
	{
		if (Owner == null
			|| !Owner.active
			|| Owner.dead
			|| Owner.CCed
			|| Owner.noItems)
		{
			Projectile.Kill();
			return;
		}

		if (Owner.HeldItem.type == ModContent.ItemType<EyeOfAnabiosis>())
		{
			if (Owner.heldProj >= 0 && Main.projectile[Owner.heldProj].type == ModContent.ProjectileType<EyeOfAnabiosis_Weapon>())
			{
				EyeOfAnabiosis_Weapon eOAW = Main.projectile[Owner.heldProj].ModProjectile as EyeOfAnabiosis_Weapon;
				if (eOAW.ChargeTimer >= EyeOfAnabiosis_Weapon.MaxChargeTime - 10)
				{
					Projectile.timeLeft = 60;
				}
			}
		}
	}

	private List<EyeOfAnabiosis_MagicCircleText> TextList { get; set; } = [];

	private void UpdateTextList()
	{
		if (Main.rand.NextBool(8))
		{
			TextList.Add(new EyeOfAnabiosis_MagicCircleText());
		}

		foreach (EyeOfAnabiosis_MagicCircleText text in TextList)
		{
			text.Update();
		}

		TextList.RemoveAll(text => text.RelativePosition.Y < -MagicCircleHeight);
	}

	public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
	{
		overPlayers.Add(index);
	}

	public override bool PreDraw(ref Color lightColor)
	{
		SpriteBatchState sBS = GraphicsUtils.GetState(Main.spriteBatch).Value;
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

		Effect shineEffect = ModAsset.EyeOfAnabiosis_Shine.Value;
		shineEffect.Parameters["uTime"].SetValue((float)Main.timeForVisualEffects);
		shineEffect.Parameters["uNoise"].SetValue(Commons.ModAsset.NoiseWave.Value);
		shineEffect.CurrentTechnique.Passes["MagicCircle_Pixel"].Apply();

		// Beam of magic circle
		var magicCirTexture = Commons.ModAsset.Point.Value;
		var magicCirPosition = Owner.gravDir == 1 ? Owner.Bottom : Owner.Top;
		magicCirPosition = magicCirPosition - Main.screenPosition + new Vector2(0, 2 * Owner.gravDir);
		var magicCirScale = new Vector2(0.30f, 0.30f);
		var magicCirRotation = Owner.gravDir == 1 ? 0 : MathF.PI;
		var magicCirColor = Color.White * 0.8f;
		Main.spriteBatch.Draw(magicCirTexture, magicCirPosition, null, magicCirColor, magicCirRotation, new Vector2(magicCirTexture.Width / 2, magicCirTexture.Height), magicCirScale, SpriteEffects.None, 0);

		// Core of magic circle
		var coreColor = new Color(0.4f, 0.8f, 2f, 0);
		var coreScale = new Vector2(0.2f, 0.32f) * 1.6f;

		Main.spriteBatch.End();
		Main.spriteBatch.Begin(sBS);

		// Main.spriteBatch.Draw(coreTexture, magicCirPosition, null, Color.White, magicCirRotation, coreTexture.Size() / 2, coreScale / 3f, SpriteEffects.None, 0);
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

		shineEffect.Parameters["uPow"].SetValue(24f);
		shineEffect.Parameters["uMul"].SetValue(1.3f);
		shineEffect.CurrentTechnique.Passes["MagicCircleCore_Pixel"].Apply();

		Texture2D coreTextureDark = Commons.ModAsset.Star_black.Value;
		Texture2D coreTexture = Commons.ModAsset.Textures_Star.Value;
		Texture2D bloom = Commons.ModAsset.Point.Value;

		Main.spriteBatch.Draw(coreTextureDark, magicCirPosition, null, Color.White * 0.5f, magicCirRotation, coreTextureDark.Size() / 2, coreScale * new Vector2(3f, 1f), SpriteEffects.None, 0);
		Main.spriteBatch.Draw(bloom, magicCirPosition, null, new Color(0.2f, 0.3f, 1f, 0) * 0.75f, magicCirRotation, bloom.Size() / 2, new Vector2(1.2f, 0.6f), SpriteEffects.None, 0);
		Main.spriteBatch.Draw(coreTexture, magicCirPosition, null, coreColor, magicCirRotation, coreTexture.Size() / 2, coreScale * new Vector2(3f, 1f), SpriteEffects.None, 0);
		Main.spriteBatch.Draw(bloom, magicCirPosition, null, new Color(1f, 1f, 1f, 0) * 0.76f, magicCirRotation, bloom.Size() / 2, new Vector2(0.4f, 0.2f), SpriteEffects.None, 0);

		Main.spriteBatch.End();
		Main.spriteBatch.Begin(sBS);

		// Text of magic circle
		var textTexture = Commons.ModAsset.AlienWriting.Value;
		foreach (var text in TextList)
		{
			var textColor = text.Color * (1 + text.RelativePosition.Y / MagicCircleHeight);
			Main.spriteBatch.Draw(textTexture, magicCirPosition + text.RelativePosition * Owner.gravDir, text.SourceRectangle, textColor, magicCirRotation, text.Origin, text.Scale, SpriteEffects.None, 0);
		}

		return false;
	}
}

public class EyeOfAnabiosis_MagicCircleBack : ModProjectile
{
	private const int MagicCircleHeight = 80;

	public override string Texture => ModAsset.EyeOfAnabiosis_Mod;

	private Player Owner => Main.player[Projectile.owner];

	public override void SetDefaults()
	{
		Projectile.width = 8;
		Projectile.height = 8;
		Projectile.aiStyle = -1;
		Projectile.timeLeft = 2;
		Projectile.tileCollide = false;
		Projectile.DamageType = DamageClass.Magic;
		Projectile.penetrate = -1;
		Projectile.ignoreWater = true;
	}

	public override void AI()
	{
		Projectile.Center = Owner.MountedCenter;
		Projectile.velocity *= 0;

		UpdateLifetime();
		UpdateTextList();
	}

	public void UpdateLifetime()
	{
		if (Owner == null
			|| !Owner.active
			|| Owner.dead
			|| Owner.CCed
			|| Owner.noItems)
		{
			Projectile.Kill();
			return;
		}

		if (Owner.HeldItem.type == ModContent.ItemType<EyeOfAnabiosis>())
		{
			Projectile.timeLeft = 60;
		}
	}

	private List<EyeOfAnabiosis_MagicCircleText> TextList { get; set; } = [];

	private void UpdateTextList()
	{
		if (Main.rand.NextBool(8))
		{
			TextList.Add(new EyeOfAnabiosis_MagicCircleText());
		}

		foreach (EyeOfAnabiosis_MagicCircleText text in TextList)
		{
			text.Update();
		}

		TextList.RemoveAll(text => text.RelativePosition.Y < -MagicCircleHeight);
	}

	public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
	{
		behindProjectiles.Add(index);
	}

	public override bool PreDraw(ref Color lightColor)
	{
		var magicCirPosition = Owner.gravDir == 1 ? Owner.Bottom : Owner.Top;
		magicCirPosition = magicCirPosition - Main.screenPosition + new Vector2(0, 2 * Owner.gravDir);
		var magicCirRotation = Owner.gravDir == 1 ? 0 : MathF.PI;

		// Text of magic circle
		var textTexture = Commons.ModAsset.AlienWriting.Value;
		foreach (var text in TextList)
		{
			var textColor = text.Color * (1 + text.RelativePosition.Y / MagicCircleHeight) * 0.3f;
			Main.spriteBatch.Draw(textTexture, magicCirPosition + text.RelativePosition * Owner.gravDir, text.SourceRectangle, textColor, magicCirRotation, text.Origin, text.Scale, SpriteEffects.None, 0);
		}

		return false;
	}
}

public class EyeOfAnabiosis_MagicCircleText
{
	public const float GenerateRange = 26f;

	public EyeOfAnabiosis_MagicCircleText()
	{
		RelativePosition = new Vector2(Main.rand.NextFloat(-GenerateRange, GenerateRange), 0);
		VelocityY = new Vector2(0, Main.rand.NextFloat(-0.4f, -0.45f));

		var texture = Commons.ModAsset.AlienWriting.Value;
		var frameWidth = texture.Width / 19;
		var frameHeight = texture.Height / 8;
		var CutHeight = Main.rand.Next(1, 4); // [2, 4]
		var CutOffsetX = Main.rand.Next(0, 19); // [0, 18]
		var CutOffsetY = Main.rand.Next(0, 9 - CutHeight); // [0, 8 - h]
		SourceRectangle = new Rectangle(CutOffsetX * frameWidth, CutOffsetY * frameHeight, frameWidth, CutHeight * frameHeight);
	}

	public Vector2 RelativePosition { get; private set; }

	public Vector2 VelocityY { get; init; }

	public Rectangle SourceRectangle { get; init; }

	public Color Color => new Color(11, 200, 230, 0);

	public Vector2 Scale => new Vector2(MathF.Sqrt(1 - MathF.Pow(RelativePosition.X / GenerateRange, 2)), 1) * 0.08f;

	public Vector2 Origin => new Vector2(SourceRectangle.Width / 2, SourceRectangle.Height);

	public void Update()
	{
		RelativePosition = RelativePosition + VelocityY;
	}
}