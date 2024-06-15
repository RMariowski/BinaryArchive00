namespace BinaryArchive00.Tests;

[Collection(nameof(ArchivesFixture))]
public sealed class Dv2CaTests
{
    private ArchiveFile Sut => _fixture.Dv2Ca;

    [Fact]
    public void Test()
    {
        Sut.IsPatch.Should().BeFalse();
        Sut.Entries.Should().HaveCount(29938);
    }

    private readonly ArchivesFixture _fixture;

    public Dv2CaTests(ArchivesFixture fixture)
    {
        _fixture = fixture;
    }
}
