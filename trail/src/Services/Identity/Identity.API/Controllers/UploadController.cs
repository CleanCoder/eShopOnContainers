using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ID.eShop.Services.Identity.API.Controllers
{
    public class UploadController : Controller
    {

        private const string MediaRootFoler = "file-storage";

        private readonly IWebHostEnvironment _env;
        private readonly string _folder;
        private readonly ILogger<UploadController> _logger;

        public UploadController(IWebHostEnvironment env, ILogger<UploadController> logger)
        {
            _env = env;
            _logger = logger;
            _folder = Path.Combine(_env.WebRootPath, MediaRootFoler);

            if (!Directory.Exists(_folder))
            {
                _logger.LogWarning($"Directory: {_folder} does not exist. Creatinig...");
                Directory.CreateDirectory(_folder);
            }
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Post(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            //Should validate if directory above exists...
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    //Upload file to directory with same filename.
                    using (var stream = new FileStream(Path.Combine(_folder, formFile.FileName), FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            return Ok(new { FileUploadedCount = files.Count, TotalFileSize = size, FileSaveDirectory = _folder });
        }
    }
}
