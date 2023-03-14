using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RadzenFileUpload.Controllers
{
    [DisableRequestSizeLimit]
    public class UploadController : Controller
    {
        private readonly IWebHostEnvironment _environment;

        public UploadController(IWebHostEnvironment enviroment)
        {
            _environment = enviroment;

        }


        [HttpPost("upload/single")]
        public async Task<IActionResult> Single(IFormFile file)
        {
            try
            {
                await UploadFile(file);

                return StatusCode(200);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost("upload/multiple")]
        public async Task<IActionResult> Multiple(IFormFile[] files)
        {
            try
            {
                foreach (var file in files)
                {
                    await UploadFile(file);
                }
                return StatusCode(200);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        public async Task UploadFile(IFormFile file)
        {
            if(file != null && file.Length > 0)
            {
                var imagePath = @"\Upload";
                var uploadPath = _environment.WebRootPath + imagePath;
                if(!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                var fullPath = Path.Combine(uploadPath, file.FileName);
                using (FileStream fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
        }

    }
}