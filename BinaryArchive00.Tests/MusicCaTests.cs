namespace BinaryArchive00.Tests;

[Collection(nameof(ArchivesFixture))]
public sealed class MusicCaTests
{
    private ArchiveFile Sut => _fixture.MusicCa;

    [Fact]
    public void Test()
    {
        Sut.IsPatch.Should().BeFalse();
        Sut.UnknownEnum.Should().Be(UnknownEnum.MusicCa);
        Sut.Entries.Should().HaveCount(15);
    }

    private readonly ArchivesFixture _fixture;

    public MusicCaTests(ArchivesFixture fixture)
    {
        _fixture = fixture;
    }
}
