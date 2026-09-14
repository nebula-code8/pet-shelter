using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using PetShelter.Uix.Views;

namespace PetShelter.Uix;

public class App : Avalonia.Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop) desktop.MainWindow = new LoginWindow();
        base.OnFrameworkInitializationCompleted();
    }
}