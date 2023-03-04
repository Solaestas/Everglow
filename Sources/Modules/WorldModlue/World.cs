﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Modules.WorldModlue
{
    internal abstract class World : ModType
    {
        protected sealed override void Register()
        {
            WorldManager.Register(this);
        }
        /// <summary>
        /// 联机同步务必检查此项在各端一致!
        /// </summary>
        public abstract int Width { get; }
        /// <summary>
        /// 联机同步务必检查此项在各端一致!
        /// </summary>
        public abstract int Height { get; }
        /// <summary>
        /// 只在单人模式或者服务端调用,注意检查
        /// </summary>
        public virtual SaveType HowSave => SaveType.None;
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
        public virtual bool ModifyTileLight(Tile tile, int x, int y, ref Color color) => false;
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
}
