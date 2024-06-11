using Firebase.Auth;
using Firebase.Auth.Providers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace ODTLearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _env;

        public FileController(Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            _env = env;
        }
        //Configure Firebase
        private static string apiKey = "AIzaSyB9Hu5ycmYSHBMk7NiIzzlhUlIJ-xLHC04";
        private static string bucket = "odt-learning-bdb8c.appspot.com";
        private static string authEmail = "leminhtan@gmail.com";
        private static string authPassword = "123123";
        

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            FileStream fs = null;
            if (file.Length > 0) 
            {
                //upload file to firebase
                string folderName = "FirebaseFiles";
                string path = Path.Combine(_env.WebRootPath, $"Images/{folderName}");
                fs = new FileStream(Path.Combine(path, file.FileName), FileMode.Open);

                //firebase uploading stuffs
                //var auth = new FirebaseAuthProvider(new FirebaseConfig(apiKey));
            }

            return BadRequest();
        }

    }
}
