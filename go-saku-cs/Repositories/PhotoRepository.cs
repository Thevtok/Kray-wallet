using Go_Saku.Net.Data;
using go_saku_cs.Models;
using Go_Saku_CS.Repositories;

namespace go_saku_cs.Repositories
{
    public interface IPhotoRepository
    {
        Task Create(PhotoUser photo);
        PhotoUser GetByID(Guid id);
        Task Update(PhotoUser photo);
        Task Delete(Guid id);
    }
    public class PhotoRepository : IPhotoRepository
    {
        private readonly UserApiDbContext _dbContext;

        public PhotoRepository(UserApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Create(PhotoUser photo)
        {
            // Menambahkan foto ke dalam database
            _dbContext.PhotoUsers.Add(photo);
            await _dbContext.SaveChangesAsync();
        }

        public PhotoUser GetByID(Guid id)
        {
            return _dbContext.PhotoUsers
                .FirstOrDefault(b => b.UserID == id);
        }
        public async Task Update(PhotoUser photo)
        {
            var existingPhoto =  _dbContext.PhotoUsers.FirstOrDefault(p => p.UserID == photo.UserID);

            if (existingPhoto != null)
            {
                existingPhoto.Url = photo.Url;

                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Photo not found");
            }
        }
        public async Task Delete(Guid id)
        {
            var existingPhoto =  _dbContext.PhotoUsers.FirstOrDefault(p => p.UserID == id);

            if (existingPhoto != null)
            {
                _dbContext.PhotoUsers.Remove(existingPhoto);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Photo not found");
            }
        }
    }

}
