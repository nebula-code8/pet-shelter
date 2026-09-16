using Avalonia.Controls;
using Avalonia.Interactivity;
using PetShelter.Application.Domain;

namespace PetShelter.Uix.Views;

public partial class AnimalCard : UserControl
{
    public Animal Animal { get; }
    private long _userId;

    public AnimalCard(Animal animal, long userId)
    {
        InitializeComponent();

        Animal = animal;
        _userId = userId;

        NameTextBlock.Text = animal.Name;
        SpeciesTextBlock.Text = $"Species: {animal.Species}";
        BreedTextBlock.Text = $"Breed: {animal.Breed}";
        GenderTextBlock.Text = $"Gender: {animal.Gender}";
        DateOfBirthTextBlock.Text = $"Date of birth: {animal.DateOfBirth}";
    }

    private async void InfoButton_Click(object? sender, RoutedEventArgs e)
    {
        var window = new AnimalInfoWindow(Animal, _userId);
        window.Closed += (_, _) =>
        {
            if (VisualRoot is MainWindow window)
            {
                window.ShowClientHome();
            }
        };

        var parentWindow = TopLevel.GetTopLevel(this) as Window;

        if (parentWindow != null)
        {
            await window.ShowDialog(parentWindow);
        }
        
    }
}