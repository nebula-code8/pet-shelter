using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using PetShelter.Application.Domain;
using PetShelter.Application.Services;
using PetShelter.Application.Services.ServiceInterfaces;

namespace PetShelter.Uix.ViewModels;

public class VolunteersViewModel :
    INotifyPropertyChanged
{
    private readonly IVolunteerService _volunteerService;
    private readonly long _associationId;

    private Volunteer? _selectedVolunteer;
    private string _errorMessage = "";

    public string AssociationName { get; }

    public ObservableCollection<Volunteer> Volunteers { get; } = new();

    public Volunteer? SelectedVolunteer
    {
        get => _selectedVolunteer;

        set
        {
            _selectedVolunteer = value;
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

    public VolunteersViewModel(
        long associationId,
        string associationName)
    {
        _associationId = associationId;
        AssociationName = associationName;

        _volunteerService = Injector.CreateInstance<IVolunteerService>();

        LoadVolunteers();
    }

    public void LoadVolunteers()
    {
        ErrorMessage = "";
        Volunteers.Clear();

        try
        {
            List<Volunteer> volunteers = _volunteerService.GetByAssociationId(_associationId);

            foreach (Volunteer volunteer in volunteers)
            {
                Volunteers.Add(volunteer);
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }

    public void ApproveSelectedVolunteer()
    {
        ErrorMessage = "";

        if (SelectedVolunteer == null)
        {
            ErrorMessage = "Please select a volunteer.";

            return;
        }

        try
        {
            _volunteerService.Approve(SelectedVolunteer.Id);

            LoadVolunteers();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }

    public void RejectSelectedVolunteer()
    {
        ErrorMessage = "";

        if (SelectedVolunteer == null)
        {
            ErrorMessage = "Please select a volunteer.";

            return;
        }

        try
        {
            _volunteerService.Reject(SelectedVolunteer.Id);

            LoadVolunteers();
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
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}