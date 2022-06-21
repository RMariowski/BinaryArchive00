namespace BinaryArchive00.Tests.LangPatchCa;

public sealed class LangPatchCaTests : IClassFixture<LangPatchCaFixture>
{
    private readonly BinaryArchive _binaryArchive;

    public LangPatchCaTests(LangPatchCaFixture fixture)
    {
        _binaryArchive = fixture.BinaryArchive;
    }

    [Fact]
    public void IsPatchFlagTest()
    {
        const bool expected = true;

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
        const int expected = 16;

        _binaryArchive.Entries.Should().HaveCount(expected);
    }
}
