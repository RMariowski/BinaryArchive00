using System.Runtime.InteropServices;
using BinaryArchive00.Utils.Image;

namespace BinaryArchive00.Utils;

public static class ArchiveEntryExtensions
{
    public static ArchiveEntryImage ToImage(this ArchiveEntry entry, bool convertToRgba32 = true)
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

        var data = convertToRgba32
            ? PixelFormatConverter.ConvertRgba16ToRgba32(uncompressedPixels)
            : MemoryMarshal.Cast<ushort, byte>(uncompressedPixels).ToArray();

        return new ArchiveEntryImage(width, height, data);
    }
}
