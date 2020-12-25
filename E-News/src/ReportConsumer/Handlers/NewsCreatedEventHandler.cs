using System.Threading.Tasks;
using Models;

namespace ReportConsumer.Handlers
{
    public class NewsCreatedEventHandler : IEventHandler<CreateMessage>
    {
       public NewsCreatedEventHandler()
       {

       }

        public async Task Handle<T>(T message)
        {
            var msg = message as CreateMessage;
            //TO-DO
        }
    }
}