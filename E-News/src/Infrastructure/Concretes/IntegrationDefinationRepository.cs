using Infrastructure.Contracts;
using MediatR;
using Models;

namespace Infrastructure.Concretes
{
    public class IntegrationDefinationRepository : Repository<AgencyInfo>, IIntegrationDefinationRepository
    {
        public IntegrationDefinationRepository(IMongoDbContext context, IMediator mediator) : base(context, mediator)
        {
        }
    }
} 