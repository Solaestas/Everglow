
namespace Everglow.Sources.Commons.Core.VFX
{
    public class VFXBatch : IDisposable
    {
        private struct VFX2D : IVertexType
        {
            public VertexDeclaration VertexDeclaration => new VertexDeclaration(
                new VertexElement(0, VertexElementFormat.Vector2, VertexElementUsage.Position, 0),
                new VertexElement(8, VertexElementFormat.Color, VertexElementUsage.Color, 0),
                new VertexElement(12, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0));
            public Vector2 position;
            public Color color;
            public Vector2 texCoord;
        }
        private static class Buffer<T> where T : IVertexType
        {
            public class Buffers : IDisposable
            {
                public DynamicVertexBuffer vertexBuffer;
                public DynamicIndexBuffer indexBuffer;
                public T[] vertices;
                public ushort[] indices;
                public ushort vertexPosition;
                public ushort indexPosition;
                public void Dispose()
                {
                    vertexBuffer.Dispose();
                    indexBuffer.Dispose();
                    GC.SuppressFinalize(this);
                }
            }
            private static Buffers instance;
            public static Buffers Instance;
            public static DynamicVertexBuffer VertexBuffer => instance.vertexBuffer;
            public static DynamicIndexBuffer IndexBuffer => instance.indexBuffer;
            public static T[] Vertices => instance.vertices;
            public static ushort[] Indices => instance.indices;
            public static Buffers Create()
            {
                Debug.Assert(instance != null, "Can't Create Twice");
                instance = new Buffers();
                return instance;
            }
        }

        //numbers Copy from SpriteBatch
        private const int MAX_VERTICES = 8192;
        private const int MAX_INDICES = 12288;
        private GraphicsDevice graphicsDevice;
        private Texture[] textures;
        private List<IDisposable> buffers = new List<IDisposable>();
        public GraphicsDevice GraphicsDevice => graphicsDevice;
        public VFXBatch(GraphicsDevice gd)
        {
            graphicsDevice = gd;
            RegisterVertex<VFX2D>();
            GenerateIndices(Buffer<VFX2D>.Indices);
            Buffer<VFX2D>.IndexBuffer.SetData(Buffer<VFX2D>.Indices);
        }
        public void RegisterVertex<T>(int maxVertices = MAX_VERTICES, int maxIndices = MAX_INDICES) where T : IVertexType
        {
            var buffer = Buffer<T>.Create();
            buffer.vertices = new T[maxVertices];
            buffer.indices = new ushort[maxIndices];
            buffer.vertexBuffer = new DynamicVertexBuffer(graphicsDevice, typeof(T), maxVertices, BufferUsage.WriteOnly);
            buffer.indexBuffer = new DynamicIndexBuffer(graphicsDevice, IndexElementSize.SixteenBits, maxIndices, BufferUsage.WriteOnly);
            buffers.Add(buffer); 
        }
        private static void GenerateIndices(ushort[] indices)
        {
            int index = -1;
            ushort num = 0;
            while(index + 3 < indices.Length)
            {
                indices[++index] = num;
                indices[++index] = (ushort)(num + 1);
                indices[++index] = (ushort)(num + 2);
                ++num;
            }
        }
        public void Begin<T>(params Texture[] textures) where T : IVertexType
        {
            Begin<T>(BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, textures);
        }
        public void Begin<T>(BlendState blendState, params Texture[] textures) where T : IVertexType
        {
            Begin<T>(blendState, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, textures);
        }
        public void Begin<T>(BlendState blendState, SamplerState samplerState, DepthStencilState depthStencilState, RasterizerState rasterizerState, params Texture[] textures) where T : IVertexType
        {
            this.textures = textures;
            graphicsDevice.RasterizerState = rasterizerState;
            graphicsDevice.DepthStencilState = depthStencilState;
            graphicsDevice.SamplerStates[0] = samplerState;
            graphicsDevice.BlendState = blendState;
            graphicsDevice.SetVertexBuffer(Buffer<T>.VertexBuffer);
            graphicsDevice.Indices = Buffer<T>.IndexBuffer;
        }
        public void Draw(Vector2 position, Color color)
        {
            var buffer = Buffer<VFX2D>.Instance;
            var vertex = buffer.vertices;
            var texture = textures[0];
         
        }
        public void Draw(Vector2 position, Rectangle? sourceRectangle, Color color)
        {

        }
        public void Draw(Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects)
        {

        }
        public void Draw(Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects)
        {

        }
        public void Draw(Rectangle destinationRectangle, Color color)
        {

        }
        public void Draw(Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color)
        {

        }
        public void Draw(Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effects)
        {

        }
        public void Flush()
        {

        }
        public void End<T>()
        {

        }

        public void Dispose()
        {
            foreach(var buffer in buffers)
            {
                buffer.Dispose();
            }
            GC.SuppressFinalize(this);
        }
    }
}
