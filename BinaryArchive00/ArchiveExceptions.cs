namespace BinaryArchive00;

public class ArchiveException(string message) 
    : Exception(message);

public class NotBinaryArchiveException(string? filePath)
    : ArchiveException(
        filePath is null
            ? "Stream is not a binary.archive00 file"
            : $"{filePath} is not a binary.archive00 file");

public class NotEndOfArchiveHeaderException()
    : ArchiveException("Not end of archive header");
