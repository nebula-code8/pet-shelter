using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using PetShelter.Application.Domain;
using PetShelter.Application.Services;
using PetShelter.Application.Services.ServiceInterfaces;

namespace PetShelter.Uix.ViewModels;

public class AnimalsViewModel : INotifyPropertyChanged
{
    private readonly IAnimalService _animalService;
    private readonly long _associationId;

    private Animal? _selectedAnimal;
    private string _errorMessage = "";

    public string AssociationName { get; }

    public ObservableCollection<Animal> Animals { get; } = new();

    public Animal? SelectedAnimal
    {
        get => _selectedAnimal;
        set
        {
            _selectedAnimal = value;
            OnPropertyChanged();
        }
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        private set
        {
            _errorMessage = value;
            OnPropertyChanged();
        }
    }

    public AnimalsViewModel(
        long associationId,
        string associationName)
    {
        _associationId = associationId;
        AssociationName = associationName;

        _animalService =
            Injector.CreateInstance<IAnimalService>();

        LoadAnimals();
    }

    public void LoadAnimals()
    {
        ErrorMessage = "";
        Animals.Clear();

        try
        {
            List<Animal> animals =
                _animalService.GetAllForAssociationAdmin(
                    _associationId
                );

            foreach (Animal animal in animals)
            {
                Animals.Add(animal);
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }

    public bool DeleteSelectedAnimal()
    {
        ErrorMessage = "";

        if (SelectedAnimal == null)
        {
            ErrorMessage = "Please select an animal.";
            return false;
        }

        try
        {
            bool deleted =
                _animalService.Delete(
                    SelectedAnimal.Id
                );

            if (!deleted)
            {
                ErrorMessage =
                    "Animal could not be deleted.";

                return false;
            }

            LoadAnimals();

            return true;
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            return false;
        }
    }
    
    public Animal? GetSelectedAnimalForEdit()
    {
        ErrorMessage = "";

        if (SelectedAnimal == null)
        {
            ErrorMessage = "Please select an animal.";
            return null;
        }

        return SelectedAnimal;
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