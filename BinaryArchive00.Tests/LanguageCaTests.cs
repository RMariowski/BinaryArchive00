namespace BinaryArchive00.Tests;

[Collection(nameof(ArchivesFixture))]
public sealed class LanguageCaTests
{
    private ArchiveFile Sut => _fixture.LanguageCa;

    [Fact]
    public void Test()
    {
        Sut.IsPatch.Should().BeFalse();
        Sut.Entries.Should().HaveCount(290);
    }

    private readonly ArchivesFixture _fixture;

    public LanguageCaTests(ArchivesFixture fixture)
    {
        _fixture = fixture;
    }
}
