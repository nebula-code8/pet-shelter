using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using PetShelter.Application.Domain;
using PetShelter.Application.Services;
using PetShelter.Application.Services.ServiceInterfaces;

namespace PetShelter.Uix.ViewModels;

public class UsersViewModel : INotifyPropertyChanged
{
    private readonly IUserService _userService;

    private string _errorMessage = "";

    public ObservableCollection<User> Users { get; } = new();

    public string ErrorMessage
    {
        get => _errorMessage;
        private set
        {
            _errorMessage = value;
            OnPropertyChanged();
        }
    }

    public UsersViewModel()
    {
        _userService = Injector.CreateInstance<IUserService>();

        LoadUsers();
    }

    public void LoadUsers()
    {
        ErrorMessage = "";
        Users.Clear();

        try
        {
            List<User> users = _userService.GetAll();

            foreach (User user in users)
            {
                Users.Add(user);
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(
        [CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(propertyName)
        );
    }
}