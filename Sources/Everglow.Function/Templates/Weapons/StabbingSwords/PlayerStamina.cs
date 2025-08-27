namespace Everglow.Commons.Templates.Weapons.StabbingSwords;
public class PlayerStaminaDrawer : ModSystem
{
	Vector2 pos = Vector2.Zero;
	public override void PostDrawInterface(SpriteBatch spriteBatch)
	{
		Player p = Main.LocalPlayer;
		pos = Vector2.Lerp(pos, p.Center - new Vector2(50, 0) * p.direction - Main.screenPosition, 0.2f);
		p.GetModPlayer<PlayerStamina>().DrawStamina(pos);
	}
}
public class PlayerStamina : ModPlayer
{
	private float stamina = 100f;
	private float maxStamina = 100f;
	private int recCD = 45;
	/// <summary>
	/// 体力消耗速率,默认为1
	/// </summary>
	public float staminaDecreasingSpeed = 1f;
	/// <summary>
	/// 额外的体力值
	/// </summary>
	public float extraStamina = 0f;
	public override void ResetInfoAccessories()
	{
		mulStaminaRecoveryValue = 1f;
		staminaDecreasingSpeed = 1f;
		extraStamina = 0f;
	}
	/// <summary>
	/// 耐力恢复速度 可更改
	/// </summary>
	public float staminaRecoveryValue = 1f;
	/// <summary>
	/// 耐力恢复倍率 可更改(乘算于耐力恢复速度之后)
	/// </summary>
	public float mulStaminaRecoveryValue = 1f;
	public override void ResetEffects()
	{
		staminaRecoveryValue = (maxStamina + extraStamina) / 100f * mulStaminaRecoveryValue;
	}
	/// <summary>
	/// 耐力恢复期
	/// </summary>
	public bool staminaRecovery = false;
	/// <summary>
	/// 检测并扣除耐力值
	/// </summary>
	/// <param name="amount"></param>
	/// <param name="pay"></param>
	/// <returns></returns>
	public bool CheckStamina(float amount, bool pay = true)
	{
		if (staminaRecovery)
			return false;
		amount *= staminaDecreasingSpeed;
		if (stamina > amount)
		{
			if (pay)
				stamina -= amount;
		}
		else
		{
			stamina = 0;
		}
		return true;
	}
	public override void PostUpdate()
	{
		maxStamina = 100f + extraStamina;
		if (stamina <= 0 && !staminaRecovery) //进入恢复期
		{
			stamina = 0;
			staminaRecovery = true;
		}
		if (staminaRecovery && recCD <= 0)
		{
			stamina += staminaRecoveryValue;
			if (stamina > maxStamina)
			{
				staminaRecovery = false;
				stamina = maxStamina;
			}
		}
		if (Player.itemAnimation == 0)
		{
			if (stamina < maxStamina)
			{
				if (recCD <= 0)
				{
					recCD = 0;
					stamina += staminaRecoveryValue / 2f;
				}
				else
				{
					recCD--;
				}
			}
			else
				stamina = maxStamina;
		}
		else
		{
			recCD = 45;
		}

		if (stamina != maxStamina)
		{
			drawAlpha = MathHelper.Lerp(drawAlpha, 1f, 0.2f);
			drawAlphaCD = 30;
		}
		else
		{
			if (drawAlphaCD > 0)
			{
				drawAlphaCD--;
			}
			else
				drawAlpha = MathHelper.Lerp(drawAlpha, 0f, 0.05f);
		}

	}
	float drawAlpha = 0f;
	int drawAlphaCD = 30;
	public void DrawStamina(Vector2 center)//最好重绘一下，现在这个比较丑
	{
		if (drawAlpha == 0)
			return;
		Texture2D tex = Terraria.GameContent.TextureAssets.MagicPixel.Value;
		float value = stamina / maxStamina;
		Color color = Main.hslToRgb(value * 0.36f - 0.15f, 1f, 0.75f);
		float alpha = drawAlpha * (staminaRecovery ? 0.4f : 0.8f);
		Main.spriteBatch.Draw(tex, center, new Rectangle(0, 0, 2, 2), color * alpha, 0, new Vector2(1f), new Vector2(1, value * 20), 0, 0);
	}
}
