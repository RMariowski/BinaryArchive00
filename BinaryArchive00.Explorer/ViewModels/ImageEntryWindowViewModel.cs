using Avalonia.Media.Imaging;
using BinaryArchive00.Utils;
using ReactiveUI;

namespace BinaryArchive00.Explorer.ViewModels;

public class ImageEntryWindowViewModel : ViewModelBase
{
    private BinaryArchiveImage? _image;
    private Bitmap? _bitmap;

    public BinaryArchive Archive { get; }
    public BinaryArchiveEntry Entry { get; }

    public BinaryArchiveImage? Image
    {
        get => _image;
        set => this.RaiseAndSetIfChanged(ref _image, value);
    }

    public Bitmap? Bitmap
    {
        get => _bitmap;
        set => this.RaiseAndSetIfChanged(ref _bitmap, value);
    }

    public ImageEntryWindowViewModel()
    {
        Archive = null!;
        Entry = null!;
        Image = null;
        Bitmap = null;
    }

    public ImageEntryWindowViewModel(BinaryArchive archive, BinaryArchiveEntry entry)
    {
        Archive = archive;
        Entry = entry;
        Image = null;
        Bitmap = null;
    }

    public void Load()
    {
        var image = Entry.ExtractImage();
        BmpSharp.Bitmap bitmap = new(image.Width, image.Height, image.Data, BmpSharp.BitsPerPixelEnum.RGBA32);
        Image = image with { Data = bitmap.GetBmpBytes(flipped: false) };
        Bitmap = new Bitmap(bitmap.GetBmpStream(fliped: true));
    }
}
