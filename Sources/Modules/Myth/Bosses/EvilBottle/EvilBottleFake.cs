using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Everglow.Myth.Bosses.EvilBottle;
using Terraria.ModLoader;
using System.IO;
using Terraria.ID;
using Terraria.Localization;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader.IO;

namespace Everglow.Myth.Bosses.EvilBottle
{
	public class EvilBottleFake : ModNPC
    {
        public override void SetStaticDefaults()
        {
            // base.DisplayName.SetDefault("魔化力场");
            //base.DisplayName.AddTranslation(GameCulture.Chinese, "魔化力场");
        }
        private bool X = true;
        private int num10;
        private int ADDnum = 0;
        private float num13;
        private bool T = false;
        public override void SetDefaults()
        {
            NPC.behindTiles = true;
            base.NPC.damage = 0;
            base.NPC.width = 120;
            base.NPC.height = 160;
            base.NPC.defense = 0;
            base.NPC.lifeMax = 2000;
            base.NPC.knockBackResist = 0f;
            base.NPC.value = (float)Item.buyPrice(0, 0, 0, 0);
            base.NPC.color = new Color(0, 0, 0, 0);
            base.NPC.aiStyle = -1;
            this.AIType = -1;
            base.NPC.lavaImmune = true;
            base.NPC.noGravity = false;
            base.NPC.noTileCollide = false;
            base.NPC.HitSound = SoundID.NPCHit1;
            base.NPC.DeathSound = SoundID.NPCDeath1;
        }
        public override void AI()
        {
            NPC.dontTakeDamage = false;
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            //EvilBottle.Addlig = 2;
            if (base.NPC.life <= 0)
            {
            }
        }
    }
}
