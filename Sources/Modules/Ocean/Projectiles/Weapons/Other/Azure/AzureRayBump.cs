using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.Localization;
using Terraria.ModLoader.IO;
namespace Everglow.Ocean.Projectiles.Weapons.Other.Azure;

//135596
public class AzureRayBump : ModProjectile
{
	//4444444
	public override void SetStaticDefaults()
	{
		//DisplayName.SetDefault("蔚蓝射线爆炸");
		Main.projFrames[base.Projectile.type] = 5;
	}
	//7359668
	public override void SetDefaults()
	{
		Projectile.width = 52;
		Projectile.height = 52;
		Projectile.aiStyle = -1;
		Projectile.friendly = false;
		Projectile.ignoreWater = true;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 44;
		Projectile.hostile = true;
		Projectile.penetrate = -1;
		Projectile.scale = 1f;
	}
	//55555
	public override Color? GetAlpha(Color lightColor)
	{
		if (Projectile.timeLeft > 60)
		{
			return new Color?(new Color(20, 20, 255, 0));
		}
		else
		{
			return new Color?(new Color(1 * Projectile.timeLeft / 60f, 1 * Projectile.timeLeft / 60f, 1 * Projectile.timeLeft / 60f, 0));
		}
	}
	private bool initialization = true;
	private float b;
	public override void AI()
	{
		base.Projectile.frame = (int)(4 - (Projectile.timeLeft / 10));
		if (Projectile.timeLeft > 120)
		{
			Lighting.AddLight(base.Projectile.Center, (float)(255 - base.Projectile.alpha) * 0f / 255f * Projectile.scale, (float)(255 - base.Projectile.alpha) * 0.23f * Projectile.scale / 255f, (float)(255 - base.Projectile.alpha) * 2.55f / 255f * Projectile.scale);
		}
		else
		{
			Lighting.AddLight(base.Projectile.Center, (float)(255 - base.Projectile.alpha) * 0f / 255f * Projectile.scale * Projectile.timeLeft / 120f, (float)(255 - base.Projectile.alpha) * 0.23f * Projectile.scale / 255f * Projectile.timeLeft / 120f, (float)(255 - base.Projectile.alpha) * 2.55f / 255f * Projectile.scale * Projectile.timeLeft / 120f);
		}
	}
	//14141414141414
	public override void Kill(int timeLeft)
	{
	}
}