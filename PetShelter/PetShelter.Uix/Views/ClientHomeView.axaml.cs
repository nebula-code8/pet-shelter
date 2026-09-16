using System;
using System.Collections.Generic;
using PetShelter.Application.Repository;
using Avalonia.Controls;
using Avalonia.Interactivity;
using PetShelter.Application.Domain;
using PetShelter.Application.Services;
using PetShelter.Application.Services.ServiceInterfaces;

namespace PetShelter.Uix.Views;

public partial class ClientHomeView : UserControl
{
    private readonly long _userId;

    private readonly IUserService _userService;
    private readonly IAssociationService _associationService;
    private readonly IAnimalService _animalService;

    public ClientHomeView(long userId)
    {
        InitializeComponent();

        _userId = userId;

        _userService = Injector.CreateInstance<IUserService>();
        _associationService = Injector.CreateInstance<IAssociationService>();
        _animalService = Injector.CreateInstance<IAnimalService>();

        LoadUser();
        LoadAdoptedAnimals();
        LoadOrganizations();
    }

    public ClientHomeView()
    {
        InitializeComponent();

        _userService = Injector.CreateInstance<IUserService>();
        _associationService = Injector.CreateInstance<IAssociationService>();
        _animalService = Injector.CreateInstance<IAnimalService>();

        HideUserDataSection();
        LoadOrganizations();
    }
    
    private void LoadAdoptedAnimals()
    {
        var animals = _animalService.GetByUserId(_userId);
        AdoptedAnimalList.Items.Clear();

        foreach (var animal in animals)
        {
            AdoptedAnimalList.Items.Add(
                new AnimalCard(animal, _userId)
            );
        }
    }

    private void HideUserDataSection()
    {
        UserDataBorder.IsVisible = false;
        MainGrid.ColumnDefinitions[0].Width = new GridLength(0);
        MainGrid.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);
    }

    private void LoadUser()
    {
        User? user = _userService.GetById(_userId);

        if (user == null)
        {
            ShowError("Unable to load user information.");
            return;
        }

        NameTextBlock.Text = user.Name;
        SurnameTextBlock.Text = user.Surname;
        EmailTextBlock.Text = user.EmailAddress;
        PhoneTextBlock.Text = user.PhoneNumber;
        AddressTextBlock.Text = user.Address;
        DateOfBirthTextBlock.Text =
            user.DateOfBirth.ToString("dd.MM.yyyy");
    }

    private void LoadOrganizations()
    {
        List<Association> associations =
            _associationService.GetAll();

        foreach (Association association in associations)
        {
            Button button = CreateOrganizationButton(association);

            OrganizationsList.Items.Add(button);
        }
    }

    private Button CreateOrganizationButton(Association association)
    {
        Button button = new Button
        {
            HorizontalContentAlignment =
                Avalonia.Layout.HorizontalAlignment.Stretch,

            HorizontalAlignment =
                Avalonia.Layout.HorizontalAlignment.Stretch,

            Padding = new Avalonia.Thickness(20),

            Background =
                Avalonia.Media.Brushes.White,

            BorderBrush =
                Avalonia.Media.Brushes.LightGray,

            Foreground =
                Avalonia.Media.Brushes.Gray,

            BorderThickness =
                new Avalonia.Thickness(1)
        };

        StackPanel panel = new StackPanel
        {
            Spacing = 5
        };

        panel.Children.Add(
            new TextBlock
            {
                Text = association.Name,
                FontSize = 18,
                FontWeight = Avalonia.Media.FontWeight.Bold
            });

        panel.Children.Add(
            new TextBlock
            {
                Text = association.EstablishmentType,
                FontSize = 14
            });

        panel.Children.Add(
            new TextBlock
            {
                Text = association.Address,
                FontSize = 13
            });

        button.Content = panel;

        button.Click += (_, _) => { OpenOrganization(association); };

        return button;
    }

    private void OpenOrganization(Association association)
    {
        if (VisualRoot is MainWindow window)
        {
            window.ShowOrganizationAnimals(association);
        }
    }

    private void LogoutButton_Click(
        object? sender,
        RoutedEventArgs e)
    {
        if (VisualRoot is MainWindow window)
        {
            window.ShowLogin();
        }
    }

    private void ShowError(string message)
    {
        // You can replace this with a proper dialog later.
        Console.WriteLine(message);
    }
}