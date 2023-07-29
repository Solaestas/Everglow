using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Localization;
using System.Collections.Generic;
using System.IO;
using Terraria.GameInput;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader.IO;
using Terraria.GameContent.Achievements;
namespace Everglow.Ocean.OceanVolcano.Items
{
    public class FlameEdge : ModItem
    {
        public override void SetStaticDefaults()
        {
            // base.// DisplayName.AddTranslation(GameCulture.Chinese, "烈焰边锋");
            // Tooltip.SetDefault("");
            GetGlowMask = Everglow.Ocean.SetStaticDefaultsGlowMask(this);
        }
        public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
            Item.glowMask = GetGlowMask;
            Item.damage = 130;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.width = 52;
            Item.height = 62;
            Item.useTime = 36;
            Item.rare = 11;
            Item.useAnimation = 18;
            Item.useStyle = 1;
            Item.knockBack = 2;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.crit = 22;
            Item.value = 10000;
            Item.scale = 1f;
            Item.shoot = ModContent.ProjectileType<Everglow.Ocean.Projectiles.FlameEdge>();
            Item.shootSpeed = 6f;
        }
        private int a = 0;
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(24, 900, false);
        }
    }
}
