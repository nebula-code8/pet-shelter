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
    private readonly IVolunteerService _volunteerService;
    private bool _isGhost;

    public LoginWindow(bool _isGhost = false)
    {
        InitializeComponent();

        _userService = Injector.CreateInstance<IUserService>();
        _volunteerService = Injector.CreateInstance<IVolunteerService>();

        if (_isGhost)
        {
            GhostLoginButton.IsVisible = false;
        }
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
                window.LoginSuccessful(userId);
            }

            if (role == Role.Admin)
            {
                AdminWindow adminWindow = new(userId);
                adminWindow.Show();
                window.Close();
                return;
            }

            if (role == Role.AssociationAdmin)
            {
                AssociationAdminWindow associationAdminWindow = new(userId);
                associationAdminWindow.Show();
                window.Close();
                return;
            }

            if (role == Role.Volunteer)
            {
                Volunteer? volunteer = _volunteerService.GetById(userId);

                if (volunteer == null)
                {
                    ShowError("Volunteer profile was not found.");
                    return;
                }

                if (volunteer.Status == VolunteerStatus.Pending)
                {
                    ShowError("Your volunteer request is still pending approval.");
                    return;
                }

                if (volunteer.Status == VolunteerStatus.Rejected)
                {
                    ShowError("Your volunteer request has been rejected.");
                    return;
                }

                VolunteerWindow volunteerWindow = new(userId);
                volunteerWindow.Show();
                window.Close();
                return;
            }

            ShowError("This user role is not supported yet.");
        }
    }

    private void RegisterButton_Click(object? sender, RoutedEventArgs e)
    {
        if (VisualRoot is MainWindow window)
        {
            window.ShowRegistration();
        }
    }
    
    private void RegisterVolunteerButton_Click(
        object? sender,
        RoutedEventArgs e)
    {
        VolunteerRegistrationWindow registrationWindow = new();
        registrationWindow.Show();
        if (VisualRoot is MainWindow window)
        {
            window.Close();
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