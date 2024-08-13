namespace WebApi.Interfaces
{
    public interface IUserRepository
    {
        public Task<string> GetUserIdAsync(string username);

    }
}
