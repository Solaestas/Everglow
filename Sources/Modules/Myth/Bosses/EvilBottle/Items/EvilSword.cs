using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
namespace Everglow.Myth.Bosses.EvilBottle.Items
{
    public class EvilSword : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("附邪魔剑");
        }
        public override void SetDefaults()
        {
            Item.damage = 42;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.width = 68;
            Item.height = 68;
            Item.useTime = 28;
            Item.rare = 3;
            Item.useAnimation = 14;
            Item.useStyle = 1;
            Item.knockBack = 4;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.crit = 8;
            Item.value = 6000;
            Item.scale = 1f;
            Item.shoot = base.Mod.Find<ModProjectile>("EvilSword").Type;
            Item.shootSpeed = 4f;

        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            Dust.NewDust(new Vector2((float)hitbox.X, (float)hitbox.Y), hitbox.Width, hitbox.Height, 27, 0f, 0f, 0, default(Color), 2f);
            Dust.NewDust(new Vector2((float)hitbox.X, (float)hitbox.Y), hitbox.Width, hitbox.Height, 27, 0f, 0f, 0, default(Color), 1.3f);
        }
        private int Cou = 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            /*Cou += 1;
            if(Cou % 5 == 0)
            {
                Cou = 0;
                Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 0, mod.ProjectileType("DarkFlameMagicF"), damage, 0f, Main.myPlayer, 0f, 0f);
            }*/
            return true;
        }
    }
}
