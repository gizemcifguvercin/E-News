using Infrastructure.Contracts;
using Microsoft.Extensions.Options;
using Models;
using MongoDB.Bson;
using MongoDB.Driver; 

namespace Infrastructure.Concretes
{
    public class MongoDbContext : IMongoDbContext
    {
        private static IMongoDatabase _db;
        private static MongoClient _client;
        private readonly DatabaseSettings _databaseSettings;
        public IMongoDatabase db => _db;

        public MongoDbContext(IOptions<DatabaseSettings> databaseSettings)
        {
            _databaseSettings = databaseSettings.Value;
            if(_db == null)
                Init();
        }

        public void Init()
        {
            if (_client == null)
                _client = new MongoClient(CreateSettings());

            if (_client != null && _db == null)
                _db = _client.GetDatabase(_databaseSettings.DatabaseName);
        }

        private MongoClientSettings CreateSettings()
        {
            var internalIdentity = new MongoInternalIdentity("admin", _databaseSettings.UserName);
            var passwordEvidence = new PasswordEvidence(_databaseSettings.Password);
            var mongoCredential = new MongoCredential(_databaseSettings.AuthMechanism, internalIdentity, passwordEvidence);
 
            return new MongoClientSettings
            {
                Credential = mongoCredential,  
                ReadPreference = ReadPreference.SecondaryPreferred,
                WriteConcern = WriteConcern.Acknowledged,
                GuidRepresentation = GuidRepresentation.Standard
            };
        } 
    } 
}
