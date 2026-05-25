using Everglow.Commons.Vertex;
using Everglow.Commons.VFX;
using rail;

namespace Everglow.Commons.Utilities.BackgroundHelper;

public abstract class BgSlide
{
	public float Distance;

	public Vector2 WorldAnchor;

	public Texture2D Texture;

	public string UniqueName;

	public Effect Shader;

	public float Alpha = 0f;

	public float Scale = 1f;

	public bool Active = true;

	/// <summary>
	/// 0 : Sky Color<br/>
	/// 1 : TileMap Colop<br/>
	/// 2 : White
	/// </summary>
	public int UseColorStyle = 0;

	public BgSlide()
	{
		SetDefaults();
	}

	public virtual void SetDefaults()
	{
		Distance = float.PositiveInfinity;
		UniqueName = string.Empty;

		// Most common cases.
		Shader = BgSlide.XWrap_YClamp_Shader;
	}

	public virtual void Update()
	{
		if (!CanActive())
		{
			FadeOut();
		}
		else
		{
			FadeIn();
		}
	}

	public virtual void FadeIn()
	{
		if (Alpha < 1)
		{
			Alpha += 0.01f;
		}
		else
		{
			Alpha = 1f;
		}
	}

	public virtual void FadeOut()
	{
		if (Alpha > 0)
		{
			Alpha -= 0.01f;
		}
		else
		{
			Alpha = 0f;
			Active = false;
		}
	}

	public virtual bool CanActive()
	{
		return false;
	}

	public virtual void Draw()
	{
		DrawPreset_Normal(this);
	}

	private static Color GetColor(BgSlide bg, Vector2 worldPos)
	{
		Color c = Color.White;
		switch (bg.UseColorStyle)
		{
			case 0:
				c = Main.ColorOfTheSkies;
				break;
			case 1:
				c = Lighting.GetColor((worldPos + new Vector2(-8)).ToTileCoordinates());
				break;
		}
		return c * bg.Alpha;
	}

	public static void DrawPreset_Piece(BgSlide bg)
	{
		Vector2 screenCenter = Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight) * 0.5f;
		Vector2 move = (screenCenter - bg.WorldAnchor) / bg.Distance;
		Main.spriteBatch.Draw(bg.Texture, bg.WorldAnchor - move - Main.screenPosition, null, GetColor(bg, bg.WorldAnchor), 0, bg.Texture.Size() * 0.5f, bg.Scale / bg.Distance, SpriteEffects.None, 0);
	}

	public static void DrawPreset_Normal(BgSlide bg)
	{
		Vector2 screenCenter = Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight) * 0.5f;
		Vector2 move = (screenCenter - bg.WorldAnchor) / bg.Distance;
		Vector2 texcoord_Move = move / bg.Texture.Size();

		int[] dirs = new int[] { 0, 1, -1, 2 };
		var bars = new List<Vertex2D>();
		for (int k = 0; k < 4; k++)
		{
			Vector2 dir = new Vector2(1, 1).RotatedBy(MathHelper.PiOver2 * dirs[k]);
			Vector2 screenPoint = new Vector2(-Main.screenWidth, -Main.screenHeight) * dir * 0.5f;
			Vector2 screen_move = screenPoint / bg.Texture.Size() + texcoord_Move;
			Color drawColor = GetColor(bg, screenCenter + screenPoint);
			bars.Add(screenCenter + screenPoint - Main.screenPosition, drawColor, new Vector3(screen_move + new Vector2(0.5f), 0));
		}
		DrawVertexBackground(bg, PrimitiveType.TriangleStrip, bars);
	}

	public static void Add_WorldPosVertex(BgSlide bg, Vector2 position, List<Vertex2D> bars)
	{
		Vector2 screenCenter = Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight) * 0.5f;
		Vector2 move = (screenCenter - bg.WorldAnchor) / bg.Distance;
		Vector2 texcoord_Move = move / bg.Texture.Size();
		Vector2 screenPoint = position - screenCenter;
		Vector2 screen_move = screenPoint / bg.Texture.Size() + texcoord_Move;

		Color drawColor = GetColor(bg, position);
		bars.Add(position - Main.screenPosition, drawColor, new Vector3(screen_move + new Vector2(0.5f), 0));
	}

	public static void Add_WorldTriangle(BgSlide bg, Vector2 v0, Vector2 v1, Vector2 v2, List<Vertex2D> bars)
	{
		Add_WorldPosVertex(bg, v0, bars);
		Add_WorldPosVertex(bg, v1, bars);
		Add_WorldPosVertex(bg, v2, bars);
	}

	public static void Add_TileBgVertice(BgSlide bg, List<Point> tiles, List<Vertex2D> bars)
	{
		foreach (var pos in tiles)
		{
			Vector2 worldTilePos = pos.ToWorldCoordinates();
			if (VFXManager.InScreen(worldTilePos, 64))
			{
				int[] dirs = new int[] { 0, 1, -1, -1, 1, 2 };
				for (int k = 0; k < dirs.Length; k++)
				{
					Add_WorldPosVertex(bg, worldTilePos + new Vector2(-24).RotatedBy(dirs[k] * MathHelper.PiOver2) + new Vector2(24), bars);
				}
			}
		}
	}

	public static void DrawVertexBackground(BgSlide bg, PrimitiveType primitiveType, List<Vertex2D> bars)
	{
		if (bars.Count > 2)
		{
			Main.graphics.graphicsDevice.Textures[0] = bg.Texture;
			if (primitiveType == PrimitiveType.TriangleList)
			{
				Main.graphics.graphicsDevice.DrawUserPrimitives(primitiveType, bars.ToArray(), 0, bars.Count / 3);
			}
			if (primitiveType == PrimitiveType.TriangleStrip)
			{
				Main.graphics.graphicsDevice.DrawUserPrimitives(primitiveType, bars.ToArray(), 0, bars.Count - 2);
			}
		}
	}

	public static Effect XClamp_YClamp_Shader = ModAsset.BgShader_X_Clamp_Y_Clamp.Value;

	public static Effect XWrap_YClamp_Shader = ModAsset.BgShader_X_Wrap_Y_Clamp.Value;

	public static Effect XClamp_YWrap_Shader = ModAsset.BgShader_X_Clamp_Y_Wrap.Value;

	public static Effect XWrap_YWrap_Shader = ModAsset.BgShader_X_Wrap_Y_Wrap.Value;
}