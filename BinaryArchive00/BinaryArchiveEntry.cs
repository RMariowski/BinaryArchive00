namespace BinaryArchive00;

public class BinaryArchiveEntry
{
    public BinaryArchive BinaryArchive { get; }
    public string FileName { get; }
    public string Type { get; }
    public int Size { get; }
    public int Offset { get; }
    public byte[]? Content { get; private set; }

    public byte[] Unknown1 { get; init; } = Array.Empty<byte>();
    public int Unknown2 { get; init; }
    public short Unknown3 { get; init; }
    public bool MusicCa { get; init; }

    public bool Loaded => Content is not null;

    public BinaryArchiveEntry(BinaryArchive binaryArchive, string fileName, string type, int size, int offset)
    {
        BinaryArchive = binaryArchive;
        FileName = fileName;
        Type = type;
        Size = size;
        Offset = offset;
    }

    public void LoadContent(byte[]? buffer = null)
    {
        if (Loaded)
            return;

        BinaryArchive.FileStream.Seek(Offset, SeekOrigin.Begin);

        if (buffer is not null && buffer.Length != Size)
            throw new BinaryArchiveException("Length of passed buffer does not match entry size");

        Content = buffer ?? new byte[Size];
        int bytesRead = BinaryArchive.FileStream.Read(Content, 0, Size);
        if (bytesRead != Size)
            throw new BinaryArchiveException("Unable to read all bytes of content");
    }

    public void WriteContentTo(Stream stream, bool loadIfRequired = true)
    {
        if (loadIfRequired)
            LoadContent();

        if (Loaded is false)
            throw new BinaryArchiveException("Entry content is not loaded");

        using BinaryWriter writer = new(stream);
        writer.Write(Content!);
    }

    public void UnloadContent()
    {
        Content = null;
    }
}
