using go_saku_cs.Models;
using go_saku_cs.Repositories;
using Go_Saku_CS.Repositories;

namespace go_saku_cs.Usecase
{
    public interface IPhotoUsecase
    {
        Task Create(PhotoUser photo);
        PhotoUser GetByID(Guid id);
        Task Update(PhotoUser photo);
        Task Delete(Guid id);
    }
    public class PhotoUsecase : IPhotoUsecase {
        private readonly IPhotoRepository _photoRepository;
        public PhotoUsecase(IPhotoRepository photoRepository)
        {
            _photoRepository = photoRepository;
        }

        public async Task Create(PhotoUser photo)
        {
            await _photoRepository.Create(photo);
        }

        public PhotoUser GetByID(Guid id)
        {
            return _photoRepository.GetByID(id);
        }

        public async Task Update(PhotoUser photo)
        {
            await _photoRepository.Update(photo);
        }

        public async Task Delete(Guid id)
        {
            await _photoRepository.Delete(id);
        }
    }
}
