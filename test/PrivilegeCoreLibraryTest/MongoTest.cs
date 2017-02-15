using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrivilegeCoreLibrary;
using System.Collections.Specialized;
using PrivilegeCoreLibrary.Model;

namespace PrivilegeCoreLibraryTest
{
    [TestClass]
    public class MongoTest
    {
        private static PrivilegeContentMongo GetContentStoreMongo()
        {
            var paras = new NameValueCollection();
            paras.Add("database", "PrivilegeDB");
            paras.Add("username", "");
            paras.Add("password", "");
            paras.Add("host", "localhost");
            paras.Add("port", "27017");
            var store = new PrivilegeContentMongo(paras);
            return store;
        }
        [TestMethod]
        public void GetMongoStore()
        {
            PrivilegeContentMongo store = GetContentStoreMongo();
            Assert.IsNotNull(store);
        }

        [TestMethod]
        public void CreateAndDeleteCompany()
        {
            var store = GetContentStoreMongo();
            var new_company = new Company()
            {
                name = "MCDonald",
                display_name = "mac donald ja",
                description = "xxxxxxxxxxxx"
            };
            store.CreateCompany(new_company);
            var company = store.GetCompany("MCDonald");
            Assert.IsTrue(store.DeleteCompany(company.id));
        }

        [TestMethod]
        public void CreateAndDeleteCustomer()
        {
            var store = GetContentStoreMongo();
            var new_customer = new Customer()
            {
                email = "biggy@ssss.hotmail.com",
                point = 5553,
                favourite = new string[] { "KFC", "MCDonald" }
            };
            store.CreateCustomer(new_customer);
            var customer = store.GetCustomer("biggy@ssss.hotmail.com");
            Assert.IsTrue(store.DeleteCustomer(customer.id));
        }
    }
}
