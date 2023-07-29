using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Everglow.Ocean.Dusts
{
    public class Wave : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.velocity *= 1f;
			dust.noGravity = true;
			dust.noLight = true;
			dust.alpha = 0;
		}
        public override Color? GetAlpha(Dust dust, Color lightColor)
        {
            return new Color?(new Color(dust.velocity.Length() / 10f, dust.velocity.Length() / 10f, dust.velocity.Length() / 10f, 0));
        }
        private float po = 0;
		public override bool Update(Dust dust)
		{
            if(po == 0)
            {
                po = Main.rand.NextFloat(0.01f,6f);
            }
			dust.position += dust.velocity;
			dust.rotation += dust.velocity.X * 0.05f;
			dust.scale *= 0.98f;
            dust.scale *= 1 + (float)Math.Sin(Main.time / 5f + po + dust.velocity.Length() * 4f) / 10f;
            float scale = dust.scale;
			Lighting.AddLight(dust.position, 0.08f * (float)dust.scale, 0.16f * (float)dust.scale, 0.2f * (float)dust.scale);
			if (dust.scale < 0.15f)
			{
				dust.active = false;
			}
			return false;
		}
	}
}
