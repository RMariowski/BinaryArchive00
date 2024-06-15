namespace BinaryArchive00.Tests;

[Collection(nameof(ArchivesFixture))]
public sealed class LangPatchCaTests
{
    private ArchiveFile Sut => _fixture.LangPatchCa;

    [Fact]
    public void Test()
    {
        Sut.IsPatch.Should().BeTrue();
        Sut.Entries.Should().HaveCount(16);
    }

    private readonly ArchivesFixture _fixture;

    public LangPatchCaTests(ArchivesFixture fixture)
    {
        _fixture = fixture;
    }
}
