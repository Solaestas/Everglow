using Everglow.Sources.Commons.Core.ModuleSystem;
using MonoMod.Cil;
using MonoMod.RuntimeDetour.HookGen;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;
using Terraria.IO;
using Terraria.Localization;
using Terraria.Net.Sockets;
using Terraria.Social.Steam;
using Terraria.Utilities;
using static Mono.Cecil.Cil.OpCodes;
using Terraria.Graphics.Light;

namespace Everglow.Sources.Modules.SubWorldModule
{
    internal class SubworldLibrary : IModule
    {
        const string ReturnNow = $"Mods.{nameof(Everglow)}.{nameof(SubworldLibrary)}.{nameof(ReturnNow)}";
        const string ReturnAll = $"Mods.{nameof(Everglow)}.{nameof(SubworldLibrary)}.{nameof(ReturnAll)}";
        public string Name => nameof(SubworldLibrary);
        public void Unload()
        {
            Hooks.Unload();
        }
        public void Load()
        {
            PrepareModtransltion();
            Hooks.Load();
        }
        static void PrepareModtransltion()
        {
            ModTranslation ReturnNow_Translation = LocalizationLoader.GetOrCreateTranslation(ReturnNow);
            ModTranslation ReturnAll_Translation = LocalizationLoader.GetOrCreateTranslation(ReturnAll);
            ReturnNow_Translation.AddTranslation(GameCulture.FromCultureName(GameCulture.CultureName.English),
                "");
            ReturnNow_Translation.AddTranslation(GameCulture.FromCultureName(GameCulture.CultureName.German),
                "");
            ReturnNow_Translation.AddTranslation(GameCulture.FromCultureName(GameCulture.CultureName.Italian),
                "");
            ReturnNow_Translation.AddTranslation(GameCulture.FromCultureName(GameCulture.CultureName.French),
                "");
            ReturnNow_Translation.AddTranslation(GameCulture.FromCultureName(GameCulture.CultureName.Spanish),
                "");
            ReturnNow_Translation.AddTranslation(GameCulture.FromCultureName(GameCulture.CultureName.Russian),
                "");
            ReturnNow_Translation.AddTranslation(GameCulture.FromCultureName(GameCulture.CultureName.Chinese),
                "退出当前(暂不可用)");
            ReturnNow_Translation.AddTranslation(GameCulture.FromCultureName(GameCulture.CultureName.Portuguese),
                "");
            ReturnNow_Translation.AddTranslation(GameCulture.FromCultureName(GameCulture.CultureName.Polish),
                "");

            ReturnAll_Translation.AddTranslation(GameCulture.FromCultureName(GameCulture.CultureName.English),
                "");
            ReturnAll_Translation.AddTranslation(GameCulture.FromCultureName(GameCulture.CultureName.German),
                "");
            ReturnAll_Translation.AddTranslation(GameCulture.FromCultureName(GameCulture.CultureName.Italian),
                "");
            ReturnAll_Translation.AddTranslation(GameCulture.FromCultureName(GameCulture.CultureName.French),
                "");
            ReturnAll_Translation.AddTranslation(GameCulture.FromCultureName(GameCulture.CultureName.Spanish),
                "");
            ReturnAll_Translation.AddTranslation(GameCulture.FromCultureName(GameCulture.CultureName.Russian),
                "");
            ReturnAll_Translation.AddTranslation(GameCulture.FromCultureName(GameCulture.CultureName.Chinese),
                "退出全部");
            ReturnAll_Translation.AddTranslation(GameCulture.FromCultureName(GameCulture.CultureName.Portuguese),
                "");
            ReturnAll_Translation.AddTranslation(GameCulture.FromCultureName(GameCulture.CultureName.Polish),
                "");
        }
        public static void FindFiles(List<string> filecollection, string rootpath, Predicate<string> predicate)
        {
            foreach (string path in Directory.GetFileSystemEntries(rootpath))
            {
                if (File.Exists(path))
                {
                    if (predicate(path))
                    {
                        filecollection.Add(path);
                    }
                }
                else
                {
                    FindFiles(filecollection, path, predicate);
                }
            }
        }
        public static void ClearEmptyFolder(string rootpath)
        {
            DirectoryInfo info = new(rootpath);
            foreach (DirectoryInfo subinfo in info.GetDirectories())
            {
                ClearEmptyFolder(subinfo.FullName);
            }
            if (info.GetFileSystemInfos().Length == 0)
            {
                Directory.Delete(info.FullName);
            }
        }
        static void Sleep(Stopwatch stopwatch, double delta, ref double target)
        {
            double now = stopwatch.ElapsedMilliseconds;
            double remaining = target - now;
            target += delta;
            if (target < now)
            {
                target = now + delta;
            }
            if (remaining <= 0)
            {
                Thread.Sleep(0);
                return;
            }
            Thread.Sleep((int)remaining);
        }
        static void SendToSubserversCallBack(object data)
        {
            using NamedPipeClientStream pipe = new NamedPipeClientStream(".", "World", PipeDirection.Out);
            pipe.Connect();
            pipe.Write((byte[])data);
        }
        public static bool SendToSubservers(MessageBuffer buffer, int start, int length)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                return false;
            }
            if (buffer.readBuffer[start + 2] == 250 && (ModNet.NetModCount < 256 ? buffer.readBuffer[start + 3] : BitConverter.ToUInt16(buffer.readBuffer, start + 3)) == ModContent.GetInstance<Everglow>().NetID)
            {
                return false;
            }
            if (!SubworldSystem.playerLocations.ContainsKey(Netplay.Clients[buffer.whoAmI].Socket.GetRemoteAddress()))
            {
                return false;
            }

