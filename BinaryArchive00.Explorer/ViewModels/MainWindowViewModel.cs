using System.Collections.ObjectModel;
using Avalonia.Controls;
using BinaryArchive00.Explorer.Views;
using DynamicData;
using ReactiveUI;

namespace BinaryArchive00.Explorer.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private BinaryArchive? _binaryArchive;
    private BinaryArchiveEntry? _selectedEntry;

    public BinaryArchive? BinaryArchive
    {
        get => _binaryArchive;
        private set => this.RaiseAndSetIfChanged(ref _binaryArchive, value);
    }

    public BinaryArchiveEntry? SelectedEntry
    {
        get => _selectedEntry;
        set => this.RaiseAndSetIfChanged(ref _selectedEntry, value);
    }

    public string NameFilter
    {
        set
        {
            Entries.Clear();
            Entries.AddRange(BinaryArchive!.Entries.Where(entry => entry.FileName.Contains(value)));
        }
    }

    public ObservableCollection<BinaryArchiveEntry> Entries { get; } = new();

    public async void OpenFile(Window window)
    {
        Clear();

        OpenFileDialog fileDialog = new();
        fileDialog.Filters!.AddRange(new FileDialogFilter[]
        {
            new() { Name = "CA Files", Extensions = { "ca" } },
            new() { Name = "All Files", Extensions = { "*" } }
        });
        string[]? result = await fileDialog.ShowAsync(window);

        if (result is not { Length: 1 })
            throw new ApplicationException("Invalid file selection");

        string filePath = result[0];
        BinaryArchive = new BinaryArchive(filePath);
        Entries.AddRange(BinaryArchive.Entries);
        SetDefaultSelectedEntry(filePath);
    }

    private void SetDefaultSelectedEntry(string filePath)
    {
        if (BinaryArchive is null || filePath.Contains("dv2.ca") is false)
            return;

        SelectedEntry = BinaryArchive.Entries.SingleOrDefault(entry => entry.FileName == "dv2logo.tga");
    }

    public void ShowEntryContent()
    {
        if (_selectedEntry is null || _binaryArchive is null)
            return;

        switch (_selectedEntry.Type)
        {
            case "gami":
                ShowImageEntryWindow();
                break;

            default:
                ShowTextEntryWindow();
                break;
        }
    }

    private void ShowImageEntryWindow()
    {
        ImageEntryWindowViewModel viewModel = new(_binaryArchive!, _selectedEntry!);
        ImageEntryWindow window = new() { DataContext = viewModel };
        viewModel.Load();
        window.Show();
    }

    private void ShowTextEntryWindow()
    {
        TextEntryWindowViewModel viewModel = new(_binaryArchive!, _selectedEntry!);
        TextEntryWindow window = new() { DataContext = viewModel };
        viewModel.Load();
        window.Show();
    }

    private void Clear()
    {
        SelectedEntry = null;
        BinaryArchive = null;
    }
}
