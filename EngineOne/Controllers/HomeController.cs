using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EngineOne.Models;
using EngineOne.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace EngineOne.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPhotoService _photoService;
        private IMemoryCache _memoryCache;





        public HomeController(ILogger<HomeController> logger, 
            IPhotoService photoService,
            IMemoryCache memoryCache)

        {
            _logger = logger;
            _photoService = photoService;
            _memoryCache = memoryCache;

    }



        public async Task<IActionResult> Index(int? page)
        {
            string cacheToken;

            bool tokenExists = _memoryCache.TryGetValue("JWTToken", out cacheToken);

            if (!tokenExists) { await setJWTinCache(); };


            IEnumerable<ApiResponse> cacheResponse;
            bool listExists = _memoryCache.TryGetValue("ImageList", out cacheResponse);

            if (!listExists)
            {
            var ImageList = await _photoService.GetPhotos();
            var objImageList = page != null ? ImageList.FirstOrDefault(_ => _.page == page) : ImageList.FirstOrDefault();
                cacheResponse = ImageList.ToList();
                var cacheOption = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(60));
                _memoryCache.Set("ImageList", cacheResponse, cacheOption);
                return View(objImageList);
             }
            var cacheImageList = _memoryCache.Get("ImageList");

            var objList = cacheImageList as IEnumerable<ApiResponse>;
            var obj = objList.FirstOrDefault();

            return View(obj);
        }


        public async Task<IActionResult>  PhotoDetails (string id)
        {

          var obj =  await _photoService.GetPhotoById(id);


            return View(obj);
        }

        public IActionResult GetNextOrPreviousPage(int page) {

            IEnumerable<ApiResponse> cacheKey;


            var listExists = _memoryCache.TryGetValue("ImageList", out cacheKey);

            if (!listExists) 
            {
                return RedirectToAction("Index", page);
            }
            var cacheImageList = _memoryCache.Get("ImageList");

           var objList = cacheImageList as IEnumerable<ApiResponse>;
           var obj = objList.FirstOrDefault(_ => _.page == page);
            return  View("Index", obj);

        }

        private async Task setJWTinCache()
        {

            string cacheResponse;
            bool tokenExists = _memoryCache.TryGetValue("JWTToken", out cacheResponse);

            if (!tokenExists)
            {
                var token = await  _photoService.GetToken();
                cacheResponse = token.token;
                var cacheOption = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(60));
                _memoryCache.Set("JWTToken", cacheResponse, cacheOption);
            }
        }

    }
}
