using Everglow.Commons.DataStructures;
using Everglow.Commons.Utilities;
using Terraria.DataStructures;

namespace Everglow.Commons.Templates.Weapons.StabbingSwords;

public class PlayerStaminaDrawer : ModSystem
{
	private Vector2 pos = Vector2.Zero;

	public override void PostDrawInterface(SpriteBatch spriteBatch)
	{
		SpriteBatchState sBS = GraphicsUtils.GetState(spriteBatch).Value;
		spriteBatch.End();
		spriteBatch.Begin(sBS.SortMode, BlendState.AlphaBlend, SamplerState.PointWrap, sBS.DepthStencilState, sBS.RasterizerState, sBS.Effect, Main.GameViewMatrix.TransformationMatrix);
		Player p = Main.LocalPlayer;
		pos = Vector2.Lerp(pos, p.Center + new Vector2(-50 * p.direction, 40 * p.gravDir), 0.2f);
		p.GetModPlayer<PlayerStamina>().DrawStamina(pos);
		spriteBatch.End();
		spriteBatch.Begin(sBS);
	}

	public override void PostDrawTiles() => base.PostDrawTiles();
}

public class PlayerStamina : ModPlayer
{
	private float stamina = 100f;
	private float maxStamina = 100f;

	public const int RecoveryCoolingDefault = 45;

	/// <summary>
	/// The time interval between recovery process and the (end of) last attack<br/>
	/// 最后一次攻击(结束)到恢复期的时间间隔
	/// </summary>
	public int RecoveryCooling = RecoveryCoolingDefault;

	/// <summary>
	/// he speed of stamina consuming, default to 1.<br/>
	/// 体力消耗速率,默认为1
	/// </summary>
	public float StaminaDecreasingSpeed = 1f;

	/// <summary>
	/// Extra stamina, usually produced by buffs or equipments.<br/>
	/// 额外的体力值, 通常由增益或装备提供
	/// </summary>
	public float ExtraStamina = 0f;

	public override void ResetInfoAccessories()
	{
		StaminaRecoveryValueRate = 1f;
		StaminaDecreasingSpeed = 1f;
		ExtraStamina = 0f;
	}

	/// <summary>
	/// The speed of stamina recovering, default to 1.<br/>
	/// 耐力恢复速度, 默认为1
	/// </summary>
	public float StaminaRecoveryValue = 1f;

	/// <summary>
	/// The rate of recovering speed, a multilicative zone of <see cref="StaminaRecoveryValue"/>, default to 1
	/// 耐力恢复倍率 可更改(乘算于耐力恢复速度之后), 默认为1
	/// </summary>
	public float StaminaRecoveryValueRate = 1f;

	public override void ResetEffects()
	{
		StaminaRecoveryValue = (maxStamina + ExtraStamina) / 100f * StaminaRecoveryValueRate;
	}

	/// <summary>
	/// 耐力恢复期
	/// </summary>
	public bool StaminaRecovery = false;

	/// <summary>
	/// 检测并扣除耐力值
	/// </summary>
	/// <param name="amount"></param>
	/// <param name="pay"></param>
	/// <returns></returns>
	public bool CheckStamina(float amount, bool pay = true)
	{
		if (StaminaRecovery)
		{
			return false;
		}

		amount *= StaminaDecreasingSpeed;
		if (stamina > amount)
		{
			if (pay)
			{
				stamina -= amount;
			}
		}
		else
		{
			stamina = 0;
		}
		return true;
	}

