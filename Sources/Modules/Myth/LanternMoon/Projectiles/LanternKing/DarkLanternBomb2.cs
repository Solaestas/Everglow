using Everglow.Myth.Common;
using Everglow.Myth.LanternMoon.NPCs.LanternGhostKing;
using Everglow.Myth.LanternMoon.Projectiles.LanternKing.VFXs;
using Terraria.Audio;
using Terraria.DataStructures;
using static Everglow.Myth.Common.MythUtils;

namespace Everglow.Myth.LanternMoon.Projectiles.LanternKing;
public class DarkLanternBomb2 : ModProjectile, IWarpProjectile
{
    public override void SetDefaults()
    {
        Projectile.width = 100;
        Projectile.height = 100;
        Projectile.aiStyle = -1;
        Projectile.hostile = false;
        Projectile.ignoreWater = true;
        Projectile.tileCollide = false;
        Projectile.extraUpdates = 3;
    }
	public override void OnSpawn(IEntitySource source)
	{
		float MinDis = (Projectile.Center - Main.MouseWorld).Length() * 1f;
		foreach (NPC npc in Main.npc)
		{
			if (npc.active)
			{
				if (npc.type == ModContent.NPCType<LanternGhostKing>())
				{
					float Dis = (npc.Center - Projectile.Center).Length() * 3f;
					if (Dis < MinDis)
						MinDis = Dis;
				}
			}
		}
		Projectile.timeLeft = (int)(900 + MinDis * 0.3);
		Projectile.ai[0] = Main.rand.NextFloat(0.3f, 1800f);
	}
	private float lightInit = 0f;
	public override void AI()
    {
		if (lightInit < 1) { lightInit += 0.01f; }
		else { lightInit = 1f; }

		Projectile.velocity *= 0f;
        if (Projectile.timeLeft < 90)
        {
            Projectile.scale += 0.05f;
        }
        if (Projectile.timeLeft < 3)
        {
            Projectile.scale += 0.05f;
            Projectile.hostile = true;
        }
    }
	
