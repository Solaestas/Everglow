using Everglow.Sources.Modules.YggdrasilModule.CorruptWormHive.VFXs;
using Everglow.Sources.Commons.Core.VFX;
using Terraria.DataStructures;
using Terraria.ObjectData;
using Everglow.Sources.Modules.YggdrasilModule.Common;
using System.IO;

namespace Everglow.Sources.Modules.YggdrasilModule.CorruptWormHive.Tiles
{
    public class BloodLightCrystal : ModTile
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<BloodLightCrystalEntity>().Hook_AfterPlacement, -1, 0, false);
            TileObjectData.addTile(Type);
        }
        public override void PostSetDefaults()
        {
            Main.tileSolid[Type] = true;
            MinPick = 375;
            ItemDrop = ModContent.ItemType<Items.BloodLightCrystal>();

            AddMapEntry(new Color(107, 34, 21, 205));
        }
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
        public override bool CreateDust(int i, int j, ref int type)
        {
            return false;
        }
        public override void NearbyEffects(int i, int j, bool closer)
        {
            Color c0 = Lighting.GetColor(i, j);

            if(c0.R > 160 && c0.G + c0.B < 80)
            {
                if (TileEntity.ByPosition.TryGetValue(new Point16(i, j), out TileEntity existing) 
                    && existing is BloodLightCrystalEntity existingAsT)
                {
                    existingAsT.startDissolve();
                }
                
                //WorldGen.KillTile(i, j,false,false,true);
            }
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            if (TileEntity.ByPosition.TryGetValue(new Point16(i, j), out TileEntity existing)
                   && existing is BloodLightCrystalEntity existingAsT)
            {
                float dissolveProgress = existingAsT.getDissolveProgress();

                if (dissolveProgress > 0) {
                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

                    Vector2 worldPos = new Vector2(i, j) * 16 + new Vector2(Main.offScreenRange);
                    Texture2D tex = ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
                    Texture2D noiseTex = YggdrasilContent.QuickTexture("Effects/DissolveNoise");
                    Main.graphics.GraphicsDevice.Textures[1] = noiseTex;
                    Effect dissolveEffect = YggdrasilContent.QuickEffect("Effects/NoiseDissolve");
                    dissolveEffect.Parameters["worldPos"].SetValue(worldPos);
                    dissolveEffect.Parameters["textureWidth"].SetValue(400);
                    dissolveEffect.Parameters["progress"].SetValue(dissolveProgress);
                    dissolveEffect.CurrentTechnique.Passes["NoiseDissolve"].Apply();
                    
                    Tile tile = Main.tile[i,j];
                    
                    spriteBatch.Draw(tex, worldPos-Main.screenPosition, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16), Lighting.GetColor(i, j));
                    spriteBatch.End();
                    spriteBatch.Begin();
                    return false;
                }
            }
            return base.PreDraw(i, j, spriteBatch);
        }
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            BrokenCrystal bc = new BrokenCrystal
            {
                timeLeft = 70,
                size = Main.rand.NextFloat(0.65f, 1f),
                velocity = new Vector2(Main.rand.NextFloat(2.5f, 7.5f), 0).RotatedByRandom(6.283),
                Active = true,
                Visible = true,
                position = new Vector2(i * 16 + 8, j * 16 + 8)
            };

            if (Main.rand.NextBool(4))
            {
                Dust d = Dust.NewDustDirect(new Vector2(i * 16, j * 16), 16, 16, ModContent.DustType<Dusts.BloodSpark>());
                d.velocity = new Vector2(Main.rand.NextFloat(1.5f, 4f), 0).RotatedByRandom(6.283);

                d.color.R = (byte)Main.rand.Next(15, 255);
                d.alpha = Main.rand.Next(10);
                d.noGravity = true;

                VFXManager.Add(bc);
            }
        }
    }
}
