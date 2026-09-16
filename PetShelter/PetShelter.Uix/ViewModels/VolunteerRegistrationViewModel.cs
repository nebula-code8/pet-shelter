using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using PetShelter.Application.Domain;
using PetShelter.Application.Services;
using PetShelter.Application.Services.ServiceInterfaces;

namespace PetShelter.Uix.ViewModels;

public class VolunteerRegistrationViewModel : INotifyPropertyChanged
{
    private readonly IVolunteerService _volunteerService;
    private readonly IAssociationService _associationService;
    private readonly IUserService _userService;

    private string _errorMessage = "";

    public string Name { get; set; } = "";
    public string Surname { get; set; } = "";
    public Gender? SelectedGender { get; set; }
    public DateTimeOffset? DateOfBirth { get; set; }
    public string PhoneNumber { get; set; } = "";
    public string EmailAddress { get; set; } = "";
    public string Address { get; set; } = "";
    public string Password { get; set; } = "";
    public string ConfirmPassword { get; set; } = "";
    public string Comment { get; set; } = "";

    public Association? SelectedAssociation { get; set; }

    public List<Gender> Genders { get; } = new()
    {
        Gender.Male,
        Gender.Female
    };

    public ObservableCollection<Association> Associations { get; } = new();

    public string ErrorMessage
    {
        get => _errorMessage;
        private set
        {
            _errorMessage = value;
            OnPropertyChanged();
        }
    }

    public VolunteerRegistrationViewModel()
    {
        _volunteerService =
            Injector.CreateInstance<IVolunteerService>();

        _associationService =
            Injector.CreateInstance<IAssociationService>();

        _userService =
            Injector.CreateInstance<IUserService>();

        LoadAssociations();
    }

    private void LoadAssociations()
    {
        Associations.Clear();

        try
        {
            List<Association> associations =
                _associationService.GetAll();

            foreach (Association association in associations)
            {
                Associations.Add(association);
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }

    public bool Register()
    {
        ErrorMessage = "";

        if (string.IsNullOrWhiteSpace(Name) ||
            string.IsNullOrWhiteSpace(Surname) ||
            string.IsNullOrWhiteSpace(PhoneNumber) ||
            string.IsNullOrWhiteSpace(EmailAddress) ||
            string.IsNullOrWhiteSpace(Address) ||
            string.IsNullOrWhiteSpace(Password) ||
            string.IsNullOrWhiteSpace(ConfirmPassword) ||
            string.IsNullOrWhiteSpace(Comment))
        {
            ErrorMessage = "Please fill in all fields.";
            return false;
        }

        if (SelectedGender == null)
        {
            ErrorMessage = "Please select your gender.";
            return false;
        }

        if (DateOfBirth == null)
        {
            ErrorMessage = "Please select your date of birth.";
            return false;
        }

        if (SelectedAssociation == null)
        {
            ErrorMessage = "Please select an association.";
            return false;
        }

        if (Password != ConfirmPassword)
        {
            ErrorMessage = "Passwords do not match.";
            return false;
        }

        DateOnly dateOfBirth =
            DateOnly.FromDateTime(DateOfBirth.Value.DateTime);

        if (dateOfBirth > DateOnly.FromDateTime(DateTime.Today))
        {
            ErrorMessage =
                "Date of birth cannot be in the future.";

            return false;
        }

        if (_userService.ExistsByEmail(
                EmailAddress.Trim()))
        {
            ErrorMessage =
                "A user with this email already exists.";

            return false;
        }

        Volunteer volunteer = new(
            Name.Trim(),
            Surname.Trim(),
            SelectedGender.Value,
            dateOfBirth,
            PhoneNumber.Trim(),
            EmailAddress.Trim(),
            Password,
            Role.Volunteer,
            Address.Trim(),
            false,
            SelectedAssociation.Id,
            Comment.Trim(),
            VolunteerStatus.Pending
        );

        try
        {
            _volunteerService.Insert(volunteer);
            return true;
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            return false;
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