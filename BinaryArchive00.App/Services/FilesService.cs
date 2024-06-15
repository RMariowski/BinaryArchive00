using Avalonia.Controls;
using Avalonia.Platform.Storage;

namespace BinaryArchive00.App.Services;

public interface IFilesService
{
    public Task<IStorageFile?> OpenFileAsync();
}

internal class FilesService(Window target) : IFilesService
{
    public async Task<IStorageFile?> OpenFileAsync()
    {
        var files = await target.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open Archive File",
            AllowMultiple = false,
            FileTypeFilter = new FilePickerFileType[]
            {
                new("CA Files") { Patterns = new[] { "*.ca" } },
                new("All Files") { Patterns = new[] { "*.*" } }
            }
        });

        return files.Count >= 1 ? files[0] : null;
    }
}
