using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using PetShelter.Application.Domain;
using PetShelter.Application.Services;
using PetShelter.Application.Services.ServiceInterfaces;

namespace PetShelter.Uix.ViewModels;

public class AssociationFormViewModel : INotifyPropertyChanged
{
    private readonly IAssociationService _associationService;
    private readonly IUserService _userService;
    private readonly Association? _association;
    private readonly IFinanceService _financeService;

    private string _errorMessage = "";

    public string AssociationName { get; set; } = "";
    public string DateOfEstablishment { get; set; } = "";
    public string PhoneNumber { get; set; } = "";
    public string EmailAddress { get; set; } = "";
    public string EstablishmentType { get; set; } = "";
    public string Description { get; set; } = "";
    public string Address { get; set; } = "";
    
    public string AccountNumber { get; set; } = "";

    public string AdminName { get; set; } = "";
    public string AdminSurname { get; set; } = "";
    public string AdminGender { get; set; } = "Male";
    public string AdminDateOfBirth { get; set; } = "";
    public string AdminPhoneNumber { get; set; } = "";
    public string AdminEmailAddress { get; set; } = "";
    public string AdminPassword { get; set; } = "";
    public string AdminAddress { get; set; } = "";

    public List<string> Genders { get; } = new()
    {
        "Male",
        "Female"
    };

    public bool IsCreateMode => _association == null;

    public string WindowTitle =>
        IsCreateMode ? "Add Association" : "Edit Association";

    public string SaveButtonText =>
        IsCreateMode ? "Add" : "Save";

    public string ErrorMessage
    {
        get => _errorMessage;
        private set
        {
            _errorMessage = value;
            OnPropertyChanged();
        }
    }

    public AssociationFormViewModel()
    {
        _associationService =
            Injector.CreateInstance<IAssociationService>();

        _userService =
            Injector.CreateInstance<IUserService>();
        
        _financeService =
            Injector.CreateInstance<IFinanceService>();
    }

    public AssociationFormViewModel(Association association)
        : this()
    {
        _association = association;

        AssociationName = association.Name;
        DateOfEstablishment =
            association.DateOfEstabishment.ToString("yyyy-MM-dd");
        PhoneNumber = association.PhoneNumber;
        EmailAddress = association.EmailAddress;
        EstablishmentType = association.EstablishmentType;
        Description = association.Description;
        Address = association.Address;
    }

    public bool Save()
    {
        ErrorMessage = "";

        if (string.IsNullOrWhiteSpace(AssociationName) ||
            string.IsNullOrWhiteSpace(DateOfEstablishment) ||
            string.IsNullOrWhiteSpace(PhoneNumber) ||
            string.IsNullOrWhiteSpace(EmailAddress) ||
            string.IsNullOrWhiteSpace(EstablishmentType) ||
            string.IsNullOrWhiteSpace(Description) ||
            string.IsNullOrWhiteSpace(Address))
        {
            ErrorMessage = "Please fill in all association fields.";
            return false;
        }

        if (!DateOnly.TryParseExact(
                DateOfEstablishment,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateOnly establishmentDate))
        {
            ErrorMessage =
                "Association date must be in yyyy-MM-dd format.";

            return false;
        }

        try
        {
            if (IsCreateMode)
            {
                return CreateAssociation(establishmentDate);
            }

            return UpdateAssociation(establishmentDate);
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            return false;
        }
    }

    private bool CreateAssociation(DateOnly establishmentDate)
    {
        if (string.IsNullOrWhiteSpace(AdminName) ||
            string.IsNullOrWhiteSpace(AdminSurname) ||
            string.IsNullOrWhiteSpace(AdminDateOfBirth) ||
            string.IsNullOrWhiteSpace(AdminPhoneNumber) ||
            string.IsNullOrWhiteSpace(AdminEmailAddress) ||
            string.IsNullOrWhiteSpace(AdminPassword) ||
            string.IsNullOrWhiteSpace(AdminAddress))
        {
            ErrorMessage = "Please fill in all administrator fields.";
            return false;
        }
        
        if (string.IsNullOrWhiteSpace(AccountNumber))
        {
            ErrorMessage = "Please enter the association bank account number.";
            return false;
        }

        if (!DateOnly.TryParseExact(
                AdminDateOfBirth,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateOnly adminDateOfBirth))
        {
            ErrorMessage =
                "Administrator date of birth must be in yyyy-MM-dd format.";

            return false;
        }

        Gender gender =
            AdminGender == "Female"
                ? Gender.Female
                : Gender.Male;

        User admin = new(
            AdminName,
            AdminSurname,
            gender,
            adminDateOfBirth,
            AdminPhoneNumber,
            AdminEmailAddress,
            AdminPassword,
            Role.AssociationAdmin,
            AdminAddress,
            false
        );

        long adminId = _userService.Insert(admin);

        Association association = new(
            AssociationName,
            establishmentDate,
            PhoneNumber,
            EmailAddress,
            EstablishmentType,
            Description,
            Address,
            adminId,
            false
        );

        long associationId =
            _associationService.Insert(association);

        _financeService.CreateBankAccount(
            associationId,
            AccountNumber
        );

        return true;
    }

    private bool UpdateAssociation(DateOnly establishmentDate)
    {
        if (_association == null)
            return false;

        Association updatedAssociation = new(
            _association.Id,
            AssociationName,
            establishmentDate,
            PhoneNumber,
            EmailAddress,
            EstablishmentType,
            Description,
            Address,
            _association.AdminId,
            _association.IsDeleted
        );

        _associationService.Update(updatedAssociation);

        return true;
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