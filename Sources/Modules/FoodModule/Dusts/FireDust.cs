using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Everglow.Sources.Modules.FoodModule.Dusts
{
    public class FireDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.frame = new Rectangle(0, 0, 64, 64);

            dust.shader = new Terraria.Graphics.Shaders.ArmorShaderData(new Ref<Effect>(ModContent.Request<Effect>("Everglow/Sources/Modules/FoodModule/Effects/GlowingDust", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value), "GlowingDustPass");;
        }

        public override Color? GetAlpha(Dust dust, Color lightColor)
        {
            return dust.color;
        }

        public override bool Update(Dust dust)
        {
            if (dust.customData is null)
            {
                dust.customData = true;
            }



            dust.scale *= 0.95f;


            dust.rotation = dust.velocity.ToRotation();
            

            dust.shader.UseColor(dust.color);

            dust.position += dust.velocity;

            if (!dust.noGravity)
                dust.velocity.Y += 0.1f;

            dust.velocity *= 0.75f;
            dust.color *= 0.95f;

            if (!dust.noLight)
                Lighting.AddLight(dust.position, dust.color.ToVector3());

            if (dust.scale < 0.05f)
                dust.active = false;

            return false;
        }
    }
}
