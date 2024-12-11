namespace Everglow.Yggdrasil.YggdrasilTown.Items.Tools;

public class OldPortableLamp : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 16;
		Item.height = 16;
		Item.scale = 0.6f;

		Item.holdStyle = ItemHoldStyleID.HoldLamp;

		Item.value = Item.buyPrice(platinum: 0, gold: 1, silver: 23);
		Item.rare = ItemRarityID.Green;
	}

	public override void HoldItem(Player player)
	{
		// Light flickering while player is moving, the variation intensity is proportional to player's velocity
		var lightStrengthBase = 0.8f;
		var offset = player.velocity.Length().SmoothStep(-3, 8) * 0.1f;
		var lightStrength = Main.rand.NextFloat(lightStrengthBase - offset, lightStrengthBase + offset);

		var lightColor = new Vector3(0.788f, 0.553f, 0.149f);

		Lighting.AddLight(player.Center, lightColor * lightStrength);
	}

	public override Vector2? HoldoutOffset() => new Vector2(0, 0);

	public override Vector2? HoldoutOrigin() => new Vector2(0, 0);
}