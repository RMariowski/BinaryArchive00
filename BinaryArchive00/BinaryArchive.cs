namespace BinaryArchive00;

public class BinaryArchive : IDisposable, IAsyncDisposable
{
    private List<BinaryArchiveEntry> _entries = new();

    public FileStream FileStream { get; }
    public string FileName { get; private set; } = string.Empty;
    public bool IsPatch { get; private set; }
    public int EntriesOffset { get; private set; }
    public short Unknown1 { get; private set; }
    public int Unknown2 { get; private set; }
    public bool MusicCa { get; private set; }

    public IReadOnlyList<BinaryArchiveEntry> Entries => _entries;

    public BinaryArchive(FileStream fileStream)
    {
        FileStream = fileStream;

        using var reader = new BinaryReader(FileStream, Encoding.UTF8, true);
        ReadHeader(reader);
        ReadEntries(reader);
    }

    public BinaryArchive(string filePath)
        : this(File.Open(filePath, FileMode.Open))
    {
    }

    private void ReadHeader(BinaryReader reader)
    {
        FileName = Path.GetFileName(FileStream.Name);

        FileStream.Seek(0L, SeekOrigin.Begin);

        byte[] fileType = reader.ReadBytes(16);
        if (Encoding.UTF8.GetString(fileType) != "binary.archive00")
            throw new BinaryArchiveException($"{FileStream.Name} file is not a binary.archive00 file");

        FileStream.Seek(4, SeekOrigin.Current); // Always zeroes
        IsPatch = reader.ReadBoolean();
        FileStream.Seek(3, SeekOrigin.Current); // Always zeroes
        EntriesOffset = reader.ReadInt32();
        FileStream.Seek(28, SeekOrigin.Current); // Always zeroes
        Unknown1 = reader.ReadInt16();
        Unknown2 = reader.ReadInt32();
        MusicCa = reader.ReadByte() == 192; // 192 = music.ca | 193 = other
        bool end = reader.ReadBoolean();
        if (end is false)
            throw new BinaryArchiveException("Not end of archive file");
    }

    private void ReadEntries(BinaryReader reader)
    {
        FileStream.Seek(EntriesOffset, SeekOrigin.Begin);

        _entries.Clear();
        while (FileStream.Position < FileStream.Length)
        {
            string fileName = Encoding.UTF8.GetString(reader.ReadBytes(16)).TrimEnd(' ', '\0');
            string type = Encoding.UTF8.GetString(reader.ReadBytes(4)).TrimEnd(' ', '\0');
            int fileSize = reader.ReadInt32();
            int fileBodyOffset = reader.ReadInt32();

            byte[] unknown1 = reader.ReadBytes(20);
            int unknown2 = reader.ReadInt32();
            short unknown3 = reader.ReadInt16();
            bool musicCa = reader.ReadByte() == 192; // 192 = music.ca | 193 = other
            bool end = reader.ReadBoolean();
            if (end is false)
                throw new BinaryArchiveException("Not end of archive entry file");

            FileStream.Seek(8, SeekOrigin.Current); // Entry separator

            BinaryArchiveEntry entry = new(this, fileName, type, fileSize, fileBodyOffset)
            {
                Unknown1 = unknown1,
                Unknown2 = unknown2,
                Unknown3 = unknown3,
                MusicCa = musicCa
            };
            _entries.Add(entry);
        }

        _entries = _entries.OrderBy(e => e.FileName).ToList();
    }

    public void Dispose()
    {
        FileStream.Dispose();
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        await FileStream.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}
