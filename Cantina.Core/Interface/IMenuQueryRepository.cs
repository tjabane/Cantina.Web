using Cantina.Core.Dto;


namespace Cantina.Core.Interface
{
    public interface IMenuQueryRepository
    {
        Task<List<MenuItem>> GetAllAsync();
        Task<MenuItem> GetByIdAsync(int id);
        Task<List<MenuItem>> SearchAsync(string searchTerm);
    }
}
