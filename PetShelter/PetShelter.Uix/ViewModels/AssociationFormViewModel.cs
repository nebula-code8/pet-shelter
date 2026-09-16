using System.ComponentModel;
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
    public DateTimeOffset? DateOfEstablishment { get; set; }
    public string PhoneNumber { get; set; } = "";
    public string EmailAddress { get; set; } = "";
    public string EstablishmentType { get; set; } = "";
    public string Description { get; set; } = "";
    public string Address { get; set; } = "";

    public string AccountNumber { get; set; } = "";

    public string AdminName { get; set; } = "";
    public string AdminSurname { get; set; } = "";
    public string AdminGender { get; set; } = "Male";
    public DateTimeOffset? AdminDateOfBirth { get; set; }
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
        DateOfEstablishment = new DateTimeOffset(
            association.DateOfEstabishment.ToDateTime(
                TimeOnly.MinValue
            )
        );
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
            DateOfEstablishment == null ||
            string.IsNullOrWhiteSpace(PhoneNumber) ||
            string.IsNullOrWhiteSpace(EmailAddress) ||
            string.IsNullOrWhiteSpace(EstablishmentType) ||
            string.IsNullOrWhiteSpace(Description) ||
            string.IsNullOrWhiteSpace(Address))
        {
            ErrorMessage = "Please fill in all association fields.";
            return false;
        }

        DateOnly establishmentDate =
            DateOnly.FromDateTime(
                DateOfEstablishment.Value.DateTime
            );

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
            AdminDateOfBirth == null ||
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

        DateOnly adminDateOfBirth =
            DateOnly.FromDateTime(
                AdminDateOfBirth.Value.DateTime
            );

        if (_userService.ExistsByEmail(
                AdminEmailAddress.Trim()))
        {
            ErrorMessage =
                "A user with this email already exists.";

            return false;
        }

        if (_associationService.ExistsByEmail(
                EmailAddress.Trim()))
        {
            ErrorMessage =
                "An association with this email already exists.";

            return false;
        }

        if (_financeService.AccountNumberExists(
                AccountNumber.Trim()))
        {
            ErrorMessage =
                "This bank account number is already in use.";

            return false;
        }

        Gender gender =
            AdminGender == "Female"
                ? Gender.Female
                : Gender.Male;

        User admin = new(
            AdminName.Trim(),
            AdminSurname.Trim(),
            gender,
            adminDateOfBirth,
            AdminPhoneNumber.Trim(),
            AdminEmailAddress.Trim(),
            AdminPassword,
            Role.AssociationAdmin,
            AdminAddress.Trim(),
            false
        );

        long adminId =
            _userService.Insert(admin);

        Association association = new(
            AssociationName.Trim(),
            establishmentDate,
            PhoneNumber.Trim(),
            EmailAddress.Trim(),
            EstablishmentType.Trim(),
            Description.Trim(),
            Address.Trim(),
            adminId,
            false
        );

        long associationId =
            _associationService.Insert(association);

        _financeService.CreateBankAccount(
            associationId,
            AccountNumber.Trim()
        );

        return true;
    }

    private bool UpdateAssociation(DateOnly establishmentDate)
    {
        if (_association == null)
            return false;

        if (_associationService.ExistsByEmail(
                EmailAddress.Trim(),
                _association.Id))
        {
            ErrorMessage =
                "An association with this email already exists.";

            return false;
        }

        Association updatedAssociation = new(
            _association.Id,
            AssociationName.Trim(),
            establishmentDate,
            PhoneNumber.Trim(),
            EmailAddress.Trim(),
            EstablishmentType.Trim(),
            Description.Trim(),
            Address.Trim(),
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