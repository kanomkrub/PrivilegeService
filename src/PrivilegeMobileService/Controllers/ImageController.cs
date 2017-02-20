using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using PrivilegeCoreLibrary;
using PrivilegeMobileService.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PrivilegeMobileService.Controllers
{
    [Route("api/[controller]")]
    public class ImageController : Controller
    {
        private readonly IPrivilegeStore _store;
        private readonly ImageStoreOptions _imageStoreOptions;
        private readonly ILogger _logger;
        public ImageController(IOptions<DataStoreOptions> storeOptions,IOptions<ImageStoreOptions> imageStoreOptions, ILogger<CustomerController> logger)
        {
            _store = new PrivilegeStoreMongo(storeOptions.Value.GetParameters());
            _imageStoreOptions = imageStoreOptions.Value;
            _logger = logger;
        }
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        //[Authorize(Policy = "StaffPolicy")]
        [HttpPost]
        public void Upload([FromForm]IFormFile file)
        {
            if (file == null) throw new Exception("File is null");
            if (file.Length == 0) throw new Exception("File is empty");

            var email = HttpContext.User.Claims.FirstOrDefault(t => t.Type == "customer_email").Value;
            var company = _store.GetCompanyStaff(email).company;
            _logger.LogInformation($"uploading file {file.FileName} to {company}");
            var writeTo = $"{_imageStoreOptions.PathRoot}\\{company}\\{file.Name}";
            using (Stream stream = file.OpenReadStream())
            {
                using (var binaryReader = new BinaryReader(stream))
                {
                    var fileContent = binaryReader.ReadBytes((int)file.Length);
                    System.IO.File.WriteAllBytes(writeTo, fileContent);
                }
            }
        }
        
        /// <summary>
        /// get image lists for a company
        /// </summary>
        /// <param name="companyname"></param>
        /// <returns></returns>
        [HttpGet("{companyname}")]
        public IEnumerable<string> Get(string companyname)
        {
            var companyImagePath = $"{_imageStoreOptions.PathRoot}\\{companyname}";
            foreach (var x in Directory.GetFiles(companyImagePath))
            {
                yield return x;
            }
        }
    }
}
