namespace BinaryArchive00;

public record ArchiveEntry(
    ArchiveFile Archive,
    string Name,
    string Type,
    int Size,
    int Offset,
    byte[] Unknown,
    byte Unknown2
)
{
    public byte[]? Content { get; private set; }
    
    public void ReadContent()
    {
        if (Content is not null)
            return;

        Archive.Stream.Seek(Offset, SeekOrigin.Begin);
        Content = new byte[Size];
        if (Archive.Stream.Read(Content, 0, Size) != Size)
            throw new UnableToReadWholeArchiveEntryException(Name);
    }

    public async Task ReadContentAsync()
    {
        if (Content is not null)
            return;

        Archive.Stream.Seek(Offset, SeekOrigin.Begin);
        Content = new byte[Size];
        if (await Archive.Stream.ReadAsync(Content.AsMemory(0, Size)) != Size)
            throw new UnableToReadWholeArchiveEntryException(Name);
    }
}
