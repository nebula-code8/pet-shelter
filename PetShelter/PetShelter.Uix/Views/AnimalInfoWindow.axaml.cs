using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using PetShelter.Application.Domain;
using PetShelter.Application.Domain.RepositoryInterfaces;
using PetShelter.Application.Services;

namespace PetShelter.Uix.Views;

public partial class AnimalInfoWindow : Window
{
    private List<AdoptionRequest> _adoptionRequests;
    public AnimalInfoWindow(Animal animal, long userId)
    {
        InitializeComponent();
        GetAdoptionRequest(userId, animal.Id);

        NameTextBlock.Text = animal.Name;
        SpeciesTextBlock.Text = animal.Species;
        BreedTextBlock.Text = animal.Breed;
        GenderTextBlock.Text = animal.Gender.ToString();
        DateOfBirthTextBlock.Text = animal.DateOfBirth.ToString();

        DescriptionTextBlock.Text = animal.Description;

        VaccinatedTextBlock.Text =
            animal.IsVaccinated ? "Yes" : "No";

        SterilizedTextBlock.Text =
            animal.IsSterilized ? "Yes" : "No";

        HealthStatusTextBlock.Text = animal.HealthStatus;
        DateArrivedTextBlock.Text = animal.DateArrived.ToString();

        AssociationIdTextBlock.Text =
            animal.AssociationId.ToString();
        
        CancelRequestButton.IsVisible = CanCancle();
        CancelRequestButton.IsEnabled = CanCancle();
    }
    
    public void GetAdoptionRequest(long userId, long animalId)
    {
        IAdoptionRepository _adoptionRepository = Injector.CreateInstance<IAdoptionRepository>();
        var adoptionRequests = _adoptionRepository.GetAll();
        _adoptionRequests = adoptionRequests.Where(r => r.UserId == userId && r.AnimalId == animalId).ToList();
    }

    private bool CanCancle()
    {
        if (_adoptionRequests.Any(r => r.AdoptionDate != null))
        {
            return false;
        }

        return true;
    }

    private async void CloseButton_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }

    private async void CancleButton_Click(object? sender, RoutedEventArgs e)
    {
        IAdoptionRepository _adoptionRepository = Injector.CreateInstance<IAdoptionRepository>();

        foreach (var request in _adoptionRequests)
        {
            request.Cancle();
            _adoptionRepository.Update(request);
        }

        await PopupWindow.ShowMessage(this, "Request Cancelation", "Adoption request has been canceld successfully.");
        Close();
    }
}