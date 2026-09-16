using PetShelter.Application.Domain;

namespace PetShelter.Application.Services.ServiceInterfaces;

public interface IVolunteerService
{
    long Insert(Volunteer volunteer);

    Volunteer? GetById(long id);

    List<Volunteer> GetByAssociationId(long associationId);

    void Approve(long volunteerId);

    void Reject(long volunteerId);
}