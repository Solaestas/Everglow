using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.ModLoader;
using Everglow.TwilightForest.Tiles;
using Everglow.Myth.TheFirefly.Water;
using System.Threading.Tasks;
using Terraria.Graphics.Effects;
using Everglow.TwilightForest.Common;

namespace Everglow.TwilightForest.Biomes
{
	public class TwilightForestSurfaceBiome : ModBiome
	{
		public override ModWaterStyle WaterStyle => ModContent.GetInstance<TwilightForestWaterStyle>();
		public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.GetInstance<TwilightForestSurfaceBackgroundStyle>();
		public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Crimson;
		public override int Music => TwilightForestContent.QuickMusic("PlaceholderTwilightForestBGM");// This is a placeholder song //TODO:Make official BGM
		public override SceneEffectPriority Priority => SceneEffectPriority.BiomeLow;
		public override string BestiaryIcon => base.BestiaryIcon;
		public override string BackgroundPath => base.BackgroundPath;
		public override Color? BackgroundColor => base.BackgroundColor;
		public override string MapBackground => BackgroundPath;

		public override bool IsBiomeActive(Player player)
		{
			bool EnoughTwilightGrass = ModContent.GetInstance<TwilightForestBiomeTileCount>().TwilightGrassCount >= 40;
			bool ZoneSurface = player.ZoneSkyHeight || player.ZoneOverworldHeight;
			return EnoughTwilightGrass && ZoneSurface;
		}
	}
	public class TwilightForestBiomeTileCount : ModSystem
	{
		public int TwilightGrassCount;
		public float InsertBiomeValue = 0;
		public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
		{
			TwilightGrassCount = tileCounts[ModContent.TileType<TwilightGrassBlock>()];
		}
		public override void ModifySunLightColor(ref Color tileColor, ref Color backgroundColor)
		{
			if (ModContent.GetInstance<TwilightForestSurfaceBiome>().IsBiomeActive(Main.LocalPlayer))
			{
				if (InsertBiomeValue < 1)
				{
					InsertBiomeValue += 0.01f;
				}
				else
				{
					InsertBiomeValue = 1f;
				}
			}
			else
			{
				if (InsertBiomeValue > 0)
				{
					InsertBiomeValue -= 0.01f;
				}
				else
				{
					InsertBiomeValue = 0;
				}
			}
			tileColor *= 1 + InsertBiomeValue * 2.2f;
			tileColor.R = (byte)(tileColor.R * (1 - InsertBiomeValue * 0.43f));
			tileColor.G = (byte)(tileColor.G * (1 - InsertBiomeValue * 0.62f));
		}
	}
}
