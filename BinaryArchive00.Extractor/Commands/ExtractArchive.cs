using BinaryArchive00.Utils;
using BinaryArchive00.Utils.Image;

namespace BinaryArchive00.Extractor.Commands;

public static class ExtractArchive
{
    public static void Command([Option("fp")] string filePath, [Option("op")] string outputPath)
    {
        if (File.Exists(filePath) is false)
        {
            Console.WriteLine($"The file at path {filePath} does not exist.");
            return;
        }

        Directory.CreateDirectory(outputPath);

        try
        {
            using var archive = ArchiveFile.Open(filePath);
            Console.WriteLine("Binary archive loaded.");

            archive.Extract(outputPath);
            Console.WriteLine("Binary archive extracted");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
    
    public static void Extract(this ArchiveFile binaryArchive, string outputPath)
    {
        var extractedCount = 0;
        var totalEntries = binaryArchive.Entries.Count;

        var types = binaryArchive.Entries.GroupBy(entry => entry.Type, entry => entry);
        Parallel.ForEach(types, group =>
        {
            var typeDirPath = Path.Join(outputPath, group.Key);
            Directory.CreateDirectory(typeDirPath);
        });
        Directory.CreateDirectory(Path.Join(outputPath, "imag-corrupted"));

        Parallel.ForEach(binaryArchive.Entries, entry =>
        {
            try
            {
                var entryPath = Path.Join(outputPath, entry.Type, entry.Name);

                switch (entry.Type)
                {
                    case "imag":
                        ExtractImage(entry, entryPath);
                        break;
                    case "wave":
                        ExtractWave(entry, entryPath);
                        break;
                    default:
                        Extract(entry, entryPath);
                        break;
                }

                Interlocked.Increment(ref extractedCount);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to extract {entry.Name}: {e.Message}");
            }
        });

        Console.WriteLine($"Extracted {extractedCount} / {totalEntries} files.");
    }

    private static void ExtractImage(ArchiveEntry entry, string entryPath)
    {
        var entryImage = entry.ToImage(PixelFormat.Bgra8888);
        if (entryImage.Width == 0 || entryImage.Height == 0)
        {
            entryPath = entryPath.Replace("imag", "imag-corrupted");
            File.WriteAllBytes(entryPath, entry.Content!);
            return;
        }

        using FileStream fileStream = new($"{entryPath}.bmp", FileMode.Create, FileAccess.Write);
        entryImage.AsBmpStream().WriteTo(fileStream);
    }

    private static void ExtractWave(ArchiveEntry entry, string entryPath)
        => File.WriteAllBytes($"{entryPath}.wav", entry.Content!);

    private static void Extract(ArchiveEntry entry, string entryPath)
        => File.WriteAllBytes(entryPath, entry.Content!);
}
