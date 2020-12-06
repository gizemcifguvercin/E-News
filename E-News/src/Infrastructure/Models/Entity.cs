using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using MediatR;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Models
{ 
    [BsonIgnoreExtraElements(Inherited = true)]
    public abstract class Entity
    {
        [BsonIgnore]
        public virtual string Id { get { return _id.ToString(); } set { _id = ObjectId.Parse(value); } }

        [BsonId]
        [BsonIgnoreIfDefault]
        [JsonIgnore]
        public virtual ObjectId _id { get; set; }
        public virtual DateTime CreatedOn { get; private set; }
        public virtual bool IsActive { get; set; }         
        private List<INotification> _domainEvents = new List<INotification>();

        [JsonIgnore]
        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents?.AsReadOnly();

        public Entity(bool isActive)
        {
             CreatedOn = DateTime.Now;
            _domainEvents = new List<INotification>();
            IsActive = isActive;
        }

        public void AddDomainEvent(INotification eventItem)
        {
            _domainEvents = _domainEvents ?? new List<INotification>();
            _domainEvents.Add(eventItem);
        }
        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }
    }
}
 