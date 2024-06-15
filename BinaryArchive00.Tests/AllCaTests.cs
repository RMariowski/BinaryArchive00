using System.Text;

namespace BinaryArchive00.Tests;

[Collection(nameof(ArchivesFixture))]
public sealed class AllCaTests
{
    private ArchiveFile[] Sut => _fixture.Archives;

    [Fact]
    public void AlwaysZeroesBytes()
    {
        foreach (var archive in Sut)
        {
            var stream = archive.Stream;
            using BinaryReader reader = new(stream, Encoding.UTF8, true);

            stream.Seek(16L, SeekOrigin.Begin);
            reader.ReadBytes(4).Should().AllSatisfy(b => Assert.Equal(0, b));
            stream.Seek(1, SeekOrigin.Current);
            reader.ReadBytes(3).Should().AllSatisfy(b => Assert.Equal(0, b));
            stream.Seek(4, SeekOrigin.Current);
            reader.ReadBytes(4).Should().AllSatisfy(b => Assert.Equal(0, b));

            stream.Seek(archive.EntriesOffset, SeekOrigin.Begin);
            while (stream.Position < stream.Length)
            {
                stream.Seek(28, SeekOrigin.Current);
                reader.ReadBytes(20).Should().AllSatisfy(b => Assert.Equal(0, b));
                stream.Seek(8, SeekOrigin.Current);
                reader.ReadBytes(8).Should().AllSatisfy(b => Assert.Equal(0, b));
            }
        }
    }

    private readonly ArchivesFixture _fixture;

    public AllCaTests(ArchivesFixture fixture)
    {
        _fixture = fixture;
    }
}
