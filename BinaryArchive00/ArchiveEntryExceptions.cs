namespace BinaryArchive00;

public class ArchiveEntryException(string message)
    : Exception(message);

public class NotEmptyArchiveEntryException(string? fileName, string entryName)
    : ArchiveEntryException(
        fileName is null
            ? $"Entry {entryName} is marked as empty but has data"
            : $"Entry {entryName} in archive {fileName} is marked as empty but has data");
            
public class UnableToReadWholeArchiveEntryException(string entryName)
    : ArchiveEntryException($"Unable to read all bytes of {entryName} content");
