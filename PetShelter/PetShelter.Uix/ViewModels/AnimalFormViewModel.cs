using System.ComponentModel;
using System.Runtime.CompilerServices;
using PetShelter.Application.Domain;
using PetShelter.Application.Services;
using PetShelter.Application.Services.ServiceInterfaces;

namespace PetShelter.Uix.ViewModels;

public class AnimalFormViewModel : INotifyPropertyChanged
{
    private readonly IAnimalService _animalService;
    private readonly long _associationId;
    private readonly Animal? _animal;

    private string _errorMessage = "";

    public string Name { get; set; } = "";
    public string Species { get; set; } = "";
    public string Breed { get; set; } = "";
    public string Gender { get; set; } = "Male";
    public DateTimeOffset? DateOfBirth { get; set; }
    public string Description { get; set; } = "";
    public bool IsVaccinated { get; set; }
    public bool IsSterilized { get; set; }
    public string HealthStatus { get; set; } = "";
    public DateTimeOffset? DateArrived { get; set; }

    public List<string> Genders { get; } = new()
    {
        "Male",
        "Female"
    };

    public bool IsCreateMode => _animal == null;

    public string WindowTitle =>
        IsCreateMode ? "Add Animal" : "Edit Animal";

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

    public AnimalFormViewModel(long associationId)
    {
        _associationId = associationId;

        _animalService =
            Injector.CreateInstance<IAnimalService>();
    }

    public AnimalFormViewModel(
        long associationId,
        Animal animal)
        : this(associationId)
    {
        _animal = animal;

        Name = animal.Name;
        Species = animal.Species;
        Breed = animal.Breed;
        Gender = animal.Gender.ToString();

        DateOfBirth = new DateTimeOffset(
            animal.DateOfBirth.ToDateTime(
                TimeOnly.MinValue
            )
        );

        Description = animal.Description;
        IsVaccinated = animal.IsVaccinated;
        IsSterilized = animal.IsSterilized;
        HealthStatus = animal.HealthStatus;

        DateArrived = new DateTimeOffset(
            animal.DateArrived.ToDateTime(
                TimeOnly.MinValue
            )
        );
    }

    public bool Save()
    {
        ErrorMessage = "";

        if (string.IsNullOrWhiteSpace(Name) ||
            string.IsNullOrWhiteSpace(Species) ||
            string.IsNullOrWhiteSpace(Breed) ||
            DateOfBirth == null ||
            string.IsNullOrWhiteSpace(Description) ||
            string.IsNullOrWhiteSpace(HealthStatus) ||
            DateArrived == null)
        {
            ErrorMessage = "Please fill in all fields.";
            return false;
        }

        DateOnly dateOfBirth =
            DateOnly.FromDateTime(
                DateOfBirth.Value.DateTime
            );

        DateOnly dateArrived =
            DateOnly.FromDateTime(
                DateArrived.Value.DateTime
            );

        if (dateOfBirth > DateOnly.FromDateTime(DateTime.Today))
        {
            ErrorMessage =
                "Date of birth cannot be in the future.";

            return false;
        }

        if (dateArrived > DateOnly.FromDateTime(DateTime.Today))
        {
            ErrorMessage =
                "Arrival date cannot be in the future.";

            return false;
        }

        if (dateArrived < dateOfBirth)
        {
            ErrorMessage =
                "Arrival date cannot be before date of birth.";

            return false;
        }

        PetShelter.Application.Domain.Gender gender =
            Gender == "Female"
                ? PetShelter.Application.Domain.Gender.Female
                : PetShelter.Application.Domain.Gender.Male;

        try
        {
            if (IsCreateMode)
            {
                Animal animal = new(
                    Name.Trim(),
                    Species.Trim(),
                    Breed.Trim(),
                    gender,
                    dateOfBirth,
                    Description.Trim(),
                    IsVaccinated,
                    IsSterilized,
                    HealthStatus.Trim(),
                    dateArrived,
                    _associationId,
                    false
                );

                _animalService.Insert(animal);

                return true;
            }

            if (_animal == null)
                return false;

            Animal updatedAnimal = new(
                _animal.Id,
                Name.Trim(),
                Species.Trim(),
                Breed.Trim(),
                gender,
                dateOfBirth,
                Description.Trim(),
                IsVaccinated,
                IsSterilized,
                HealthStatus.Trim(),
                dateArrived,
                _associationId,
                _animal.IsDeleted
            );

            _animalService.Update(updatedAnimal);

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