using System.Threading.Tasks;
using Infrastructure.Contracts;
using MediatR;
using Models; 
using MongoDB.Driver;

namespace Infrastructure.Concretes
{
    public class Repository<T> : IRepository<T> 
    where T : Entity
    {
        protected readonly IMongoCollection<T> _items;
        private readonly IMediator _mediator;

        public Repository(IMongoDbContext context, IMediator mediator)
        {
            _items = context.db.GetCollection<T>(typeof(T).Name);
            _mediator = mediator;
        }

        public virtual async Task<bool> InsertAsync(T item)
        {
            await _items.InsertOneAsync(item); 
            return true;        
        }

        public virtual async Task<bool> Save(T item)
        {
            bool result = false;

            if (item.DomainEvents != null)
            {
                foreach (var domainEvent in item.DomainEvents)
                {
                    await _mediator.Publish(domainEvent);
                }

                item.ClearDomainEvents();
            }

            result = await InsertAsync(item); 
            return result;     
       } 
    }
} 