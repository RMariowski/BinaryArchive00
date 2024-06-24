using BinaryArchive00.Utils;
using ClosedXML.Excel;

namespace BinaryArchive00.Extractor.Commands;

public static class ExtractAllExcel
{
    public static void Command([Option("op")] string outputPath,
        [Option("ofn")] string outputFileName = "extracted.xlsx")
    {
        Directory.CreateDirectory(outputPath);

        try
        {
            var caPaths = Directory.GetFiles(DV2.GetInstallationPath(), "*.ca", SearchOption.AllDirectories);
            var archives = caPaths.Select(caPath => ArchiveFile.Open(caPath, readEntriesContent: false)).ToArray();
           
            using XLWorkbook wb = new();
            AddArchivesWorksheet(wb, archives);
            AddEntriesWorksheet(wb, archives.SelectMany(archive => archive.Entries).ToArray());
            wb.SaveAs(Path.Combine(outputPath, outputFileName));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    private static void AddArchivesWorksheet(XLWorkbook wb, ArchiveFile[] archives)
    {
        var rows = archives.Select(archive => new ArchiveTableRow(
            archive.FilePath!,
            archive.FileName!,
            archive.IsPatch,
            archive.EntriesOffset,
            BitConverter.ToString(archive.Unknown),
            BitConverter.ToString(archive.Unknown2),
            (byte)archive.UnknownEnum,
            archive.Entries.Count
        )).ToArray();
        
        var ws = wb.AddWorksheet("Archives");
        ws.FirstCell().InsertTable(rows, "Extracted Data", true);
    }
    
    private static void AddEntriesWorksheet(XLWorkbook wb, ArchiveEntry[] entries)
    {
        var rows = entries.Select(entry => new ArchiveEntryTableRow(
            entry.Archive.FilePath!,
            entry.Archive.FileName!,
            entry.Name,
            entry.Type,
            entry.Size,
            entry.Offset,
            BitConverter.ToString(entry.Unknown),
            entry.Unknown2
        )).ToArray();
        
        var ws = wb.AddWorksheet("Entries");
        ws.FirstCell().InsertTable(rows, "Extracted Data", true);
    }

    private record ArchiveTableRow(
        string FilePath,
        string FileName,
        bool IsPatch,
        int EntriesOffset,
        string Unknown,
        string Unknown2,
        byte UnknownEnum,
        int EntriesCount);
    
    private record ArchiveEntryTableRow(
        string ArchiveFilePath,
        string ArchiveFileName,
        string Name,
        string Type,
        int Size,
        int Offset,
        string Unknown,
        byte Unknown2);
}
