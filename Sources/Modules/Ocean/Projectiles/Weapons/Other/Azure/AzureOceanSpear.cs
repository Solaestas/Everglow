using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace MythMod.Projectiles
{
    public class AzureOceanSpear : ModProjectile
	{
		public override void SetStaticDefaults()
		{
            base.DisplayName.SetDefault("碧海长矛");
		}
		public override void SetDefaults()
		{
			base.projectile.width = 128;
			base.projectile.aiStyle = 19;
			base.projectile.melee = true;
			base.projectile.timeLeft = 19;
			base.projectile.height = 128;
			base.projectile.friendly = true;
			base.projectile.hostile = false;
			base.projectile.tileCollide = false;
			base.projectile.ignoreWater = true;
			base.projectile.penetrate = -1;
			base.projectile.ownerHitCheck = true;
			base.projectile.hide = true;
		}
		public override void AI()
		{
			Main.player[base.projectile.owner].direction = base.projectile.direction;
			Main.player[base.projectile.owner].heldProj = base.projectile.whoAmI;
			Main.player[base.projectile.owner].itemTime = Main.player[base.projectile.owner].itemAnimation;
			base.projectile.position.X = Main.player[base.projectile.owner].position.X + (float)(Main.player[base.projectile.owner].width / 2) - (float)(base.projectile.width / 2);
			base.projectile.position.Y = Main.player[base.projectile.owner].position.Y + (float)(Main.player[base.projectile.owner].height / 2) - (float)(base.projectile.height / 2);
			base.projectile.position += base.projectile.velocity * base.projectile.ai[0];
			if (Main.rand.Next(4) == 0)
			{
			}
			if (base.projectile.ai[0] == 0f)
			{
				base.projectile.ai[0] = 3f;
				base.projectile.netUpdate = true;
			}
			if (Main.player[base.projectile.owner].itemAnimation < Main.player[base.projectile.owner].itemAnimationMax / 3)
			{
				base.projectile.ai[0] -= 2.4f;
				if (base.projectile.localAI[0] == 0f && Main.myPlayer == base.projectile.owner)
				{
					base.projectile.localAI[0] = 1f;
				}
			}
			else
			{
				base.projectile.ai[0] += 0.95f;
			}
			if (Main.player[base.projectile.owner].itemAnimation == 0)
			{
				base.projectile.Kill();
			}
			base.projectile.rotation = (float)Math.Atan2((double)base.projectile.velocity.Y, (double)base.projectile.velocity.X) + 2.355f;
			if (base.projectile.spriteDirection == -1)
			{
				base.projectile.rotation -= 1.57f;
			}
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
		}
	}
}
