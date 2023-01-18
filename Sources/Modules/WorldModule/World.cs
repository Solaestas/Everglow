using ReLogic.Content;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.Utilities;

namespace Everglow.Sources.Modules.WorldModule
{
    internal abstract class World : ModType
    {
        public World(uint width, uint height, SaveType howsave)
        {
            Width = (int)width;
            Height = (int)height;
            HowSave = howsave;
        }
        protected sealed override void Register()
        {
            WorldManager.Register(this);
        }
        /// <summary>
        /// 联机同步务必检查此项在各端一致!
        /// </summary>
        public int Width { get; init; }
        /// <summary>
        /// 联机同步务必检查此项在各端一致!
        /// </summary>
        public int Height { get; init; }
        /// <summary>
        /// 只在单人模式或者服务端调用,注意检查
        /// </summary>
        public SaveType HowSave { get; init; }
        /// <summary>
        /// 是否使用自定义地图背景,启用后DrawCustomMap和PostDrawMapContent将被调用
        /// </summary>
        public bool UseCustomMap { get; init; }
        internal virtual string GetFilePath(WorldHistory history)
        {
            return HowSave switch
            {
                SaveType.PerWorld => Path.Combine(Main.WorldPath, nameof(SaveType.PerWorld), history.root.GetFileName(false), Mod.Name, Name + ".wld"),
                SaveType.PerPlayer => Path.Combine(Main.WorldPath, nameof(SaveType.PerPlayer), Main.ActivePlayerFileData.GetFileName(false), Mod.Name, Name + ".wld"),
                SaveType.Public => Path.Combine(Main.WorldPath, nameof(SaveType.Public), Mod.Name, Name + ".wld"),
                _ => "",
            };
        }
        /// <summary>
        /// 只在单人模式或者客户端调用,注意检查
        /// </summary>
        public virtual bool SavePlayer => false;
        /// <summary>
        /// 只在单人模式或者客户端调用,注意检查
        /// <br>在不保存地图时设置为true也不会保存</br>
        /// </summary>
        public virtual bool SaveMap => false;
        /// <summary>
        /// 保留地下判定,为false关闭地下,地狱等环境和背景判定
        /// </summary>
        public virtual bool EnableUnderground => false;
        /// <summary>
        /// 正常更新时间(昼夜交替),永昼永夜
        /// </summary>
        public virtual bool EnableUpdateTime => false;
        /// <summary>
        /// 正常刷怪,关闭后GloablNPC那个无效化
        /// </summary>
        public virtual bool EnableNPCSpawn => false;
        /// <summary>
        /// 正常天气,关闭后只有晴天
        /// </summary>
        public virtual bool EnableWeather => false;
        /// <summary>
        /// 正常更新物块和液体,关闭后植物不再生长,液体不更新位置,
        /// </summary>
        public virtual bool EnableUpdateTilesAndWater => true;
        /// <summary>
        /// 整合创建世界,我不喜欢GenTask,自己实现创建世界吧
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void CreateWorld(GameTime gameTime) { }
        /// <summary>
        /// 修改玩家重力,在原版基础重力结算完毕后
        /// </summary>
        /// <param name="player"></param>
        /// <param name="gravity"></param>
        public virtual void ModifyPlayerGravity(Player player, ref float gravity) { }
        /// <summary>
        /// 修改NPC重力,在原版基础重力结算完毕后
        /// </summary>
        /// <param name="npc"></param>
        /// <param name="gravity"></param>
        public virtual void ModifyNPCGravity(NPC npc, ref float gravity) { }
        /// <summary>
        /// 修改光照,我好像忘了一个fastrandom了,之后写钩子再改
        /// </summary>
        /// <param name="tile"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public virtual bool HijackTileLight(Tile tile, int x, int y, FastRandom fastRandom, ref Color color) => false;
        /// <summary>
        /// 自定义绘制地图背景和底板
        /// <br>应依次绘制两个部分</br>
        /// <br>地图背景</br>
        /// <br>地图底板</br>
        /// </summary>
        /// <param name="ContainerRange">底板的映射区域,参考Map.png的(40,4,848,240)范围</param>
        public virtual void DrawCustomMap(Rectangle ContainerRange) { }
        /// <summary>
        /// 在地图物块绘制完毕后调用,用于遮罩不可见区域
        /// </summary>
        /// <param name="ContentRange">与DrawCustomMap.ContainerRange一致</param>
        public virtual void PostDrawMapContent(Rectangle ContentRange) { }
        public virtual bool HijackPlayerBimoes(Player player, Point where) { return false; }
        public virtual void DrawScreenWhenEnterWorld(GameTime time) 
        {
            PlayerInput.SetZoom_Unscaled();
            Main.instance.GraphicsDevice.Clear(Color.Black);
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);
            Main.spriteBatch.DrawString(FontAssets.MouseText.Value, Main.statusText, new Vector2(Main.screenWidth, Main.screenHeight) / 2 - FontAssets.DeathText.Value.MeasureString(Main.statusText) / 2, Color.White);
            Main.DrawCursor(Main.DrawThickCursor());
            Main.spriteBatch.End();
        }
        //TODO 各种功能拓展

        /// <summary>
        /// 储存的方式
        /// </summary>
        public enum SaveType
        {
            /// <summary>
            /// 不储存
            /// </summary>
            None,
            /// <summary>
            /// 和世界绑定
            /// </summary>
            PerWorld,
            /// <summary>
            /// 和玩家绑定
            /// </summary>
            PerPlayer,
            /// <summary>
            /// 通用的
            /// </summary>
            Public
        }
    }
    class TestWorld : World
    {
        public TestWorld() : base(3000, 6000, SaveType.PerWorld) { }
    }
    class TestWorldCommand : ModCommand
    {
        public override string Command => "Test";

        public override CommandType Type => CommandType.World;

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if(WorldManager.Activing<TestWorld>())
            {
                Task.Factory.StartNew(() =>
                {
                    Main.NewText("将返回");
                    Thread.Sleep(5000);
                    WorldManager.TryBack(true);
                });
            }
            else
            {
                Task.Factory.StartNew(() =>
                {
                    Main.NewText("将进入");
                    Thread.Sleep(5000);
                    WorldManager.TryEnter<TestWorld>();
                });
            }
        }
    }
}