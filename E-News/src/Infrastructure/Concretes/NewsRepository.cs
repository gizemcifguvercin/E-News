using Infrastructure.Contracts;
using MediatR;
using Models;

namespace Infrastructure.Concretes
{
    public class NewsRepository : Repository<News>, INewsRepository
    {
        public NewsRepository(IMongoDbContext context, IMediator mediator) : base(context, mediator)
        {
        }
    }
} 