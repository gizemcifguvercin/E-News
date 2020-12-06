using Models;
using MongoDB.Driver;

namespace Infrastructure.Contracts
{
    public interface IMongoDbContext
    {
        void Init();
        IMongoDatabase db { get; }
    }
}