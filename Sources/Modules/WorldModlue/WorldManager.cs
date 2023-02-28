using Everglow.Sources.Commons.Function.NetUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Everglow.Sources.Modules.WorldModlue
{
    internal static class WorldManager
    {
        static bool AnyTryEnterInProgress;
        static Dictionary<string, World> worlddic = new();
        /// <summary>
        /// activing不为null时此项不应为null
        /// </summary>
        static WorldHistory currenthistory;
        /// <summary>
        /// 当前处于的世界
        /// </summary>
        static World activing;
        internal static void Register(World world)
        {
            ModTypeLookup<World>.Register(world);
            worlddic[world.FullName] = world;
        }
        internal static ProgressToken TryEnter<T>(Action<ProgressToken.State> whenInvalid = null) where T : World
        {
            ProgressToken token = new ProgressToken();
            if (whenInvalid is not null)
            {
                token.WhenInvalid += whenInvalid;
            }
            if(!AnyTryEnterInProgress)
            {
                try
                {
                    if (worlddic.TryGetValue(ModContent.GetInstance<T>().FullName, out World target))
                    {
                        Task.Factory.StartNew(() => Enter(target, token));
                    }
                    else
                    {
                        token.Exception(new KeyNotFoundException(ModContent.GetInstance<T>().FullName));
                    }
                }
                catch(Exception e)
                {
                    token.Exception(e);
                }
            }
            else
            {
                token.StopByOther("There is a world occupied in processing");
            }
            return token;
        }
        static void Enter(World world, ProgressToken token)
        {
            try
            {
                //TODO 进入世界
            }
            catch when(token.IsCancelled)
            {
            }
            catch(Exception e)
            {
                token.Exception(e);
            }
        }
    }
}
