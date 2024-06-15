namespace BinaryArchive00.Tests;

[Collection(nameof(ArchivesFixture))]
public sealed class GiantglenCaTests
{
    private ArchiveFile Sut => _fixture.GiantglenCa;

    [Fact]
    public void Test()
    {
        Sut.IsPatch.Should().BeFalse();
        Sut.Entries.Should().HaveCount(9);
    }

    private readonly ArchivesFixture _fixture;

    public GiantglenCaTests(ArchivesFixture fixture)
    {
        _fixture = fixture;
    }
}
