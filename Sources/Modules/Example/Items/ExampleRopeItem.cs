using Everglow.Commons.DataStructures;
using Everglow.Commons.Physics.MassSpringSystem;
using Everglow.Commons.Utilities;

namespace Everglow.Example.Items;

public class ExampleRopeItem : ModItem
{
	public Rope ItemRope;

	public override void HoldItem(Player player)
	{
		if (ItemRope is null)
		{
			AddRope();
		}
		ItemRope.Masses[0].Position = Main.MouseWorld;
		ItemRope.ApplyForce_VelocityDecay();
	}

	public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
	{
		SpriteBatchState sBS = GraphicsUtils.GetState(spriteBatch).Value;
		spriteBatch.End();
		spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		if (ItemRope is null)
		{
			AddRope();
		}
		Texture2D tex = Commons.ModAsset.TileBlock.Value;
		foreach (var mass in ItemRope.Masses)
		{
			spriteBatch.Draw(tex, mass.Position - Main.screenPosition, null, Color.White, 0, tex.Size() * 0.5f, 0.5f, SpriteEffects.None, 0);
		}
		spriteBatch.End();
		spriteBatch.Begin(sBS);
		base.PostDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
	}

	public void AddRope()
	{
		ItemRope = Rope.Create_Fixed_StartPos(Main.MouseWorld, 20, 5, 0.2f);
		GlobalRopeManager.EularRopeSystem.AddMassSpringMesh(ItemRope);
	}
}