namespace BinaryArchive00.Tests;

public abstract class CaFixtureBase : IDisposable
{
    public BinaryArchive BinaryArchive { get; }

    protected CaFixtureBase(string fileName)
    {
        string filePath = Path.Join(Consts.PathToFiles, fileName);
        BinaryArchive = new BinaryArchive(filePath);
    }

    public void Dispose()
    {
        BinaryArchive.Dispose();
        GC.SuppressFinalize(this);
    }
}
