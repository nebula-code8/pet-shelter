namespace PetShelter.Uix.ViewModels;

public class AdminViewModel
{
    public long UserId { get; }

    public AdminViewModel(long userId)
    {
        UserId = userId;
    }
}