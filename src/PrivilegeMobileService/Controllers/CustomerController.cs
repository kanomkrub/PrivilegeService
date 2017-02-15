using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PrivilegeCoreLibrary.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PrivilegeMobileService.Controllers
{
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {
        // GET: api/values
        [HttpGet]
        public Customer Get()
        {
            var emailClaim = Request.HttpContext.User.Claims.FirstOrDefault(t => t.Subject.ToString() == "customer_email" || t.Type =="customer_email");
            if (emailClaim == null) throw new KeyNotFoundException("Email not found in jwt tokens");
            var email = emailClaim.Value;
            return new Customer() { email = email };
        }
        
    }
}
