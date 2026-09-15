using Avalonia.Controls;
using PetShelter.Application.Domain;
using PetShelter.Uix.Views;

namespace PetShelter.Uix;

public partial class MainWindow : Window
{
    private long? _loggedInUserId;

    public MainWindow()
    {
        InitializeComponent();

        ShowLogin();
    }

    public void ShowLogin()
    {
        _loggedInUserId = null;

        MainContent.Content = new LoginWindow();
    }

    public void ShowRegistration()
    {
        MainContent.Content = new RegistrationWindow();
    }

    public void ShowClientHome(long userId)
    {
        _loggedInUserId = userId;

        MainContent.Content =
            new ClientHomeView(userId);
    }

    public void ShowOrganizationAnimals(
        Association association)
    {
        MainContent.Content =
            new OrganizationAnimalView(association);
    }
    
    public void ShowClientHome()
    {
        if (_loggedInUserId == null)
        {
            ShowLogin();
            return;
        }

        MainContent.Content =
            new ClientHomeView(_loggedInUserId.Value);
    }
    
    
}