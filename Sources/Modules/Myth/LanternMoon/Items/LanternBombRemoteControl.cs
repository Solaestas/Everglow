using Everglow.Myth.LanternMoon.Projectiles;
using Terraria.DataStructures;

namespace Everglow.Myth.LanternMoon.Items;

public class LanternBombRemoteControl : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.MagicWeapons;

    public override void SetDefaults()
    {
        Item.DamageType = DamageClass.Magic;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.rare = ItemRarityID.White;
        Item.noUseGraphic = true;
        Item.autoReuse = false;
        Item.useTurn = true;
        Item.mana = 7;
        Item.width = 20;
        Item.height = 38;
        Item.useAnimation = 1;
        Item.useTime = 1;
        Item.value = 10000;
        Item.shoot = ProjectileID.WoodenArrowFriendly;
        Item.shootSpeed = 10;
        Item.damage = 9999;
        Item.knockBack = 15;
    }
    public override bool AltFunctionUse(Player player)
    {
        return true;
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        if (player.altFunctionUse == 2)
        {
            //var lanternExplosion = new LanternExplosion
            //{
            //	velocity = Vector2.Zero,
            //	Active = true,
            //	Visible = true,
            //	position = Main.MouseWorld,
            //	maxTime = Main.rand.Next(156, 176),
            //	ai = new float[] { Main.rand.NextFloat(0.1f, 1f), Main.rand.NextFloat(4.15f, 4.6f), Main.rand.NextFloat(8f, 12f) },
            //	FireBallVelocity = new Vector2[]
            //		{
            //	RandomVector2(4f, 3f),
            //	RandomVector2(4f, 3f),
            //	RandomVector2(4f, 3f),
            //	RandomVector2(4f, 3f),
            //	RandomVector2(3f, 2f),
            //	RandomVector2(3f, 2f),
            //	RandomVector2(3f, 1.5f),
            //	RandomVector2(2.5f, 0.5f),
            //	RandomVector2(2.5f, 0.5f),
            //	RandomVector2(2.5f, 0.01f),
            //	RandomVector2(2, 0.01f),
            //	RandomVector2(1, 0.01f)
            //		}
            //};
            //Ins.VFXManager.Add(lanternExplosion);

            //for (int x = 0; x < 75; x++)
            //{
            //	var flameDust = new FlameDust
            //	{
            //		velocity = RandomVector2(45f, 20f),
            //		Active = true,
            //		Visible = true,
            //		position = Main.MouseWorld,
            //		maxTime = Main.rand.Next(66, 126),
            //		ai = new float[] { Main.rand.NextFloat(0.1f, 1f), 0, Main.rand.NextFloat(0.4f, 2.4f) }
            //	};
            //	Ins.VFXManager.Add(flameDust);
            //}
            //SoundEngine.PlaySound(SoundID.DD2_BetsyFireballImpact.WithPitchOffset(-1f), Main.MouseWorld);
            //Projectile.NewProjectile(source, Main.MouseWorld, Vector2.zeroVector, ModContent.ProjectileType<DarkLanternBombExplosion_II>(), damage, knockback);
            //int posX = (int)(Main.MouseWorld.X / 16);
            //int posY = (int)(Main.MouseWorld.Y / 16);
            //for (int x = -25;x < 26;x++)
            //{
            //	for (int y = -25; y < 26; y++)
            //	{
            //		if(new Vector2(x, y).Length() <= 25)
            //		{
            //			WorldGen.KillTile(x + posX, y + posY);
            //		}
            //	}
            //}
            Projectile.NewProjectileDirect(Item.GetSource_FromAI(), Main.MouseWorld, Vector2.zeroVector, ModContent.ProjectileType<Firework12Inches>(), 50, 0f, player.whoAmI, 0, 0);
            return false;
        }
        for (int j = -10; j < 10; j++)
        {
            Vector2 v2 = new Vector2(j * 90, MathF.Sin(j) * 100);
            //Projectile p = Projectile.NewProjectileDirect(Item.GetSource_FromAI(), Main.MouseWorld + v2, Vector2.zeroVector, ModContent.ProjectileType<Firework6Inches>(), 50, 0f, player.whoAmI, 0, 0);

        }
        Projectile p = Projectile.NewProjectileDirect(Item.GetSource_FromAI(), Main.MouseWorld, Vector2.zeroVector, ModContent.ProjectileType<Firework6Inches>(), 50, 0f, player.whoAmI, 0, 0);

        return false;
    }
    public Vector2 RandomVector2(float maxLength, float minLength = 0)
    {
        if (maxLength <= minLength)
        {
            maxLength = minLength + 0.001f;
        }
        return new Vector2(Main.rand.NextFloat(minLength, maxLength), 0).RotatedByRandom(6.283);
    }
}