	public override bool PreDraw(ref Color lightColor)
    {
		var mainTex = (Texture2D)ModContent.Request<Texture2D>(Texture);
		float timeValue = (900 - Projectile.timeLeft) / 900f;
		float colorValue = timeValue * (float)(Math.Sin(Projectile.ai[1]) + 2) / 3f;
		var colorT = new Color(colorValue, colorValue, colorValue, 0.5f * colorValue);

		float timer = (float)(Main.timeForVisualEffects * 0.01f + Projectile.ai[0]);
        float sizeValue = (float)(Math.Sin(timer + Math.Sin(timer) * 6) * (0.95 + Math.Sin(timer + 0.24 + Math.Sin(timer))) + 3) / 30f;
		Texture2D textureLight = ModAsset.LanternKing_LightEffect.Value;
		Texture2D flameRing = ModAsset.CoreFlame.Value;
		Main.spriteBatch.Draw(textureLight, Projectile.Center - Main.screenPosition, null, new Color(1f, 0.05f, 0f, 0) * 0.4f, 0, textureLight.Size() / 2f, sizeValue * 12f * lightInit, SpriteEffects.None, 0f);
        Main.spriteBatch.Draw(textureLight, Projectile.Center - Main.screenPosition, null, new Color(1f, 0.05f, 0f, 0) * 0.4f, (float)(Math.PI * 0.5), textureLight.Size() / 2f, sizeValue * 6f * lightInit, SpriteEffects.None, 0f);
		Main.spriteBatch.Draw(ModAsset.LanternFire.Value, Projectile.Center - Main.screenPosition, new Rectangle(0, 30 * Projectile.frame, 20, 30), colorT, 0, new Vector2(10, 15), Projectile.scale * 0.5f, SpriteEffects.None, 1f);
		for (float k = 0; k < timeValue; k += 0.5f)
        {
			if (k > 0.5)
			{
				Main.spriteBatch.Draw(mainTex, Projectile.Center - Main.screenPosition, null, new Color(1f, 1f, 1f, 0), Projectile.rotation, mainTex.Size() / 2f, Projectile.scale, SpriteEffects.None, 1f);
			}
			else
			{
				Main.spriteBatch.Draw(mainTex, Projectile.Center - Main.screenPosition, null, colorT, Projectile.rotation, mainTex.Size() / 2f, Projectile.scale, SpriteEffects.None, 1f);
			}
		}
		float timeValueX2 = Math.Min(timeValue * 2, 1);
		colorValue = (float)(Math.Sin(2400d / (Projectile.timeLeft + 35)) * 0.75f - 0.15f);
		colorValue = Math.Max(0.05f, colorValue) - (Projectile.timeLeft / 400);
		colorValue += 1 - timeValueX2;
		DrawTexCircle(82 * timeValueX2, 22 * timeValueX2 + 22, new Color(0.75f, 0.45f, 0.45f, 0) * colorValue, Projectile.Center - Main.screenPosition, flameRing, Main.time / 17);
		return false;
    }
	private static void DrawTexCircle_VFXBatch(VFXBatch spriteBatch, float radius, float width, Color color, Vector2 center, Texture2D tex, double addRot = 0)
	{
		var circle = new List<Vertex2D>();

		for (int h = 0; h < radius / 2; h += 1)
		{
			circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radius - width, 0)).RotatedBy(h / radius * Math.PI * 4 + addRot), color, new Vector3(h * 2 / radius, 1, 0)));
			circle.Add(new Vertex2D(center + new Vector2(0, radius).RotatedBy(h / radius * Math.PI * 4 + addRot), color, new Vector3(h * 2 / radius, 0, 0)));
		}
		circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radius - width, 0)).RotatedBy(addRot), color, new Vector3(1, 1, 0)));
		circle.Add(new Vertex2D(center + new Vector2(0, radius).RotatedBy(addRot), color, new Vector3(1, 0, 0)));
		circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radius - width, 0)).RotatedBy(addRot), color, new Vector3(0, 1, 0)));
		circle.Add(new Vertex2D(center + new Vector2(0, radius).RotatedBy(addRot), color, new Vector3(0, 0, 0)));
		if (circle.Count > 2)
			spriteBatch.Draw(tex, circle, PrimitiveType.TriangleStrip);
	}
	public void DrawWarp(VFXBatch sb)
	{
	}
    public override void OnKill(int timeLeft)
    {
		ScreenShaker Gsplayer = Main.player[Projectile.owner].GetModPlayer<ScreenShaker>();
		Gsplayer.FlyCamPosition = new Vector2(0, 33).RotatedByRandom(6.283);
		var p = Projectile.NewProjectileDirect(Projectile.GetSource_Death(), Projectile.Center, Vector2.zeroVector, ModContent.ProjectileType<DarkLanternBombExplosion>(), Projectile.damage, Projectile.knockBack);
		p.friendly = Projectile.friendly;
		p.hostile = Projectile.hostile;	

		Vector2 GorePos;
		GorePos = new Vector2(Main.rand.NextFloat(-0.4f, 1.4f), 0).RotatedByRandom(6.283) * 6f;
		int gra0 = Gore.NewGore(null, Projectile.Center, GorePos, ModContent.Find<ModGore>("Everglow/FloatLanternGore3").Type, 1f);
		Main.gore[gra0].timeLeft = Main.rand.Next(300, 600);
		GorePos = new Vector2(Main.rand.NextFloat(-0.4f, 1.4f), 0).RotatedByRandom(6.283) * 6f;
		int gra1 = Gore.NewGore(null, Projectile.Center, GorePos, ModContent.Find<ModGore>("Everglow/FloatLanternGore4").Type, 1f);
		Main.gore[gra1].timeLeft = Main.rand.Next(300, 600);
		GorePos = new Vector2(Main.rand.NextFloat(-0.4f, 1.4f), 0).RotatedByRandom(6.283) * 6f;
		int gra2 = Gore.NewGore(null, Projectile.Center, GorePos, ModContent.Find<ModGore>("Everglow/FloatLanternGore5").Type, 1f);
		Main.gore[gra2].timeLeft = Main.rand.Next(300, 600);
		GorePos = new Vector2(Main.rand.NextFloat(-0.4f, 1.4f), 0).RotatedByRandom(6.283) * 6f;
		int gra3 = Gore.NewGore(null, Projectile.Center, GorePos, ModContent.Find<ModGore>("Everglow/FloatLanternGore6").Type, 1f);
		Main.gore[gra3].timeLeft = Main.rand.Next(300, 600);
		SoundEngine.PlaySound(SoundID.DD2_BetsyFireballImpact.WithVolumeScale(0.4f), Projectile.Center);
	}
}