namespace BinaryArchive00.Extensions;

public static class BinaryReaderExtensions
{
    public static string ReadBytesAndConvertToTrimmedUtf8String(this BinaryReader binaryReader, int bytesCount)
    {
        return Encoding.UTF8.GetString(binaryReader.ReadBytes(bytesCount)).TrimEnd(' ', '\0');
    }
}
