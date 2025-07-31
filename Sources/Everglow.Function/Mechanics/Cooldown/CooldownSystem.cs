using Everglow.Commons.Graphics;
using Everglow.Commons.Utilities;
using Everglow.Commons.Vertex;
using ReLogic.Graphics;
using Terraria.GameContent;

namespace Everglow.Commons.Mechanics.Cooldown;

public class CooldownSystem : ModSystem
{
	public const float DefaultCooldownAlpha = 0.4f;

	private static Dictionary<string, float> cooldownAlpha;
	private static bool bossNearby = false;

	public override void Load()
	{
		cooldownAlpha = [];
	}

	public override void Unload()
	{
		cooldownAlpha = null;
	}

	public override void PostDrawInterface(SpriteBatch spriteBatch)
	{
		var p = Main.LocalPlayer;
		var mp = Main.LocalPlayer.GetModPlayer<CooldownPlayer>();

		bossNearby = Main.npc.Any(npc => npc.active && npc.boss);
		UpdateAlpha();

		if (Main.playerInventory)
		{
			return;
		}

		// Draw cooldowns
		Vector2 baseDrawPosition = new Vector2(32, 80) + Vector2.UnitY * 50 * MathF.Ceiling(Main.LocalPlayer.CountBuffs() / 11f);
		Vector2 spacing = Vector2.UnitX * 38f;

		string coolDownTextKey = string.Empty;
		Point drawPos = baseDrawPosition.ToPoint();
		foreach (var (cKey, cooldown) in mp.cooldowns)
		{
			coolDownTextKey = DrawCooldownIcon(spriteBatch, p, coolDownTextKey, cKey, drawPos, new Point(Main.mouseX, Main.mouseY));
			drawPos += spacing.ToPoint();
		}

		if (coolDownTextKey != string.Empty)
		{
			var cdText = mp.cooldowns[coolDownTextKey].cooldown;
			Main.instance.MouseText(cdText.DisplayName + "\n" + cdText.Discription);
			Main.LocalPlayer.mouseInterface = true;
		}
	}

	public static void UpdateAlpha()
	{
		var toAdd = Main.LocalPlayer.GetModPlayer<CooldownPlayer>().cooldowns.Where(c => !cooldownAlpha.ContainsKey(c.Key)).Select(o => o.Key);
		foreach (var key in toAdd)
		{
			cooldownAlpha[key] = DefaultCooldownAlpha;
		}

		var toDelete = cooldownAlpha.Where(c => !Main.LocalPlayer.GetModPlayer<CooldownPlayer>().cooldowns.ContainsKey(c.Key)).Select(o => o.Key);
		foreach (var key in toDelete)
		{
			cooldownAlpha.Remove(key);
		}
	}

	public static string DrawCooldownIcon(SpriteBatch spriteBatch, Player player, string hoveredCooldownIndex, string cooldownSlotOnPlayer, Point position, Point mousePosition)
	{
		var mp = player.GetModPlayer<CooldownPlayer>();
		if (!player.HasCooldown(cooldownSlotOnPlayer))
		{
			return hoveredCooldownIndex;
		}

		int width = 32, height = 32;
		(int drawPosX, int drawPosY) = (position.X, position.Y);
		Vector2 drawPosition = new Vector2(drawPosX, drawPosY);
		Vector2 textPosition = new Vector2(drawPosX, drawPosY + height);
		Rectangle mouseRectangle = new Rectangle(drawPosX, drawPosY, width, height);
		Color drawColor = bossNearby ? Color.White : Color.White * cooldownAlpha[cooldownSlotOnPlayer];

		// Draw icon
		var sBS = GraphicsUtils.GetState(spriteBatch).Value;
		spriteBatch.End();
		spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.UIScaleMatrix);
		var effect = ModAsset.Cooldown_IconShader.Value;
		effect.Parameters["uCut"].SetValue(ModAsset.Cooldown_Cut.Value);
		effect.Parameters["uBg"].SetValue(ModAsset.Cooldown_Background.Value);
		effect.CurrentTechnique.Passes[0].Apply();

		List<Vertex2D> vertices = [];
		vertices.Add(drawPosition, drawColor, new(0, 0, 0));
		vertices.Add(drawPosition + new Vector2(width, 0), drawColor, new(1, 0, 0));
		vertices.Add(drawPosition + new Vector2(0, height), drawColor, new(0, 1, 0));
		vertices.Add(drawPosition + new Vector2(width, height), drawColor, new(1, 1, 0));
		Main.graphics.GraphicsDevice.Textures[0] = mp.cooldowns[cooldownSlotOnPlayer].cooldown.Texture;
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices.ToArray(), 0, vertices.Count - 2);

		// Draw time progress circle
		spriteBatch.End();
		spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.UIScaleMatrix);
		spriteBatch.GraphicsDevice.Textures[0] = ModAsset.White.Value;
		var progress = mp.cooldowns[cooldownSlotOnPlayer].Progress;
		var progressPos = drawPosition + new Vector2(width / 2, height / 2);
		var progressAlpha = cooldownAlpha[cooldownSlotOnPlayer];
		progressAlpha += (1 - progressAlpha) * 0.5f;
		var progressColor = Color.Lerp(Color.Green, Color.Red, progress) * progressAlpha * 0.6f;
		var progressSize = new Vector2(0, 7);
		ValueBarHelper.DrawCircle(spriteBatch, progressPos, progressColor, progressSize, progressSize, progress, clockwise: false);

		// Draw progress cursor
		// TODO: Enhance the progress cursor appearance
		spriteBatch.Draw(ModAsset.White.Value, progressPos, null, drawColor, MathHelper.Pi - progress * MathHelper.TwoPi, Vector2.Zero, new Vector2(1 / 256f, 13 / 256f), SpriteEffects.None, 0);

		spriteBatch.End();
		spriteBatch.Begin(sBS);

		// Draw icon frame
		spriteBatch.Draw(ModAsset.Cooldown_IconFrame.Value, drawPosition, null, drawColor, 0f, default, 1f, SpriteEffects.None, 0f);

		// Draw time text
		var cooldownTime = mp.cooldowns[cooldownSlotOnPlayer].timeLeft;
		if (cooldownTime > 2)
		{
			string text = Lang.LocalizedDuration(new TimeSpan(0, 0, cooldownTime / 60), abbreviated: true, showAllAvailableUnits: false);
			spriteBatch.DrawString(FontAssets.ItemStack.Value, text, textPosition, drawColor, 0f, default, 0.8f, SpriteEffects.None, 0f);
		}

		// Set mouse text and adjust alpha on mouse hover
		if (mouseRectangle.Contains(mousePosition))
		{
			hoveredCooldownIndex = cooldownSlotOnPlayer;
			cooldownAlpha[cooldownSlotOnPlayer] += 0.1f;
		}
		else
		{
			cooldownAlpha[cooldownSlotOnPlayer] -= 0.05f;
		}

		if (cooldownAlpha[cooldownSlotOnPlayer] > 1f)
		{
			cooldownAlpha[cooldownSlotOnPlayer] = 1f;
		}
		else if (cooldownAlpha[cooldownSlotOnPlayer] < DefaultCooldownAlpha)
		{
			cooldownAlpha[cooldownSlotOnPlayer] = DefaultCooldownAlpha;
		}

		return hoveredCooldownIndex;
	}
}