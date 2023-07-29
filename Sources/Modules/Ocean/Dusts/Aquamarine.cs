using System;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace Everglow.Ocean.Dusts
{
	// Token: 0x0200006D RID: 109
    public class Aquamarine : ModDust
	{
		// Token: 0x060001C5 RID: 453 RVA: 0x00002B3A File Offset: 0x00000D3A
		public override void OnSpawn(Dust dust)
		{
			dust.velocity *= 0.1f;
			dust.noGravity = true;
            dust.noLight = false ;
			dust.scale *= 1f;
			dust.alpha = 0;
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x0003162C File Offset: 0x0002F82C
		public override bool Update(Dust dust)
		{
			dust.position += dust.velocity;
			dust.rotation += 0.1f;
			dust.scale *= 0.95f;
			float scale = dust.scale;
			Lighting.AddLight(dust.position, 0f, 0.75f * dust.scale, 0.8f * dust.scale);
			if (dust.scale < 0.25f)
			{
				dust.active = false;
			}
			return false;
		}
        public override Color? GetAlpha(Dust dust, Color lightColor)
        {
            return new Color?(new Color(dust.scale, dust.scale, 1f, 0));
        }
    }
}
