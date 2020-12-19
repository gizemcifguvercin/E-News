using MediatR;
using Models;

namespace Infrastructure.Events 
{
    public class NewsDomainEvent : INotification
    {
        public News News { get; set; }

        public NewsDomainEvent(News news) 
        {
            News = news;
        }
    }

}