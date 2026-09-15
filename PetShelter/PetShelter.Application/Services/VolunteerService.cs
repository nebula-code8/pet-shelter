using PetShelter.Application.Domain;
using PetShelter.Application.Domain.RepositoryInterfaces;
using PetShelter.Application.Services.ServiceInterfaces;

namespace PetShelter.Application.Services;

public class VolunteerService : IVolunteerService
{
    private readonly IVolunteerRepository _volunteerRepository;

    public VolunteerService()
    {
        _volunteerRepository = Injector.CreateInstance<IVolunteerRepository>();
    }

    public long Insert(Volunteer volunteer)
    {
        Validate(volunteer);

        return _volunteerRepository.Insert(volunteer);
    }
    
    public Volunteer? GetById(long id)
    {
        return _volunteerRepository.GetById(id);
    }

    private static void Validate(Volunteer volunteer)
    {
        if (string.IsNullOrWhiteSpace(volunteer.Name) ||
            string.IsNullOrWhiteSpace(volunteer.Surname) ||
            string.IsNullOrWhiteSpace(volunteer.PhoneNumber) ||
            string.IsNullOrWhiteSpace(volunteer.EmailAddress) ||
            string.IsNullOrWhiteSpace(volunteer.Password) ||
            string.IsNullOrWhiteSpace(volunteer.Address) ||
            string.IsNullOrWhiteSpace(volunteer.VolunteerComment))
        {
            throw new Exception("Please fill in all fields.");
        }

        if (volunteer.AssociationId <= 0)
        {
            throw new Exception(
                "Please select an association."
            );
        }
    }
}