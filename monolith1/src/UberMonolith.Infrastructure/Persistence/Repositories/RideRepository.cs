namespace UberMonolith.Infrastructure;

public class RideRepository: BaseRepository<Ride>, IRiderRepository
{
    public RiderRepository(AppDbContext context) : base(context)
    {  

    }
}