	public override void PostUpdate()
	{
		maxStamina = 100f + ExtraStamina;

		// Start Recovery Process | 进入恢复期
		if (stamina <= 0 && !StaminaRecovery)
		{
			stamina = 0;
			StaminaRecovery = true;
		}
		if (StaminaRecovery && RecoveryCooling <= 0)
		{
			stamina += StaminaRecoveryValue;
			if (stamina > maxStamina)
			{
				StaminaRecovery = false;
				stamina = maxStamina;
			}
		}
		if (Player.itemAnimation == 0)
		{
			if (stamina < maxStamina)
			{
				if (RecoveryCooling <= 0)
				{
					RecoveryCooling = 0;
					stamina += StaminaRecoveryValue / 2f;
				}
				else
				{
					RecoveryCooling--;
				}
			}
			else
			{
				stamina = maxStamina;
			}
		}
		else
		{
			RecoveryCooling = RecoveryCoolingDefault;
		}

		if (stamina != maxStamina)
		{
			drawAlpha = MathHelper.Lerp(drawAlpha, 1f, 0.2f);
			drawAlphaCD = 0;
		}
		else
		{
			if (drawAlphaCD > 0)
			{
				drawAlphaCD--;
			}
			else
			{
				drawAlpha = MathHelper.Lerp(drawAlpha, 0f, 0.08f);
			}
		}
	}

	private float drawAlpha = 0f;
	private int drawAlphaCD = 0;

	public void DrawStamina(Vector2 center)
	{
		if (drawAlpha == 0)
		{
			return;
		}

		Texture2D tex_dark = ModAsset.StabbingSwordStamina_black.Value;
		Texture2D tex = ModAsset.StabbingSwordStamina.Value;
		Texture2D tex_bloom = ModAsset.StabbingSwordStamina_bloom.Value;
		float value = stamina / maxStamina;
		Color color = Main.hslToRgb(value * 0.36f - 0.15f, 1f, 0.75f);
		color.A = 0;
		float alpha = drawAlpha * (StaminaRecovery ? 0.4f : 0.8f);
		int height = (int)(tex.Height * value);
		int height2 = height + 2;
		if (height <= 1)
		{
			height2 = 0;
		}
		if (height2 > tex.Height)
		{
			height2 = tex.Height;
		}
		Vector2 drawPos = center - Main.screenPosition;
		Rectangle frame = new Rectangle(0, tex.Height - height, tex.Width, height);
		Rectangle frame2 = new Rectangle(0, tex.Height - height2, tex.Width, height2);
		Main.spriteBatch.Draw(tex_dark, center - Main.screenPosition, null, Color.White * alpha, 0, new Vector2(tex.Width * 0.5f, tex.Height), 1f, 0, 0);
		Main.spriteBatch.Draw(tex, center - Main.screenPosition + new Vector2(0, tex.Height - height2), frame2, color * alpha * 3, 0, new Vector2(tex.Width * 0.5f, tex.Height), 1f, 0, 0);
		Main.spriteBatch.Draw(tex_dark, center - Main.screenPosition + new Vector2(0, tex.Height - height), frame, Color.White * alpha, 0, new Vector2(tex.Width * 0.5f, tex.Height), 1f, 0, 0);
		Main.spriteBatch.Draw(tex, center - Main.screenPosition + new Vector2(0, tex.Height - height), frame, color * alpha * 0.7f, 0, new Vector2(tex.Width * 0.5f, tex.Height), 1f, 0, 0);
		Main.spriteBatch.Draw(tex_bloom, center - Main.screenPosition + new Vector2(0, 45), null, color * alpha * 0.4f, 0, new Vector2(tex_bloom.Width * 0.5f, tex_bloom.Height), 1f, 0, 0);
		if (stamina == maxStamina)
		{
			Main.spriteBatch.Draw(tex_bloom, center - Main.screenPosition + new Vector2(0, 45), null, color * alpha * 0.6f, 0, new Vector2(tex_bloom.Width * 0.5f, tex_bloom.Height), 1f, 0, 0);
		}
		Vector2 topLeft = drawPos - new Vector2(tex.Width * 0.5f, tex.Height);
		Rectangle drawArea = new Rectangle((int)topLeft.X, (int)topLeft.Y, frame.Width, frame.Height);
		if(drawArea.Contains((int)Main.MouseWorld.X, (int)Main.MouseWorld.Y))
		{
			//Main.instance.MouseText += "Stamina";
		}
	}

	public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
	{
		base.DrawEffects(drawInfo, ref r, ref g, ref b, ref a, ref fullBright);
	}

	public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo) => base.ModifyDrawInfo(ref drawInfo);
}