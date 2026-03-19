namespace UberMonolith.Domain;

public interface IRideRepository : IRepository<Ride>
{
   void RequestRide(Ride ride);
    void GetRideFareEstimate();
}
