using Cantina.Core.Dto;


namespace Cantina.Core.Interface
{
    public interface IMenuCommandRepository
    {
        Task AddAsync(MenuItem menuItem);
        Task UpdateAsync(MenuItem menuItem);
        Task DeleteAsync(int id);
    }
}
