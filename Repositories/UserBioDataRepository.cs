using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using health_pal_backend.Models;
using health_pal_backend.Repositories;

namespace Dotnet_Core_Project.Repositories
{
    public class UserBioDataRepository : IUserBioDataRepository
    {
        private readonly Container _container;

        public UserBioDataRepository(CosmosClient cosmosClient, IConfiguration configuration)
        {
            _container = cosmosClient.GetContainer(configuration["CosmosDb:DatabaseName"], configuration["CosmosDb:ContainerName"]);
        }

        public async Task AddAsync(UserBioDataModel userBioData)
        {
            await _container.CreateItemAsync(userBioData, new PartitionKey(userBioData.Id));
        }

        public async Task<UserBioDataModel?> GetIdByAsync(int id)
        {
            try
            {
                var response = await _container.ReadItemAsync<UserBioDataModel>(id.ToString(), new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<IEnumerable<UserBioDataModel>> GetAllAsync()
        {
            var query = _container.GetItemQueryIterator<UserBioDataModel>();
            var results = new List<UserBioDataModel>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response);
            }
            return results;
        }

        public async Task UpdateAsync(UserBioDataModel userBioData)
        {
            await _container.UpsertItemAsync(userBioData, new PartitionKey(userBioData.Id));
        }

        public async Task DeleteAsync(int id)
        {
            await _container.DeleteItemAsync<UserBioDataModel>(id.ToString(), new PartitionKey(id));
        }
    }
}
