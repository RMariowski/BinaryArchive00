namespace BinaryArchive00;

public partial class ArchiveFile : IDisposable
{
    private const int HeaderSize = 64;

    private static readonly byte[] Signature =
        [98, 105, 110, 97, 114, 121, 46, 97, 114, 99, 104, 105, 118, 101, 48, 48]; // binary.archive00

    private readonly List<ArchiveEntry> _entries = [];
    private bool _disposed;

    public Stream Stream { get; }
    public bool IsStreamOwner { get; }
    public string? FilePath { get; }
    public string? FileName { get; }

    public bool IsPatch { get; private set; }
    public int EntriesOffset { get; private set; }
    public byte[] Unknown { get; private set; } = [];
    public short Unknown2 { get; private set; }
    public int Unknown3 { get; private set; }
    public byte Unknown4 { get; private set; }

    public IReadOnlyList<ArchiveEntry> Entries => _entries;

    public ArchiveFile(string filePath, bool readEntriesContent = true)
        : this(
            stream: File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read),
            leaveOpen: false,
            readEntriesContent)
    {
    }

    public ArchiveFile(FileStream stream, bool leaveOpen = false, bool readEntriesContent = true)
    {
        ArgumentNullException.ThrowIfNull(stream);

        if (!stream.CanSeek)
        {
            throw new ArgumentException("Stream is not seekable", nameof(stream));
        }

        Stream = stream;
        IsStreamOwner = !leaveOpen;
        FilePath = stream.Name;
        FileName = Path.GetFileName(FilePath);

        Read(readEntriesContent);
    }

    public ArchiveFile(Stream stream, bool leaveOpen = false, bool readEntriesContent = true)
    {
        ArgumentNullException.ThrowIfNull(stream);

        if (!stream.CanSeek)
        {
            throw new ArgumentException("Stream is not seekable", nameof(stream));
        }

        Stream = stream;
        IsStreamOwner = !leaveOpen;
        FilePath = null;
        FileName = null;

        Read(readEntriesContent);
    }

    ~ArchiveFile()
        => Dispose(disposing: false);

    public static ArchiveFile Open(string filePath, bool readEntriesContent = true)
        => new(filePath, readEntriesContent);

    public static ArchiveFile Open(FileStream stream, bool leaveOpen = false, bool readEntriesContent = true)
        => new(stream, leaveOpen, readEntriesContent);

    public static ArchiveFile Open(Stream stream, bool leaveOpen = false, bool readEntriesContent = true)
        => new(stream, leaveOpen, readEntriesContent);

    public void Close()
        => Dispose();

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
        {
            if (IsStreamOwner)
            {
                Stream.Dispose();
            }
        }

        _disposed = true;
    }
}
