using System;

namespace Models
{
    public class NewsCreated  
    { 
        public string AgencyCode { get; set; }
        public string NewsContent { get; set; }
        public  DateTime CreatedOn { get; private set; }
        public  bool IsActive { get; set; }   
         
         public NewsCreated(string agencyCode, string newsContent, DateTime createdOn, bool isActive )
        {
            AgencyCode = agencyCode;
            NewsContent = newsContent;
            CreatedOn = createdOn;
            IsActive = isActive;
        }
    }
}