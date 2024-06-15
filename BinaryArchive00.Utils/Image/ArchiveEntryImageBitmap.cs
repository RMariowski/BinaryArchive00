namespace BinaryArchive00.Utils.Image;

public static class ArchiveEntryImageBitmap
{
    private const int BytesPerPixel = 4;
    private const int HeaderInfoSizeInBytes = 56;

    public static byte[] AsBmpBytes(this ArchiveEntryImage image)
    {
        using var stream = image.AsBmpStream();
        return stream.ToArray();
    }

    public static MemoryStream AsBmpStream(this ArchiveEntryImage image)
    {
        var rawImageSize = image.Width * image.Height * BytesPerPixel;

        var stream = new MemoryStream(rawImageSize);
        var writer = new BinaryWriter(stream);
        writer.Write(GetHeaderBytes(rawImageSize));
        writer.Write(GetHeaderInfoBytes(image.Width, image.Height, rawImageSize));
        writer.Write(image.GetPixelDataFlipped());

        stream.Seek(0, SeekOrigin.Begin);
        return stream;
    }

    private static byte[] GetHeaderBytes(int rawImageSize)
    {
        var bytes = new byte[14];
        bytes[0] = 0x42; // B
        bytes[1] = 0x4D; // M

        var fileSize = (uint)(bytes.Length + HeaderInfoSizeInBytes + rawImageSize);
        var sizeBytes = BitConverter.GetBytes(fileSize);

        var pixelDataOffset = (uint)(bytes.Length + HeaderInfoSizeInBytes);
        var offset = BitConverter.GetBytes(pixelDataOffset);

        // BMP byte order is little endian
        if (!BitConverter.IsLittleEndian)
        {
            Array.Reverse(sizeBytes);
            Array.Reverse(offset);
        }

        sizeBytes.CopyTo(bytes, 2);
        offset.CopyTo(bytes, 10);

        return bytes;
    }

    private static byte[] GetHeaderInfoBytes(int imageWidth, int imageHeight, int rawImageSize)
    {
        var size = BitConverter.GetBytes(HeaderInfoSizeInBytes);
        var width = BitConverter.GetBytes(imageWidth);
        var height = BitConverter.GetBytes(imageHeight);
        var colorPlanes = BitConverter.GetBytes(1);
        var bitsPerPixel = BitConverter.GetBytes((short)32);
        var compressionMethod = BitConverter.GetBytes(3 /* BI_BITFIELDS */);
        var imageSize = BitConverter.GetBytes(rawImageSize);
        var horizontalPixelPerMeter = BitConverter.GetBytes(3780 /* 96 DPI */);
        var verticalPixelPerMeter = BitConverter.GetBytes(3780 /* 96 DPI */);
        var numberOfColorsInPalette = BitConverter.GetBytes(0);
        var numberOfImportantColorsUsed = BitConverter.GetBytes(0);
        var redChannelBitMaskBytes = BitConverter.GetBytes(0x00FF0000);
        var greenChannelBitMaskBytes = BitConverter.GetBytes(0x0000FF00);
        var blueChannelBitMaskBytes = BitConverter.GetBytes(0x000000FF);
        var alphaChannelBitMaskBytes = BitConverter.GetBytes(0xFF000000);

        // BMP byte order is little endian
        if (!BitConverter.IsLittleEndian)
        {
            Array.Reverse(size);
            Array.Reverse(width);
            Array.Reverse(height);
            Array.Reverse(colorPlanes);
            Array.Reverse(bitsPerPixel);
            Array.Reverse(compressionMethod);
            Array.Reverse(imageSize);
            Array.Reverse(horizontalPixelPerMeter);
            Array.Reverse(verticalPixelPerMeter);
            Array.Reverse(numberOfColorsInPalette);
            Array.Reverse(numberOfImportantColorsUsed);
            Array.Reverse(redChannelBitMaskBytes);
            Array.Reverse(greenChannelBitMaskBytes);
            Array.Reverse(blueChannelBitMaskBytes);
            Array.Reverse(alphaChannelBitMaskBytes);
        }

        var byteArray = new byte[HeaderInfoSizeInBytes];
        size.CopyTo(byteArray, 0);
        width.CopyTo(byteArray, 4);
        height.CopyTo(byteArray, 8);
        colorPlanes.CopyTo(byteArray, 12);
        bitsPerPixel.CopyTo(byteArray, 14);
        compressionMethod.CopyTo(byteArray, 16);
        imageSize.CopyTo(byteArray, 20);
        horizontalPixelPerMeter.CopyTo(byteArray, 24);
        verticalPixelPerMeter.CopyTo(byteArray, 28);
        numberOfColorsInPalette.CopyTo(byteArray, 32);
        numberOfImportantColorsUsed.CopyTo(byteArray, 36);
        redChannelBitMaskBytes.CopyTo(byteArray, 0x28);
        greenChannelBitMaskBytes.CopyTo(byteArray, 0x2C);
        blueChannelBitMaskBytes.CopyTo(byteArray, 0x30);
        alphaChannelBitMaskBytes.CopyTo(byteArray, 0x34);
        return byteArray;
    }

    // Get reversed order of rows.
    // For Bitmap image, pixel rows are stored from bottom to top.
    // So first row in bitmap file is the lowest row in Image.
    private static byte[] GetPixelDataFlipped(this ArchiveEntryImage image)
    {
        var totalPixels = image.Width * image.Height;
        var reversedBytes = new byte[totalPixels * BytesPerPixel];

        for (var row = 0; row < image.Height; row++)
        {
            var sourceIndex = row * image.Width * BytesPerPixel;
            var destinationIndex = (image.Height - row - 1) * image.Width * BytesPerPixel;
            Array.Copy(image.PixelData, sourceIndex, reversedBytes, destinationIndex, image.Width * BytesPerPixel);
        }

        return reversedBytes;
    }
}
