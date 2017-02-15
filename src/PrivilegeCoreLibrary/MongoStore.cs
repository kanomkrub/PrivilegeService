using MongoDB.Driver;
using PrivilegeCoreLibrary.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace PrivilegeCoreLibrary
{
    public class PrivilegeContentMongo : PrivilegeContentStore
    {
        protected static IMongoClient _client;
        protected static IMongoDatabase _database;
        internal PrivilegeContentMongo(NameValueCollection parameter)
        {
            var database = parameter["database"];
            var username = parameter["username"];
            var password = parameter["password"];
            //try
            //{
            //    password = BasicCrypto.DecryptLocal(password);
            //}
            //catch (Exception) { }
            var host = parameter["host"];
            var port = int.Parse(parameter["port"]);
            //_client = new MongoClient(
            //new MongoClientSettings
            //{Credentials = new[]
            //     {MongoCredential.CreateCredential(database,username,password)},
            //    Server = new MongoServerAddress(host, port)});
            _client = new MongoClient($"mongodb://{host}:{port}/{database}");
            _database = _client.GetDatabase(database);
        }


        internal override void CreateCompany(Company anno)
        {
            anno.id = Guid.NewGuid().ToString();
            var collection = _database.GetCollection<Company>(PrivilegeContentCollections.company);
            collection.InsertOneAsync(anno).Wait();
        }

        internal override void UpdateCompany(Company company)
        {
            throw new NotImplementedException();
        }

        internal override bool DeleteCompany(string id)
        {
            var collection = _database.GetCollection<Company>(PrivilegeContentCollections.company);
            var filter = Builders<Company>.Filter.Eq(t => t.id, id);
            return collection.DeleteOneAsync(filter).Result.DeletedCount > 0;
        }
        internal override Company GetCompany(string name)
        {
            var collection = _database.GetCollection<Company>(PrivilegeContentCollections.company);
            var filter = Builders<Company>.Filter.Eq(t => t.name, name);
            return collection.Find(filter).SingleAsync().Result;
        }

        internal override void CreateCustomer(Customer customer)
        {
            customer.id = Guid.NewGuid().ToString();
            var collection = _database.GetCollection<Customer>(PrivilegeContentCollections.customer);
            collection.InsertOneAsync(customer).Wait();
        }

        internal override Customer GetCustomer(string email)
        {
            var collection = _database.GetCollection<Customer>(PrivilegeContentCollections.customer);
            var filter = Builders<Customer>.Filter.Eq(t => t.email, email);
            return collection.Find(filter).SingleAsync().Result;
        }

        internal override void UpdateCustomer(Customer customer)
        {
            throw new NotImplementedException();
        }

        internal override bool DeleteCustomer(string id)
        {
            var collection = _database.GetCollection<Customer>(PrivilegeContentCollections.customer);
            var filter = Builders<Customer>.Filter.Eq(t => t.id, id);
            return collection.DeleteOneAsync(filter).Result.DeletedCount > 0;
        }

    }
}
