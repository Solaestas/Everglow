using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Everglow.Myth.TheMarbleRemains.NPCs.Bosses.EvilBottle.Dusts
{
    public class PurpleEffect2 : ModDust
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
            if(Main.rand.Next(100) > 2)
            {
                return new Color?(new Color(dust.scale / 1.9f, 0, dust.scale, dust.scale / 1.3f));
            }
            else
            {
                return new Color?(new Color(1f, 1f, 1f, 0));
            }
        }
        private float po = 0;
		public override bool Update(Dust dust)
		{
            dust.position += dust.velocity;
            dust.rotation += 0.1f;
            dust.scale *= 0.96f;
            dust.velocity *= 0.95f;
            float scale = dust.scale;
            Lighting.AddLight(dust.position, 0.6f, 0, 0.8f);
            if (dust.scale < 0.15f)
			{
				dust.active = false;
			}
			return false;
		}
	}
}
