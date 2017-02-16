using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace PrivilegeMobileService.Model
{
    public class DataStoreOptions
    {
        public string StorageType { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public string DatabaseName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public NameValueCollection GetParameters()
        {
            var parameters = new NameValueCollection();
            parameters.Add("host", Host);
            parameters.Add("port", Port);
            parameters.Add("database", DatabaseName);
            parameters.Add("username", UserName);
            parameters.Add("password", Password);
            return parameters;
        }
    }
}
