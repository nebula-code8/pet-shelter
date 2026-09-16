using System.Linq;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;
using PetShelter.Application.Services;
using PetShelter.Application.Services.ServiceInterfaces;

namespace PetShelter.Uix.Views;

public partial class AllAdoptersWindow : Window
{
    public AllAdoptersWindow()
    {
        InitializeComponent();

        try
        {
            var service = Injector.CreateInstance<IAdoptionRequestService>();
            var adopters = service.GetAdopters();

            var items = adopters.Select(a => $"{a.Name} {a.Surname} - {a.EmailAddress}").ToList();
            var listBox = this.FindControl<ListBox>("AdoptersListBox");
            if (listBox != null)
                listBox.ItemsSource = items;
        }
        catch (Exception)
        {
            var listBox = this.FindControl<ListBox>("AdoptersListBox");
            if (listBox != null)
                listBox.ItemsSource = new List<string> { "Failed to load adopters." };
        }
    }

    private void CloseButton_Click(object? sender, RoutedEventArgs e)
    {
        // If this window has an owner (was shown as a dialog), simply close it to return to the owner.
        if (this.Owner is Window)
        {
            Close();
            return;
        }

        // Otherwise, open the main window so the user is returned somewhere instead of exiting the app.
        MainWindow mainWindow = new();
        mainWindow.Show();
        Close();
    }

    private void InitializeComponent()
    {
        Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
    }
}
