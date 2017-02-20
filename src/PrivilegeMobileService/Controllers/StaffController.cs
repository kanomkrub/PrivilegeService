using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PrivilegeCoreLibrary.Model;
using PrivilegeCoreLibrary;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PrivilegeMobileService.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PrivilegeMobileService.Controllers
{
    [Route("api/[controller]")]
    public class StaffController : Controller
    {
        private IPrivilegeStore _store;
        private readonly ILogger _logger;
        public StaffController(IOptions<DataStoreOptions> storeOptions, ILogger<CustomerController> logger)
        {
            _store = new PrivilegeStoreMongo(storeOptions.Value.GetParameters());
            _logger = logger;
        }

        // GET: api/values
        [HttpGet]
        public Staff Get()
        {
            var email = Request.HttpContext.User.Claims.FirstOrDefault(t => t.Type == "customer_email").Value;
            return _store.GetCompanyStaff(email);
        }
    }
}
