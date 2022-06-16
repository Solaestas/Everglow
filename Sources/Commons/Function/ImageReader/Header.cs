using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Commons.Function.ImageReader
{
    internal struct Header
    {
        public int width;
        public int height;
        public bool red;
        public bool green;
        public bool blue;
        public bool alpha;
        public Header(BinaryReader reader)
        {
            width = reader.ReadInt32();
            height = reader.ReadInt32();
            BitVector32 bits = new BitVector32(reader.ReadInt32());
            red = bits[1];
            green = bits[1 << 1];
            blue = bits[1 << 2];
            alpha = bits[1 << 3];
        }
    }
}
