namespace BinaryArchive00.Tests.LanguageCa;

public sealed class LanguageCaTests : IClassFixture<LanguageCaFixture>
{
    private readonly BinaryArchive _binaryArchive;

    public LanguageCaTests(LanguageCaFixture fixture)
    {
        _binaryArchive = fixture.BinaryArchive;
    }

    [Fact]
    public void IsPatchFlagTest()
    {
        const bool expected = false;

        _binaryArchive.IsPatch.Should().Be(expected);
    }

    [Fact]
    public void MusicCaFlagTest()
    {
        const bool expected = false;

        _binaryArchive.MusicCa.Should().Be(expected);
        foreach (var entry in _binaryArchive.Entries)
            entry.MusicCa.Should().Be(expected);
    }

    [Fact]
    public void EntriesCountTest()
    {
        const int expected = 290;

        _binaryArchive.Entries.Should().HaveCount(expected);
    }
}
