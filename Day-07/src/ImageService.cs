using System;
using System.Net.Http;
using System.Threading.Tasks;
using Unsplasharp;

namespace MarcusTurewicz.ImageSearch
{
    public class ImageService
    {
        private UnsplasharpClient _client;
        private static HttpClient _httpClient = new HttpClient();
        
        public ImageService(string accessKey, string secretKey)
        {
            _client = new UnsplasharpClient(accessKey, secretKey);
        } 

        public async Task<(byte[] Bytes, string Filename)> GetImageAsync(string search)
        {
            var photo = (await _client.SearchPhotos(search, 1, 1))[0];
            return (await _httpClient.GetByteArrayAsync(photo.Links.Download), "image.png");
        }
    }
}