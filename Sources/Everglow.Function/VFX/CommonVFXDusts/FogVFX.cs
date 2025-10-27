using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Everglow.Commons;
using Everglow.Commons.DataStructures;
using Everglow.Commons.Graphics;
using Everglow.Commons.Utilities;
using Everglow.Commons.Vertex;
using Everglow.Commons.VFX;
using Microsoft.Xna.Framework.Graphics.PackedVector;

namespace Everglow.Commons.VFX.CommonVFXDusts;
public class FogVFX : MEACVFX
{
    public override void SetDefault()
    {
        maxTimeleft = 60;
        texPath = ModAsset.FBM_Mod;
        alpha = 0;
       
    }
    public bool substract = false;
    public override void AI()
    {
        if (ai0 == 0)
        {
            Velocity *= 0.95f;
            if (ai1 == 0)
			{
                ai1 = Main.rand.NextFloatDirection() * 0.05f;
			}
            rotation += ai1;
            if (timeleft > 15)
                MathHelper.Lerp(alpha,1f, 0.06f);
            else
				MathHelper.Lerp(alpha, 0f, 0.06f);
        }
        if (ai0 == 1)
        {
            Velocity *= 0.95f;
            if (ai1 == 0)
            {
                ai1 = Main.rand.NextFloatDirection() * 0.05f;
            }
            rotation += ai1;
            if (timeleft > maxTimeleft - 20)
            {
				MathHelper.Lerp(alpha,1f, 0.1f);
            }
            else
				MathHelper.Lerp(alpha, 0f, 0.015f);
        }
        
    }
    public override void Draw()
    {
        //Main.NewText("Draw", Color.Red);
        //base.Draw();
        //substract = false;
        if (substract)
        {
            
            Ins.Batch.End();
            Ins.Batch.Begin(CustomBlendStates.Subtract);
        }

        
        Ins.Batch.Draw(Texture, Center-Main.screenPosition, null, drawColor * alpha, rotation, Texture.Size() / 2, scale, SpriteEffects.None);
        /*
        Vector2 position = Center - Main.screenPosition;
        Color color = drawColor;
        
        Vertex2D[] bars = new Vertex2D[]
        {
            new Vertex2D(position, color, new(0,0,0)),
            new Vertex2D(position+new Vector2(100,0), color,  new(1,0,0)),
            new Vertex2D(position+new Vector2(0,100), color,  new(0,1,0)),
            new Vertex2D(position+new Vector2(100,100), color,  new(1,1,0))
        };
        Ins.Batch.BindTexture<Vertex2D>(Texture).Draw(bars, PrimitiveType.TriangleStrip);*/

        if (substract)
        {
            Ins.Batch.End();
            Ins.Batch.Begin(BlendState.AlphaBlend);
        }
    }
    private struct VFX2D : IVertexType
    {
        public Color color;

        public Vector2 position;

        public Vector2 texCoord;

        public VFX2D(Vector2 position, Color color, Vector2 texCoord)
        {
            this.position = position;
            this.color = color;
            this.texCoord = texCoord;
        }

        public VertexDeclaration VertexDeclaration => new(
                                            new VertexElement(0, VertexElementFormat.Vector2, VertexElementUsage.Position, 0),
            new VertexElement(8, VertexElementFormat.Color, VertexElementUsage.Color, 0),
            new VertexElement(12, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0));
    }
}
