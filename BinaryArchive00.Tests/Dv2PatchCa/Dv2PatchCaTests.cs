namespace BinaryArchive00.Tests.Dv2PatchCa;

public sealed class Dv2PatchCaTests : IClassFixture<Dv2PatchCaFixture>
{
    private readonly BinaryArchive _binaryArchive;

    public Dv2PatchCaTests(Dv2PatchCaFixture fixture)
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
        const int expected = 22;

        _binaryArchive.Entries.Should().HaveCount(expected);
    }
}
