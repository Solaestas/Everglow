using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.Xna.Framework;

namespace Everglow.Sources.Commons.Core.Utils
{
    internal struct SpriteBatchState
    {
        public SpriteSortMode SortMode
        {
            get;set;
        }
        public BlendState BlendState
        {
            get; set;
        }
        public SamplerState SamplerState
        {
            get; set;
        }
        public DepthStencilState DepthStencilState
        {
            get; set;
        }
        public RasterizerState RasterizerState
        {
            get; set;
        }
    }
    internal static class RenderingUtils
    {
        public static SpriteBatchState SaveSpriteBatchState(SpriteBatch sb)
        {
            var type = typeof(SpriteBatch);
            return new SpriteBatchState
            {
                SortMode = (SpriteSortMode)type.GetField("sortMode", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(sb),
                BlendState = (BlendState)type.GetField("blendState", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(sb),
                SamplerState = (SamplerState)type.GetField("samplerState", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(sb),
                DepthStencilState = (DepthStencilState)type.GetField("depthStencilState", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(sb),
                RasterizerState = (RasterizerState)type.GetField("rasterizerState", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(sb)
            };
        }

        public static void RestoreSpriteBatchState(SpriteBatch sb, SpriteBatchState state)
        {
            sb.Begin(state.SortMode, state.BlendState, state.SamplerState, state.DepthStencilState, state.RasterizerState);
        }
    }
}
