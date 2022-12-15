﻿namespace Everglow.Sources.Modules.MythModule.TheFirefly.Dusts
{
    public class BlueParticleDark : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.noLight = false;
            dust.scale *= 1f;
            dust.alpha = 0;
        }

        public override bool Update(Dust dust)
        {
            dust.position += dust.velocity;
            dust.rotation += 0.1f;
            dust.velocity *= 0.95f;
            dust.alpha++;
            Lighting.AddLight(dust.position, 0, 0, (float)((255 - dust.alpha) * 0.0015f));
            if (dust.alpha > 254)
            {
                dust.active = false;
            }
            return false;
        }

        public override Color? GetAlpha(Dust dust, Color lightColor)
        {
            float k = (255 - dust.alpha) / 255f;
            if (dust.scale > 0.6f)
            {
                return new Color?(new Color(0f, 0.6f * k, 0.9f * k, 0f));
            }
            else
            {
                return new Color?(new Color(0, 0.6f * k, 0.9f * k, 0));
            }
        }
    }
}