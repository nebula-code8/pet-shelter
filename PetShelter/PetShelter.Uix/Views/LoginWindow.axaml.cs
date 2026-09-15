using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using PetShelter.Application.Domain;
using PetShelter.Application.Repository;
using PetShelter.Application.Services;
using PetShelter.Application.Services.ServiceInterfaces;

namespace PetShelter.Uix.Views;

public partial class LoginWindow : UserControl
{
    private readonly IUserService _userService;

    public LoginWindow()
    {
        InitializeComponent();
        _userService = Injector.CreateInstance<IUserService>();
    }

    private void LoginButton_Click(object? sender, RoutedEventArgs e)
    {
        string email = EmailTextBox.Text ?? "";
        string password = PasswordTextBox.Text ?? "";
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            ShowError("Please enter your email and password.");
            return;
        }

        var result = _userService.AuthenticateUser(email, password);
        if (result == null)
        {
            ShowError("Invalid email or password.");
            return;
        }

        long userId = result.Value.Id;
        Role role = result.Value.Role;
        ErrorTextBlock.IsVisible =
            false; 
        
        if (VisualRoot is MainWindow window)
        {
            if (role == Role.Client)
            {
                window.ShowClientHome(userId);
            }
        }
        
        // TODO:
        // Navigate to the appropriate page depending on the role.
        // // // Example:
        // // // if (role == Role.Client)
        // // MainWindow.Navigate(new ClientView(userId));
        // // // else if (role == Role.Volunteer)
        // // MainWindow.Navigate(new VolunteerView(userId));
        // // // else if (role == Role.Admin)
        // // MainWindow.Navigate(new AdminView(userId));
    }

    private void RegisterButton_Click(object? sender, RoutedEventArgs e)
    {
        if (VisualRoot is MainWindow window)
        {
            window.ShowRegistration();
        }
    }

    private void ShowError(string message)
    {
        ErrorTextBlock.Text = message;
        ErrorTextBlock.IsVisible = true;
    }

    private void GhostLogin_Click(object? sender, RoutedEventArgs e)
    {
        if (VisualRoot is MainWindow window)
        {
            window.ShowGhostHome();
        }
    }
}