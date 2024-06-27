namespace BinaryArchive00.Utils.Image;

public static class PixelFormatConverter
{
    public static byte[] ConvertArgb4444ToRgba8888(ushort[] data)
    {
        ArgumentNullException.ThrowIfNull(data);

        // Each ushort (16 bits) will be expanded to 4 bytes (32 bits)
        var rgba32 = new byte[data.Length * 4];

        for (var i = 0; i < data.Length; i++)
        {
            var rgba16 = data[i];

            // Extract individual components (4 bits each)
            var a4 = (byte)((rgba16 >> 12) & 0x0F);
            var r4 = (byte)((rgba16 >> 8) & 0x0F);
            var g4 = (byte)((rgba16 >> 4) & 0x0F);
            var b4 = (byte)(rgba16 & 0x0F);

            var index = i * 4;
            rgba32[index] = (byte)(r4 * 17);
            rgba32[index + 1] = (byte)(g4 * 17);
            rgba32[index + 2] = (byte)(b4 * 17);
            rgba32[index + 3] = (byte)(a4 * 17);
        }

        return rgba32;
    }
    
    public static byte[] ConvertArgb4444ToBgra8888(ushort[] data)
    {
        ArgumentNullException.ThrowIfNull(data);

        // Each ushort (16 bits) will be expanded to 4 bytes (32 bits)
        var rgba32 = new byte[data.Length * 4];

        for (var i = 0; i < data.Length; i++)
        {
            var rgba16 = data[i];

            // Extract individual components (4 bits each)
            var a4 = (byte)((rgba16 >> 12) & 0x0F);
            var r4 = (byte)((rgba16 >> 8) & 0x0F);
            var g4 = (byte)((rgba16 >> 4) & 0x0F);
            var b4 = (byte)(rgba16 & 0x0F);

            var index = i * 4;
            rgba32[index] = (byte)(b4 * 17);
            rgba32[index + 1] = (byte)(g4 * 17);
            rgba32[index + 2] = (byte)(r4 * 17);
            rgba32[index + 3] = (byte)(a4 * 17);
        }

        return rgba32;
    }
}
