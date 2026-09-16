using System.Linq;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;
using PetShelter.Application.Services;
using PetShelter.Application.Services.ServiceInterfaces;
using PetShelter.Application.Domain.RepositoryInterfaces;
using PetShelter.Application.Domain;

namespace PetShelter.Uix.Views;

public partial class TemporaryAdoptionWindow : Window
{
    private readonly long _userId;

    private record AnimalItem(long Id, string Display);

    public TemporaryAdoptionWindow(long userId)
    {
        InitializeComponent();
        _userId = userId;

        LoadAnimals();
    }

    private void LoadAnimals()
    {
        var repo = Injector.CreateInstance<IAnimalRepository>();
        var volunteerService = Injector.CreateInstance<IVolunteerService>();
        var volunteer = volunteerService.GetById(_userId);

        List<Animal> animals;
        if (volunteer == null)
            animals = new List<Animal>();
        else
            animals = repo.GetAvailableForAssociation(volunteer.AssociationId);

        var listBox = this.FindControl<ListBox>("AnimalsListBox");
        if (listBox == null) return;

        var items = animals.Select(a => new AnimalItem(a.Id, $"{a.Name} (#{a.Id})")).ToList();
        listBox.ItemsSource = items.Select(i => i.Display).ToList();
        listBox.Tag = items;
    }

    private AnimalItem? GetSelectedAnimal()
    {
        var listBox = this.FindControl<ListBox>("AnimalsListBox");
        if (listBox == null) return null;
        if (listBox.SelectedIndex < 0) return null;
        var items = listBox.Tag as List<AnimalItem>;
        if (items == null) return null;
        return items.ElementAtOrDefault(listBox.SelectedIndex);
    }

    private void ConfirmButton_Click(object? sender, RoutedEventArgs e)
    {
        var sel = GetSelectedAnimal();
        if (sel == null) return;

        var service = Injector.CreateInstance<ITemporaryAdoptionService>();
        try
        {
            service.AddTemporaryAdoption(_userId, sel.Id);
            Close();
        }
        catch (Exception ex)
        {
            var dlg = new Window { Width = 300, Height = 120, Content = new TextBlock { Text = ex.Message } };
            dlg.ShowDialog(this);
        }
    }

    private void CancelButton_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }

    private void InitializeComponent()
    {
        Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
    }
}
