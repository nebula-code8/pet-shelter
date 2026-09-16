namespace PetShelter.Application.Domain;

public class TemporaryAdoption
{
    public long UserId { get; private set; }
    public long AnimalId { get; private set; }
    public DateOnly StartDate { get; private set; }
    public DateOnly? EndDate { get; private set; }

    public TemporaryAdoption(long userId, long animalId, DateOnly startDate, DateOnly? endDate)
    {
        UserId = userId;
        AnimalId = animalId;
        StartDate = startDate;
        EndDate = endDate;
    }
}
