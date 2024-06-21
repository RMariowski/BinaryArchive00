namespace BinaryArchive00.Tests;

[Collection(nameof(ArchivesFixture))]
public sealed class TestProfileCaTests
{
    private ArchiveFile Sut => _fixture.TestProfileCa;

    [Fact]
    public void Test()
    {
        Sut.IsPatch.Should().BeFalse();
        Sut.UnknownEnum.Should().Be(UnknownEnum.CustomMapOrProfile);
        Sut.Entries.Should().HaveCount(2);
    }

    private readonly ArchivesFixture _fixture;

    public TestProfileCaTests(ArchivesFixture fixture)
    {
        _fixture = fixture;
    }
}
