using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using EngineOne.Models;
using EngineOne.Repository.IRepository;
using EngineOne.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace EngineOne.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly IOptions<AppSettings> appSettings;
        private readonly IPhotoRepository photoRepository;
        private IMemoryCache memoryCache;


        public PhotoService(IOptions<AppSettings> appSettings, IPhotoRepository photoRepository, IMemoryCache memoryCache)
        {
            this.appSettings = appSettings;
            this.photoRepository = photoRepository;
            this.memoryCache = memoryCache;

        }

        public async Task<PictureDetails> GetPhotoById(string id)
        {
            return await this.photoRepository.GetById(this.appSettings.Value.PhotoApi.PhotoUri.ToString(), (string)this.memoryCache.Get("JWTToken"), id);
        }

        public async Task<IEnumerable<ApiResponse>> GetPhotos()
        {
            return await this.photoRepository.GetAllAsync(this.appSettings.Value.PhotoApi.PhotoUri.ToString(), (string)this.memoryCache.Get("JWTToken"));
          
        }

       public async Task<AuthResponse> GetToken() 
        {
            return await this.photoRepository.GetToken();

        }

    }
}
