using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Events;
using Terraria.GameInput;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Everglow.Sources.Modules.Food
{
    public class CalamityPlayer : ModPlayer
    {
        public bool DragonfruitBuff ;
        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {       
            if (DragonfruitBuff)
            {
                target.AddBuff(BuffID.OnFire, 180);
            }
							
		}
	}
}
