using FoodOrderingWeb.DataAccess;
using FoodOrderingWeb.Models;
using FoodOrderingWeb.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderingWeb.Repository.EF
{
    public class EF_PictureList : Interface_PictureListRepository
    {
        private readonly ApplicationDatabaseContext _context;
        public EF_PictureList(ApplicationDatabaseContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<PictureLists>> GetAllAsync()
        {
            return await _context.PictureLists.Include(p => p.FoodItem).ToListAsync();
        }
        public async Task<PictureLists> GetByIdAsync(int id)
        {
            return await _context.PictureLists.Include(p => p.FoodItem).FirstAsync(p=>p.Id == id);
        }
        public async Task AddAsync(PictureLists pictureLists)
        {
            _context.PictureLists.Add(pictureLists);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(PictureLists pictureLists)
        {
            _context.PictureLists.Update(pictureLists);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var picture = await _context.PictureLists.FindAsync(id);
            if (picture != null)
            {
                _context.PictureLists.Remove(picture);
                await _context.SaveChangesAsync();
            }
        }
    }
}
