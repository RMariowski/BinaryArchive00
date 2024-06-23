using ConsoleTables;

namespace BinaryArchive00.Notebook.Base;

public static class Helpers
{
    public static void PrintTable(string[] columns, object[][] rows)
    {
        ConsoleTable table = new(columns);
        foreach (var row in rows)
            table.AddRow(row);
        table.Write();
    }

    public static void SaveTable(string filePath, string[] columns, object[][] rows)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(filePath));

        var consoleOut = Console.Out;
        using FileStream fileStream = new(filePath, FileMode.Create, FileAccess.Write);
        using StreamWriter writer = new(fileStream);
        writer.AutoFlush = true;
        Console.SetOut(writer);

        ConsoleTable table = new(columns);
        foreach (var row in rows)
            table.AddRow(row);
        table.Write();

        Console.SetOut(consoleOut);
    }

    public static void SaveEntry(ArchiveEntry entry, string filePath)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(filePath));
        File.WriteAllBytes(filePath, entry.Content);
    }

    public static bool AreEqual(byte[] array1, byte[] array2)
    {
        if (array1 == array2)
            return true;

        if (array1.Length != array2.Length)
            return false;

        return new Span<byte>(array1).SequenceEqual(new Span<byte>(array2));
    }

    public static void PrintDifferences(byte[] array1, byte[] array2)
    {
        var length = Math.Min(array1.Length, array2.Length);

        for (int i = 0; i < length; i++)
        {
            if (array1[i] != array2[i])
            {
                Console.WriteLine($"Difference at index {i}: array1 = {array1[i]}, array2 = {array2[i]}");
            }
        }

        if (array1.Length == array2.Length)
            return;

        Console.WriteLine("The arrays have different lengths.");
        if (array1.Length > array2.Length)
        {
            for (var i = length; i < array1.Length; i++)
            {
                Console.WriteLine($"Extra byte in array1 at index {i}: {array1[i]}");
            }
        }
        else
        {
            for (var i = length; i < array2.Length; i++)
            {
                Console.WriteLine($"Extra byte in array2 at index {i}: {array2[i]}");
            }
        }
    }
}
