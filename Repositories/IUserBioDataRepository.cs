using System;
using health_pal_backend.Models;

namespace health_pal_backend.Repositories;

public interface IUserBioDataRepository
{
    Task AddAsync(UserBioDataModel userBioData);
    Task<UserBioDataModel?> GetIdByAsync(int id);
    Task<IEnumerable<UserBioDataModel>> GetAllAsync();
    Task UpdateAsync(UserBioDataModel userBioData);
    Task DeleteAsync(int id);
}
