using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using PetShelter.Application.Domain;
using PetShelter.Application.Domain.RepositoryInterfaces;
using PetShelter.Application.Repository;
using PetShelter.Application.Services;
using PetShelter.Application.Services.ServiceInterfaces;

namespace PetShelter.Uix.Views;

public partial class RegistrationWindow : UserControl
{
    private readonly IUserService _userService;

    public RegistrationWindow()
    {
        InitializeComponent();

        _userService = Injector.CreateInstance<IUserService>();
    }

    private void RegisterButton_Click(object? sender, RoutedEventArgs e)
    {
        string name = NameTextBox.Text?.Trim() ?? "";
        string surname = SurnameTextBox.Text?.Trim() ?? "";
        string phone = PhoneTextBox.Text?.Trim() ?? "";
        string email = EmailTextBox.Text?.Trim() ?? "";
        string address = AddressTextBox.Text?.Trim() ?? "";
        string password = PasswordTextBox.Text ?? "";
        string confirmPassword = ConfirmPasswordTextBox.Text ?? "";

        // Basic validation

        if (GenderComboBox.SelectedItem is not ComboBoxItem genderItem)
        {
            ShowError("Please select your gender.");
            return;
        }

        if (DateOfBirthPicker.SelectedDate == null)
        {
            ShowError("Please select your date of birth.");
            return;
        }

        if (password != confirmPassword)
        {
            ShowError("Passwords do not match.");
            return;
        }

        Gender gender;

        if (genderItem.Content?.ToString() == "Male")
        {
            gender = Gender.Male;
        }
        else
        {
            gender = Gender.Female;
        }

        DateOnly dateOfBirth =
            DateOnly.FromDateTime(
                DateOfBirthPicker.SelectedDate.Value.DateTime
            );

        User user = new User(
            name,
            surname,
            gender,
            dateOfBirth,
            phone,
            email,
            password,
            Role.Client,
            address,
            false
        );

        try
        {
            _userService.Insert(user);
            ShowSuccess();
            BackButton_Click(null, null);
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }

    private void BackButton_Click(object? sender, RoutedEventArgs e)
    {
        if (VisualRoot is MainWindow window)
        {
            window.ShowLogin();
        }
    }

    private void ShowError(string message)
    {
        ErrorTextBlock.Foreground =
            Avalonia.Media.Brushes.Red;

        ErrorTextBlock.Text = message;
        ErrorTextBlock.IsVisible = true;
    }

    private void ShowSuccess()
    {
        ErrorTextBlock.Foreground =
            Avalonia.Media.Brushes.Green;

        ErrorTextBlock.Text =
            "Account created successfully!";

        ErrorTextBlock.IsVisible = true;
    }
}