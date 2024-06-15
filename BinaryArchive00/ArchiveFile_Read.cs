using BinaryArchive00.Extensions;

namespace BinaryArchive00;

public partial class ArchiveFile
{
    private void Read(bool readEntriesContent)
    {
        using BinaryReader reader = new(Stream, Encoding.UTF8, true);
        ReadHead(reader);
        ReadEntries(reader, readEntriesContent);
    }

    private void ReadHead(BinaryReader reader)
    {
        var stream = reader.BaseStream;
        stream.Seek(0L, SeekOrigin.Begin);

        if (stream.Length < HeaderSize || reader.ReadBytes(16).SequenceEqual(Signature) is false)
            throw new NotBinaryArchiveException(FilePath);

        stream.Seek(4, SeekOrigin.Current); // Always zeroes
        IsPatch = reader.ReadBoolean();
        stream.Seek(3, SeekOrigin.Current); // Always zeroes
        EntriesOffset = reader.ReadInt32();
        stream.Seek(4, SeekOrigin.Current); // Always zeroes
        stream.Seek(14, SeekOrigin.Current); // Copyright notice (visible for maps)
        Unknown = reader.ReadBytes(2); // TODO: Bytes to check, only custom maps
        stream.Seek(8, SeekOrigin.Current); // Always zeroes
        Unknown2 = reader.ReadInt16(); // TODO: Bytes to check
        Unknown3 = reader.ReadInt32(); // TODO: Bytes to check
        Unknown4 = reader.ReadByte(); // TODO: Bytes to check

        if (reader.ReadBoolean() is false)
            throw new NotEndOfArchiveHeaderException();
    }

    private void ReadEntries(BinaryReader reader, bool readEntriesContent)
    {
        var stream = reader.BaseStream;
        stream.Seek(EntriesOffset, SeekOrigin.Begin);

        while (stream.Position < stream.Length)
        {
            var entry = ReadEntry(reader, readEntriesContent);
            _entries.Add(entry);
        }
    }

    private ArchiveEntry ReadEntry(BinaryReader reader, bool readEntriesContent)
    {
        var stream = reader.BaseStream;

        var name = reader.ReadBytesAndConvertToTrimmedUtf8String(16);
        var type = reader.ReadBytesAndConvertToTrimmedUtf8String(4).Reverse();
        var size = reader.ReadInt32();
        var offset = reader.ReadInt32();
        stream.Seek(20, SeekOrigin.Current); // Always zeroes
        var unknown = reader.ReadInt32(); // TODO: Bytes to check
        var unknown2 = reader.ReadInt16(); // TODO: Bytes to check
        var unknown3 = reader.ReadByte(); // TODO: Bytes to check

        var notEmpty = reader.ReadBoolean();
        if (size != 0 && notEmpty is false)
            throw new NotEmptyArchiveEntryException(FileName, name);

        stream.Seek(8, SeekOrigin.Current); // Always zeroes

        ArchiveEntry entry = new(this, name, type, size, offset, unknown, unknown2, unknown3);

        if (readEntriesContent is false)
            return entry;

        var endOfEntryPosition = stream.Position;
        entry.ReadContent();
        stream.Seek(endOfEntryPosition, SeekOrigin.Begin);

        return entry;
    }
}
