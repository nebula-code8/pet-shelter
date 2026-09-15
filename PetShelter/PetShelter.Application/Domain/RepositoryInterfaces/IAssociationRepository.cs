namespace PetShelter.Application.Domain.RepositoryInterfaces;

public interface IAssociationRepository
{
    public long Insert(Association association);
    public int Update(Association association);
    public Association? GetById(long id);
    Association? GetByAdminId(long adminId);
    public List<Association> GetAll();
    public bool Delete(long id);
    
}