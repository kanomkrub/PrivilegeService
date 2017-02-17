﻿using System;
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
        // GET api/values/5
        [HttpGet("{id}")]
        public Promotion Get(string id)
        {
            return _store.GetPromotion(id);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
