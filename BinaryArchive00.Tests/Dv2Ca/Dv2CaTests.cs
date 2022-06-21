using BinaryArchive00.Utils;

namespace BinaryArchive00.Tests.Dv2Ca;

public sealed class Dv2CaTests : IClassFixture<Dv2CaFixture>
{
    private readonly BinaryArchive _binaryArchive;

    public Dv2CaTests(Dv2CaFixture fixture)
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
        const int expected = 29938;

        _binaryArchive.Entries.Should().HaveCount(expected);
    }

    [Fact]
    public void Dv2LogoImageEntryTest()
    {
        byte[] expected = File.ReadAllBytes(Path.Join(Consts.PathToFiles, "dv2logo.imag"));
        var entry = _binaryArchive.Entries.Single(entry => entry.FileName == "dv2logo.tga");

        var image = entry.ExtractImage(false);

        image.Width.Should().Be(640);
        image.Height.Should().Be(480);
        image.Data.Should().BeEquivalentTo(expected);
    }
}
