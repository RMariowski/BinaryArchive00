using System.Runtime.InteropServices;
using BinaryArchive00.Utils.Image;

namespace BinaryArchive00.Utils;

public static class ArchiveEntryExtensions
{
    public static ArchiveEntryImage ToImage(this ArchiveEntry entry, PixelFormat pixelFormat)
    {
        if (entry.Type != "imag")
            throw new ArchiveEntryException("Entry is not type of image");

        entry.ReadContent();

        var width = BitConverter.ToUInt16(entry.Content!, 0);
        var height = BitConverter.ToUInt16(entry.Content!, 2);

        const byte imageHeaderSize = 20;
        var pixelBytes = entry.Content.AsSpan(imageHeaderSize, entry.Size - imageHeaderSize);
        var pixels = MemoryMarshal.Cast<byte, ushort>(pixelBytes);
        var uncompressedPixels = ResourceUncompressor.UncompressImage(pixels, width, height);

        var data = pixelFormat switch
        {
            PixelFormat.Argb4444 => MemoryMarshal.Cast<ushort, byte>(uncompressedPixels).ToArray(),
            PixelFormat.Rgba8888 => PixelFormatConverter.ConvertArgb4444ToRgba8888(uncompressedPixels),
            PixelFormat.Bgra8888 => PixelFormatConverter.ConvertArgb4444ToBgra8888(uncompressedPixels),
            _ => throw new ArgumentOutOfRangeException(nameof(pixelFormat), pixelFormat, null)
        };

        return new ArchiveEntryImage(width, height, data, pixelFormat);
    }
}
