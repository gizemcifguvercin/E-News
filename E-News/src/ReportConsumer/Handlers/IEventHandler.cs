using System.Threading.Tasks;

namespace ReportConsumer.Handlers
{
    public interface IEventHandler<T>
    {
        Task Handle(T message);
    }
}