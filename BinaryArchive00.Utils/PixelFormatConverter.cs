namespace BinaryArchive00.Utils;

public static class PixelFormatConverter
{
    public static byte[] FromRgba16ToRgba32(ushort[] rgba16)
    {
        if (rgba16 is null)
            throw new ArgumentNullException(nameof(rgba16));

        var rgba32 = new byte[rgba16.Length * 4];
        for (int i32 = 0, i16 = 0; i16 < rgba16.Length; i32 += 4, i16++)
        {
            byte[] bytes = BitConverter.GetBytes(rgba16[i16]);

            rgba32[i32] = (byte)((bytes[0] & 0x0f) << 4);
            rgba32[i32 + 1] = (byte)(bytes[0] & 0xf0);
            rgba32[i32 + 2] = (byte)((bytes[1] & 0x0f) << 4);
            rgba32[i32 + 3] = (byte)(bytes[1] & 0xf0);
        }

        return rgba32;
    }

    public static byte[] FromRgba16ToRgba32(byte[] rgba16)
    {
        if (rgba16 is null)
            throw new ArgumentNullException(nameof(rgba16));

        var rgba32 = new byte[rgba16.Length * 2];
        for (int p = 0, p2 = 0; p2 < rgba16.Length; p += 4, p2 += 2)
        {
            rgba32[p] = (byte)((rgba16[p2] & 0x0f) << 4);
            rgba32[p + 1] = (byte)(rgba16[p2] & 0xf0);
            rgba32[p + 2] = (byte)((rgba16[p2 + 1] & 0x0f) << 4);
            rgba32[p + 3] = (byte)(rgba16[p2 + 1] & 0xf0);
        }

        return rgba32;
    }
}
