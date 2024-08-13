namespace BlazorWebAssemblyApp.Services
{
    public interface IUserService
    {
        public Task<string> GetUserId (string username);
    }
}
