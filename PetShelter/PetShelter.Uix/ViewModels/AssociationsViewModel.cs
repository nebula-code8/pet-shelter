using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using PetShelter.Application.Domain;
using PetShelter.Application.Services;
using PetShelter.Application.Services.ServiceInterfaces;

namespace PetShelter.Uix.ViewModels;

public class AssociationsViewModel : INotifyPropertyChanged
{
    private readonly IAssociationService _associationService;

    private Association? _selectedAssociation;
    private string _errorMessage = "";

    public ObservableCollection<Association> Associations { get; } = new();

    public Association? SelectedAssociation
    {
        get => _selectedAssociation;
        set
        {
            _selectedAssociation = value;
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

    public AssociationsViewModel()
    {
        _associationService = Injector.CreateInstance<IAssociationService>();

        LoadAssociations();
    }

    public void LoadAssociations()
    {
        ErrorMessage = "";
        Associations.Clear();

        try
        {
            List<Association> associations = _associationService.GetAll();

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

    public void DeleteSelectedAssociation()
    {
        ErrorMessage = "";

        if (SelectedAssociation == null)
        {
            ErrorMessage = "Please select an association.";
            return;
        }

        try
        {
            _associationService.Delete(SelectedAssociation.Id);

            Associations.Remove(SelectedAssociation);
            SelectedAssociation = null;
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }
    
    public Association? GetSelectedAssociationForEdit()
    {
        ErrorMessage = "";

        if (SelectedAssociation == null)
        {
            ErrorMessage = "Please select an association.";
            return null;
        }

        return SelectedAssociation;
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}