using Everglow.Sources.Commons.Core;
using Everglow.Sources.Commons.Core.ModuleSystem;
using Everglow.Sources.Commons.Core.Network.PacketHandle;
using Everglow.Sources.Commons.Core.Profiler.Fody;
using ReLogic.Content;
using Terraria.GameContent.UI.Elements;
using Terraria.IO;
using Terraria.Social;
using Terraria.UI;

namespace Everglow.Sources.Modules.ZY.WorldSystem
{
    [ProfilerMeasure]
    internal class WorldSystem : IModule
    {
        public Dictionary<WorldFileData, World> dataToWorld = new Dictionary<WorldFileData, World>();
        public Dictionary<WorldFileData, UIWorldListItem> dataToUI = new Dictionary<WorldFileData, UIWorldListItem>();
        public List<(World world, string displayName, string fileName, int gameMode, string seed)> worlds = new List<(World world, string displayName, string fileName, int gameMode, string seed)>();
        public string Name => "WorldSystem";
        public static World CurrentWorld
        {
            get; internal set;
        }
        public void Load()
        {
            On.Terraria.Main.LoadWorlds += Main_LoadWorlds;
            On.Terraria.GameContent.UI.Elements.UIWorldListItem.ctor += UIWorldListItem_ctor;

            ModContent.GetInstance<HookSystem>().AddMethod(() =>
              {
                  dataToWorld[Main.ActiveWorldFileData].EnterWorld_Server();
              }, CallOpportunity.PostEnterWorld_Server);

            ModContent.GetInstance<HookSystem>().AddMethod(() =>
              {
                  if (Main.netMode != NetmodeID.MultiplayerClient)
                  {
                      CurrentWorld = World.CreateInstance(World.GetWorldName(Main.ActiveWorldFileData.WorldGeneratorVersion));
                  }
                  else
                  {
                      var pack = Everglow.Instance.GetPacket(1);
                      pack.Write((byte)0);
                      pack.Send();
                  }
              }, CallOpportunity.PostEnterWorld_Single);

            //TestHook
            //ModContent.GetInstance<HookSystem>().AddMethod(() =>
            //{
            //    if (Main.netMode == NetmodeID.MultiplayerClient)
            //    {
            //        Main.NewText(CurrentWorld.Name);
            //    }
            //}, CallOpportunity.PostUpdateEverything);

        }
        public void Unload()
        {
            CurrentWorld = null;
        }
        /// <summary>
        /// 添加虚假的世界选择
        /// </summary>
        /// <param name="world"></param>
        /// <param name="displayName"></param>
        /// <param name="fileName"></param>
        /// <param name="gameMode"></param>
        /// <param name="seed"></param>
        public void AddWorld(World world, string displayName, string fileName, int gameMode, string seed) => worlds.Add((world, displayName, fileName, gameMode, seed));
        private void UIWorldListItem_ctor(On.Terraria.GameContent.UI.Elements.UIWorldListItem.orig_ctor orig, UIWorldListItem self, WorldFileData data, int orderInList, bool canBePlayed)
        {
            if (dataToWorld.ContainsKey(data))
            {
                dataToUI[data] = self;
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
                //构造
                fName["Elements"].SetValue(self, new List<UIElement>());
                fName["BackgroundColor"].SetValue(self, new Color(63, 82, 151) * 0.7f);
                fName["BorderColor"].SetValue(self, Color.Black);
                fName["_cornerSize"].SetValue(self, 12);
                fName["_barSize"].SetValue(self, 4);
                self.MaxWidth = StyleDimension.Fill;
                self.MaxHeight = StyleDimension.Fill;
                self.MinWidth = StyleDimension.Empty;
                self.MinHeight = StyleDimension.Empty;
                self.SetPadding(12);
                fName["_needsTextureLoading"].SetValue(self, true);

                var info = typeof(UIElement).GetField("_idCounter", BindingFlags.NonPublic | BindingFlags.Static);
                int count = (int)info.GetValue(null);
                typeof(UIElement).GetProperty("UniqueId")
                    .GetSetMethod(true)
                    .Invoke(self, new object[] { count });
                info.SetValue(null, count + 1);

                if (File.Exists(data.Path))
                {
                    fName["_fileSize"].SetValue(self, (ulong)(long)Terraria.Utilities.FileUtilities.GetFileSize(data.Path, data.IsCloudSave));
                }
                else
                {
                    fName["_fileSize"].SetValue(self, 0ul);
                }
                fName["_orderInList"].SetValue(self, orderInList);
                fName["_data"].SetValue(self, data);
                fName["_canBePlayed"].SetValue(self, canBePlayed);
                type.GetMethod("LoadTextures", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, null);
                type.GetMethod("InitializeAppearance", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, null);

                //图标UI
                var _worldIcon = new UIImage(dataToWorld[data].WorldIcon);
                _worldIcon.Left.Set(4f, 0f);
                fName["_worldIcon"].SetValue(self, _worldIcon);
                _worldIcon.OnDoubleClick += dataToWorld[data].EnterWorld;
                _worldIcon.Recalculate();
                self.Append(_worldIcon);

                //进入世界UI
                float num = 4f;
                UIImageButton uIImageButton = new UIImageButton((Asset<Texture2D>)fName["_buttonPlayTexture"].GetValue(self))
                {
                    VAlign = 1f
                };
                uIImageButton.Left.Set(num, 0f);
                uIImageButton.OnClick += dataToWorld[data].EnterWorld;
                fName["OnDoubleClick"].SetValue(self, (UIElement.MouseEvent)dataToWorld[data].EnterWorld);
                uIImageButton.OnMouseOut += mName["ButtonMouseOut"].CreateDelegate<UIElement.MouseEvent>(self);
                uIImageButton.OnMouseOver += mName["PlayMouseOver"].CreateDelegate<UIElement.MouseEvent>(self);
                uIImageButton.OnMouseOut += mName["ButtonMouseOut"].CreateDelegate<UIElement.MouseEvent>(self);
                self.Append(uIImageButton);
                num += 24f;


                //收藏UI
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


                if (SocialAPI.Cloud != null)
                {
                    UIImageButton uIImageButton3 = new UIImageButton(data.IsCloudSave ?
                        (Asset<Texture2D>)fName["_buttonCloudActiveTexture"].GetValue(self) :
                        (Asset<Texture2D>)fName["_buttonCloudInactiveTexture"].GetValue(self))
                    {
                        VAlign = 1f
                    };
                    uIImageButton3.Left.Set(num, 0f);
                    uIImageButton3.OnClick += mName["CloudButtonClick"].CreateDelegate<UIElement.MouseEvent>(self);
                    uIImageButton3.OnMouseOver += mName["CloudMouseOver"].CreateDelegate<UIElement.MouseEvent>(self);
                    uIImageButton3.OnMouseOut += mName["ButtonMouseOut"].CreateDelegate<UIElement.MouseEvent>(self);
                    uIImageButton3.SetSnapPoint("Cloud", orderInList, null, null);
                    self.Append(uIImageButton3);
                    num += 24f;
                }
                if (data.WorldGeneratorVersion != 0UL)
                {
                    UIImageButton uIImageButton4 = new UIImageButton((Asset<Texture2D>)fName["_buttonSeedTexture"].GetValue(self))
                    {
                        VAlign = 1f
                    };
                    uIImageButton4.Left.Set(num, 0f);
                    uIImageButton4.OnClick += mName["SeedButtonClick"].CreateDelegate<UIElement.MouseEvent>(self);
                    uIImageButton4.OnMouseOver += mName["SeedMouseOver"].CreateDelegate<UIElement.MouseEvent>(self);
                    uIImageButton4.OnMouseOut += mName["ButtonMouseOut"].CreateDelegate<UIElement.MouseEvent>(self);
                    uIImageButton4.SetSnapPoint("Seed", orderInList, null, null);
                    self.Append(uIImageButton4);
                    num += 24f;
                }

                //重命名UI
                UIImageButton uIImageButton5 = new UIImageButton((Asset<Texture2D>)fName["_buttonRenameTexture"].GetValue(self))
                {
                    VAlign = 1f
                };
                uIImageButton5.Left.Set(num, 0f);
                uIImageButton5.OnClick += mName["RenameButtonClick"].CreateDelegate<UIElement.MouseEvent>(self);
                uIImageButton5.OnMouseOver += mName["RenameMouseOver"].CreateDelegate<UIElement.MouseEvent>(self);
                uIImageButton5.OnMouseOut += mName["ButtonMouseOut"].CreateDelegate<UIElement.MouseEvent>(self);
                uIImageButton5.SetSnapPoint("Rename", orderInList, null, null);
                self.Append(uIImageButton5);
                num += 24f;

                //删除UI
                UIImageButton uIImageButton6 = new UIImageButton((Asset<Texture2D>)fName["_buttonDeleteTexture"].GetValue(self))
                {
                    VAlign = 1f,
                    HAlign = 1f
                };
                if (!data.IsFavorite)
                {
                    uIImageButton6.OnClick += mName["DeleteButtonClick"].CreateDelegate<UIElement.MouseEvent>(self);
                }
                uIImageButton6.OnMouseOver += mName["DeleteMouseOver"].CreateDelegate<UIElement.MouseEvent>(self);
                uIImageButton6.OnMouseOut += mName["DeleteMouseOut"].CreateDelegate<UIElement.MouseEvent>(self);
                fName["_deleteButton"].SetValue(self, uIImageButton6);
                self.Append(uIImageButton6);
                num += 4f;

                //文本
                var _buttonLabel = new UIText("", 1f, false)
                {
                    VAlign = 1f
                };
                _buttonLabel.Left.Set(num, 0f);
                _buttonLabel.Top.Set(-3f, 0f);
                self.Append(_buttonLabel);
                fName["_buttonLabel"].SetValue(self, _buttonLabel);

                var _deleteButtonLabel = new UIText("", 1f, false)
                {
                    VAlign = 1f,
                    HAlign = 1f
                };
                _deleteButtonLabel.Left.Set(-30f, 0f);
                _deleteButtonLabel.Top.Set(-3f, 0f);
                self.Append(_deleteButtonLabel);
                fName["_deleteButtonLabel"].SetValue(self, _deleteButtonLabel);

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
            orig();
            List<WorldFileData> datas = Main.WorldList;
            foreach (WorldFileData d in datas)
            {
                var name = World.GetWorldName(d.WorldGeneratorVersion);
                var world = World.CreateInstance(name);
                dataToWorld.Add(d, world);
                world.data = d;
            }

            foreach (var (world, displayName, fileName, gameMode, seed) in worlds)
            {
                var data = world.CreateMetaData(displayName, fileName, gameMode, seed);
                datas.Add(data);
                dataToWorld.Add(data, world);
            }
        }

    }

    internal class WorldVersionPacket : IPacket
    {
        public ulong version;
        public void Receive(BinaryReader reader, int whoAmI)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                version = reader.ReadUInt64();
            }
        }

        public void Send(BinaryWriter writer)
        {
            //如果是服务器，就发送version
            //如果是客户端，只利用whoAmI
            if (Main.netMode == NetmodeID.Server)
            {
                writer.Write(Main.ActiveWorldFileData.WorldGeneratorVersion);
            }
        }
    }

    [HandlePacket(typeof(WorldVersionPacket))]
    internal class WorldVersionPacketHandler : IPacketHandler
    {
        public void Handle(IPacket packet, int whoAmI)
        {
            //客户端收到Packet后根据version生成World
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                WorldVersionPacket pack = packet as WorldVersionPacket;
                WorldSystem.CurrentWorld = World.CreateInstance(World.GetWorldName(pack.version));
            }
            //服务端收到Packet后会返回一个同样的Packet
            else if (Main.netMode == NetmodeID.Server)
            {
                //直接复用当前的Packet
                Everglow.PacketResolver.Send(new WorldVersionPacket(), whoAmI);
            }
        }
    }
}
