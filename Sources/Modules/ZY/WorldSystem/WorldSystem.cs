using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.IO;
using Terraria.ModLoader.IO;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using ZYMod.WorldSystem;
using Everglow.Sources.Commons.Core;
using Everglow.Sources.Commons.ModuleSystem;

namespace Everglow.Sources.Modules.ZY.WorldSystem
{
    internal class WorldSystem : IModule
    {
        public static World CurrentWorld { get; internal set; }
        public void Load()
        {
            On.Terraria.Main.LoadWorlds += Main_LoadWorlds;
            On.Terraria.GameContent.UI.Elements.UIWorldListItem.ctor += UIWorldListItem_ctor;
        }
        private void UIWorldListItem_ctor(On.Terraria.GameContent.UI.Elements.UIWorldListItem.orig_ctor orig, UIWorldListItem self, WorldFileData data, int orderInList, bool canBePlayed)
        {
            if (World.GetWorldName(data) != "Terraria")
            {
                uiDats[self] = data;
                var type = self.GetType();
                var Fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).Concat(
                    typeof(UIPanel).GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).Concat(
                    typeof(UIElement).GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)));
                var Methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                var fName = new Dictionary<string, FieldInfo>();
                var mName = new Dictionary<string, MethodInfo>();
                foreach (var field in Fields)
                {
                    fName[field.Name] = field;
                }
                foreach (var method in Methods)
                {
                    mName[method.Name] = method;
                }
                fName["Elements"].SetValue(self, new List<UIElement>());
                fName["BackgroundColor"].SetValue(self, new Color(63, 82, 151) * 0.7f);
                fName["BorderColor"].SetValue(self, Color.Black);
                fName["_cornerSize"].SetValue(self, 12);
                fName["_barSize"].SetValue(self, 4);
                self.MaxWidth = StyleDimension.Fill;
                self.MaxHeight = StyleDimension.Fill;
                self.MinWidth = StyleDimension.Empty;
                self.MinHeight = StyleDimension.Empty;
                self.SetPadding(6);
                fName["_needsTextureLoading"].SetValue(self, true);
                fName["_fileSize"].SetValue(self, 0ul);
                fName["_orderInList"].SetValue(self, orderInList);
                //this._orderInList = orderInList;
                fName["_data"].SetValue(self, data);
                //this._data = data;
                //this._fileSize = (ulong)((long)FileUtilities.GetFileSize(data.Path, data.IsCloudSave));
                fName["_canBePlayed"].SetValue(self, canBePlayed);
                //this._canBePlayed = canBePlayed;
                type.GetMethod("LoadTextures", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, null);
                //this.LoadTextures();
                type.GetMethod("InitializeAppearance", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, null);
                //this.InitializeAppearance();
                var _worldIcon = new UIImage(customDatas[data].WorldIcon);
                float num = 4f;
                _worldIcon.Left.Set(4f, 0f);
                //_worldIcon.OnDoubleClick += this.PlayGame;
                fName["_worldIcon"].SetValue(self, _worldIcon);
                self.Append(_worldIcon);

                UIImageButton uIImageButton = new UIImageButton((Asset<Texture2D>)fName["_buttonPlayTexture"].GetValue(self));
                //UIImageButton uIImageButton = new UIImageButton(this._buttonPlayTexture);
                uIImageButton.VAlign = 1f;
                uIImageButton.Left.Set(num, 0f);
                uIImageButton.OnClick += customDatas[data].EnterWorld;
                fName["OnDoubleClick"].SetValue(self, (UIElement.MouseEvent)customDatas[data].EnterWorld);
                //uIImageButton.OnMouseOver += mName["PlayMouseOver"].CreateDelegate<UIElement.MouseEvent>();
                uIImageButton.OnMouseOut += mName["ButtonMouseOut"].CreateDelegate<UIElement.MouseEvent>(self);
                self.Append(uIImageButton);
                num += 24f;



                //UIImageButton uIImageButton2 = new UIImageButton(data.IsFavorite ? this._buttonFavoriteActiveTexture : this._buttonFavoriteInactiveTexture);
                UIImageButton uIImageButton2 = new UIImageButton(data.IsFavorite ?
                    (Asset<Texture2D>)fName["_buttonFavoriteActiveTexture"].GetValue(self)
                    : (Asset<Texture2D>)fName["_buttonFavoriteInactiveTexture"].GetValue(self))
                {
                    VAlign = 1f
                };
                uIImageButton2.Left.Set(num, 0f);
                uIImageButton2.OnClick += mName["FavoriteButtonClick"].CreateDelegate<UIElement.MouseEvent>(self);
                uIImageButton2.OnMouseOver += mName["FavoriteMouseOver"].CreateDelegate<UIElement.MouseEvent>(self);
                uIImageButton2.OnMouseOut += mName["ButtonMouseOut"].CreateDelegate<UIElement.MouseEvent>(self);
                uIImageButton2.SetVisibility(1f, data.IsFavorite ? 0.8f : 0.4f);
                self.Append(uIImageButton2);
                num += 24f;


