namespace Models
{
    public class AgencyInfo : Entity
    {
        public string AgencyCode { get; set; }
        public string EndpointURL { get; set; }
        public string User { get; set; }
        public string Password { get; set; }

        public AgencyInfo(bool isActive) : base(isActive)
        {
        }
    }
}