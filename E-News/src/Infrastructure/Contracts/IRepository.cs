using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace Infrastructure.Contracts
{
    public interface IRepository 
    {
    }
 
    public interface IRepository<T> : IRepository where T : Entity
    { 
        Task<bool> InsertAsync(T item); 
        Task<bool> Save(T item); 
        Task<List<T>> FindAsync();

    }
}