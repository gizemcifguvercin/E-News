using Infrastructure.Contracts;
using MediatR;
using Models;

namespace Infrastructure.Concretes
{
    public class AgencyInfoRepository : Repository<AgencyInfo>, IAgencyInfoRepository
    {
        public AgencyInfoRepository(IMongoDbContext context, IMediator mediator) : base(context, mediator)
        {
        }
    }
} 