using Infrastructure.Events;

namespace Models
{
    public class News : Entity
    {
        public string AgencyCode { get; set; }
        public string NewsContent { get; set; }
        
        public News(string agencyCode, string newsContent, bool isActive) : base(isActive)
        {
            AgencyCode = agencyCode;
            NewsContent = newsContent;
        }
        public void CreateNewsMessage()
        {
            AddDomainEvent(new CreateNewsDomainEvent(this));
        }
    }
}