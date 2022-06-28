using System.Text;
using ReactiveUI;

namespace BinaryArchive00.Explorer.ViewModels;

public class TextEntryWindowViewModel : ViewModelBase
{
    private string _text = string.Empty;

    public BinaryArchive Archive { get; }
    public BinaryArchiveEntry Entry { get; }

    public string Text
    {
        get => _text;
        set => this.RaiseAndSetIfChanged(ref _text, value);
    }

    public TextEntryWindowViewModel()
    {
        Archive = null!;
        Entry = null!;
    }

    public TextEntryWindowViewModel(BinaryArchive archive, BinaryArchiveEntry entry)
    {
        Archive = archive;
        Entry = entry;
    }

    public void Load()
    {
        Entry.LoadContent();
        Text = Encoding.UTF8.GetString(Entry.Content!);
    }
}
