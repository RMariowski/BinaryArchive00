namespace BinaryArchive00.Tests.GiantglenCa;

public sealed class GiantglenCaTests : IClassFixture<GiantglenCaFixture>
{
    private readonly BinaryArchive _binaryArchive;

    public GiantglenCaTests(GiantglenCaFixture fixture)
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
        const int expected = 9;

        _binaryArchive.Entries.Should().HaveCount(expected);
    }
}
