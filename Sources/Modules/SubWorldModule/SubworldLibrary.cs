using Everglow.Sources.Commons.Core.ModuleSystem;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour.HookGen;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.Localization;
using Terraria.Net;
using Terraria.Net.Sockets;
using Terraria.Social;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace Everglow.Sources.Modules.SubWorldModule
{
    internal class SubworldLibrary : IModule
    {
        public string Name => nameof(SubworldLibrary);
        public void Load()
        {
            ModTranslation modTranslation = LocalizationLoader.GetOrCreateTranslation("Mods.SubworldLibrary.Return");
            modTranslation.AddTranslation(1, "Return");
            modTranslation.AddTranslation(2, "Wiederkehren");
            modTranslation.AddTranslation(3, "Ritorno");
            modTranslation.AddTranslation(4, "Retour");
            modTranslation.AddTranslation(5, "Regresar");
            modTranslation.AddTranslation(6, "Возвращаться");
            modTranslation.AddTranslation(7, "返回");
            modTranslation.AddTranslation(8, "Regressar");
            modTranslation.AddTranslation(9, "Wracać");
            LocalizationLoader.AddTranslation(modTranslation);
            FieldInfo current = typeof(SubworldSystem).GetField(nameof(SubworldSystem.current), BindingFlags.Static | BindingFlags.NonPublic);
            FieldInfo cache = typeof(SubworldSystem).GetField(nameof(SubworldSystem.cache), BindingFlags.Static | BindingFlags.NonPublic);
            FieldInfo hideUnderworld = typeof(SubworldSystem).GetField(nameof(SubworldSystem.hideUnderworld));
            MethodInfo normalUpdates = typeof(Subworld).GetMethod("get_NormalUpdates");
            MethodInfo shouldSave = typeof(Subworld).GetMethod("get_ShouldSave");
            bool dedServ = Main.dedServ;
            if (dedServ)
            {
                IL.Terraria.Main.DedServ_PostModLoad += delegate (ILContext il)
                {
                    ConstructorInfo constructor = typeof(GameTime).GetConstructor(Type.EmptyTypes);
                    MethodInfo method = typeof(Main).GetMethod("Update", BindingFlags.Instance | BindingFlags.NonPublic);
                    FieldInfo field = typeof(Main).GetField("saveTime", BindingFlags.Static | BindingFlags.NonPublic);
                    ILCursor cursor = new(il);
                    if (cursor.TryGotoNext(MoveType.After, i => i.MatchStindI1()))
                    {
                        cursor.Emit(OpCodes.Call, typeof(SubworldSystem).GetMethod(nameof(SubworldSystem.LoadIntoSubworld), BindingFlags.Static | BindingFlags.NonPublic));
                        ILLabel label1 = cursor.DefineLabel();
                        cursor.Emit(OpCodes.Brfalse, label1);
                        cursor.Emit(OpCodes.Ldarg_0);
                        cursor.Emit(OpCodes.Newobj, constructor);
                        cursor.Emit(OpCodes.Callvirt, method);
                        cursor.Emit(OpCodes.Newobj, typeof(Stopwatch).GetConstructor(Type.EmptyTypes));
                        cursor.Emit(OpCodes.Stloc_1);
                        cursor.Emit(OpCodes.Ldloc_1);
                        cursor.Emit(OpCodes.Callvirt, typeof(Stopwatch).GetMethod("Start"));
                        cursor.Emit(OpCodes.Ldc_I4_0);
                        cursor.Emit(OpCodes.Stsfld, typeof(Main).GetField("gameMenu"));
                        cursor.Emit(OpCodes.Ldc_R8, 16.666666666666668);
                        cursor.Emit(OpCodes.Stloc_2);
                        cursor.Emit(OpCodes.Ldloc_2);
                        cursor.Emit(OpCodes.Stloc_3);
                        ILLabel label2 = cursor.DefineLabel();
                        cursor.Emit(OpCodes.Br, label2);
                        ILLabel illabel3 = cursor.DefineLabel();
                        cursor.MarkLabel(illabel3);
                        cursor.Emit(OpCodes.Call, typeof(Main).Assembly.GetType("Terraria.ModLoader.Engine.ServerHangWatchdog").GetMethod("Checkin", BindingFlags.Static | BindingFlags.NonPublic));
                        cursor.Emit(OpCodes.Ldsfld, typeof(Netplay).GetField("HasClients"));
                        ILLabel illabel4 = cursor.DefineLabel();
                        cursor.Emit(OpCodes.Brfalse, illabel4);
                        cursor.Emit(OpCodes.Ldarg_0);
                        cursor.Emit(OpCodes.Newobj, constructor);
                        cursor.Emit(OpCodes.Callvirt, method);
                        ILLabel illabel5 = cursor.DefineLabel();
                        cursor.Emit(OpCodes.Br, illabel5);
                        cursor.MarkLabel(illabel4);
                        cursor.Emit(OpCodes.Ldsfld, field);
                        cursor.Emit(OpCodes.Callvirt, typeof(Stopwatch).GetMethod("get_IsRunning"));
                        cursor.Emit(OpCodes.Brfalse, illabel5);
                        cursor.Emit(OpCodes.Ldsfld, field);
                        cursor.Emit(OpCodes.Callvirt, typeof(Stopwatch).GetMethod("Stop"));
                        cursor.Emit(OpCodes.Br, illabel5);
                        cursor.MarkLabel(illabel5);
                        cursor.Emit(OpCodes.Ldloc_1);
                        cursor.Emit(OpCodes.Ldloc_2);
                        cursor.Emit(OpCodes.Ldloca, 3);
                        cursor.Emit(OpCodes.Call, typeof(SubworldLibrary).GetMethod("Sleep", BindingFlags.Static | BindingFlags.NonPublic));
                        cursor.MarkLabel(label2);
                        cursor.Emit(OpCodes.Ldsfld, typeof(Terraria.Netplay).GetField("Disconnect"));
                        cursor.Emit(OpCodes.Brfalse, illabel3);
                        cursor.Emit(OpCodes.Ret);
                        cursor.MarkLabel(label1);
                    }
                };
                AsyncSend += delegate (ILContext il)
                {
                    ILCursor cursor = new(il);
                    cursor.Emit(OpCodes.Ldarg_0);
                    cursor.Emit(OpCodes.Ldarg_1);
                    cursor.Emit(OpCodes.Ldarg_2);
                    cursor.Emit(OpCodes.Ldarg_3);
                    cursor.Emit(OpCodes.Ldarga, 5);
                    cursor.Emit(OpCodes.Call, typeof(SubworldLibrary).GetMethod(nameof(SubworldLibrary.SendToSubservers), new Type[]
                    {
                        typeof(ISocket),
                        typeof(byte[]),
                        typeof(int),
                        typeof(int),
                        typeof(object).MakeByRefType()
                    }));
                    ILLabel illabel = cursor.DefineLabel();
                    cursor.Emit(OpCodes.Brfalse, illabel);
                    cursor.Emit(OpCodes.Ret);
                    cursor.MarkLabel(illabel);
                };
            }
            else
            {
                IL.Terraria.Main.DoDraw += delegate (ILContext il)
                {
                    ILCursor cursor = new(il);
                    if (cursor.TryGotoNext(MoveType.After, i => i.MatchStsfld(typeof(Terraria.Main), nameof(Main.HoverItem))))
                    {
                        cursor.Emit(OpCodes.Ldsfld, typeof(Main).GetField(nameof(Main.gameMenu)));
                        ILLabel label1 = cursor.DefineLabel();
                        cursor.Emit(OpCodes.Brfalse, label1);
                        cursor.Emit(OpCodes.Ldsfld, current);
                        cursor.Emit(OpCodes.Dup);
                        ILLabel label2 = cursor.DefineLabel();
                        cursor.Emit(OpCodes.Brtrue, label2);
                        cursor.Emit(OpCodes.Pop);
                        cursor.Emit(OpCodes.Ldsfld, cache);
                        cursor.Emit(OpCodes.Dup);
                        cursor.Emit(OpCodes.Brtrue, label2);
                        cursor.Emit(OpCodes.Pop);
                        cursor.Emit(OpCodes.Br, label1);
                        cursor.MarkLabel(label2);
                        cursor.Emit(OpCodes.Ldc_R4, 1f);
                        cursor.Emit(OpCodes.Dup);
                        cursor.Emit(OpCodes.Dup);
                        cursor.Emit(OpCodes.Stsfld, typeof(Main).GetField("_uiScaleWanted", BindingFlags.Static | BindingFlags.NonPublic));
                        cursor.Emit(OpCodes.Stsfld, typeof(Main).GetField("_uiScaleUsed", BindingFlags.Static | BindingFlags.NonPublic));
                        cursor.Emit(OpCodes.Call, typeof(Matrix).GetMethod(nameof(Matrix.CreateScale), new Type[]
                        {
                            typeof(float)
                        }));
                        cursor.Emit(OpCodes.Stsfld, typeof(Main).GetField("_uiScaleMatrix", BindingFlags.Static | BindingFlags.NonPublic));
                        cursor.Emit(OpCodes.Ldarg_0);
                        cursor.Emit(OpCodes.Callvirt, typeof(Subworld).GetMethod(nameof(Subworld.DrawSetup)));
                        cursor.Emit(OpCodes.Ret);
                        cursor.MarkLabel(label1);
                    }
                };
                IL.Terraria.Main.DrawBackground += delegate (ILContext il)
                {
                    ILCursor cursor = new(il);
                    if (cursor.TryGotoNext(i => i.MatchLdcI4(330)))
                    {
                        ILLabel label = cursor.DefineLabel();
                        cursor.Emit(OpCodes.Conv_R8);
                        cursor.Emit(OpCodes.Br, label);
                        if (cursor.TryGotoNext(i => i.MatchStloc(2), i => i.MatchLdcR4(255f)))
                        {
                            cursor.MarkLabel(label);
                        }
                    }
                };
                IL.Terraria.Main.OldDrawBackground += delegate (ILContext il)
                {
                    ILCursor cursor = new(il);
                    if (cursor.TryGotoNext(i => i.MatchLdcI4(230)))
                    {
                        ILLabel label = cursor.DefineLabel();
                        cursor.Emit(OpCodes.Conv_R8);
                        cursor.Emit(OpCodes.Br, label);
                        if (cursor.TryGotoNext(i => i.MatchStloc(18), i => i.MatchLdcI4(0)))
                        {
                            cursor.MarkLabel(label);
                        }
                    }
                };
                IL.Terraria.IngameOptions.Draw += delegate (ILContext il)
                {
                    ILCursor cursor = new(il);
                    if (cursor.TryGotoNext(i => i.MatchLdsfld(typeof(Terraria.Lang), "inter"), i => i.MatchLdcI4(35)))
                    {
                        cursor.Emit(OpCodes.Ldsfld, current);
                        ILLabel label1 = cursor.DefineLabel();
                        cursor.Emit(OpCodes.Brfalse, label1);
                        cursor.Emit(OpCodes.Ldstr, "Mods.SubworldLibrary.Return");
                        cursor.Emit(OpCodes.Call, typeof(Language).GetMethod(nameof(Language.GetTextValue), new Type[]
                        {
                            typeof(string)
                        }));
                        ILLabel label2 = cursor.DefineLabel();
                        cursor.Emit(OpCodes.Br, label2);
                        cursor.MarkLabel(label1);
                        if (cursor.TryGotoNext(MoveType.After, i => i.MatchCallvirt(typeof(LocalizedText), "get_Value")))
                        {
                            cursor.MarkLabel(label2);
                            if (cursor.TryGotoNext(i => i.MatchLdnull(), i => i.MatchCall(typeof(Terraria.WorldGen), nameof(WorldGen.SaveAndQuit))))
                            {
                                cursor.Emit(OpCodes.Ldsfld, current);
                                label1 = cursor.DefineLabel();
                                cursor.Emit(OpCodes.Brfalse, label1);
                                cursor.Emit(OpCodes.Call, typeof(SubworldSystem).GetMethod("Exit"));
                                label2 = cursor.DefineLabel();
                                cursor.Emit(OpCodes.Br, label2);
                                cursor.MarkLabel(label1);
                                if (cursor.TryGotoNext(MoveType.After, i => i.MatchCall(typeof(Terraria.WorldGen), nameof(WorldGen.SaveAndQuit))))
                                {
                                    cursor.MarkLabel(label2);
                                    if (cursor.TryGotoPrev(MoveType.AfterLabel, i => i.MatchLdloc(23), i => i.MatchLdcI4(1), i => i.MatchAdd(), i => i.MatchStloc(23)))
                                    {
                                        cursor.Emit(OpCodes.Ldsfld, typeof(SubworldSystem).GetField(nameof(SubworldSystem.noReturn)));
                                        cursor.Emit(OpCodes.Brtrue, label2);
                                    }
                                }
                            }
                        }
                    }
                };
                IL.Terraria.Graphics.Light.TileLightScanner.GetTileLight += delegate (ILContext il)
                {
                    ILCursor cursor = new(il);
                    if (cursor.TryGotoNext(MoveType.After, i => i.MatchStloc(1)))
                    {
                        cursor.Emit(OpCodes.Ldsfld, current);
                        ILLabel label1 = cursor.DefineLabel();
                        cursor.Emit(OpCodes.Brfalse, label1);
                        cursor.Emit(OpCodes.Ldsfld, current);
                        cursor.Emit(OpCodes.Ldloc_0);
                        cursor.Emit(OpCodes.Ldarg_1);
                        cursor.Emit(OpCodes.Ldarg_2);
                        cursor.Emit(OpCodes.Ldloca, 1);
                        cursor.Emit(OpCodes.Ldarg_3);
                        cursor.Emit(OpCodes.Callvirt, typeof(Subworld).GetMethod("GetLight"));
                        cursor.Emit(OpCodes.Brfalse, label1);
                        cursor.Emit(OpCodes.Ret);
                        cursor.MarkLabel(label1);
                        if (cursor.TryGotoNext(MoveType.AfterLabel, i => i.MatchLdarg(2), i => i.MatchCall(typeof(Terraria.Main), "get_UnderworldLayer")))
                        {
                            cursor.Emit(OpCodes.Ldsfld, hideUnderworld);
                            ILLabel label2 = cursor.DefineLabel();
                            cursor.Emit(OpCodes.Brfalse, label2);
                            if (cursor.TryGotoNext(MoveType.After, i => i.MatchCall(typeof(Terraria.Graphics.Light.TileLightScanner), "ApplyHellLight")))
                            {
                                cursor.MarkLabel(label2);
                            }
                        }
                    }
                };
                IL.Terraria.Player.UpdateBiomes += delegate (ILContext il)
                {
                    ILCursor cursor = new(il);
                    if (cursor.TryGotoNext(MoveType.After, i => i.MatchStloc(9)))
                    {
                        cursor.Emit(OpCodes.Ldsfld, hideUnderworld);
                        ILLabel label1 = cursor.DefineLabel();
                        cursor.Emit(OpCodes.Brtrue, label1);
                        cursor.Emit(OpCodes.Ldc_I4_0);
                        ILLabel label2 = cursor.DefineLabel();
                        cursor.Emit(OpCodes.Br, label2);
                        cursor.MarkLabel(label1);
                        if (cursor.TryGotoNext(i => i.MatchStloc(10)))
                        {
                            cursor.MarkLabel(label2);
                        }
                    }
                };
                //IL.Terraria.Main.DrawUnderworldBackground += delegate (ILContext il)
                //{
                //    ILCursor cursor = new(il);
                //    cursor.Emit(OpCodes.Ldsfld, hideUnderworld);
                //    ILLabel label = cursor.DefineLabel();
                //    cursor.Emit(OpCodes.Brtrue, label);
                //    cursor.Emit(OpCodes.Ret);
                //    cursor.MarkLabel(label);
                //};
                On.Terraria.Main.DrawBackground += Main_DrawBackground;
                IL.Terraria.Netplay.AddCurrentServerToRecentList += delegate (ILContext il)
                {
                    ILCursor cursor = new(il);
                    cursor.Emit(OpCodes.Ldsfld, current);
                    ILLabel label = cursor.DefineLabel();
                    cursor.Emit(OpCodes.Brfalse, label);
                    cursor.Emit(OpCodes.Ret);
                    cursor.MarkLabel(label);
                };
            }
            //IL.Terraria.WorldGen.do_worldGenCallBack += delegate (ILContext il)
            //{
            //    ILCursor cursor = new(il);
            //    if (cursor.TryGotoNext(MoveType.After, i => i.MatchCall(typeof(Terraria.IO.WorldFile), "saveWorld")))
            //    {
            //        cursor.Emit(OpCodes.Call, typeof(SubworldSystem).GetMethod(nameof(SubworldSystem.GenerateSubworlds), BindingFlags.Static | BindingFlags.NonPublic));
            //    }
            //};
            On.Terraria.WorldGen.GenerateWorld += delegate (On.Terraria.WorldGen.orig_GenerateWorld orig, int seed, GenerationProgress progress)
            {
                orig(seed, progress);
                SubworldSystem.GenerateSubworlds();
            };
            //IL.Terraria.Main.EraseWorld += delegate (ILContext il)
            //{
            //    ILCursor cursor = new(il);
            //    cursor.Emit(OpCodes.Ldarg_0);
            //    cursor.Emit(OpCodes.Call, typeof(SubworldSystem).GetMethod(nameof(SubworldSystem.EraseSubworlds), BindingFlags.Static | BindingFlags.NonPublic));
            //};
            On.Terraria.Main.EraseWorld += delegate (On.Terraria.Main.orig_EraseWorld orig, int i)
            {
                try
                {
                    if (!Main.WorldList[i].IsCloudSave)
                    {
                        if (OperatingSystem.IsWindows())
                        {
                            FileOperationAPIWrapper.MoveToRecycleBin(Main.WorldList[i].Path);
                            FileOperationAPIWrapper.MoveToRecycleBin(Main.WorldList[i].Path + ".bak");
                            for (int j = 2; j <= 9; j++)
                            {
                                FileOperationAPIWrapper.MoveToRecycleBin(Main.WorldList[i].Path + ".bak" + j.ToString());
                            }
                        }
                        else
                        {
                            File.Delete(Main.WorldList[i].Path);
                            File.Delete(Main.WorldList[i].Path + ".bak");
                        }
                    }
                    else if (SocialAPI.Cloud != null)
                    {
                        SocialAPI.Cloud.Delete(Main.WorldList[i].Path);
                    }
                    string path = Path.ChangeExtension(Main.WorldList[i].Path, ".twld");
                    if (Main.WorldList[i].IsCloudSave)
                    {
                        if (SocialAPI.Cloud != null)
                        {
                            SocialAPI.Cloud.Delete(path);
                        }
                        return;
                    }
                    if (OperatingSystem.IsWindows())
                    {
                        FileOperationAPIWrapper.MoveToRecycleBin(path);
                        FileOperationAPIWrapper.MoveToRecycleBin(path + ".bak");
                        return;
                    }
                    File.Delete(path);
                    File.Delete(path + ".bak");
                    SubworldSystem.EraseSubworlds(i);
                    Main.LoadWorlds();
                }
                catch
                {
                }
            };
            //IL.Terraria.Main.DoUpdateInWorld += delegate (ILContext il)
            //{
            //    ILCursor cursor = new(il);
            //    if (cursor.TryGotoNext(MoveType.After, i => i.MatchCall(typeof(SystemLoader), nameof(SystemLoader.PreUpdateTime))))
            //    {
            //        cursor.Emit(OpCodes.Ldsfld, current);
            //        ILLabel label1 = cursor.DefineLabel();
            //        cursor.Emit(OpCodes.Brfalse, label1);
            //        cursor.Emit(OpCodes.Ldsfld, current);
            //        cursor.Emit(OpCodes.Callvirt, normalUpdates);
            //        ILLabel label2 = cursor.DefineLabel();
            //        cursor.Emit(OpCodes.Brfalse, label2);
            //        cursor.MarkLabel(label1);
            //        if (cursor.TryGotoNext(i => i.MatchCall(typeof(SystemLoader), nameof(SystemLoader.PreUpdateTime))))
            //        {
            //            cursor.MarkLabel(label2);
            //        }
            //    }
            //};
            On.Terraria.Main.UpdateTime += delegate (On.Terraria.Main.orig_UpdateTime orig)
            {
                if (SubworldSystem.current?.NormalUpdates ?? true)
                {
                    orig();
                }
            };
            IL.Terraria.WorldGen.UpdateWorld += delegate (ILContext il)
            {
                ILCursor cursor = new(il);
                if (cursor.TryGotoNext(i => i.MatchCall(typeof(Terraria.WorldGen), "UpdateWorld_Inner")))
                {
                    cursor.Emit(OpCodes.Ldsfld, current);
                    ILLabel label1 = cursor.DefineLabel();
                    cursor.Emit(OpCodes.Brfalse, label1);
                    cursor.Emit(OpCodes.Ldsfld, current);
                    cursor.Emit(OpCodes.Callvirt, normalUpdates);
                    ILLabel label2 = cursor.DefineLabel();
                    cursor.Emit(OpCodes.Brfalse, label2);
                    cursor.MarkLabel(label1);
                    cursor.Index++;
                    cursor.MarkLabel(label2);
                }
            };
            IL.Terraria.Player.Update += delegate (ILContext il)
            {
                ILCursor cursor = new(il);
                if (cursor.TryGotoNext(MoveType.AfterLabel, i => i.MatchLdsfld(typeof(Terraria.Main), nameof(Main.maxTilesX)), i => i.MatchLdcI4(4200)))
                {
                    cursor.Emit(OpCodes.Ldsfld, current);
                    ILLabel label1 = cursor.DefineLabel();
                    cursor.Emit(OpCodes.Brfalse, label1);
                    cursor.Emit(OpCodes.Ldsfld, current);
                    cursor.Emit(OpCodes.Callvirt, normalUpdates);
                    ILLabel label2 = cursor.DefineLabel();
                    cursor.Emit(OpCodes.Brfalse, label2);
                    cursor.MarkLabel(label1);
                    if (cursor.TryGotoNext(MoveType.After, i => i.MatchLdloc(3), i => i.MatchMul(), i => i.MatchStfld(typeof(Terraria.Player), nameof(Player.gravity))))
                    {
                        cursor.MarkLabel(label2);
                    }
                }
            };
            IL.Terraria.NPC.UpdateNPC_UpdateGravity += delegate (ILContext il)
            {
                ILCursor cursor = new(il);
                if (cursor.TryGotoNext(MoveType.AfterLabel, i => i.MatchLdsfld(typeof(Terraria.Main), "maxTilesX"), i => i.MatchLdcI4(4200)))
                {
                    cursor.Emit(OpCodes.Ldsfld, current);
                    ILLabel label1 = cursor.DefineLabel();
                    cursor.Emit(OpCodes.Brfalse, label1);
                    cursor.Emit(OpCodes.Ldsfld, current);
                    cursor.Emit(OpCodes.Callvirt, normalUpdates);
                    ILLabel label2 = cursor.DefineLabel();
                    cursor.Emit(OpCodes.Brfalse, label2);
                    cursor.MarkLabel(label1);
                    if (cursor.TryGotoNext(MoveType.After, i => i.MatchLdloc(1), i => i.MatchMul(), i => i.MatchStsfld(typeof(Terraria.NPC), "gravity")))
                    {
                        cursor.MarkLabel(label2);
                    }
                }
            };
            IL.Terraria.Liquid.Update += delegate (ILContext il)
            {
                ILCursor cursor = new(il);
                if (cursor.TryGotoNext(i => i.MatchLdarg(0), i => i.MatchLdfld(typeof(Terraria.Liquid), "y"), i => i.MatchCall(typeof(Terraria.Main), nameof(Main.UnderworldLayer))))
                {
                    cursor.Emit(OpCodes.Ldsfld, current);
                    ILLabel label1 = cursor.DefineLabel();
                    cursor.Emit(OpCodes.Brfalse, label1);
                    cursor.Emit(OpCodes.Ldsfld, current);
                    cursor.Emit(OpCodes.Callvirt, normalUpdates);
                    ILLabel label2 = cursor.DefineLabel();
                    cursor.Emit(OpCodes.Brfalse, label2);
                    cursor.MarkLabel(label1);
                    if (cursor.TryGotoNext(MoveType.After, i => i.MatchConvU1(), i => i.MatchStfld(typeof(Terraria.Tile), "liquid")))
                    {
                        cursor.MarkLabel(label2);
                    }
                }
            };
            IL.Terraria.Player.SavePlayer += delegate (ILContext il)
            {
                ILCursor cursor = new(il);
                if (cursor.TryGotoNext(i => i.MatchCall(typeof(Terraria.Player), "InternalSaveMap")))
                {
                    cursor.Index -= 3;
                    cursor.Emit(OpCodes.Ldsfld, cache);
                    ILLabel label1 = cursor.DefineLabel();
                    cursor.Emit(OpCodes.Brfalse, label1);
                    cursor.Emit(OpCodes.Ldsfld, cache);
                    cursor.Emit(OpCodes.Callvirt, shouldSave);
                    ILLabel label2 = cursor.DefineLabel();
                    cursor.Emit(OpCodes.Brfalse, label2);
                    cursor.MarkLabel(label1);
                    if (cursor.TryGotoNext(MoveType.AfterLabel, i => i.MatchLdsfld(typeof(Terraria.Main), nameof(Main.ServerSideCharacter))))
                    {
                        cursor.MarkLabel(label2);
                        cursor.Emit(OpCodes.Ldsfld, cache);
                        label1 = cursor.DefineLabel();
                        cursor.Emit(OpCodes.Brfalse, label1);
                        cursor.Emit(OpCodes.Ldsfld, cache);
                        cursor.Emit(OpCodes.Callvirt, typeof(Subworld).GetMethod("get_NoPlayerSaving"));
                        label2 = cursor.DefineLabel();
                        cursor.Emit(OpCodes.Brtrue, label2);
                        cursor.MarkLabel(label1);
                        if (cursor.TryGotoNext(MoveType.After, i => i.MatchCall(typeof(FileUtilities), nameof(FileUtilities.ProtectedInvoke))))
                        {
                            cursor.MarkLabel(label2);
                        }
                    }
                }
            };
            IL.Terraria.IO.WorldFile.SaveWorld_bool_bool += delegate (ILContext il)
            {
                ILCursor cursor = new(il);
                cursor.Emit(OpCodes.Ldsfld, cache);
                ILLabel label = cursor.DefineLabel();
                cursor.Emit(OpCodes.Brfalse, label);
                cursor.Emit(OpCodes.Ldsfld, cache);
                cursor.Emit(OpCodes.Callvirt, shouldSave);
                cursor.Emit(OpCodes.Brtrue, label);
                cursor.Emit(OpCodes.Ret);
                cursor.MarkLabel(label);
            };
            IL.Terraria.NetMessage.CheckBytes += delegate (ILContext il)
            {
                ILCursor cursor = new(il);
                if (cursor.TryGotoNext(MoveType.After, i => i.MatchCallvirt(typeof(Stream), "get_Position"), i => i.MatchStloc(5)))
                {
                    cursor.Emit(OpCodes.Ldsfld, typeof(Terraria.NetMessage).GetField("buffer"));
                    cursor.Emit(OpCodes.Ldarg_0);
                    cursor.Emit(OpCodes.Ldelem_Ref);
                    cursor.Emit(OpCodes.Ldloc_2);
                    cursor.Emit(OpCodes.Ldloc, 4);
                    cursor.Emit(OpCodes.Call, typeof(SubworldLibrary).GetMethod(nameof(SubworldLibrary.SendToSubservers), new Type[]
                    {
                        typeof(Terraria.MessageBuffer),
                        typeof(int),
                        typeof(int)
                    }));
                    ILLabel label = cursor.DefineLabel();
                    cursor.Emit(OpCodes.Brtrue, label);
                    if (cursor.TryGotoNext(i => i.MatchLdsfld(typeof(Terraria.NetMessage), nameof(NetMessage.buffer)), i => i.MatchLdarg(0), i => i.MatchLdelemRef(), i => i.MatchLdfld(typeof(Terraria.MessageBuffer), nameof(MessageBuffer.reader)), i => i.MatchCallvirt(typeof(BinaryReader), "get_BaseStream")))
                    {
                        cursor.MarkLabel(label);
                    }
                }
            };
            SubworldSystem.SetUp();
        }

        private void Main_DrawBackground(On.Terraria.Main.orig_DrawBackground orig, Main self)
        {
            if(!SubworldSystem.hideUnderworld)
            {
                orig(self);
            }
        }

        public void Unload()
        {
        }
        internal static event ILContext.Manipulator AsyncSend
        {
            add
            {
                HookEndpointManager.Modify(typeof(SocialSocket).GetMethod("Terraria.Net.Sockets.ISocket.AsyncSend", BindingFlags.Instance | BindingFlags.NonPublic), value);
                HookEndpointManager.Modify(typeof(TcpSocket).GetMethod("Terraria.Net.Sockets.ISocket.AsyncSend", BindingFlags.Instance | BindingFlags.NonPublic), value);
            }
            remove
            {
                HookEndpointManager.Unmodify(typeof(SocialSocket).GetMethod("Terraria.Net.Sockets.ISocket.AsyncSend", BindingFlags.Instance | BindingFlags.NonPublic), value);
                HookEndpointManager.Unmodify(typeof(TcpSocket).GetMethod("Terraria.Net.Sockets.ISocket.AsyncSend", BindingFlags.Instance | BindingFlags.NonPublic), value);
            }
        }
        public static bool SendToSubservers(Terraria.MessageBuffer buffer, int start, int length)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                return false;
            }
            else
            {
                if (buffer.readBuffer[start + 2] == 250 && ((ModNet.NetModCount < 256) ? buffer.readBuffer[start + 3] : BitConverter.ToUInt16(buffer.readBuffer, start + 3)) == Everglow.Instance.NetID)
                {
                    return false;
                }
                else
                {
                    if (!SubworldSystem.playerLocations.ContainsKey(Netplay.Clients[buffer.whoAmI].Socket.GetRemoteAddress()))
                    {
                        return false;
                    }
                    else
                    {
                        Netplay.Clients[buffer.whoAmI].TimeOutTimer = 0;
                        byte[] array = new byte[length + 1];
                        array[0] = (byte)buffer.whoAmI;
                        Buffer.BlockCopy(buffer.readBuffer, start, array, 1, length);
                        Task.Factory.StartNew(new Action<object>(SendToSubserversCallBack), array);
                        return array[3] != 2;
                    }
                }
            }
        }
        public static bool SendToSubservers(ISocket socket, byte[] data, int start, int length, ref object state)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                return false;
            }
            else
            {
                return SubworldSystem.playerLocations.ContainsKey(socket.GetRemoteAddress()) && state is not bool;
            }
        }
        private static void SendToSubserversCallBack(object data)
        {
            using (NamedPipeClientStream namedPipeClientStream = new(".", "World", PipeDirection.Out))
            {
                namedPipeClientStream.Connect();
                namedPipeClientStream.Write((byte[])data);
            }
        }
        private static void Sleep(Stopwatch stopwatch, double delta, ref double target)
        {
            double num = stopwatch.ElapsedMilliseconds;
            double num2 = target - num;
            target += delta;
            if (target < num)
            {
                target = num + delta;
            }
            if (num2 <= 0.0)
            {
                Thread.Sleep(0);
            }
            else
            {
                Thread.Sleep((int)num2);
            }
        }
        public static void HandlePacket(BinaryReader reader, int whoAmI)
        {
            switch (reader.ReadByte())
            {
                case 0:
                    {
                        if (Main.netMode == NetmodeID.Server)
                        {
                            ushort num = reader.ReadUInt16();
                            RemoteAddress remoteAddress = Netplay.Clients[whoAmI].Socket.GetRemoteAddress();
                            if (remoteAddress is not null)
                            {
                                new SubworldPacket()
                                    .Write(0)
                                    .Write(num)
                                    .Send(whoAmI, -1);
                                //ModPacket packet = Everglow.Instance.GetPacket(256);
                                //packet.Write(0);
                                //packet.Write(num);
                                //packet.Send(whoAmI, -1);
                                Main.player[whoAmI].active = false;
                                bool flag3 = !SubworldSystem.playerLocations.ContainsValue(num);
                                if (flag3)
                                {
                                    SubworldSystem.StartSubserver(SubworldSystem.subworlds[num].FullName);
                                }
                                SubworldSystem.playerLocations[remoteAddress] = num;
                            }
                        }
                        else
                        {
                            Netplay.Connection.State = 1;
                            SubworldSystem.current = SubworldSystem.subworlds[reader.ReadUInt16()];
                            Task.Factory.StartNew(new Action(SubworldSystem.ExitWorldCallBack));
                        }
                        break;
                    }
                case 1:
                    {
                        if (Main.netMode == NetmodeID.Server)
                        {
                            RemoteAddress remoteAddress2 = Netplay.Clients[whoAmI].Socket.GetRemoteAddress();
                            if (remoteAddress2 is not null)
                            {
                                Task.Factory.StartNew(new Action<object>(SubworldLibrary.SendToSubserversCallBack), new byte[]
                                {
                                    0,
                                    6,
                                    0,
                                    250,
                                    0,
                                    2,
                                    0
                                });
                                SubworldSystem.playerLocations.Remove(remoteAddress2);
                                Netplay.Clients[whoAmI].State = 0;
                                new SubworldPacket()
                                    .Write(1)
                                    .Send(whoAmI, -1);
                                //ModPacket packet2 = Everglow.Instance.GetPacket(256);
                                //packet2.Write(1);
                                //packet2.Send(whoAmI, -1);
                            }
                        }
                        else
                        {
                            Netplay.Connection.State = 1;
                            SubworldSystem.current = null;
                            Task.Factory.StartNew(new Action(SubworldSystem.ExitWorldCallBack));
                        }
                        break;
                    }
                case 2:
                    {
                        byte b = reader.ReadByte();
                        Main.player[b] = new Terraria.Player();
                        Terraria.RemoteClient remoteClient = Netplay.Clients[b];
                        remoteClient.IsActive = false;
                        remoteClient.Socket = null;
                        remoteClient.State = 0;
                        remoteClient.ResetSections();
                        remoteClient.SpamClear();
                        NetMessage.SyncDisconnectedPlayer(b);
                        bool flag = false;
                        for (int i = 0; i < 255; i++)
                        {
                            if (Netplay.Clients[i].State > 0)
                            {
                                flag = true;
                                break;
                            }
                        }
                        if (!flag)
                        {
                            Netplay.Disconnect = true;
                            Netplay.HasClients = false;
                        }
                        break;
                    }
            }
        }
    }
}
