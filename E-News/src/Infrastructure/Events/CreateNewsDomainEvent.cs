using MediatR;
using Models;

namespace Infrastructure.Events 
{
    public class CreateNewsDomainEvent : INotification
    {
        public News News { get; set; }

        public CreateNewsDomainEvent(News news) 
        {
            News = news;
        }
    }

}