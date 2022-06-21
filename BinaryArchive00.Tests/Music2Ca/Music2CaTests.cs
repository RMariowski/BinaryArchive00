namespace BinaryArchive00.Tests.Music2Ca;

public sealed class Music2CaTests : IClassFixture<Music2CaFixture>
{
    private readonly BinaryArchive _binaryArchive;

    public Music2CaTests(Music2CaFixture fixture)
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
        const int expected = 3;

        _binaryArchive.Entries.Should().HaveCount(expected);
    }
}
