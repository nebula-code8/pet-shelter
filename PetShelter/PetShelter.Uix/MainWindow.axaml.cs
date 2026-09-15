using Avalonia.Controls;
using PetShelter.Application.Domain;
using PetShelter.Application.Services;
using PetShelter.Application.Services.ServiceInterfaces;
using PetShelter.Uix.Views;

namespace PetShelter.Uix;

public partial class MainWindow : Window
{
    private long? _loggedInUserId;
    private Animal? _pendingAdoptionAnimal;
    
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

    public void ShowGhostHome()
    {
        _loggedInUserId = null;
        MainContent.Content = new ClientHomeView();
    }

    public bool IsGhost()
    {
        return _loggedInUserId == null;
    }
    
    public long? GetLoggedInUserId()
    {
        return _loggedInUserId;
    }
    
    public void AdoptAnimal(Animal animal)
    {
        if (_loggedInUserId == null)
        {
            // Remember what the guest wanted to adopt
            _pendingAdoptionAnimal = animal;

            // Show login page
            MainContent.Content = new LoginWindow(true);

            return;
        }

        SendAdoptionRequest(animal);
    }
    
    private void SendAdoptionRequest(Animal animal)
    {
        if (_loggedInUserId == null)
            return;

        IAdoptionRequestService adoptionRequestService =
            Injector.CreateInstance<IAdoptionRequestService>();

        adoptionRequestService.SendAddoptionRequest(
            _loggedInUserId.Value,
            animal.Id);

        PopupWindow.ShowMessage(
            this,
            "Adoption Request",
            "Vas zahtev za udomljavanje je uspesno prosledjen.");
    }
    
    public void LoginSuccessful(long userId)
    {
        _loggedInUserId = userId;

        if (_pendingAdoptionAnimal != null)
        {
            Animal animal = _pendingAdoptionAnimal;

            // Clear it so it can't accidentally be submitted twice
            _pendingAdoptionAnimal = null;

            SendAdoptionRequest(animal);

            
        }

        ShowClientHome(userId);
    }
}