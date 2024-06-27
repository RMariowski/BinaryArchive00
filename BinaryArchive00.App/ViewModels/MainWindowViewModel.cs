using Avalonia.Media.Imaging;
using BinaryArchive00.App.Helpers;
using BinaryArchive00.App.Services;
using BinaryArchive00.Utils;
using BinaryArchive00.Utils.Image;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using NAudio.Wave;

namespace BinaryArchive00.App.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly WaveOutEvent _wavePlayer = new();

    [ObservableProperty] private ArchiveFile? _archive;
    [ObservableProperty] private string? _filterName;
    [ObservableProperty] private ArchiveEntry? _selectedEntry;
    [ObservableProperty] private Bitmap? _bitmap;
    [ObservableProperty] private string? _text;
    [ObservableProperty] private WaveStream? _waveStream;

    public ObservableRangeCollection<ArchiveEntry> Entries { get; } = [];

    [RelayCommand]
    private async Task OpenFile(CancellationToken token)
    {
        Clear();
        try
        {
            var filesService = App.Current!.Services!.GetService<IFilesService>();
            if (filesService is null)
                throw new NullReferenceException("Missing File Service instance");

            var file = await filesService.OpenFileAsync();
            if (file is null)
                return;

            Archive = new ArchiveFile(file.Path.LocalPath, readEntriesContent: false);
            Entries.AddRange(Archive.Entries);
        }
        catch (Exception e)
        {
            ErrorMessages?.Add(e.Message);
        }
    }

    partial void OnFilterNameChanged(string? value)
    {
        if (Archive is null)
            return;

        Entries.ReplaceRange(value is null
            ? Archive.Entries
            : Archive.Entries.Where(entry => entry.Name.Contains(value, StringComparison.OrdinalIgnoreCase)));
    }

    async partial void OnSelectedEntryChanged(ArchiveEntry? value)
    {
        if (value is null)
            return;

        var readContentTask = value.ReadContentAsync();
        ClearEntryPreview();
        await readContentTask;

        switch (value.Type)
        {
            case "imag":
                SetImagePreview(value);
                break;
            case "wave":
                SetAudioPreview(value);
                break;
            default:
                SetTextPreview(value);
                break;
        }
    }

    private void SetImagePreview(ArchiveEntry value)
    {
        var image = value.ToImage(PixelFormat.Bgra8888);
        Bitmap = new Bitmap(image.AsBmpStream());
    }

    private void SetAudioPreview(ArchiveEntry value)
    {
        MemoryStream memoryStream = new(value.Content!);
        WaveStream = value.Name.EndsWith(".mp3") ? new Mp3FileReader(memoryStream) : new WaveFileReader(memoryStream);
        _wavePlayer.Stop();
        _wavePlayer.Init(WaveStream);
    }

    private void SetTextPreview(ArchiveEntry value)
    {
        Text = Encoding.UTF8.GetString(value.Content!);
    }

    [RelayCommand]
    private void PlayAudio()
    {
        WaveStream!.Seek(0, SeekOrigin.Begin);
        _wavePlayer.Play();
    }

    [RelayCommand]
    private void StopAudio()
    {
        _wavePlayer.Stop();
    }

    private void Clear()
    {
        ErrorMessages?.Clear();
        ClearEntryPreview();
        Entries.Clear();
        SelectedEntry = null;
        Archive?.Dispose();
        Archive = null;
    }

    private void ClearEntryPreview()
    {
        Bitmap = null;
        Text = null;
        WaveStream?.Dispose();
        WaveStream = null;
    }
}
