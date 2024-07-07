using FoodOrderingWeb.Models;

namespace FoodOrderingWeb.Repository.Interface
{
    public interface Interface_PictureListRepository
    {
        Task<IEnumerable<PictureLists>> GetAllAsync();
        Task<PictureLists> GetByIdAsync(int id);
        Task AddAsync(PictureLists pictureLists);
        Task UpdateAsync(PictureLists pictureLists);
        Task DeleteAsync(int id);
    }
}
