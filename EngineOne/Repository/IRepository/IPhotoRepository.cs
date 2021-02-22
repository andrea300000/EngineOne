using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EngineOne.Models;

namespace EngineOne.Repository.IRepository
{
    public interface IPhotoRepository 
    {
        Task<IEnumerable<ApiResponse>> GetAllAsync(string url, string token);
        Task<PictureDetails> GetById(string url, string token, string id);
        Task<AuthResponse> GetToken();
    }
}
