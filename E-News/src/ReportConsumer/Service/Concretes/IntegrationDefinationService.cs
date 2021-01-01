using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Concretes;
using Microsoft.Extensions.Caching.Memory;
using Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using ReportConsumer.Service.Contracts;

namespace ReportConsumer.Service.Concretes
{
    public class IntegrationDefinationService : IIntegrationDefinationService
    {
        private readonly IMemoryCache _memoryCache;
        private IIntegrationDefinationRepository _integrationDefinationRepository;

        public IntegrationDefinationService(IMemoryCache memoryCache,
            IIntegrationDefinationRepository integrationDefinationRepository)
        {
            _memoryCache = memoryCache;
            _integrationDefinationRepository = integrationDefinationRepository;
        }

        public async Task<List<AgencyInfo>> GetDefinationsFromDB()
        {
            var documents = await _integrationDefinationRepository.FindAsync();
            if (documents.Count == 0) throw new Exception("Entegrasyon detayı bulunamadı");

            var integrationDefinations =
                BsonSerializer.Deserialize<List<AgencyInfo>>(documents.ToJson()).ToList();
            return integrationDefinations;
        }

        public Task<List<AgencyInfo>> GetDefinationsFromCache() =>
            _memoryCache.GetOrCreate("integrationdata", entry => GetDefinationsFromDB());


        public async Task<AgencyInfo> GetIntegrationDetails(string agencyCode)
        {
            var details = await GetDefinationsFromCache();
            var model = details.FirstOrDefault(
                x => x.AgencyCode == agencyCode);
            return model;
        }
    }
}