using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Linq;
using System.Collections.Generic;
using PetShelter.Application.Services.ServiceInterfaces;
using PetShelter.Application.Domain;
using PetShelter.Application.Domain.RepositoryInterfaces;
using PetShelter.Application.Services;

namespace PetShelter.Uix.Views;

public partial class VolunteerWindow : Window
{
    private readonly long _userId;

    public VolunteerWindow(long userId)
    {
        InitializeComponent();

        _userId = userId;
        LoadTemporaryAdoptions();
    }

    private async void AdoptersButton_Click(object? sender, RoutedEventArgs e)
    {
        AllAdoptersWindow window = new();
        await window.ShowDialog(this);
    }

    private async void AdoptionRequestsButton_Click(object? sender, RoutedEventArgs e)
    {
        AdoptionRequestsWindow window = new();
        await window.ShowDialog(this);
    }

    private async void TemporarilyAdoptButton_Click(object? sender, RoutedEventArgs e)
    {
        TemporaryAdoptionWindow window = new(_userId);
        await window.ShowDialog(this);
        LoadTemporaryAdoptions();
    }

    private void LoadTemporaryAdoptions()
    {
        var listBox = this.FindControl<ListBox>("TemporaryAdoptionsListBox");
        if (listBox == null) return;

        var tempService = Injector.CreateInstance<ITemporaryAdoptionService>();
        var volunteerService = Injector.CreateInstance<IVolunteerService>();
        var animalRepo = Injector.CreateInstance<IAnimalRepository>();

        var volunteer = volunteerService.GetById(_userId);
        if (volunteer == null)
        {
            listBox.ItemsSource = new List<string>();
            return;
        }

        // Show temporary adoptions only for animals that belong to the volunteer's association
        var allTemps = tempService.GetAllForAssociation(volunteer.AssociationId);
        var items = new List<string>();
        foreach (var t in allTemps)
        {
            try
            {
                var a = animalRepo.GetById(t.AnimalId);
                if (a == null) continue;
                if (a.AssociationId != volunteer.AssociationId) continue;

                string animalName = $"Animal #{t.AnimalId}";
                animalName = $"{a.Name} (#{a.Id})";
                items.Add($"{animalName} - since {t.StartDate:d}");
            }
            catch
            {
                // ignore problematic entries
            }
        }

        listBox.ItemsSource = items;
    }

    private void LogoutButton_Click(object? sender, RoutedEventArgs e)
    {
        MainWindow mainWindow = new();
        mainWindow.Show();

        Close();
    }

    private void InitializeComponent()
    {
        Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
    }
}
