using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EngineOne.Models;
using EngineOne.Repository.IRepository;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace EngineOne.Repository
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly IHttpClientFactory clientFactory;
        private readonly IOptions<AppSettings> appSettings;
        private readonly ILogger<PhotoRepository> logger;



        public PhotoRepository(IHttpClientFactory clientFactory, IOptions<AppSettings> appSettings, ILogger<PhotoRepository> logger)
        {
            this.clientFactory = clientFactory;
            this.appSettings = appSettings;
            this.logger = logger;
        }
        public async Task<IEnumerable<ApiResponse>> GetAllAsync(string url, string token)
        {
            int totalPages = 0;
            int currentPage = 1;
            var nextUrl = $"{url}?page={currentPage}";
            var pages = new List<ApiResponse>();


            var client = this.clientFactory.CreateClient();
            if (token != null && token.Length != 0)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            else
            {
                return null;
            }

            do
            {
                var request = new HttpRequestMessage(HttpMethod.Get, nextUrl);

                HttpResponseMessage response = await client.SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {

                    var jsonString = await response.Content.ReadAsStringAsync();
                    var responseObj = JsonConvert.DeserializeObject<ApiResponse>(jsonString);

                    if (responseObj != null)
                    {
                        pages.Add(responseObj);
                        totalPages = responseObj.pageCount;

                        currentPage++;
                        nextUrl = $"{url}?page={currentPage}";
                    }

                }
            } while (currentPage <= totalPages);

            return pages;
        }


        public async Task<PictureDetails> GetById(string url, string token, string id)
        {
        var client = this.clientFactory.CreateClient();
            var urlDetails = string.Concat(url, id.ToString());
            if (token != null && token.Length != 0)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
       

            var request = new HttpRequestMessage(HttpMethod.Get, urlDetails);

            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {

                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<PictureDetails>(jsonString);

               

            }

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) 
            {
                this.logger.LogError("Token has expired");
            }


                return null;

        }

       public async Task <AuthResponse> GetToken() {
            var client = this.clientFactory.CreateClient();
            var url = this.appSettings.Value.PhotoApi.AuthUri;
            var body = new Dictionary<string, string>();
            body.Add("apiKey", this.appSettings.Value.PhotoApi.Apikey);

            using (var request = new HttpRequestMessage(HttpMethod.Post, url))
            {
                var json = JsonConvert.SerializeObject(body);
                using (var stringContent = new StringContent(json, Encoding.UTF8, "application/json"))
                {
                    request.Content = stringContent;

                    using (var response = await client
                        .SendAsync(request, HttpCompletionOption.ResponseHeadersRead)
                        .ConfigureAwait(false))
                    {
                        response.EnsureSuccessStatusCode();
                        var jsonString = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<AuthResponse>(jsonString);

                    }
                }
            }
        }


    }
}

