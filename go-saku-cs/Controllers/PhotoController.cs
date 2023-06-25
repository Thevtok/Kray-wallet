using Go_Saku.Net.Utils;
using go_saku_cs.Models;
using go_saku_cs.Usecase;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace go_saku_cs.Controllers


{
    [ApiController]
    [Route("api/photo")]
 
        public class PhotoController : ControllerBase
        {
            private readonly IPhotoUsecase _photoUsecase;

            public PhotoController(IPhotoUsecase photoUsecase)
            {
                _photoUsecase = photoUsecase;
            }

        [HttpPost("userid/{user_id}")]
        public async Task<ActionResult<IEnumerable<PhotoUser>>> Upload(Guid user_id, IFormFile photo)
        {

            // Validasi ekstensi file
            string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
            string fileExtension = Path.GetExtension(photo.FileName).ToLower();
            if (!allowedExtensions.Contains(fileExtension))
            {
                return BadRequest("Invalid file extension. Allowed extensions are: .jpg, .jpeg, .png");
            }

            // Simpan file ke direktori atau penyimpanan yang sesuai
            string fileName = photo+ fileExtension;
            string filePath = Path.Combine("E:/github/go-saku-cs/go-saku-cs/File", fileName);
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await photo.CopyToAsync(fileStream);
            }

           

            // Simpan informasi foto ke use case atau repository
            var newPhoto = new PhotoUser
            {
                UserID = user_id,
                Url = fileName
            };
            await _photoUsecase.Create(newPhoto);

            return Ok();
        }






    }
}
