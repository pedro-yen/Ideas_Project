using Backend.Challenge.Dtos;
using Raven.Client.Documents;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Challenge._2._Data.Repositories
{
    public class MainRepository : IRepository
    {
        private readonly IDocumentStore _store;
        public MainRepository(IDocumentStore store)
        {
            _store = store;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            using var session = _store.OpenAsyncSession(); 
            await session.StoreAsync(user); 
            await session.SaveChangesAsync(); 
            return user; 
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            using var session = _store.OpenAsyncSession();
            var users = await session.Query<User>().ToListAsync();
            return users;
        }

        public async Task<List<User>> GetUsersByIdsAsync(string[] ids)
        {
            using var session = _store.OpenAsyncSession();
            var users = await session.LoadAsync<User>(ids);
            return users.Values.ToList();
        }
    }
}
