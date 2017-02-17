using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PrivilegeCoreLibrary;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PrivilegeMobileService.Model;
using System.Collections.Specialized;
using PrivilegeCoreLibrary.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PrivilegeMobileService.Controllers
{
    [Route("api/[controller]")]
    public class CompanyController : Controller
    {
        private IPrivilegeStore _store;
        private readonly ILogger _logger;
        public CompanyController(IOptions<DataStoreOptions> storeOptions, ILogger<CustomerController> logger)
        {
            _store = new PrivilegeStoreMongo(storeOptions.Value.GetParameters());
            _logger = logger;
        }
        
        // GET api/values/5
        [HttpGet("{name}")]
        [AllowAnonymous]
        public Company Get(string name)
        {
            return _store.GetCompany(name);
        }
        
        /// <summary>
        /// update company details (only accessible by company staff)
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update")]
        [Authorize(Policy = "CompanyPolicy")]
        public Company Post([FromBody]Company company)
        {
            var email = Request.HttpContext.User.Claims.FirstOrDefault(t => t.Type == "customer_email").Value;
            if (_store.GetCompanyStaff(email).company != company.name) throw new UnauthorizedAccessException($"update company details only accessible by company staff");
            _logger.LogInformation($"update company {company.name} details by");
            var updated = _store.UpdateCompany(company);
            if (!updated) throw new KeyNotFoundException($"{company.name} not found.");
            return _store.GetCompany(company.name);
        }
        
    }
}
