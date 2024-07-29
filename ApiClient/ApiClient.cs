using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using DataAccess.Entities;
using JsonPlaceholderDataAccess.Entities;

namespace JsonPlaceholderApiClient
{
    public class ApiClient : IApiClient 
    {
        private readonly HttpClient _httpClient;

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Post>> GetPostsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Post>>("https://jsonplaceholder.typicode.com/posts");
        }
        public async Task<List<Comments>> GetCommentsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Comments>>("https://jsonplaceholder.typicode.com/comments");
        }

        public async Task<List<Albums>> GetAlbumsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Albums>>("https://jsonplaceholder.typicode.com/albums");
        }

        public async Task<List<Photos>> GetPhotosAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Photos>>("https://jsonplaceholder.typicode.com/photos");
        }

        public async Task<List<Todos>> GetTodosAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Todos>>("https://jsonplaceholder.typicode.com/todos");
        }

        public async Task<List<Users>> GetUsersAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Users>>("https://jsonplaceholder.typicode.com/users");
        }
    }
}
