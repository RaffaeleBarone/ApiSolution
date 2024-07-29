using DataAccess.Entities;
using JsonPlaceholderDataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonPlaceholderApiClient
{
    public interface IApiClient
    {
        Task<List<Post>> GetPostsAsync();
        Task<List<Comments>> GetCommentsAsync();
        Task<List<Albums>> GetAlbumsAsync();
        Task<List<Photos>> GetPhotosAsync();
        Task<List<Todos>> GetTodosAsync();
        Task<List<Users>> GetUsersAsync();

    }
}
