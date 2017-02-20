using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PrivilegeCoreLibrary.Model;
using PrivilegeCoreLibrary;
using Microsoft.Extensions.Options;
using PrivilegeMobileService.Model;
using System.Collections.Specialized;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PrivilegeMobileService.Controllers
{
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {
        private IPrivilegeStore _dataStore;
        private readonly ILogger _logger;
        public CustomerController(IOptions<DataStoreOptions> storeOptions, ILogger<CustomerController> logger)
        {
            _dataStore = new PrivilegeStoreMongo(storeOptions.Value.GetParameters());
            _logger = logger;
        }

        /// <summary>
        /// Get Customer privilege info using indentity from jwt 
        /// create customer if customer not exists
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Customer Get()
        {
            var emailClaim = Request.HttpContext.User.Claims.FirstOrDefault(t => t.Type =="customer_email");
            var fullnameClaim = Request.HttpContext.User.Claims.FirstOrDefault(t => t.Type == "customer_fullname");
            if (emailClaim == null) throw new KeyNotFoundException("Email not found in jwt tokens");
            var email = emailClaim.Value;
            var fullname = fullnameClaim?.Value;
            var customer = _dataStore.GetCustomer(email);
            if (customer != null) return customer;
            else
            {
                var newCustomer = new Customer() { email = email };
                _dataStore.CreateCustomer(newCustomer);
                return newCustomer;
            }
        }

        /// <summary>
        /// set favourite merchant stores
        /// </summary>
        /// <param name="favourites"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("setfavourite")]
        public bool SetFavourites([FromBody]string[] favourites)
        {
            var email = Request.HttpContext.User.Claims.FirstOrDefault(t => t.Type == "customer_email").Value;
            var customer = _dataStore.GetCustomer(email);
            customer.favourite = favourites;
            return _dataStore.UpdateCustomer(customer);
        }
    }
}