                //if (SocialAPI.Cloud != null)
                //{
                //    UIImageButton uIImageButton3 = new UIImageButton(this._data.IsCloudSave ? this._buttonCloudActiveTexture : this._buttonCloudInactiveTexture);
                //    uIImageButton3.VAlign = 1f;
                //    uIImageButton3.Left.Set(num, 0f);
                //    uIImageButton3.OnClick += this.CloudButtonClick;
                //    uIImageButton3.OnMouseOver += this.CloudMouseOver;
                //    uIImageButton3.OnMouseOut += this.ButtonMouseOut;
                //    uIImageButton3.SetSnapPoint("Cloud", orderInList, null, null);
                //    base.Append(uIImageButton3);
                //    num += 24f;
                //}
                //if (this._data.WorldGeneratorVersion != 0UL)
                //{
                //    UIImageButton uIImageButton4 = new UIImageButton(this._buttonSeedTexture);
                //    uIImageButton4.VAlign = 1f;
                //    uIImageButton4.Left.Set(num, 0f);
                //    uIImageButton4.OnClick += this.SeedButtonClick;
                //    uIImageButton4.OnMouseOver += this.SeedMouseOver;
                //    uIImageButton4.OnMouseOut += this.ButtonMouseOut;
                //    uIImageButton4.SetSnapPoint("Seed", orderInList, null, null);
                //    base.Append(uIImageButton4);
                //    num += 24f;
                //}
                //UIImageButton uIImageButton5 = new UIImageButton(this._buttonRenameTexture);
                UIImageButton uIImageButton5 = new UIImageButton((Asset<Texture2D>)fName["_buttonRenameTexture"].GetValue(self));
                uIImageButton5.VAlign = 1f;
                uIImageButton5.Left.Set(num, 0f);
                //uIImageButton5.OnClick += this.RenameButtonClick;
                //uIImageButton5.OnMouseOver += this.RenameMouseOver;
                //uIImageButton5.OnMouseOut += this.ButtonMouseOut;
                uIImageButton5.SetSnapPoint("Rename", orderInList, null, null);
                self.Append(uIImageButton5);
                num += 24f;
                //UIImageButton uIImageButton6 = new UIImageButton(this._buttonDeleteTexture)
                UIImageButton uIImageButton6 = new UIImageButton((Asset<Texture2D>)fName["_buttonDeleteTexture"].GetValue(self))
                {
                    VAlign = 1f,
                    HAlign = 1f
                };
                //if (!data.IsFavorite)
                //{
                //    uIImageButton6.OnClick += this.DeleteButtonClick;
                //}
                //uIImageButton6.OnMouseOver += this.DeleteMouseOver;
                //uIImageButton6.OnMouseOut += this.DeleteMouseOut;
                fName["_deleteButton"].SetValue(self, uIImageButton6);
                //this._deleteButton = uIImageButton6;
                self.Append(uIImageButton6);
                num += 4f;
                //this._buttonLabel = new UIText("", 1f, false);
                var _buttonLabel = new UIText("", 1f, false);
                _buttonLabel.VAlign = 1f;
                _buttonLabel.Left.Set(num, 0f);
                _buttonLabel.Top.Set(-3f, 0f);
                fName["_buttonLabel"].SetValue(self, _buttonLabel);
                self.Append(_buttonLabel);
                var _deleteButtonLabel = new UIText("", 1f, false);
                _deleteButtonLabel.VAlign = 1f;
                _deleteButtonLabel.HAlign = 1f;
                _deleteButtonLabel.Left.Set(-30f, 0f);
                _deleteButtonLabel.Top.Set(-3f, 0f);
                fName["_deleteButtonLabel"].SetValue(self, _buttonLabel);
                self.Append(_deleteButtonLabel);
                uIImageButton.SetSnapPoint("Play", orderInList, null, null);
                uIImageButton2.SetSnapPoint("Favorite", orderInList, null, null);
                uIImageButton5.SetSnapPoint("Rename", orderInList, null, null);
                uIImageButton6.SetSnapPoint("Delete", orderInList, null, null);

            }
            else
            {
                orig(self, data, orderInList, canBePlayed);
            }
        }

        private void Main_LoadWorlds(On.Terraria.Main.orig_LoadWorlds orig)
        {
            //var world = ZYContent.GetInstance<Elsewhere>();
            //var data = world.CreateMetaData("eee", 3, "dsa");
            //Main.ActiveWorldFileData = data;
            //Main.maxTilesX = data.WorldSizeX;
            //Main.maxTilesY = data.WorldSizeY;
            //Main.WorldFileMetadata = data.Metadata;
            //Main.worldName = Main.ActiveWorldFileData.Name;
            //WorldGen.clearWorld();
            ////WorldGen.GenerateWorld(0);
            //Main.spawnTileX = data.WorldSizeX / 2;
            //Main.spawnTileY = data.WorldSizeY / 2;
            //WorldFile.SaveWorld();
            orig();
            List<WorldFileData> datas = Main.WorldList;
            bool has = false;
            foreach (WorldFileData d in datas)
            {
                if (World.GetWorldName(d) != "Terraria")
                {
                    customDatas.Add(d, World.Worlds[d.WorldGeneratorVersion]);
                    World.Worlds[d.WorldGeneratorVersion].data = d;
                    has = true;
                }
            }

            if (!has)
            {
                World world = new Elsewhere();
                WorldFileData data = world.CreateMetaData("elsewhere", "elsewhere", 0, "");
                datas.Add(data);
                customDatas.Add(data, world);
                world.data = data;
            }
        }
        public void Unload()
        {

        }
        public string WorldType { get; set; } = "Vanilla";

        public string Name => "WorldSystem";

        public string Description => "WorldSystem";

        public Dictionary<WorldFileData, World> customDatas = new Dictionary<WorldFileData, World>();
        public Dictionary<UIWorldListItem, WorldFileData> uiDats = new Dictionary<UIWorldListItem, WorldFileData>();
    }
}
