using PetShelter.Application.Domain;

namespace PetShelter.Application.Services.ServiceInterfaces;

public interface IVolunteerService
{
    long Insert(Volunteer volunteer);
}