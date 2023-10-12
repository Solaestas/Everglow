using System.Reflection;
using Everglow.Commons.Coroutines;
using ReLogic.Graphics;
using Terraria.GameContent;

namespace Everglow.Commons.BiomesText;

public class BiomeLable : ModSystem
{
	public Texture2D Lable => ModAsset.Textboard.Value;
	public int Width => 324;
	public int Height => 100;
	public int duration = -1;
	public int timeLag = 50;
	private CoroutineManager _coroutineManager = new CoroutineManager();
	public List<ModBiome> TotalBiomes;
	public Dictionary<ModBiome, int> ActiveModBiomeCache = new Dictionary<ModBiome, int>();
	public Dictionary<string, int> ActiveVanillaBiomeCache = new Dictionary<string, int>();
	public static Dictionary<string, BiomeData> VanillaBiomeDatas = new Dictionary<string, BiomeData>();
	public Queue<BiomeData> NewActiveBiomeQueue = new Queue<BiomeData>();
	public struct BiomeData
	{
		public string Name;
		public Texture2D Icon;
		public BiomeData(string name, Texture2D icon)
		{
			Name = name;
			Icon = icon;
		}
	}

	public override void PostSetupContent()
	{
		if (TotalBiomes is null)
		{
			TotalBiomes = (List<ModBiome>)typeof(BiomeLoader).GetField("list", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(LoaderManager.Get<BiomeLoader>());
		}
		base.PostSetupContent();
	}
	public override void OnWorldLoad()
	{
		foreach (ModBiome modBiome in TotalBiomes)
		{
			ActiveModBiomeCache[modBiome] = 0;
		}
		ActiveVanillaBiomeCache["Beach"] = 0;
		VanillaBiomeDatas["Beach"] = new BiomeData("Ocean", ModAsset.Ocean.Value);
		ActiveVanillaBiomeCache["Corrupt"] = 0;
		VanillaBiomeDatas["Corrupt"] = new BiomeData("The Corruption", ModAsset.The_Corruption.Value);
		ActiveVanillaBiomeCache["Crimson"] = 0;
		VanillaBiomeDatas["Crimson"] = new BiomeData("The Crimson", ModAsset.The_Crimson.Value);
		ActiveVanillaBiomeCache["Desert"] = 0;
		VanillaBiomeDatas["Desert"] = new BiomeData("Desert", ModAsset.Desert.Value);
		ActiveVanillaBiomeCache["DirtLayerHeight"] = 0;
		ActiveVanillaBiomeCache["Dungeon"] = 0;
		VanillaBiomeDatas["Dungeon"] = new BiomeData("Dungeon", ModAsset.Dungeon.Value);
		ActiveVanillaBiomeCache["Forest"] = 0;
		VanillaBiomeDatas["Forest"] = new BiomeData("Forest", ModAsset.Forest.Value);
		ActiveVanillaBiomeCache["GemCave"] = 0;
		ActiveVanillaBiomeCache["Glowshroom"] = 0;
		VanillaBiomeDatas["Glowshroom"] = new BiomeData("Glowshroom Land", ModAsset.Glowing_Mushroom_biome.Value);
		ActiveVanillaBiomeCache["Granite"] = 0;
		ActiveVanillaBiomeCache["Graveyard"] = 0;
		ActiveVanillaBiomeCache["Hallow"] = 0;
		VanillaBiomeDatas["Hallow"] = new BiomeData("The Hallow", ModAsset.The_Hallow.Value);
		ActiveVanillaBiomeCache["Jungle"] = 0;
		VanillaBiomeDatas["Jungle"] = new BiomeData("Jungle", ModAsset.Jungle.Value);
		ActiveVanillaBiomeCache["LihzhardTemple"] = 0;
		VanillaBiomeDatas["LihzhardTemple"] = new BiomeData("Lihzhard Temple", ModAsset.LihzhardTemple.Value);
		ActiveVanillaBiomeCache["Marble"] = 0;
		ActiveVanillaBiomeCache["Meteor"] = 0;
		ActiveVanillaBiomeCache["NormalCaverns"] = 0;
		VanillaBiomeDatas["NormalCaverns"] = new BiomeData("Cavern", ModAsset.Cavern.Value);
		ActiveVanillaBiomeCache["NormalSpace"] = 0;
		VanillaBiomeDatas["NormalSpace"] = new BiomeData("Space", ModAsset.Space.Value);
		ActiveVanillaBiomeCache["NormalUnderground"] = 0;
		ActiveVanillaBiomeCache["OldOneArmy"] = 0;
		ActiveVanillaBiomeCache["OverworldHeight"] = 0;
		ActiveVanillaBiomeCache["PeaceCandle"] = 0;
		ActiveVanillaBiomeCache["Purity"] = 0;
		ActiveVanillaBiomeCache["Rain"] = 0;
		ActiveVanillaBiomeCache["RockLayerHeight"] = 0;
		ActiveVanillaBiomeCache["Sandstorm"] = 0;
		ActiveVanillaBiomeCache["ShadowCandle"] = 0;
		ActiveVanillaBiomeCache["Shimmer"] = 0;
		ActiveVanillaBiomeCache["SkyHeight"] = 0;
		ActiveVanillaBiomeCache["Snow"] = 0;
		VanillaBiomeDatas["Snow"] = new BiomeData("Snowland", ModAsset.Snowland.Value);
		ActiveVanillaBiomeCache["TowerNebula"] = 0;
		ActiveVanillaBiomeCache["TowerSolar"] = 0;
		ActiveVanillaBiomeCache["TowerStardust"] = 0;
		ActiveVanillaBiomeCache["TowerVortex"] = 0;
		ActiveVanillaBiomeCache["UndergroundDesert"] = 0;
		VanillaBiomeDatas["UndergroundDesert"] = new BiomeData("Underground Desert", ModAsset.Underground_Desert.Value);
		ActiveVanillaBiomeCache["UnderworldHeight"] = 0;
		VanillaBiomeDatas["UnderworldHeight"] = new BiomeData("Underground World", ModAsset.The_Underworld.Value);
		ActiveVanillaBiomeCache["WaterCandle"] = 0;
		base.OnWorldLoad();
	}
	public override void PostDrawInterface(SpriteBatch spriteBatch)
	{
		Player player = Main.LocalPlayer;
		foreach (ModBiome modBiome in TotalBiomes)
		{
			if (modBiome.IsBiomeActive(player))
			{
				if (ActiveModBiomeCache[modBiome] < timeLag)
				{
					ActiveModBiomeCache[modBiome]++;
					if(ActiveModBiomeCache[modBiome] == timeLag)
					{
						NewActiveBiomeQueue.Enqueue(new BiomeData(modBiome.Name, ModContent.Request<Texture2D>(modBiome.BestiaryIcon, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value));
					}			
				}
			}
			else
			{
				if (ActiveModBiomeCache[modBiome] > 0)
				{
					ActiveModBiomeCache[modBiome] = 0;
				}
			}
		}
		CheckNewActiveVanillaBiome("Beach", player.ZoneBeach);
		CheckNewActiveVanillaBiome("Corrupt", player.ZoneCorrupt);
		CheckNewActiveVanillaBiome("Crimson", player.ZoneCrimson);
		CheckNewActiveVanillaBiome("Desert", player.ZoneDesert);
		CheckNewActiveVanillaBiome("Dungeon", player.ZoneDungeon);
		CheckNewActiveVanillaBiome("Forest", player.ZoneForest);
		CheckNewActiveVanillaBiome("Glowshroom", player.ZoneGlowshroom);
		CheckNewActiveVanillaBiome("Hallow", player.ZoneHallow);
		CheckNewActiveVanillaBiome("Jungle", player.ZoneJungle);
		CheckNewActiveVanillaBiome("LihzhardTemple", player.ZoneLihzhardTemple);
		CheckNewActiveVanillaBiome("NormalCaverns", player.ZoneNormalCaverns);
		CheckNewActiveVanillaBiome("NormalSpace", player.ZoneNormalSpace);
		CheckNewActiveVanillaBiome("Snow", player.ZoneSnow);
		CheckNewActiveVanillaBiome("UndergroundDesert", player.ZoneUndergroundDesert);
		CheckNewActiveVanillaBiome("UnderworldHeight", player.ZoneUnderworldHeight);
		if (NewActiveBiomeQueue.Count > 2)
		{
			NewActiveBiomeQueue.Dequeue();
		}
		if (duration == -1)
		{
			if (NewActiveBiomeQueue.Count > 0)
			{
				_coroutineManager.StartCoroutine(new Coroutine(DrawNewBiome(spriteBatch, NewActiveBiomeQueue.Dequeue())));
			}
		}
		_coroutineManager.Update();
		base.PostDrawInterface(spriteBatch);
	}
	private void CheckNewActiveVanillaBiome(string name, bool value)
	{
		if (value)
		{
			if (ActiveVanillaBiomeCache[name] < timeLag)
			{
				ActiveVanillaBiomeCache[name]++;
				if(ActiveVanillaBiomeCache[name] == timeLag)
				{
					NewActiveBiomeQueue.Enqueue(VanillaBiomeDatas[name]);
				}
			}
		}
		else
		{
			if (ActiveVanillaBiomeCache[name] > 0)
			{
				ActiveVanillaBiomeCache[name] = 0;
			}
		}
	}
	/// <summary>
	/// 落地
	/// </summary>
	/// <returns></returns>
	private IEnumerator<ICoroutineInstruction> DrawNewBiome(SpriteBatch spriteBatch, BiomeData biomedata)
	{
		Texture2D biomeIcon = biomedata.Icon;
		for (int i = 0; i < 148; i++)
		{
			duration = i;
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.EffectMatrix);
			spriteBatch.Draw(Lable, new Vector2(Main.screenWidth / 2f, 200), new Rectangle(0, Height * (i / 2), Width, Height), Color.White, 0, new Vector2(Width, Height) * 0.5f, 1f, SpriteEffects.None, 0);
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.EffectMatrix);
			Effect dissolve = ModAsset.Dissolve_BiomeLable.Value;
			var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
			var model = Matrix.CreateTranslation(new Vector3(0, 0, 0)) * Main.GameViewMatrix.EffectMatrix;
			float dissolveDuration = (1f - i / 148f) * 2;
			dissolve.Parameters["uTransform"].SetValue(model * projection);
			dissolve.Parameters["uNoise"].SetValue(ModAsset.Noise_spiderNet.Value);
			dissolve.Parameters["duration"].SetValue(dissolveDuration);
			dissolve.Parameters["uDissolveColor"].SetValue(new Vector4(1, 1f, 1f, 1f));
			dissolve.Parameters["uNoiseSize"].SetValue(4f);
			dissolve.Parameters["uNoiseXY"].SetValue(new Vector2(0.4f, 0.4f));
			dissolve.CurrentTechnique.Passes[0].Apply();
			spriteBatch.DrawString(FontAssets.MouseText.Value, biomedata.Name, new Vector2(Main.screenWidth / 2f - 10, 180), Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
			spriteBatch.Draw(biomeIcon, new Vector2(Main.screenWidth / 2f - 55, 196), null, Color.White, 0, biomeIcon.Size() * 0.5f, 1f, SpriteEffects.None, 0);
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.EffectMatrix);
			if (i == 147)
			{
				duration = -1;
			}
			else
			{
				if (Main.gamePaused)
				{
					i -= 1;
				}
			}
			yield return new SkipThisFrame();
		}
	}
}
