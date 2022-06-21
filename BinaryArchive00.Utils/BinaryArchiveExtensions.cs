using System.Runtime.InteropServices;

namespace BinaryArchive00.Utils;

public static class BinaryArchiveEntryExtensions
{
    public static BinaryArchiveImage ExtractImage(this BinaryArchiveEntry entry, bool convertToRgba32 = true)
    {
        if (entry.Type != "gami")
            throw new BinaryArchiveException("Entry is not type of image");

        entry.LoadContent();

        const byte imageHeaderSize = 20;
        var inputBytes = entry.Content.AsSpan(imageHeaderSize, entry.Size - imageHeaderSize);
        var input = MemoryMarshal.Cast<byte, ushort>(inputBytes);

        var width = BitConverter.ToUInt16(entry.Content!, 0);
        var height = BitConverter.ToUInt16(entry.Content!, 2);

        ushort[] output = UncompressImage(input, width, height);

        byte[] data = convertToRgba32
            ? PixelFormatConverter.FromRgba16ToRgba32(output)
            : MemoryMarshal.Cast<ushort, byte>(output).ToArray();

        return new BinaryArchiveImage(width, height, data);
    }

    private static ushort[] UncompressImage(Span<ushort> input, ushort imageWidth, ushort imageHeight)
    {
        var output = new ushort[imageWidth * imageHeight];

        // i = input position, o = output position
        for (int i = 0, o = 0; i < input.Length;)
        {
            ushort value = input[i];
            i++;

            if (value < 0x100)
            {
                int count = 2 * value + 2;
                int carry = count & 3;

                // direct copy, divide by 2 as working in 16 bits
                for (var j = 0; j < count / 2; j++)
                    output[o + j] = input[i + j];

                // append an extra n bytes
                output[o + value] = carry switch
                {
                    2 => input[i + value],
                    1 => (ushort)(input[i + value] & 0xFF),
                    _ => output[o + value]
                };

                i += value + 1;
                o += value + 1;
            }
            else
            {
                int index = value >> 8;
                int count = (value & 0xFF) + 1;

                // invalid index, read the next uint16
                if (index == 0xFF)
                {
                    index = input[i];
                    i++;
                }

                // "big count" flag, read the next uint32
                if (count == 0x100)
                {
                    count = input[i] | (input[i + 1] << 16);
                    i += 2;
                }

                // back reference using a relative index
                for (var j = 0; j < count; j++, o++)
                    output[o] = output[o - index];
            }
        }

        return output;
    }
}
