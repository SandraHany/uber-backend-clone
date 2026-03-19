namespace UberMonolith.Domain;

public interface IUnitOfWork : IDisposable
{
    IRiderRepository Riders { get; }

    int Complete();

}
