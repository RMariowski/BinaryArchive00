namespace BinaryArchive00.Notebook;

[Collection(nameof(ArchivesFixture))]
public sealed class Entries
{
    [Fact]
    public void Extract_EntriesList()
    {
        var entries = _allEntries.OrderBy(entry => entry.Unknown2)
            .ThenBy(entry => entry.Archive.FileName)
            .ThenBy(entry => entry.Name);
        var data = entries.Select(entry => new object[]
        {
            entry.Archive.FileName, entry.Name, entry.Type, BitConverter.ToString(entry.Unknown), entry.Unknown2
        }).ToArray();
        SaveTable("Extracted/entries-list.txt", ["Archive", "Name", "Type", "Unknown", "Unknown2"], data);
    }

    [Fact]
    public void Extract_EntriesGroupUnknown()
    {
        var wholeGroup = _allEntries.GroupBy(entry => BitConverter.ToString(entry.Unknown), entry => entry.Name)
            .Where(group => group.Count() > 1)
            .ToArray();
        var data = wholeGroup.Select(g => new object[] { g.Key, g.Count() }).ToArray();
        SaveTable("Extracted/entries-group-unknown.txt", ["Unknown", "Count"], data);

        var l4Group = _allEntries
            .GroupBy(entry => BitConverter.ToString(entry.Unknown.TakeLast(4).ToArray()), entry => entry.Name)
            .Where(group => group.Count() > 1);

        wholeGroup.Should().HaveSameCount(l4Group);
    }

    [Fact]
    public void Extract_EntriesGroupLast2BytesUnknown()
    {
        Dictionary<string, List<string>> group = new();
        foreach (var entry in _allEntries)
        {
            var key = BitConverter.ToString(entry.Unknown.TakeLast(2).ToArray());
            if (!group.TryGetValue(key, out var value))
            {
                value = [];
                group[key] = value;
            }

            value.Add($"{entry.Name}({entry.Type})");
        }

        var data = group
            .Select(kvp => new object[] { kvp.Key, kvp.Value.Count }).ToArray();
        SaveTable("Extracted/entries-group-l2unknown.txt", ["Unknown", "Count"], data);
    }

    [Fact]
    public void Extract_EntriesGroupLast3BytesUnknown()
    {
        Dictionary<string, List<string>> group = new();
        foreach (var entry in _allEntries)
        {
            var key = BitConverter.ToString(entry.Unknown.TakeLast(3).ToArray());
            if (!group.TryGetValue(key, out var value))
            {
                value = [];
                group[key] = value;
            }

            value.Add($"{entry.Name}({entry.Type})");
        }

        var data = group
            .Select(kvp => new object[] { kvp.Key, kvp.Value.Count, string.Join(", ", kvp.Value) }).ToArray();
        SaveTable("Extracted/entries-group-l3unknown.txt", ["Unknown", "Count", "Entries"], data);
    }

    private readonly ArchivesFixture _fixture;
    private readonly ArchiveEntry[] _allEntries;

    public Entries(ArchivesFixture fixture)
    {
        _fixture = fixture;
        _allEntries = _fixture.Archives.SelectMany(archive => archive.Entries).ToArray();
    }
}
