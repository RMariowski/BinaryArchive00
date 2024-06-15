using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using BinaryArchive00.App.Services;
using BinaryArchive00.App.ViewModels;
using BinaryArchive00.App.Views;
using Microsoft.Extensions.DependencyInjection;

namespace BinaryArchive00.App;

public class App : Application
{
    public new static App? Current => Application.Current as App;

    public IServiceProvider? Services { get; private set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow { DataContext = new MainWindowViewModel() };

            ServiceCollection services = [];
            services.AddSingleton<IFilesService>(new FilesService(desktop.MainWindow));
            Services = services.BuildServiceProvider();
        }

        base.OnFrameworkInitializationCompleted();
    }
}
