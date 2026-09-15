using PetShelter.Application.Domain;

namespace PetShelter.Application.Services.ServiceInterfaces;

public interface IAssociationService
{
    public List<Association> GetAll();
}