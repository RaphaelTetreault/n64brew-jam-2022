namespace TiledToN64
{
    public struct GameObject
        : ICSourcePrintable
    {
        public byte id;
        public byte unk;
        public float px;
        public float py;
        public float pz;

        public void Write(StreamWriter writer)
        {
            writer.Write("{");
            writer.Write($"0x{id:X2}, ");
            writer.Write($"0x{unk:X2}, ");
            writer.Write($"{px:0.}f, ");
            writer.Write($"{py:0.}f, ");
            writer.Write($"{pz:0.}f");
            writer.Write("}},");
        }
    }
}
