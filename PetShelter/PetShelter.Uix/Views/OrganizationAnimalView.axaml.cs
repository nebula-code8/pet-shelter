using System;
using System.Collections.Generic;
using PetShelter.Application.Repository;
using PetShelter.Application.Services.ServiceInterfaces;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using PetShelter.Application.Domain;
using PetShelter.Application.Services;

namespace PetShelter.Uix.Views;

public partial class OrganizationAnimalView : UserControl
{
    private readonly Association _association;
    private readonly IAnimalService _animalService;
    private readonly IAdoptionRequestService _adoptionRequestService;

    public OrganizationAnimalView(Association association)
    {
        InitializeComponent();

        _association = association;
        _animalService = Injector.CreateInstance<IAnimalService>();
        _adoptionRequestService = Injector.CreateInstance<IAdoptionRequestService>();

        LoadOrganization();
        LoadAnimals();
    }

    private void LoadOrganization()
    {
        OrganizationNameTextBlock.Text =
            _association.Name;

        OrganizationTypeTextBlock.Text =
            $"Type: {_association.EstablishmentType}";

        OrganizationDescriptionTextBlock.Text =
            $"Description: {_association.Description}";

        OrganizationAddressTextBlock.Text =
            $"Address: {_association.Address}";

        OrganizationPhoneTextBlock.Text =
            $"Phone: {_association.PhoneNumber}";

        OrganizationEmailTextBlock.Text =
            $"Email: {_association.EmailAddress}";
    }

    private void LoadAnimals()
    {
        List<Animal> animals =
            _animalService.GetByAssociation(
                _association.Id);

        foreach (Animal animal in animals)
        {
            AnimalsList.Items.Add(
                CreateAnimalCard(animal));
        }
    }

    private Border CreateAnimalCard(Animal animal)
    {
        Border card = new Border
        {
            Width = 300,
            Margin = new Avalonia.Thickness(0, 0, 15, 15),
            Padding = new Avalonia.Thickness(20),
            Background = Brushes.White,
            CornerRadius = new Avalonia.CornerRadius(10),
            BorderBrush = Brushes.LightGray,
            BorderThickness = new Avalonia.Thickness(1)
        };

        StackPanel panel = new StackPanel
        {
            Spacing = 7
        };

        panel.Children.Add(
            new TextBlock
            {
                Foreground = Brushes.Gray,
                Text = animal.Name,
                FontSize = 20,
                FontWeight = FontWeight.Bold
            });

        panel.Children.Add(
            new TextBlock
            {
                Foreground = Brushes.Gray,
                Text = $"Species: {animal.Species}"
            });

        panel.Children.Add(
            new TextBlock
            {
                Foreground = Brushes.Gray,
                Text = $"Breed: {animal.Breed}"
            });

        panel.Children.Add(
            new TextBlock
            {
                Foreground = Brushes.Gray,
                Text = $"Gender: {animal.Gender}"
            });

        panel.Children.Add(
            new TextBlock
            {
                Foreground = Brushes.Gray,
                Text = $"Health: {animal.HealthStatus}"
            });

        panel.Children.Add(
            new TextBlock
            {
                Foreground = Brushes.Gray,
                Text = $"Vaccinated: {(animal.IsVaccinated ? "Yes" : "No")}"
            });

        panel.Children.Add(
            new TextBlock
            {
                Foreground = Brushes.Gray,
                Text = $"Sterilized: {(animal.IsSterilized ? "Yes" : "No")}"
            });

        panel.Children.Add(
            new TextBlock
            {
                Foreground = Brushes.Gray,
                Text = animal.Description,
                TextWrapping = Avalonia.Media.TextWrapping.Wrap
            });

        Button adoptButton = new Button
        {
            Foreground = Brushes.Gray,
            Content = "Adopt Animal",
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
            Margin = new Avalonia.Thickness(0, 10, 0, 0)
        };

        adoptButton.Click += (_, _) =>
        {
            if (VisualRoot is MainWindow window)
            {
                window.AdoptAnimal(animal);
            }
        };

        panel.Children.Add(adoptButton);

        card.Child = panel;

        return card;
    }

    private void BackButton_Click(
        object? sender,
        RoutedEventArgs e)
    {
        if (VisualRoot is MainWindow window)
        {
            window.ShowClientHome();
        }
    }
}