using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Commons.Weapons.StabbingSwords;
public class PlayerStaminaDrawer : ModSystem
{
    Vector2 pos=Vector2.Zero;
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
    private int recCD = 45;
    /// <summary>
    /// 耐力恢复期
    /// </summary>
	public bool staminaRecovery = false;

    /// <summary>
    /// 耐力恢复速度 可更改
    /// </summary>
	public float staminaRecoveryValue = 1f;

	public override void ResetEffects()
	{
        staminaRecoveryValue = 1f;
	}
    /// <summary>
    /// 检测并扣除耐力值
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="pay"></param>
    /// <returns></returns>
	public bool CheckStamina(float amount,bool pay=true)
    {
        if (staminaRecovery)
            return false;
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
        if (stamina <= 0 && !staminaRecovery) //进入恢复期
        {
            stamina = 0;
            staminaRecovery = true;
        }
        if(staminaRecovery)
        {
            stamina += staminaRecoveryValue;
            if(stamina>100f)
            {
                staminaRecovery = false;
                stamina = 100f;
            }
        }
        if (Player.itemAnimation == 0)
        {
            if (stamina < 100)
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
                stamina = 100;
        }
        else
        {
            recCD = 45;
        }

        if (stamina != 100)
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
        float value = stamina / 100;
        Color color = Main.hslToRgb(value*0.36f-0.15f, 1f, 0.75f);
        float alpha = drawAlpha * (staminaRecovery ? 0.4f : 0.8f);
        Main.spriteBatch.Draw(tex, center, new Rectangle(0, 0, 2,2), color * alpha, 0, new Vector2(1f), new Vector2(1, value * 20), 0, 0);
    }
}
