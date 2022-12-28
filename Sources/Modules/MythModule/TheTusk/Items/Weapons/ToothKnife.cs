using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.TheTusk.Items.Weapons
{
    public class ToothKnife : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tooth Blade");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "齿刃");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Russian, "Зубное лезвие");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 48;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Melee;
            Item.damage = 25;
            Item.knockBack = 6;
            Item.crit = 6;

            Item.value = 4000;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item1;

            Item.shoot = ModContent.ProjectileType<Projectiles.Weapon.Tusk0>(); // ID of the projectiles the sword will shoot
            Item.shootSpeed = 8f; // Speed of the projectiles the sword will shoot
        }
        // This method gets called when firing your weapon/sword.
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            /*Vector2 target = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			float ceilingLimit = target.Y;
			if (ceilingLimit > player.Center.Y - 200f)
			{
				ceilingLimit = player.Center.Y - 200f;
			}
			position = player.Center - new Vector2(Main.rand.NextFloat(401) * player.direction, 600f);
			position.Y -= 100;
			Vector2 heading = target - position;

			if (heading.Y < 0f)
			{
				heading.Y *= -1f;
			}

			if (heading.Y < 20f)
			{
				heading.Y = 20f;
			}

			heading.Normalize();
			heading *= velocity.Length();
			heading.Y += Main.rand.Next(-40, 41) * 0.02f;
			Projectile.NewProjectile(source, position, heading, type, damage, knockback, player.whoAmI, 0f, ceilingLimit);*/
            Vector2 v = new Vector2(0, 35);
            for (int k = -2; k < 3; k++)
            {
                int Ntype = ModContent.NPCType<NPCs.Bosses.BloodTusk.PhatomTusk1>();
                if (Main.rand.Next(10) > 6)
                {
                    Ntype = ModContent.NPCType<NPCs.Bosses.BloodTusk.PhatomTusk0>();
                }
                int h = NPC.NewNPC(null, (int)position.X + Main.rand.Next(-10, 10) + (k * 50), (int)position.Y + Main.rand.Next(-16, -10), Ntype, 0, player.GetWeaponDamage(player.HeldItem) /* player.GetDamage(DamageClass.Generic).Additive*/);
                Main.npc[h].velocity = v;
            }
            return false;
        }
    }
}
