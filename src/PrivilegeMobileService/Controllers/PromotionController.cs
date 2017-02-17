using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using PrivilegeCoreLibrary.Model;
using PrivilegeCoreLibrary;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PrivilegeMobileService.Model;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PrivilegeMobileService.Controllers
{
    [Route("api/[controller]")]
    public class PromotionController : Controller
    {
        private IPrivilegeStore _store;
        private readonly ILogger _logger;
        public PromotionController(IOptions<DataStoreOptions> storeOptions, ILogger<CustomerController> logger)
        {
            _store = new PrivilegeStoreMongo(storeOptions.Value.GetParameters());
            _logger = logger;
        }
        
        [HttpGet("{company}")]
        [AllowAnonymous]
        public List<Promotion> GetPromotions(string company)
        {
            return _store.GetPromotions(company);
        }
        
        [HttpPut]
        [Route("add")]
        public void Put([FromBody]Promotion value)
        {
        }
        
        [HttpPost]
        [Route("edit")]
        public void Edit([FromBody]Promotion value)
        {

        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