            Netplay.Clients[buffer.whoAmI].TimeOutTimer = 0;

            byte[] packet = new byte[length + 1];
            packet[0] = (byte)buffer.whoAmI;
            Buffer.BlockCopy(buffer.readBuffer, start, packet, 1, length);
            Task.Factory.StartNew(SendToSubserversCallBack, packet);

            //string str = ">R" + packet[3] + "(" + length + ") ";
            //for (int i = 0; i < length + 1; i++)
            //{
            //	str += packet[i] + " ";
            //}
            //ModContent.GetInstance<SubworldLibrary>().Logger.Info(str);

            return packet[3] != 2; // TODO: 需要更安全的编译代码
        }
        public static bool SendToSubservers_2(ISocket socket, byte[] data, int start, int length, ref object state)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                return false;
            }
            if (!SubworldSystem.playerLocations.ContainsKey(socket.GetRemoteAddress()))
            {
                return false;
            }
            return state is not bool;
        }
        static void Insert_IngameOptions_Draw(SpriteBatch sprite, ref int num9, ref Vector2 vector, ref Vector2 vector2, bool flag4)
        {
            if (SubworldSystem.current is not null)
            {
                if (IngameOptions.DrawLeftSide(sprite, Language.GetTextValue(ReturnAll), num9, vector, vector2, IngameOptions.leftScale, 0.7f, 0.8f, 0.01f))
                {
                    IngameOptions.leftHover = num9;
                    if (flag4)
                    {
                        SteamedWraps.StopPlaytimeTracking();
                        SystemLoader.PreSaveAndQuit();
                        IngameOptions.Close();
                        Main.menuMode = 10;
                        Main.gameMenu = true;
                        SubworldSystem.ExitAll();
                    }
                }
                num9++;
                if (IngameOptions.DrawLeftSide(sprite, Language.GetTextValue(ReturnNow), num9, vector, vector2, IngameOptions.leftScale, 0.7f, 0.8f, 0.01f))
                {
                    IngameOptions.leftHover = num9;
                    if (flag4)
                    {
                        SteamedWraps.StopPlaytimeTracking();
                        SystemLoader.PreSaveAndQuit();
                        IngameOptions.Close();
                        Main.menuMode = 10;
                        Main.gameMenu = true;
                        SubworldSystem.ExitNow();
                    }
                }
                num9++;
            }
        }
        static void Insert_Player_UpdateBiomes(ref bool IsInUnderworld, ref bool IsUnderTheSea)
        {
            if (SubworldSystem.current?.HideUnderworld ?? false)
            {
                IsInUnderworld = IsUnderTheSea = false;
            }
        }
        static void Insert_Player_Update_ModifyBasicGravity(Player player, ref float basicfloat, ref float maxFallSpeed)
        {
            SubworldSystem.current?.ModifyPlayerBasicGravity(player, ref basicfloat, ref maxFallSpeed);
        }
        static void Insert_NPC_Update_Gravity_ModifyBasicGravity(NPC npc, ref float basicgravity, ref float maxFallSpeed)
        {
            SubworldSystem.current?.ModifyNPCBasicGravity(npc, ref basicgravity, ref maxFallSpeed);
        }
        static bool Insert_TileLightScanner_GetTileLight(Tile tile, int x, int y, ref FastRandom random, ref Color outcolor)
        {
            return SubworldSystem.current?.GetTileLight(tile, x, y, ref random, ref outcolor) ?? true;
        }
        class Hooks
        {
            private static event ILContext.Manipulator AsyncSend
            {
                add
                {
                    HookEndpointManager.Modify(typeof(SocialSocket).GetMethod("Terraria.Net.Sockets.ISocket.AsyncSend", BindingFlags.NonPublic | BindingFlags.Instance), value);
                    HookEndpointManager.Modify(typeof(TcpSocket).GetMethod("Terraria.Net.Sockets.ISocket.AsyncSend", BindingFlags.NonPublic | BindingFlags.Instance), value);
                }
                remove
                {
                    HookEndpointManager.Unmodify(typeof(SocialSocket).GetMethod("Terraria.Net.Sockets.ISocket.AsyncSend", BindingFlags.NonPublic | BindingFlags.Instance), value);
                    HookEndpointManager.Unmodify(typeof(TcpSocket).GetMethod("Terraria.Net.Sockets.ISocket.AsyncSend", BindingFlags.NonPublic | BindingFlags.Instance), value);
                }
            }
            internal static void Unload()
            {
                if (Main.dedServ)
                {
                    IL.Terraria.Main.DedServ_PostModLoad -= Main_DedServ_PostModLoad;
                    AsyncSend -= Hooks_AsyncSend;
                }
                else
                {
                    IL.Terraria.Main.DoDraw -= Main_DoDraw;
                    IL.Terraria.Main.DrawBackground -= Main_DrawBackground;
                    IL.Terraria.Main.OldDrawBackground -= Main_OldDrawBackground;
                    IL.Terraria.IngameOptions.Draw -= IngameOptions_Draw;
                    IL.Terraria.Graphics.Light.TileLightScanner.GetTileLight -= TileLightScanner_GetTileLight;
                    IL.Terraria.Player.UpdateBiomes -= Player_UpdateBiomes;
                    On.Terraria.Main.DrawUnderworldBackground -= Main_DrawUnderworldBackground;
                    On.Terraria.Netplay.AddCurrentServerToRecentList -= Netplay_AddCurrentServerToRecentList;
                }
                On.Terraria.Main.EraseWorld -= Main_EraseWorld;
                On.Terraria.Main.ErasePlayer -= Main_ErasePlayer;
                IL.Terraria.Main.DoUpdateInWorld -= Main_DoUpdateInWorld;
                On.Terraria.WorldGen.UpdateWorld -= WorldGen_UpdateWorld;
                IL.Terraria.Player.Update -= Player_Update;
                IL.Terraria.NPC.UpdateNPC_UpdateGravity -= NPC_UpdateNPC_UpdateGravity;
                IL.Terraria.Liquid.Update -= Liquid_Update;
                On.Terraria.IO.WorldFile.SaveWorld -= WorldFile_SaveWorld;
                On.Terraria.Player.InternalSavePlayerFile -= Player_InternalSavePlayerFile;
                On.Terraria.Player.InternalSaveMap -= Player_InternalSaveMap;
                IL.Terraria.NetMessage.CheckBytes -= NetMessage_CheckBytes;
                On.Terraria.WorldGen.setWorldSize -= WorldGen_setWorldSize;
                if (Main.tile.Width != 8401 || Main.tile.Height != 2401)
                {
                    var createmethod = typeof(Tilemap).GetConstructor(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, new Type[] { typeof(ushort), typeof(ushort) });
                    Main.tile = (Tilemap)createmethod.Invoke(new object[] { (ushort)8401, (ushort)2401 });
                    Main.Map = new(Main.maxTilesX, Main.maxTilesY);
                    Main.mapMinX = 0;
                    Main.mapMinY = 0;
                    Main.mapMaxX = Main.maxTilesX;
                    Main.mapMaxY = Main.maxTilesY;
                    Main.mapTargetX = 5;
                    Main.mapTargetY = 2;
                    Main.instance.mapTarget = new RenderTarget2D[Main.mapTargetX, Main.mapTargetY];
                    Main.mapWasContentLost = new bool[Main.mapTargetX, Main.mapTargetY];
                    Main.initMap = new bool[Main.mapTargetX, Main.mapTargetY];
                    Main.instance.TilePaintSystem = new();
                    Main.instance.TilesRenderer = new(Main.instance.TilePaintSystem);
                    Main.instance.WallsRenderer = new(Main.instance.TilePaintSystem);
                }
            }
            internal static void Load()
            {
                if (Main.dedServ)
                {
                    IL.Terraria.Main.DedServ_PostModLoad += Main_DedServ_PostModLoad;
                    AsyncSend += Hooks_AsyncSend;
                }
                else
                {
                    IL.Terraria.Main.DoDraw += Main_DoDraw;
                    IL.Terraria.Main.DrawBackground += Main_DrawBackground;
                    IL.Terraria.Main.OldDrawBackground += Main_OldDrawBackground;
                    IL.Terraria.IngameOptions.Draw += IngameOptions_Draw;
                    IL.Terraria.Graphics.Light.TileLightScanner.GetTileLight += TileLightScanner_GetTileLight;
                    IL.Terraria.Player.UpdateBiomes += Player_UpdateBiomes;
                    On.Terraria.Main.DrawUnderworldBackground += Main_DrawUnderworldBackground;
                    On.Terraria.Netplay.AddCurrentServerToRecentList += Netplay_AddCurrentServerToRecentList;
                }
                On.Terraria.Main.EraseWorld += Main_EraseWorld;
                On.Terraria.Main.ErasePlayer += Main_ErasePlayer;
                IL.Terraria.Main.DoUpdateInWorld += Main_DoUpdateInWorld;
                On.Terraria.WorldGen.UpdateWorld += WorldGen_UpdateWorld;
                IL.Terraria.Player.Update += Player_Update;
                IL.Terraria.NPC.UpdateNPC_UpdateGravity += NPC_UpdateNPC_UpdateGravity;
                IL.Terraria.Liquid.Update += Liquid_Update;
                On.Terraria.IO.WorldFile.SaveWorld += WorldFile_SaveWorld;
                On.Terraria.Player.InternalSavePlayerFile += Player_InternalSavePlayerFile;
                On.Terraria.Player.InternalSaveMap += Player_InternalSaveMap;
                IL.Terraria.NetMessage.CheckBytes += NetMessage_CheckBytes;
                On.Terraria.WorldGen.setWorldSize += WorldGen_setWorldSize;
            }
            private static void WorldGen_setWorldSize(On.Terraria.WorldGen.orig_setWorldSize orig)
            {
                int fixedwidth = ((Main.maxTilesX - 1) / 200 + 1) * 200;
                int fixedheight = ((Main.maxTilesY - 1) / 150 + 1) * 150;
                if (fixedwidth + 1 > Main.tile.Width || fixedheight + 1 > Main.tile.Height)
                {
                    var createmethod = typeof(Tilemap).GetConstructor(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, new Type[] { typeof(ushort), typeof(ushort) });
                    Main.tile = (Tilemap)createmethod.Invoke(new object[] { (ushort)(fixedwidth + 1), (ushort)(fixedheight + 1) });
                    Main.Map = new(Main.maxTilesX, Main.maxTilesY);
                    Main.mapMinX = 0;
                    Main.mapMinY = 0;
                    Main.mapMaxX = Main.maxTilesX;
                    Main.mapMaxY = Main.maxTilesY;

                    //Main.mapTargetX = (Main.maxTilesX / Main.textureMaxWidth) + 1;
                    //Main.mapTargetY = (Main.maxTilesY / Main.textureMaxHeight) + 1;
                    //Main.instance.mapTarget = new RenderTarget2D[Main.mapTargetX, Main.mapTargetY];
                    //Main.mapWasContentLost = new bool[Main.mapTargetX, Main.mapTargetY];
                    //Main.initMap = new bool[Main.mapTargetX, Main.mapTargetY];

                    Main.instance.mapTarget = new RenderTarget2D[(Main.maxTilesX / Main.textureMaxWidth) + 1,
                        (Main.maxTilesY / Main.textureMaxHeight) + 1];
                    Main.mapWasContentLost = new bool[Main.instance.mapTarget.GetLength(0), Main.instance.mapTarget.GetLength(1)];
                    Main.initMap = new bool[Main.instance.mapTarget.GetLength(0), Main.instance.mapTarget.GetLength(1)];

                    Main.instance.TilePaintSystem = new();
                    Main.instance.TilesRenderer = new(Main.instance.TilePaintSystem);
                    Main.instance.WallsRenderer = new(Main.instance.TilePaintSystem);
                }
                Main.bottomWorld = Main.maxTilesY * 16;
                Main.rightWorld = Main.maxTilesX * 16;
                Main.maxSectionsX = (Main.maxTilesX - 1) / 200 + 1;
                Main.maxSectionsY = (Main.maxTilesY - 1) / 150 + 1;
            }
            private static void NetMessage_CheckBytes(ILContext il)
            {
                ILCursor c = new(il);

                ILLabel Skip = c.DefineLabel();

                if (!c.TryGotoNext(MoveType.After, i => i.MatchCallvirt(typeof(Stream), "get_Position"), i => i.MatchStloc(7)))
                {
                    throw new OperationCanceledException("IL Patch Is Failed.");
                }

                c.Emit(Ldsfld, typeof(NetMessage).GetField(nameof(NetMessage.buffer)));
                c.Emit(Ldarg_0);
                c.Emit(Ldelem_Ref);
                c.Emit(Ldloc_2);
                c.Emit(Ldloc, 4);
                c.Emit(Call, typeof(SubworldLibrary).GetMethod(nameof(SubworldLibrary.SendToSubservers)));
                c.Emit(Brtrue, Skip);

                if (!c.TryGotoNext(i => i.MatchLdsfld(typeof(NetMessage), nameof(NetMessage.buffer)), i => i.MatchLdarg(0), i => i.MatchLdelemRef(), i => i.MatchLdfld(typeof(MessageBuffer), nameof(MessageBuffer.reader)), i => i.MatchCallvirt(typeof(BinaryReader), "get_BaseStream")))
                {
                    throw new OperationCanceledException("IL Patch Is Failed.");
                }

                c.MarkLabel(Skip);
            }
            private static void Player_InternalSaveMap(On.Terraria.Player.orig_InternalSaveMap orig, bool isCloudSave)
            {
                if (SubworldSystem.cache is not null && (SubworldSystem.cache.HowSaveWorld == Subworld.SaveSetting.NoSave || !SubworldSystem.cache.SaveMap))
                {
                    return;
                }
                orig(isCloudSave);
            }
            private static void Player_InternalSavePlayerFile(On.Terraria.Player.orig_InternalSavePlayerFile orig, PlayerFileData playerFile)
            {
                if (SubworldSystem.cache is not null && SubworldSystem.cache.HowSaveWorld == Subworld.SaveSetting.NoSave)
                {
                    return;
                }
                orig(playerFile);
            }
            private static void WorldFile_SaveWorld(On.Terraria.IO.WorldFile.orig_SaveWorld orig)
            {
                if (SubworldSystem.cache is not null && SubworldSystem.cache.HowSaveWorld == Subworld.SaveSetting.NoSave)
                {
                    return;
                }
                orig();
            }
            private static void Liquid_Update(ILContext il)
            {
                ILCursor c = new(il);

                ILLabel Skip_HideUnderworld = c.DefineLabel();

                if (!c.TryGotoNext(i => i.MatchLdloc(7), i => i.MatchSub()))
                {
                    throw new OperationCanceledException("IL Patch Is Failed.");
                }

                c.EmitDelegate(() => SubworldSystem.current?.HideUnderworld ?? false);
                c.Emit(Brfalse, Skip_HideUnderworld);
                c.Emit(Ldc_I4, 0);
                c.Emit(Stloc, 7);
                c.MarkLabel(Skip_HideUnderworld);
            }
            private static void NPC_UpdateNPC_UpdateGravity(ILContext il)
            {
                ILCursor c = new(il);

                if (!c.TryGotoNext(i => i.MatchStsfld(typeof(NPC), "gravity")))
                {
                    throw new OperationCanceledException("IL Patch Is Failed.");
                }

                c.Emit(Ldarg, 0);
                c.Emit(Ldsflda, typeof(NPC).GetField("gravity", BindingFlags.NonPublic | BindingFlags.Static));
                c.Emit(Ldarga, 1);
                c.Emit(Call, typeof(SubworldLibrary).GetMethod(nameof(Insert_NPC_Update_Gravity_ModifyBasicGravity), BindingFlags.NonPublic | BindingFlags.Static));
            }
            private static void Player_Update(ILContext il)
            {
                ILCursor c = new(il);

                if (!c.TryGotoNext(i => i.MatchStfld(typeof(Player), nameof(Player.gravity))))
                {
                    throw new OperationCanceledException("IL Patch Is Failed.");
                }

                c.Emit(Ldarg, 0);
                c.Emit(Ldarg, 0);
                c.Emit(Ldflda, typeof(Player).GetField(nameof(Player.gravity)));
                c.Emit(Ldarg, 0);
                c.Emit(Ldflda, typeof(Player).GetField(nameof(Player.maxFallSpeed)));
                c.Emit(Call, typeof(SubworldLibrary).GetMethod(nameof(Insert_Player_Update_ModifyBasicGravity), BindingFlags.NonPublic | BindingFlags.Static));
            }
            private static void WorldGen_UpdateWorld(On.Terraria.WorldGen.orig_UpdateWorld orig)
            {
                if (SubworldSystem.current?.NormalTime ?? true)
                {
                    orig();
                }
            }
            private static void Main_DoUpdateInWorld(ILContext il)
            {
                ILCursor c = new(il);

                ILLabel Skip_TimeUpdate = c.DefineLabel();

                if (!c.TryGotoNext(i => i.MatchCall(typeof(SystemLoader), nameof(SystemLoader.PreUpdateTime))))
                {
                    throw new OperationCanceledException("IL Patch Is Failed.");
                }

                c.EmitDelegate(() => SubworldSystem.current?.NormalTime ?? true);
                c.Emit(Brfalse, Skip_TimeUpdate);

                if (!c.TryGotoNext(MoveType.After, i => i.MatchCall(typeof(SystemLoader), nameof(SystemLoader.PostUpdateTime))))
                {
                    throw new OperationCanceledException("IL Patch Is Failed.");
                }
                c.MarkLabel(Skip_TimeUpdate);
            }
            private static void Main_ErasePlayer(On.Terraria.Main.orig_ErasePlayer orig, int i)
            {
                PlayerFileData Erasing = Main.PlayerList[i];
                string rootpath = Path.Combine(Main.PlayerPath, nameof(Subworld), Path.GetFileNameWithoutExtension(Erasing.Path));
                if (Directory.Exists(rootpath))
                {
                    List<string> subworlds = new();
                    FindFiles(subworlds, rootpath, file => file.Contains(".wld") || file.Contains("twld"));
                    bool iswindow = OperatingSystem.IsWindows();
                    foreach (string file in subworlds)
                    {
                        if (iswindow)
                        {
                            FileOperationAPIWrapper.MoveToRecycleBin(file);
                        }
                        else
                        {
                            File.Delete(file);
                        }
                    }
                    ClearEmptyFolder(rootpath);
                }
                orig(i);
            }
            private static void Main_EraseWorld(On.Terraria.Main.orig_EraseWorld orig, int i)
            {
                WorldFileData Erasing = Main.WorldList[i];
                string rootpath = Path.Combine(Main.WorldPath, nameof(Subworld), Path.GetFileNameWithoutExtension(Erasing.Path));
                if (Directory.Exists(rootpath))
                {
                    List<string> subworlds = new();
                    FindFiles(subworlds, rootpath, file => file.Contains(".wld") || file.Contains("twld"));
                    bool iswindow = OperatingSystem.IsWindows();
                    foreach (string file in subworlds)
                    {
                        if (iswindow)
                        {
                            FileOperationAPIWrapper.MoveToRecycleBin(file);
                        }
                        else
                        {
                            File.Delete(file);
                        }
                    }
                    ClearEmptyFolder(rootpath);
                }
                orig(i);
            }
            private static void Netplay_AddCurrentServerToRecentList(On.Terraria.Netplay.orig_AddCurrentServerToRecentList orig)
            {
                if (SubworldSystem.current is not null)
                {
                    return;
                }
                orig();
            }
            private static void Main_DrawUnderworldBackground(On.Terraria.Main.orig_DrawUnderworldBackground orig, Main self, bool flat)
            {
                if (SubworldSystem.current?.HideUnderworld ?? false)
                {
                    return;
                }
                orig(self, flat);
            }
            private static void Player_UpdateBiomes(ILContext il)
            {
                ILCursor c = new(il);

                if (!c.TryGotoNext(MoveType.After, i => i.MatchStloc(11)))
                {
                    throw new OperationCanceledException("IL Patch Is Failed.");
                }

                c.Emit(Ldloca, 10);
                c.Emit(Ldloca, 11);
                c.Emit(Call, typeof(SubworldLibrary).GetMethod(nameof(SubworldLibrary.Insert_Player_UpdateBiomes), BindingFlags.NonPublic | BindingFlags.Static));
            }
            private static void TileLightScanner_GetTileLight(ILContext il)
            {
                ILCursor c = new(il);

                ILLabel Skip_Return = c.DefineLabel();
                ILLabel Skip_ApplyHellLight = c.DefineLabel();

                if (!c.TryGotoNext(MoveType.After, i => i.MatchStloc(1)))
                {
                    throw new OperationCanceledException("IL Patch Is Failed.");
                }

                c.Emit(Ldloc, 0);
                c.Emit(Ldarg, 1);
                c.Emit(Ldarg, 2);
                c.Emit(Ldloca, 1);
                c.Emit(Ldarga, 3);
                c.Emit(Call, typeof(SubworldLibrary).GetMethod(nameof(SubworldLibrary.Insert_TileLightScanner_GetTileLight), BindingFlags.NonPublic | BindingFlags.Static));
                c.Emit(Brtrue, Skip_Return);
                c.Emit(Ret);
                c.MarkLabel(Skip_Return);

                if (!c.TryGotoNext(MoveType.AfterLabel, i => i.MatchLdarg(2), i => i.MatchCall(typeof(Main), "get_UnderworldLayer")))
                {
                    throw new OperationCanceledException("IL Patch Is Failed.");
                }

                c.EmitDelegate(() => SubworldSystem.current?.HideUnderworld ?? false);
                c.Emit(Brtrue, Skip_ApplyHellLight);

                if (!c.TryGotoNext(MoveType.After, i => i.MatchCall(typeof(TileLightScanner), "ApplyHellLight")))
                {
                    throw new OperationCanceledException("IL Patch Is Failed.");
                }
                c.MarkLabel(Skip_ApplyHellLight);
            }
            private static void IngameOptions_Draw(ILContext il)
            {
                ILCursor c = new(il);

                ILLabel Skip_520f = c.DefineLabel();
                ILLabel Skip_480f = c.DefineLabel();
                ILLabel Skip_ReplaceLanginter35 = c.DefineLabel();
                ILLabel SkipLanginter35 = c.DefineLabel();
                ILLabel Skip_Orig_SaveAndQuit = c.DefineLabel();

                if (!c.TryGotoNext(MoveType.After, i => i.MatchLdcR4(670)))
                {
                    throw new OperationCanceledException("IL Patch Is Failed.");
                }

                c.Emit(Ldsfld, typeof(SubworldSystem).GetField(nameof(SubworldSystem.current), BindingFlags.NonPublic | BindingFlags.Static));
                c.Emit(Brfalse, Skip_520f);
                c.Emit(Ldc_R4, 520f);
                c.Emit(Br, Skip_480f);
                c.MarkLabel(Skip_520f);

                if (!c.TryGotoNext(MoveType.After, i => i.MatchLdcR4(480)))
                {
                    throw new OperationCanceledException("IL Patch Is Failed.");
                }
                c.MarkLabel(Skip_480f);

                if (!c.TryGotoNext(i => i.MatchStloc(18)))
                {
                    throw new OperationCanceledException("IL Patch Is Failed.");
                }

                c.EmitDelegate(() => SubworldSystem.current is null ? 0 : 1);
                c.Emit(Add);

                //if (!c.TryGotoNext(i => i.MatchLdsfld(typeof(Lang), nameof(Lang.inter)), i => i.MatchLdcI4(35)))
                //{
                //    throw new OperationCanceledException("IL Patch Is Failed.");
                //}

                //c.Emit(Ldsfld, typeof(SubworldSystem).GetField(nameof(SubworldSystem.current), BindingFlags.NonPublic | BindingFlags.Static));
                //c.Emit(Brfalse, Skip_ReplaceLanginter35);
                //c.EmitDelegate(() => Language.GetTextValue(ReturnAll));
                //c.Emit(Br, SkipLanginter35);
                //c.MarkLabel(Skip_ReplaceLanginter35);

                //if (!c.TryGotoNext(MoveType.After, i => i.MatchCallvirt(typeof(LocalizedText), "get_Value")))
                //{
                //    throw new OperationCanceledException("IL Patch Is Failed.");
                //}
                //c.MarkLabel(SkipLanginter35);

                //if (!c.TryGotoNext(i => i.MatchLdnull(), i => i.MatchCall(typeof(WorldGen), nameof(WorldGen.SaveAndQuit))))
                //{
                //    throw new OperationCanceledException("IL Patch Is Failed.");
                //}

                //c.EmitDelegate(() =>
                //{
                //    if (SubworldSystem.current is null)
                //    {
                //        WorldGen.SaveAndQuit(null);
                //    }
                //    else
                //    {
                //        SubworldSystem.ExitAll();
                //    }
                //});
                //c.Emit(Br, Skip_Orig_SaveAndQuit);

                if (!c.TryGotoNext(/*MoveType.After, */i => i.MatchCall(typeof(WorldGen), nameof(WorldGen.SaveAndQuit))))
                {
                    throw new OperationCanceledException("IL Patch Is Failed.");
                }
                //c.MarkLabel(Skip_Orig_SaveAndQuit);
                c.EmitDelegate(() =>
                {
                    SubworldSystem.cache?.Unload();
                    SubworldSystem.current?.Unload();
                    SubworldSystem.current = SubworldSystem.cache = null;
                });

                if (!c.TryGotoNext(i => i.MatchLdsfld(typeof(IngameOptions), nameof(IngameOptions.category))))
                {
                    throw new OperationCanceledException("IL Patch Is Failed.");
                }

                c.Emit(Ldarg, 1);
                c.Emit(Ldloca, 21);
                c.Emit(Ldloca, 19);
                c.Emit(Ldloca, 20);
                c.Emit(Ldloc, 9);
                c.Emit(Call, typeof(SubworldLibrary).GetMethod(nameof(SubworldLibrary.Insert_IngameOptions_Draw), BindingFlags.NonPublic | BindingFlags.Static));
            }
            private static void Main_OldDrawBackground(ILContext il)
            {
                ILCursor c = new(il);

                ILLabel Skip_Hide = c.DefineLabel();
                ILLabel Skip_Calculate = c.DefineLabel();

                if (!c.TryGotoNext(i => i.MatchLdcI4(230)))
                {
                    throw new OperationCanceledException("IL Patch Is Failed.");
                }

                c.EmitDelegate(() => SubworldSystem.current?.HideUnderworld ?? false);
                c.Emit(Brfalse, Skip_Hide);

                c.Emit(Conv_R8);
                c.Emit(Br, Skip_Calculate);

                c.MarkLabel(Skip_Hide);

                if (!c.TryGotoNext(i => i.MatchStloc(18), i => i.MatchLdcI4(0)))
                {
                    return;
                }

                c.MarkLabel(Skip_Calculate);
            }
            private static void Main_DrawBackground(ILContext il)
            {
                ILCursor c = new(il);

                ILLabel Skip_Hide = c.DefineLabel();
                ILLabel Skip_Calculate = c.DefineLabel();

                if (!c.TryGotoNext(i => i.MatchLdcI4(330)))
                {
                    throw new OperationCanceledException("IL Patch Is Failed.");
                }

                c.EmitDelegate(() => SubworldSystem.current?.HideUnderworld ?? false);
                c.Emit(Brfalse, Skip_Hide);

                c.Emit(Conv_R8);
                c.Emit(Br, Skip_Calculate);

                c.MarkLabel(Skip_Hide);

                if (!c.TryGotoNext(i => i.MatchStloc(2), i => i.MatchLdcR4(255)))
                {
                    return;
                }

                c.MarkLabel(Skip_Calculate);
            }
            private static void Main_DoDraw(ILContext il)
            {
                ILCursor c = new(il);

                ILLabel Skip_DrawSetUp = c.DefineLabel();
                ILLabel SkipWhenNull = c.DefineLabel();

                if (!c.TryGotoNext(MoveType.After, i => i.MatchStsfld(typeof(Main), nameof(Main.HoverItem))))
                {
                    throw new OperationCanceledException("IL Patch Is Failed.");
                }

                c.Emit(Ldsfld, typeof(Main).GetField(nameof(Main.gameMenu)));
                c.Emit(Brfalse, Skip_DrawSetUp);
                {
                    c.EmitDelegate(() => SubworldSystem.current ?? SubworldSystem.cache);
                    c.Emit(Dup);
                    c.Emit(Brfalse, SkipWhenNull);
                    {
                        c.Emit(Ldc_R4, 1f);
                        c.Emit(Dup);
                        c.Emit(Stsfld, typeof(Main).GetField("_uiScaleWanted", BindingFlags.NonPublic | BindingFlags.Static));
                        c.Emit(Stsfld, typeof(Main).GetField("_uiScaleUsed", BindingFlags.NonPublic | BindingFlags.Static));
                        c.EmitDelegate(() => Matrix.CreateScale(1f));
                        c.Emit(Stsfld, typeof(Main).GetField("_uiScaleMatrix", BindingFlags.NonPublic | BindingFlags.Static));
                        c.Emit(Ldarg_0);
                        c.Emit(Callvirt, typeof(Subworld).GetMethod(nameof(Subworld.DrawSetUp)));
                        c.Emit(Br, Skip_DrawSetUp);
                    }
                    c.MarkLabel(SkipWhenNull);
                    c.Emit(Pop);
                }
                c.MarkLabel(Skip_DrawSetUp);
            }
            private static void Hooks_AsyncSend(ILContext il)
            {
                ILCursor c = new(il);

                ILLabel Skip = c.DefineLabel();

                c.Emit(Ldarg_0);
                c.Emit(Ldarg_1);
                c.Emit(Ldarg_2);
                c.Emit(Ldarg_3);
                c.Emit(Ldarga, 5);
                c.Emit(Call, typeof(SubworldLibrary).GetMethod(nameof(SubworldLibrary.SendToSubservers_2)));
                c.Emit(Brfalse, Skip);
                c.Emit(Ret);
                c.MarkLabel(Skip);
            }
            private static void Main_DedServ_PostModLoad(ILContext il)
            {
                ILCursor c = new(il);

                MethodInfo Update = typeof(Main).GetMethod("Update", BindingFlags.NonPublic | BindingFlags.Instance);
                FieldInfo SaveTime = typeof(Main).GetField("saveTime", BindingFlags.NonPublic | BindingFlags.Static);
                MethodInfo Checkin = typeof(Main).Assembly.GetType("Terraria.ModLoader.Engine.ServerHangWatchdog").GetMethod("Checkin", BindingFlags.NonPublic | BindingFlags.Static);

                ILLabel Skip_LoadIntoSubworlds = c.DefineLabel();
                ILLabel DoLoop = c.DefineLabel();
                ILLabel BreakLoop = c.DefineLabel();
                ILLabel Skip_HasClient = c.DefineLabel();

                if (!c.TryGotoNext(MoveType.After, i => i.MatchStindI1()))
                {
                    throw new OperationCanceledException("IL Patch Is Failed.");
                }

                c.EmitDelegate(SubworldSystem.LoadIntoSubworlds);
                c.Emit(Brfalse, Skip_LoadIntoSubworlds);
                {
                    c.Emit(Ldarg_0);
                    c.EmitDelegate(() => new GameTime());
                    c.Emit(Callvirt, Update);


                    c.EmitDelegate(() => new Stopwatch());
                    c.Emit(Stloc_1);
                    c.Emit(Ldloc_1);
                    c.Emit(Callvirt, typeof(Stopwatch).GetMethod(nameof(Stopwatch.Start)));

                    c.EmitDelegate(() => Main.gameMenu = false);

                    c.Emit(Ldc_R8, 50 / 3f);
                    c.Emit(Stloc_2);
                    c.Emit(Ldloc_2);
                    c.Emit(Stloc_3);

                    c.MarkLabel(DoLoop);
                    c.Emit(Ldsfld, typeof(Netplay).GetField(nameof(Netplay.Disconnect)));
                    c.Emit(Brtrue, BreakLoop);
                    {
                        c.Emit(Call, Checkin);

                        c.Emit(Ldsfld, typeof(Netplay).GetField(nameof(Netplay.HasClients)));
                        c.Emit(Brfalse, Skip_HasClient);
                        {
                            c.Emit(Ldarg_0);
                            c.EmitDelegate(() => new GameTime());
                            c.Emit(Callvirt, Update);
                            c.Emit(Br, Skip_HasClient);
                        }
                        c.Emit(Ldsfld, SaveTime);
                        c.EmitDelegate<Action<Stopwatch>>(stopwatch =>
                        {
                            if (stopwatch.IsRunning)
                            {
                                stopwatch.Stop();
                            }
                        });
                        c.MarkLabel(Skip_HasClient);

                        c.Emit(Ldloc_1);
                        c.Emit(Ldloc_2);
                        c.Emit(Ldloca, 3);
                        c.Emit(Call, typeof(SubworldLibrary).GetMethod(nameof(Sleep), BindingFlags.NonPublic | BindingFlags.Static));
                    }
                    c.MarkLabel(BreakLoop);

                    c.EmitDelegate(() =>
                    {
                        if (Main.menuMode != MenuID.Error && Netplay.SaveOnServerExit)
                        {
                            Console.WriteLine("Saving before exit...");
                            WorldFile.SaveWorld();
                        }
                        SystemLoader.OnWorldUnload();
                        Main.instance.YouCanSleepNow();
                    });

                    c.Emit(Ret);
                }
                c.MarkLabel(Skip_LoadIntoSubworlds);
            }
        }
    }
}