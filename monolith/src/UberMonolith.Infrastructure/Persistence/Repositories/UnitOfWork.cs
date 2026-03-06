using UberMonolith.Domain;  
namespace UberMonolith.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public IRiderRepository Riders { get; private set; }

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Riders = new RiderRepository(_context);
    }

    public int Complete()
    {
        return _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
