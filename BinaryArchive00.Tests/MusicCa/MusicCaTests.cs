namespace BinaryArchive00.Tests.MusicCa;

public sealed class MusicCaTests : IClassFixture<MusicCaFixture>
{
    private readonly BinaryArchive _binaryArchive;

    public MusicCaTests(MusicCaFixture fixture)
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
        const bool expected = true;

        _binaryArchive.MusicCa.Should().Be(expected);
        foreach (var entry in _binaryArchive.Entries)
            entry.MusicCa.Should().Be(expected);
    }

    [Fact]
    public void EntriesCountTest()
    {
        const int expected = 15;

        _binaryArchive.Entries.Should().HaveCount(expected);
    }
}
