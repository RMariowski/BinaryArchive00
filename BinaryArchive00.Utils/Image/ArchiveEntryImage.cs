namespace BinaryArchive00.Utils.Image;

public record ArchiveEntryImage(
    ushort Width,
    ushort Height,
    byte[] PixelData,
    PixelFormat PixelFormat);
