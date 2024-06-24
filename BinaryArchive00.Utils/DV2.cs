namespace BinaryArchive00.Utils;

public static class DV2
{
    public static string GetInstallationPath()
    {
        string? path = null;
        if (OperatingSystem.IsWindows())
        {
            const string keyName = @"HKEY_CURRENT_USER\SOFTWARE\JoWooD\DV2Addon";
            path = Microsoft.Win32.Registry.GetValue(keyName, "path.install", null) as string;
        }

        if (string.IsNullOrWhiteSpace(path))
        {
            path = Environment.GetEnvironmentVariable("DV2_PATH");
        }

        return path ??
               throw new InvalidOperationException("The path to DV2 is not set in registry or environment variable.");
    }
}
