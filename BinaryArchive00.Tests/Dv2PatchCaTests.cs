namespace BinaryArchive00.Tests;

[Collection(nameof(ArchivesFixture))]
public sealed class Dv2PatchCaTests
{
    private ArchiveFile Sut => _fixture.Dv2PatchCa;

    [Fact]
    public void Test()
    {
        Sut.IsPatch.Should().BeTrue();
        Sut.UnknownEnum.Should().Be(UnknownEnum.Other);
        Sut.Entries.Should().HaveCount(22);
    }

    private readonly ArchivesFixture _fixture;

    public Dv2PatchCaTests(ArchivesFixture fixture)
    {
        _fixture = fixture;
    }
}
