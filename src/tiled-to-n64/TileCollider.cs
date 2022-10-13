namespace TiledToN64
{
    public struct TileCollider
        : ICSourcePrintable
    {
        public byte layer;
        public byte height;

        public void Write(StreamWriter writer)
        {
            writer.Write($"0x{layer:X2}{height:X2}");
        }
    }
}
