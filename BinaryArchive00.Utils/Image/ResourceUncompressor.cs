namespace BinaryArchive00.Utils.Image;

public static class ResourceUncompressor
{
    public static ushort[] UncompressImage(Span<ushort> input, ushort imageWidth, ushort imageHeight)
    {
        var output = new ushort[imageWidth * imageHeight];

        // i = input position, o = output position
        for (int i = 0, o = 0; i < input.Length;)
        {
            var value = input[i];
            i++;

            if (value < 0x100)
            {
                var count = 2 * value + 2;
                var carry = count & 3;

                // Direct copy, divide by 2 as working in 16 bits
                for (var j = 0; j < count / 2; j++)
                    output[o + j] = input[i + j];

                // Append an extra n bytes
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
                var index = value >> 8;
                var count = (value & 0xFF) + 1;

                // Invalid index, read the next uint16
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

                // Back reference using a relative index
                for (var j = 0; j < count; j++, o++)
                    output[o] = output[o - index];
            }
        }

        return output;
    }
}
