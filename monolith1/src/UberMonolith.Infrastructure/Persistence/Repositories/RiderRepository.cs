using UberMonolith.Domain;
namespace UberMonolith.Infrastructure;

public class RiderRepository : BaseRepository<Rider>, IRiderRepository
{
    public RiderRepository(AppDbContext context) : base(context)
    {  

    }

}
