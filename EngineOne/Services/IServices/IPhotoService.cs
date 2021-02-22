using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EngineOne.Models;

namespace EngineOne.Services.IServices
{
   public interface IPhotoService
    {
        Task<IEnumerable<ApiResponse>> GetPhotos ();
        Task<PictureDetails> GetPhotoById(string id);
        Task<AuthResponse> GetToken();
    }
}
