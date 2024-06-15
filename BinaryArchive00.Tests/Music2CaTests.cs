namespace BinaryArchive00.Tests;

[Collection(nameof(ArchivesFixture))]
public sealed class Music2CaTests
{
    private ArchiveFile Sut => _fixture.Music2Ca;

    [Fact]
    public void Test()
    {
        Sut.IsPatch.Should().BeFalse();
        Sut.Entries.Should().HaveCount(3);
    }

    private readonly ArchivesFixture _fixture;

    public Music2CaTests(ArchivesFixture fixture)
    {
        _fixture = fixture;
    }
}
