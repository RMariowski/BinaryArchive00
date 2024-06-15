namespace BinaryArchive00.Extractor;

internal static class Program
{
    private static void Main(string[] args)
    {
        if (args.Length != 2)
        {
            Console.WriteLine("Please provide a file path and an output path as arguments.");
            return;
        }

        var filePath = args[0];
        var outputPath = args[1];

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
}
