using BlazorWebAssemblyApp.Services;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Interfaces;

namespace WebApi.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<string> GetUserIdAsync(string username)
        {
          var user = await _context.Users.FirstOrDefaultAsync(u=>u.UserName == username);
            if (user == null) return default;

            return  user.Id;
        }

    }
}
