using PetShelter.Application.Domain;

namespace PetShelter.Application.Services.ServiceInterfaces;

public interface IAssociationService
{
    long Insert(Association association);
    void Update(Association association);
    Association? GetById(long id);
    List<Association> GetAll();
    void Delete(long id);
}