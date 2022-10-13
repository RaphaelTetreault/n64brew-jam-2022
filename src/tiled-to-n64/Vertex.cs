namespace TiledToN64
{
    public struct VertexA
        : ICSourcePrintable
    {
        // position
        public float px;
        public float py;
        public float pz;
        //
        public ushort flag;
        // uv
        public float u;
        public float v;
        // color
        public byte cr;
        public byte cg;
        public byte cb;
        public byte ca;

        public void Write(StreamWriter writer)
        {
            writer.Write("{");
            writer.Write($"{FloatToFixed10_5(px)}, ");
            writer.Write($"{FloatToFixed10_5(py)}, ");
            writer.Write($"{FloatToFixed10_5(pz)}, ");
            writer.Write($"{flag}, ");
            writer.Write($"{FloatNormalToByte(u)}, ");
            writer.Write($"{FloatNormalToByte(v)}, ");
            writer.Write($"{cr}, ");
            writer.Write($"{cg}, ");
            writer.Write($"{cb}, ");
            writer.Write($"{ca}}},");
        }

        public short FloatToFixed(float value, int fracBits)
        {
            short fixedValue = checked((short)(value * (1 << fracBits)));
            return fixedValue;
        }
        public short FloatToFixed10_5(float value)
        {
            return FloatToFixed(value, 5);
        }
        public byte FloatNormalToByte(float value)
        {
            if (value < 0f || value > 1f)
                throw new ArgumentException("Normal value out fo range 0f-1f");

            var byteValue = (byte)(value * byte.MaxValue);
            return byteValue;
        }
    }
}
