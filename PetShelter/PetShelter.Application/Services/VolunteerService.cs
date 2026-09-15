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
    
    public List<Volunteer> GetByAssociationId(long associationId)
    {
        return _volunteerRepository.GetByAssociationId(
            associationId
        );
    }
    
    public void Approve(long volunteerId)
    {
        Volunteer? volunteer = _volunteerRepository.GetById(volunteerId);

        if (volunteer == null)
        {
            throw new Exception("Volunteer was not found.");
        }
        if (volunteer.Status != VolunteerStatus.Pending)
        {
            throw new Exception("Only pending volunteer requests can be approved.");
        }
        if (!_volunteerRepository.UpdateStatus(volunteerId, VolunteerStatus.Approved))
        {
            throw new Exception("Volunteer status could not be updated.");
            
        }
    }
    
    public void Reject(long volunteerId)
    {
        Volunteer? volunteer = _volunteerRepository.GetById(volunteerId);

        if (volunteer == null)
        {
            throw new Exception("Volunteer was not found.");
        }
        if (volunteer.Status != VolunteerStatus.Pending)
        {
            throw new Exception("Only pending volunteer requests can be rejected.");
        }

        if (!_volunteerRepository.UpdateStatus(volunteerId, VolunteerStatus.Rejected))
        {
            throw new Exception("Volunteer status could not be updated.");
        }
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