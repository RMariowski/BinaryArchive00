namespace BinaryArchive00.Notebook;

[Collection(nameof(ArchivesFixture))]
public sealed class Archives
{
    [Fact]
    public void Extract_Unknown()
    {
        var data = _fixture.Archives.Select(archive => new object[]
            { archive.FileName, BitConverter.ToString(archive.Unknown) });
        SaveTable("Extracted/ca-list-u.txt", ["Name", "Unknown"], data.ToArray());
    }

    [Fact]
    public void Extract_Unknown2()
    {
        var data = _fixture.Archives.Select(archive => new object[]
            { archive.FileName, BitConverter.ToString(archive.Unknown2) });
        SaveTable("Extracted/ca-list-u2.txt", ["Name", "Unknown2"], data.ToArray());
    }

    [Fact]
    public void Extract_UnknownEnum()
    {
        var data = _fixture.Archives.Select(archive => new object[]
            { archive.FileName, BitConverter.ToString([(byte)archive.UnknownEnum]) });
        SaveTable("Extracted/ca-list-ue.txt", ["Name", "Unknown-Enum"], data.ToArray());
    }

    private readonly ArchivesFixture _fixture;

    public Archives(ArchivesFixture fixture)
    {
        _fixture = fixture;
    }
}
