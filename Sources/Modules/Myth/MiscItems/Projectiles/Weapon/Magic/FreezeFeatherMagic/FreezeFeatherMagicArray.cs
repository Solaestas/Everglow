using Everglow.Myth.MiscItems.Projectiles.Weapon.Magic.FireFeatherMagic;

namespace Everglow.Myth.MiscItems.Projectiles.Weapon.Magic.FreezeFeatherMagic;
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
//TODO:紫幽看看这个Visual类不正常，使用水晶颅骨升级冰火羽毛魔法书触发
internal class FreezeFeatherMagicArray : VisualProjectile
{
	public float WingPower = 0;
	public FrozenWingSlot FrozenWing = new FrozenWingSlot();
	public override string Texture => "Everglow/" + ModAsset.FreezeFeatherMagicPath;
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
		if (player.HeldItem.type == ModContent.ItemType<MiscItems.Weapons.FreezeFeatherMagic>())
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
		if (player.wingTime <= 1 && !player.mount._active)
		{
			if (WingPower > 0)
			{
				if (player.controlUp)
				{
					player.wingTime += 1;
					WingPower -= 0.1666666f;

					FrozenWing.FunctionalItem = new Item(ItemID.FrozenWings, 1);
					FrozenWing.valid = true;
					//TODO:即便这样子都用不了
					FrozenWing.ApplyEquipEffects();
					Main.NewText(FrozenWing.IsEnabled());
					//player.noFallDmg= true;
					//player.wings = 9;
					//player.wingTimeMax = 60;
					//player.wingsLogic = 9;
					//player.extraAccessory = true;
					//player.extraAccessorySlots = 1;
					//player.equippedWings = new Item(ItemID.FrozenWings, 1);
					//player.armor.SetValue(new Item(ItemID.FrozenWings , 1), player.armor.Length - 12);
					//player.velocity.Y -= 0.3f;
					//player.WingMovement();
					//player.WingAirLogicTweaks();
					//player.WingAirVisuals();
					//player.WingFrame(true, true);
					//player.GetWingStats(9);
				}
				else
				{
					FrozenWing.valid = false;
					FrozenWing.FunctionalItem.SetDefaults();
				}
			}
			else
			{
				player.noFallDmg = false;
				FrozenWing.valid = false;
				FrozenWing.FunctionalItem.SetDefaults();
			}
		}
		else
		{
			FrozenWing.valid = false;
			FrozenWing.FunctionalItem.SetDefaults();
		}
	}

	public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
	{
		behindNPCs.Add(index);
	}

	public override bool PreDraw(ref Color lightColor)
	{
		Projectile.hide = false;
		return false;
	}

	internal int Timer = 0;
	internal Vector2 RingPos = Vector2.Zero;

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
public class FrozenWingSlot : ModAccessorySlot
{
	public bool valid = false;
	public override bool IsEnabled() => valid;
	public override bool IsHidden() => false;
	public override bool CanAcceptItem(Item checkItem, AccessorySlotType context)
	{
		if (checkItem.wingSlot > 0) // if is Wing, then can go in slot
			return true;

		return false; // Otherwise nothing in slot
	}

	// Designates our slot to be a priority for putting wings in to. NOTE: use ItemLoader.CanEquipAccessory if aiming for restricting other slots from having wings!
	public override bool ModifyDefaultSwapSlot(Item item, int accSlotToSwapTo)
	{
		if (item.wingSlot > 0) // If is Wing, then we want to prioritize it to go in to our slot.
			return true;

		return false;
	}
}